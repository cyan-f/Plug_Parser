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
		}

		// Getters
		public double getStrength()
		{
			return previousStrength;
		}

		// Controls
		public void buzz(double strength, long duration)
		{
			setOverride(strength, duration);
		}

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


		// Actions
		public async Task vibe(ButtplugClientDevice toy)
		{
			await toy.SendVibrateCmd(getCurrentPower());
		}

		// Private helpers
		private double getCurrentPower()
		{
			double val = 0;

			if (isOverriding)
			{
				val = overrideStrength;

				long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
				if ((currentTime - overrideStartTime) >= overrideDuration)
				{
					unsetOverride();
				}
			}
			else if (isInterpolated)
			{
				val = interpolate(Power_Calculator.getWaveValue(settings)); // TODO
			}

			previousStrength = val;
			return val;
		}

		private double interpolate(double target)
		{
			if (System.Math.Abs(target - previousStrength) < 0.05)
			{
				isInterpolated = false;
				return target;
			}

			return 0; // TODO
		}
	}
}
