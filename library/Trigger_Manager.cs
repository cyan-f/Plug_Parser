using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Advanced_Combat_Tracker;

namespace Plug_Parser_Plugin
{
	class Trigger_Manager
	{
		List<CustomTrigger> listTriggers;

		public Trigger_Manager()
		{
			listTriggers = new List<CustomTrigger>();

			listTriggers.Add(new CustomTrigger("Triangulate", (int)CustomTriggerSoundTypeEnum.None, "", false, "", true)
			{
				Active = true,
				Tabbed = true
			});

			updateActiveTriggers();
		}

		private void addSimpleTrigger(string regex)
		{

		}

		private void updateActiveTriggers()
		{
			foreach(CustomTrigger trigger in listTriggers)
			{
				bool triggerExists = ActGlobals.oFormActMain.CustomTriggers.ContainsKey(trigger.Key);
				if (!triggerExists)
				{
					Log_Manager.write("Adding trigger: " + trigger.Key);
					ActGlobals.oFormActMain.AddEditCustomTrigger(trigger);
				}
			}
		}

		public void check()
		{
			updateActiveTriggers();

			foreach (CustomTrigger trigger in listTriggers)
			{
				Log_Manager.write(trigger.LastAudioAlert.ToString() + ", " + trigger.ToString());
			}
		}
	}
}
