using Buttplug.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	public static class Connection_Manager
	{
		public static async Task connectClientToServer(ButtplugClient client)
		{
			try
			{
				await client.ConnectAsync();
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
			}
			catch (Exception e)
			{
				Log_Manager.write(e.Message);
				Log_Manager.write(e.InnerException.Message);
				Log_Manager.write(e.InnerException.InnerException.Message);
			}
		}

		public static async Task connectDevicesToClient(ButtplugClient client)
		{
			Log_Manager.write("Scanning for new devices...");
			await client.StartScanningAsync();

			Log_Manager.write("Client currently knows about these devices:");
			foreach (var device in client.Devices)
			{
				Log_Manager.write($"- {device.Name}");
			}
		}

		private static void clientDeviceAdded(object sender, DeviceAddedEventArgs args)
		{
			Log_Manager.write($"Device ${args.Device.Name} connected");
		}
	}
}
