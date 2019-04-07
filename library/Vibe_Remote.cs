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

		public remote_settings(double minimumStrength, double maximumStrength, 
			double buzzStrength, double baseStrength, 
			double erraticness, double variance, 
			double frequency, double period, double phaseShift)
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
		private const double PERIOD = 1000 * FREQUENCY;
		private const double PHASE_SHIFT = 0;
		#endregion

		private bool isOverriding;
		private double overrideStrength;
		// For temporary "buzzes"
		private double overrideDuration;
		private long overrideStartTime;

		// Stores last sent vibration strength for interpolation
		private double previousStrength;
		private bool isInterpolated;

		public Vibe_Remote()
		{
			isOverriding = false;
			overrideStrength = 0;

			previousStrength = 0;

			// Initialize default settings
			settings = new remote_settings(MINIMUM_STRENGTH, MAXIMUM_STRENGTH,
				BUZZ_STRENGTH, BASE_STRENGTH, ERRATICNESS, VARIANCE, FREQUENCY, PERIOD, PHASE_SHIFT);
		}

		// Getters
		public double getPreviousStrength()
		{
			return previousStrength;
		}

		public double updateStrength()
		{
			double val = 0;

			if (isOverriding)
			{
				val = overrideStrength;

				long timePassed = DateTimeOffset.Now.ToUnixTimeMilliseconds() - overrideStartTime;
				Log_Manager.write("Passed: " + timePassed + ", Target: " + overrideDuration);
				if (timePassed >= overrideDuration)
				{
					unsetOverride();
				}
			}
			else if (isInterpolated)
			{
				val = interpolate(Power_Calculator.getWaveValue(settings));
			}
			else
			{
				val = Power_Calculator.getWaveValue(settings);
			}
			
			previousStrength = val;
			return val;
		}

		// Controls

		// Flat buzz
		public void buzz(double strength, long duration)
		{
			setOverride(strength, duration);
		}

		// Augments normal strength
		public void spike(double strength, long duration)
		{

		}

		public void accelerate(double factor)
		{

		}

		public void deccelerate(double factor)
		{

		}

		public void excite(double factor)
		{

		}

		public void calm(double factor)
		{

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
			if (System.Math.Abs(target - previousStrength) < 0.05)
			{
				isInterpolated = false;
				return target;
			}

			return target; // TODO
		}

		private void setFrequency(double freq)
		{
			settings.frequency = freq;
			settings.period = 1000 * freq;
		}

		private void setPeriod(double per)
		{
			settings.period = per;
			settings.frequency = per / 1000;
		}
	}
}
