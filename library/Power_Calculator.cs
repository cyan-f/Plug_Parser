using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	static class Power_Calculator
	{
		// Mathematical constants
		private const double RADIAN = 2 * Math.PI;
		private const double SIN_MIN_RADIAN = 1.5 * Math.PI;
		private const double SECOND_IN_MILLIS = 1000;

		public static double getStrength(Remote_Settings terms)
		{
			double val = getWaveValue(terms);

			if (terms.spikeTimeLeft > 0)
			{
				val = applySpike(terms, val);
			}

			return val;
		}

		private static double applySpike(Remote_Settings terms, double val)
		{
			if (val >= terms.baseStrength)
			{
				val *= terms.spikeAmount;
			}

			return val;
		}

		private static double getWaveValue(Remote_Settings terms)
		{
			// Angle in radians.
			double angle = (DateTime.Now.Millisecond + (DateTime.Now.Second * SECOND_IN_MILLIS)) 
				% (terms.period + (terms.period * terms.phaseShift)) 
				/ (terms.period) * RADIAN;

			double val = ((Math.Sin(angle) * (terms.variance)) + (terms.baseStrength));

			return val;
		}

		private static double getWaveMinimum(Remote_Settings terms)
		{
			double val = (Math.Sin(SIN_MIN_RADIAN) * terms.variance) + terms.baseStrength;

			return val;
		}
	}
}
