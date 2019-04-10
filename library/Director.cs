using System;
using System.IO;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	class Director
	{
		private Client_Manager manager = null;
		// UI Manager

		private double killCount;
		
		public Director()
		{

		}

		public void stopScanning()
		{
			manager.stopScanning();
		}

		public async Task begin()
		{
			Log_Manager.write("Beginning...");
			while(manager == null)
			{
				Log_Manager.write("No manager, creating...");
				manager = new Client_Manager();
				Log_Manager.write("Created.");
			}

			try
			{
				await manager.connect();
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

			manager.begin();
		}

		public async Task reconnect()
		{

			await manager.disconnect();

			try
			{
				await manager.connect();
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

			manager.begin();
		}

		public void setServerType(bool isEmbedded)
		{
			manager.setServerType(isEmbedded);
		}

		public void scanForPlugs()
		{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			manager.scanForPlugs();
#pragma warning restore CS4014
		}

		// Scuffed method to process actions/attacks.
		public async Task addAction(string attacker, string victim, string attackType, string damagetype, string swingType,
			string special, long damage, bool wasCrit)
		{
			Log_Manager.write("Attacker: " + attacker + ", Victim: " + victim + ", Attack Type:" + attackType + 
				", Damage Type: " + damagetype + ", Swing Type: " + swingType + ", Special: " + special);

			if (attacker == "YOU")
			{
				if (attackType == "Killing")
				{
					killCount++;
					if (killCount >= 4)
					{
						manager.queueAction(C_Actions.YOU_KILLED_ENOUGH);
						killCount = 0;
					}
					else
					{
						manager.queueAction(C_Actions.YOU_KILLED);
					}
				}
				else
				{
					manager.queueAction(C_Actions.YOU_HIT);
				}
			}
			else if (attackType == "Killing")
			{
				manager.queueAction(C_Actions.YOU_KILLED);
			}

		}

		public void queueVibeOverride(bool isOverriding, double strength)
		{
			manager.setOverriding(isOverriding, strength);
		}

		public void setChartQuality(long q)
		{
			manager.setChartQuality(q);
		}

		public double getCurrentStrength()
		{
			return manager.getCurrentStrength();
		}
	}
}