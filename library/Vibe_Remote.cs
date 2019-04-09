using Buttplug.Client;
using System;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	class Vibe_Remote
	{
		private Remote_Settings settings;

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
			settings = new Remote_Settings();
		}

		#region Getters
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
		#endregion

		#region Controls
		// Flat buzz
		public void buzz(double strength, long duration)
		{
			if (strength > overrideStrength)
			{
				setOverride(strength, duration);
			}
		}

		// Stops vibration for duration (milliseconds)
		public void pause(long duration)
		{
			//setOverride(0, duration);
		}

		/*
		 * Multiplies strength by factor for duration.
		 * 
		 * factor - small decimal value, typically between 1.0~2.0
		 * duration - time in milliseconds
		 */
		public void spike(double factor, double duration)
		{
			settings.spikeAmount = factor;
			settings.spikeTimeLeft = duration;
		}

		public void accelerate(double factor)
		{
			double newFrequency = settings.frequency * factor;
			setFrequency(newFrequency);

			//Log_Manager.write("NF: " + newFrequency);
		}

		public void excite(double factor)
		{

		}

		// Increases baseStrength by given amount.
		public void bump(double amount)
		{
			settings.baseStrength += amount;
		}
		#endregion

		#region Meta Controls
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
		#endregion

		#region Tasks
		public async Task vibe(ButtplugClientDevice toy)
		{
			await toy.SendVibrateCmd(getPower());
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
		#endregion

		#region Helpers
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
		#endregion

		
	}
}
