using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Advanced_Combat_Tracker;

namespace Plug_Parser_Plugin
{
	class Trigger_Manager
	{
		private const string CATEGORY = "PP_Triggers";

		// Regex for detecting added added amount of experience points.
		// '@' makes string ignore escape characters
		private const string RX_BONUS_EXPERIENCE = @"\(\+\d+\d%\) experience points";
		private const string RX_INTEGER = @"\d+\d";


		SortedList<string, CustomTrigger> ppTriggers;

		public Trigger_Manager()
		{
			ppTriggers = new SortedList<string, CustomTrigger>();

			ppTriggers.Add("Triangulate", new CustomTrigger("Triangulate", 
				(int)CustomTriggerSoundTypeEnum.None, "", false, "", true)
			{
				Active = true,
				Tabbed = true,
				Category = CATEGORY
			});

			ppTriggers.Add("Bonus Experience", new CustomTrigger(RX_BONUS_EXPERIENCE,
				(int)CustomTriggerSoundTypeEnum.None, "", false, "", true)
			{
				Active = true,
				Tabbed = true,
				Category = CATEGORY
			});

			updateActiveTriggers();
		}

		private void addSimpleTrigger(string regex, bool isTabbed = true)
		{
			bool alreadyExists = ppTriggers.ContainsKey(regex);
			if (!alreadyExists)
			{
				ppTriggers.Add(regex, new CustomTrigger(regex, (int)CustomTriggerSoundTypeEnum.None, "", false, "", true)
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
			foreach(KeyValuePair<string, CustomTrigger> pair in ppTriggers)
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

			foreach (KeyValuePair<string, CustomTrigger> pair in ppTriggers)
			{
				// TODO Figure out how to grab the matched lines of text from the log.
				CustomTrigger trigger = pair.Value;
				//trigger.ResultsTab.Controls.

				Log_Manager.write(trigger.LastAudioAlert.ToString() + ", " + trigger.ToString());
			}
		}

		public double parseLogLine(string line)
		{
			Regex rxBonusExp = new Regex(RX_BONUS_EXPERIENCE);
			Regex rxInteger = new Regex(RX_INTEGER);
			MatchCollection matches = rxBonusExp.Matches(line);

			foreach (Match match in matches)
			{
				int value = Int32.Parse(Regex.Match(match.Value, RX_INTEGER).Value);
				Log_Manager.write("You received " + value + "% bonus experience.");

				if (value > 100)
				{
					return (double)value / 100;
				}
				else
				{
					return (double)100;
				}
			}

			return -1;
		}
	}
}
