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

		public static double getWaveValue(remote_settings terms)
		{
			// Angle in radians.
			double angle = (DateTime.Now.Millisecond + (DateTime.Now.Second * SECOND_IN_MILLIS)) 
				% (terms.period + (terms.period * terms.phaseShift)) 
				/ (terms.period) * RADIAN;

			double val = ((Math.Sin(angle) * (terms.variance)) + (terms.baseStrength));

			return val;
		}

		private double getWaveMinimum(remote_settings terms)
		{
			double val = (Math.Sin(SIN_MIN_RADIAN) * terms.variance) + terms.baseStrength;

			return val;
		}
	}
}
