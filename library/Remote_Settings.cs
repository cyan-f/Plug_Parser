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
		public double buzzRemaining;
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
			this.buzzRemaining = 0;
			this.baseStrength = Settings.Defaults.Generic.BASE_STRENGTH;
			this.erraticness = Settings.Defaults.Generic.ERRATICNESS;
			this.variance = Settings.Defaults.Generic.VARIANCE;
			this.period = Settings.Defaults.Generic.PERIOD;
			this.frequency = Settings.Defaults.Generic.FREQUENCY;
			this.phaseShift = Settings.Defaults.Generic.PHASE_SHIFT;
		}

		// Complete ctor
		public Remote_Settings(double minimumStrength, double maximumStrength,
			double buzzStrength, double buzzRemaining,
			double baseStrength,
			double erraticness, double variance,
			double frequency, double period, double phaseShift)
		{
			this.minimumStrength = minimumStrength;
			this.maximumStrength = maximumStrength;
			this.buzzStrength = buzzStrength;
			this.buzzRemaining = buzzRemaining;
			this.baseStrength = baseStrength;
			this.erraticness = erraticness;
			this.variance = variance;
			this.period = period;
			this.frequency = frequency;
			this.phaseShift = phaseShift;
		}

		// TODO separate into amplitude and base methods.
		public void addAmplitude(double add)
		{
			baseStrength += add;
			if (baseStrength <= 5)
			{
				variance = 0;
				return;
			}

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

		public void addBuzz(double add, double timeMillis)
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

			buzzRemaining += timeMillis;
		}

		public void addFrequency(double add)
		{
			frequency += add;
			period = 1000 * (1 / frequency);
		}
	}
}
