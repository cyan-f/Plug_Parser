using Buttplug.Client;
using System;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	public struct remoteSettings
	{
		public double minimumStrength;
		public double maximumStrength;

		public double buzzStrength;
		public double baseStrength;

		public double erraticness;
		public double variance;
		
		public double frequency;
		public double phaseShift;

		public remoteSettings(double minimumStrength, double maximumStrength, 
			double buzzStrength, double baseStrength, 
			double erraticness, double variance, 
			double frequency, double phaseShift)
		{
			this.minimumStrength = minimumStrength;
			this.maximumStrength = maximumStrength;
			this.buzzStrength = buzzStrength;
			this.baseStrength = baseStrength;
			this.erraticness = erraticness;
			this.variance = variance;
			this.frequency = frequency;
			this.phaseShift = phaseShift;
		}
	}

	class Vibe_Remote
	{
		private remoteSettings settings;

		#region Default values

		#endregion

		private bool isOverriding;
		private double overrideStrength;

		// Stores last sent vibration strength for interpolation
		private double previousStrength;
		private bool isInterpolated;

		public Vibe_Remote()
		{
			isOverriding = false;
			overrideStrength = 0;

			// Initialize default settings
		}

		// Controls
		public void buzz(ButtplugClientDevice toy, double power, double duration)
		{
			// TODO
		}

		// Actions
		public async Task vibe(ButtplugClientDevice toy)
		{
			await toy.SendVibrateCmd(getCurrentPower());
		}

		public double getCurrentPower()
		{
			if (isOverriding)
			{
				return overrideStrength;
			}
			else if (isInterpolated)
			{
				interpolate(0); // TODO
			}
			
			return 0; // TODO
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
