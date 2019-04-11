﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Advanced_Combat_Tracker;

namespace Plug_Parser_Plugin
{
	class Trigger_Manager
	{
		private const string CATEGORY = "PP";

		SortedList<string, CustomTrigger> listTriggers;

		public Trigger_Manager()
		{
			listTriggers = new SortedList<string, CustomTrigger>();

			listTriggers.Add("Triangulate", new CustomTrigger("Triangulate", (int)CustomTriggerSoundTypeEnum.None, "", false, "", true)
			{
				Active = true,
				Tabbed = true,
				Category = CATEGORY
			});

			updateActiveTriggers();
		}

		private void addSimpleTrigger(string regex, bool isTabbed = true)
		{
			bool alreadyExists = listTriggers.ContainsKey(regex);
			if (!alreadyExists)
			{
				listTriggers.Add(regex, new CustomTrigger(regex, (int)CustomTriggerSoundTypeEnum.None, "", false, "", true)
				{
					Active = true,
					Tabbed = isTabbed,
					Category = CATEGORY
				});
			}
			else
			{
				Log_Manager.write("Trigger " + regex + " already exists in listTriggers.");
			}
		}

		private void updateActiveTriggers()
		{
			foreach(KeyValuePair<string, CustomTrigger> pair in listTriggers)
			{
				CustomTrigger trigger = pair.Value;

				bool triggerExists = ActGlobals.oFormActMain.CustomTriggers.ContainsKey(trigger.Key);
				if (!triggerExists)
				{
					Log_Manager.write("Adding trigger, " + trigger.Key + ", to ACT.");
					ActGlobals.oFormActMain.AddEditCustomTrigger(trigger);
				}
				else
				{
					Log_Manager.write("Trigger, " + trigger.Key + ", already exists in ACT.");
				}
			}
		}

		public void check()
		{
			updateActiveTriggers();

			foreach (KeyValuePair<string, CustomTrigger> pair in listTriggers)
			{
				// TODO Figure out how to grab the matched lines of text from the log.
				CustomTrigger trigger = pair.Value;
				Log_Manager.write(trigger.LastAudioAlert.ToString() + ", " + trigger.ToString());
			}
		}
	}
}
