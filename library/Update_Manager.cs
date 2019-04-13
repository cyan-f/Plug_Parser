using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	class Update_Manager
	{
		private bool updating = false;
		private int updateDelay = 4;

		private UI_Manager manUI = null;

		public Update_Manager(UI_Manager mUI)
		{
			manUI = mUI;
		}

		private async Task update()
		{
			while (updating)
			{
				// Get UI Input


				// Get Game Events


				// Update Power


				// Vibrate


				// Update UI
				manUI.update();


				await Task.Delay(updateDelay);
			}
		}

		public void startUpdating()
		{
			updating = true;
			update();
		}

		public void stopUpdating()
		{
			updating = false;
		}
	}
}
