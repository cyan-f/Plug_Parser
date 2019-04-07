using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	class Power_Calculator
	{
		// Mathematical constants
		private const double RADIAN = 2 * Math.PI;
		private const double SIN_MIN_RADIAN = 1.5 * Math.PI;
		private const double SECOND_IN_MILLIS = 1000;

		// Defaults
		private const double DEFAULT_MIDDLE = 15;
		private const double DEFAULT_AMPLITUDE = 10;
		private const double DEFAULT_PERIOD = 6000;
		private const double DEFAULT_MAX_STRENGTH_WITH_BOOST = 90;

		// Wave properties
		private double wavePivot;
		private double waveAmplitude;
		private double wavePeriod; // milliseconds
		private double periodFactor; // wavePeriod = wavePeriod / periodFactor

		public Power_Calculator()
		{
			wavePivot = DEFAULT_MIDDLE;
			waveAmplitude = DEFAULT_AMPLITUDE;
			wavePeriod = DEFAULT_PERIOD;

			periodFactor = 1;
		}

		

		

		public void setPeriodFactor(double f)
		{
			periodFactor = f;
		}

		public void adjustPeriodFactor(double f)
		{
			periodFactor = periodFactor * f;
		}

		public void setBasePeriod(double newPeriod)
		{
			wavePeriod = newPeriod;
		}

		public void setPeriod()
		{
			wavePeriod = DEFAULT_PERIOD;
		}

		public double getWaveValue()
		{
			// Angle in radians.
			double angle = (((DateTime.Now.Millisecond + (DateTime.Now.Second * SECOND_IN_MILLIS)) % 
				(wavePeriod / periodFactor)) / (wavePeriod / periodFactor)) * RADIAN;

			double val = (Math.Sin(angle) * (waveAmplitude) + (wavePivot));

			return val;
		}

		public double getWaveMinimum()
		{
			double val = (Math.Sin(SIN_MIN_RADIAN) * waveAmplitude) + wavePivot;

			return val;
		}


		// Steadily brings pattern back to base.
		public void decay()
		{
			adjustPeriodFactor(0.99);
			Log_Manager.write("periodFactor: " + periodFactor);
		}
	}
}
