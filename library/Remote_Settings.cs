using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	class Remote_Settings
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

		// Default, generic ctor
		public Remote_Settings()
		{
			this.minimumStrength = Settings.Defaults.Generic.MINIMUM_STRENGTH;
			this.maximumStrength = Settings.Defaults.Generic.MAXIMUM_STRENGTH;
			this.buzzStrength = Settings.Defaults.Generic.BUZZ_STRENGTH;
			this.baseStrength = Settings.Defaults.Generic.BASE_STRENGTH;
			this.erraticness = Settings.Defaults.Generic.ERRATICNESS;
			this.variance = Settings.Defaults.Generic.VARIANCE;
			this.period = Settings.Defaults.Generic.PERIOD;
			this.frequency = Settings.Defaults.Generic.FREQUENCY;
			this.phaseShift = Settings.Defaults.Generic.PHASE_SHIFT;
		}

		// Complete ctor
		public Remote_Settings(double minimumStrength, double maximumStrength,
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

		public void addAmplitude(double add)
		{
			variance += add;

			if (variance > 50)
			{
				variance = 50;
			}

			if (variance < 0)
			{
				variance = 0;
			}
		}

		public void addBuzz(double add)
		{
			buzzStrength += add;

			if (buzzStrength > 50)
			{
				buzzStrength = 50;
			}

			if (buzzStrength < 0)
			{
				buzzStrength = 0;
			}
		}

		public void addFrequency(double add)
		{
			frequency += add;
		}
	}
}
