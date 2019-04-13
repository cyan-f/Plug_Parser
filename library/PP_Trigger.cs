using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	class PP_Trigger
	{
		public string key { get => key; set => key = value; }
		public CustomTrigger trigger { get => trigger; set => trigger = value; }
		public List<string> responses { get => responses; set => responses = value; }

		public PP_Trigger(string key, CustomTrigger trigger, string response, double intensity)
		{
			this.key = key;
			this.trigger = trigger;
			responses.Add(response);
		}
	}
}
