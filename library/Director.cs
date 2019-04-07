using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	class Director
	{
		private Device_Manager manager = null;
		
		public Director()
		{

		}

		public void pressedAnyKey()
		{
			manager.receiveCommand();
		}

		public void begin()
		{
			Log_Manager.write("Beginning...");
			while(manager == null)
			{
				Log_Manager.write("No manager, creating...");
				manager = new Device_Manager();
				Log_Manager.write("Created.");
			}
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			manager.connect();
#pragma warning restore CS4014

			manager.begin();
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
			Log_Manager.write(attacker + ", " + victim + ", " + attackType + ", " + damagetype + ", " + swingType + ", " + special);

			if ((attacker == "YOU") || (attacker == "Unknown"))
			{
				// TODO parse this, idk

				await Task.Delay(1000);
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