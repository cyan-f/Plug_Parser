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

		// Wave properties
		private static double wavePivot;
		private static double waveAmplitude;
		private static double wavePeriod; // milliseconds
		private static double periodFactor; // wavePeriod = wavePeriod / periodFactor

		public static double getWaveValue(remoteSettings terms)
		{
			// Angle in radians.
			double angle = (DateTime.Now.Millisecond + (DateTime.Now.Second * SECOND_IN_MILLIS)) 
				% (terms.frequency + (terms.frequency * terms.phaseShift)) 
				/ (wavePeriod / periodFactor) * RADIAN;

			double val = (Math.Sin(angle) * (waveAmplitude) + (wavePivot));

			return val;
		}

		private double getWaveMinimum()
		{
			double val = (Math.Sin(SIN_MIN_RADIAN) * waveAmplitude) + wavePivot;

			return val;
		}
	}
}
