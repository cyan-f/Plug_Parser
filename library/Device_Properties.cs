using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	public static class Device_Properties
	{
		private const string DEFAULT_DEVICE_NAME = "Hush";

		public static int getAccuracy(string deviceName = DEFAULT_DEVICE_NAME)
		{
			return 5;
		}
	}
}
