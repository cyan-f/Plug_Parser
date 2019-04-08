using Buttplug.Client;
using System;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	public struct remote_settings
	{
		public double minimumStrength;
		public double maximumStrength;

		public double buzzStrength;
		public double baseStrength;

		public double erraticness;
		public double variance;
		
		public double frequency;
		public double period;
		public double phaseShift; // Adds (phaseShift * period) to [current time]

		public double spikeAmount;
		public double spikeTimeLeft;

		public remote_settings(double minimumStrength, double maximumStrength, 
			double buzzStrength, double baseStrength, 
			double erraticness, double variance, 
			double frequency, double period, double phaseShift,
			double spikeAmount, double spikeTimeLeft)
		{
			this.minimumStrength = minimumStrength;
			this.maximumStrength = maximumStrength;
			this.buzzStrength = buzzStrength;
			this.baseStrength = baseStrength;
			this.erraticness = erraticness;
			this.variance = variance;
			this.period = period;
			this.frequency = frequency;
			this.phaseShift = phaseShift;
			this.spikeAmount = spikeAmount;
			this.spikeTimeLeft = spikeTimeLeft;
		}
	}

	class Vibe_Remote
	{
		private remote_settings settings;

		#region Default values
		private const double MINIMUM_STRENGTH = 0;
		private const double MAXIMUM_STRENGTH = 100;
		private const double BUZZ_STRENGTH = 0;
		private const double BASE_STRENGTH = 15;
		private const double ERRATICNESS = 0;
		private const double VARIANCE = 10;
		private const double FREQUENCY = 1;
		private const double PERIOD = 1000 * (1 / FREQUENCY);
		private const double PHASE_SHIFT = 0;
		private const double SPIKE_AMOUNT = 0;
		private const double SPIKE_DURATION = 0;
		#endregion

		private bool isOverriding;
		private double overrideStrength;
		// For temporary "buzzes"
		private double overrideDuration;
		private long overrideStartTime;

		private long lastTime;
		private long thisTime;
		private long deltaTime;

		// Stores last sent vibration strength for interpolation
		private double previousStrength;
		private bool isInterpolated;

		public Vibe_Remote()
		{
			isOverriding = false;
			overrideStrength = 0;

			lastTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

			previousStrength = 0;

			// Initialize default settings
			settings = new remote_settings(MINIMUM_STRENGTH, MAXIMUM_STRENGTH,
				BUZZ_STRENGTH, BASE_STRENGTH, ERRATICNESS, VARIANCE, FREQUENCY, PERIOD, PHASE_SHIFT,
				SPIKE_AMOUNT, SPIKE_DURATION);
		}

		// Getters
		public double getPreviousStrength()
		{
			return previousStrength;
		}

		public double updateStrength()
		{
			thisTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			deltaTime = thisTime - lastTime;
			lastTime = thisTime;

			double val = 0;

			if (isOverriding)
			{
				val = overrideStrength;
				overrideDuration -= deltaTime;
				
				if (overrideDuration <= 0)
				{
					unsetOverride();
				}
			}
			else if (isInterpolated)
			{
				val = interpolate(Power_Calculator.getStrength(settings));
			}
			else
			{
				val = Power_Calculator.getStrength(settings);
			}

			decay();

			previousStrength = val;
			return val;
		}

		// Controls

		// Flat buzz
		public void buzz(double strength, long duration)
		{
			if (strength > overrideStrength)
			{
				setOverride(strength, duration);
			}
		}

		// Augments normal strength
		public void spike(double strength, double duration)
		{
			settings.spikeAmount = strength;
			settings.spikeTimeLeft = duration;
		}

		public void accelerate(double factor)
		{
			double newFrequency = settings.frequency * factor;
			setFrequency(newFrequency);

			Log_Manager.write("NF: " + newFrequency);
		}

		public void excite(double factor)
		{

		}

		// Increases baseStrength by given amount.
		public void bump(double amount)
		{
			settings.baseStrength += amount;
		}

		// Meta controls
		public void setOverride(double strength)
		{
			setOverride(strength, long.MaxValue);
		}
		
		public void setOverride(double strength, long duration)
		{
			isOverriding = true;
			overrideStrength = strength;
			overrideDuration = duration;
			overrideStartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		}

		public void unsetOverride()
		{
			isOverriding = false;
			overrideDuration = 0;
		}


		// Tasks
		public async Task vibe(ButtplugClientDevice toy)
		{
			await toy.SendVibrateCmd(getPower());
		}

		// Private helpers
		private double getPower()
		{
			return previousStrength / 100;
		}

		private double interpolate(double target)
		{
			return target; // TODO

			if (System.Math.Abs(target - previousStrength) < 0.05)
			{
				isInterpolated = false;
				return target;
			}
		}

		private void setFrequency(double freq)
		{
			settings.frequency = freq;
			settings.period = 1000 * (1 / freq);
		}

		private void setPeriod(double per)
		{
			settings.period = per;
			settings.frequency = (1 / per) / 1000;
		}

		private void setPhase(double phase)
		{
			settings.phaseShift = phase;
		}

		private void decay()
		{
			if (settings.spikeTimeLeft > 0)
			{
				settings.spikeTimeLeft -= deltaTime;
			}

			if (settings.frequency > 1)
			{
				accelerate(0.95 * (1 - (deltaTime / 1000)));
			}

			if (settings.baseStrength > 15)
			{
				bump(-(deltaTime / 2000));
			}
		}
	}
}
