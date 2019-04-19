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
		private long lastDecayTime;
		private long thisTime;
		private long deltaTime;

		// Stores last sent vibration strength for interpolation
		//	and low-quality chart updates.
		private double previousStrength = 0;

		private bool isInterpolated = false;

		public Vibe_Remote()
		{
			isOverriding = false;
			overrideStrength = 0;

			lastTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

			previousStrength = 0;

			// Initialize default settings
			settings = new Remote_Settings();
		}
		
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

			if ((thisTime - lastDecayTime) >= 1000)
			{
				decay();
			}

			previousStrength = val;
			return val;
		}

		#region Controls
		public void buzz(double strength)
		{
			
		}

		public void safeWord()
		{
			setOverride(0);
		}

		public void greenLight()
		{
			unsetOverride();
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


			lastDecayTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		}
		#endregion

		#region Helpers
		private double getPower()
		{
			return previousStrength / 100;
		}

		// TODO
		private double interpolate(double target)
		{
			return target;

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
