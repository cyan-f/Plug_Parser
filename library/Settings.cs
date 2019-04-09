using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	public class Settings
	{
		public Settings()
		{

		}
		public class Defaults
		{
			public class Generic
			{
				public const double MINIMUM_STRENGTH = 0;
				public const double MAXIMUM_STRENGTH = 100;
				public const double BUZZ_STRENGTH = 0;
				public const double BASE_STRENGTH = 15;
				public const double ERRATICNESS = 0;
				public const double VARIANCE = 10;
				public const double FREQUENCY = 1;
				public const double PERIOD = 1000 * (1 / FREQUENCY);
				public const double PHASE_SHIFT = 0;
				public const double SPIKE_AMOUNT = 0;
				public const double SPIKE_DURATION = 0;
			}
			public class Hush
			{

				public const double MINIMUM_STRENGTH = 0;
				public const double MAXIMUM_STRENGTH = 100;
				public const double BUZZ_STRENGTH = 0;
				public const double BASE_STRENGTH = 15;
				public const double ERRATICNESS = 0;
				public const double VARIANCE = 10;
				public const double FREQUENCY = 1;
				public const double PERIOD = 1000 * (1 / FREQUENCY);
				public const double PHASE_SHIFT = 0;
				public const double SPIKE_AMOUNT = 0;
				public const double SPIKE_DURATION = 0;

			}
		}
	}
}
