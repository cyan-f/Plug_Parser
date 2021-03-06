﻿using System;
using System.IO;
using System.Threading.Tasks;

using Advanced_Combat_Tracker;

namespace Plug_Parser_Plugin
{
	class Director
	{
		private Client_Manager manClient = null;
		private Trigger_Manager manTrigger = null;
		// UI Manager

		private double killCount = 0;
		private bool loggingCombatEvents = false;
		
		public Director()
		{
			manTrigger = new Trigger_Manager();
		}

		public void stopScanning()
		{
			manClient.stopScanning();
		}

		public async Task begin()
		{
			Log_Manager.write("Beginning...");
			while(manClient == null)
			{
				Log_Manager.write("No manager, creating...");
				manClient = new Client_Manager();
				Log_Manager.write("Created.");
			}

			try
			{
				await manClient.connect();
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
				Log_Manager.write("Failed to connect to server.");
				return;
			}
			catch (Exception e)
			{
				Log_Manager.write(e.Message);
				Log_Manager.write("Failed to connect to server.");
			}

			manClient.begin();
		}

		public async Task reconnect()
		{

			await manClient.disconnect();

			try
			{
				await manClient.connect();
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
				Log_Manager.write("Failed to connect to server.");
				return;
			}
			catch (Exception e)
			{
				Log_Manager.write(e.Message);
				Log_Manager.write("Failed to connect to server.");
			}

			manClient.begin();
		}

		public void setServerType(bool isEmbedded)
		{
			manClient.setServerType(isEmbedded);
		}

		public void scanForPlugs()
		{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			manClient.scanForPlugs();
#pragma warning restore CS4014
		}

		// Scuffed method to process actions/attacks.
		public async Task addAction(string attacker, string victim, string attackType, string damagetype, string swingType,
			string special, long damage, bool wasCrit)
		{
			if (loggingCombatEvents)
			{
				Log_Manager.write("Attacker: " + attacker + ", Victim: " + victim + ", Attack Type:" + attackType +
					", Damage Type: " + damagetype + ", Swing Type: " + swingType + ", Special: " + special);
			}

			if (attacker == "YOU")
			{
				if (attackType == "Killing")
				{
					killCount++;
					if (killCount >= PP_Settings.lengthOfKillstreak)
					{
						manClient.queueAction(C_Actions.YOU_KILLED_ENOUGH);
						killCount = 0;
					}
				}
				else
				{
					manClient.queueAction(C_Actions.YOU_HIT);
				}
			}
		}

		public void queueVibeOverride(bool isOverriding, double strength)
		{
			manClient.setOverriding(isOverriding, strength);
		}

		public void setChartQuality(long q)
		{
			manClient.setChartQuality(q);
		}

		public double getCurrentStrength()
		{
			return manClient.getCurrentStrength();
		}

		public void setLoggingCombatEvents(bool b)
		{
			loggingCombatEvents = b;
		}

		public void checkTrigger()
		{
			manTrigger.check();
		}

		public void parseLogLine(string line)
		{

			double freq = -1;
			if (PP_Settings.basingFrequencyOnBonusXP)
			{
				freq = manTrigger.parseLogLine(line);
			}

			if (freq != -1)
			{
				manClient.queueAction(C_Actions.YOU_KILLED, freq);
			}
		}

		public void safeword()
		{
			manClient.safeword();
		}

		public void greenLight()
		{
			manClient.greenLight();
		}
	}
}