using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	class Device_Manager
	{
		private const int DELAY_TIME = 10;
		private const int TIME_BETWEEN_DECAY = 1000; // milliseconds

		private bool receivedCommand;
		private bool isUpdating;
		private bool isRunning;

		private Power_Calculator powerCalculator;
		
		// TODO convert to lists/arrays of objects
		private ButtplugWebsocketConnector connector;
		private ButtplugClient client;
		private Vibe_Remote remote;

		public Device_Manager()
		{
			receivedCommand = false;
			isUpdating = false;
			isRunning = true;

			remote = new Vibe_Remote();

			Log_Manager.write("Created Manager");
		}

		// Controls
		#region Setters
		public void setUpdating(bool u)
		{
			isUpdating = u;
		}

		public void setRunning(bool r)
		{
			isRunning = r;
		}

		public void setOverriding(bool isO, double power)
		{
			// TODO
		}
		#endregion

		#region Getters
		public double getCurrentStrength()
		{
			return 0; // TODO
		}
		#endregion


		public void receiveCommand()
		{
			receivedCommand = true;
		}

		private async Task waitForCommand()
		{
			//Log_Manager.write("Press \"Stop Scanning\" after your device has been found...");
			Log_Manager.write("Scanning will stop after first device is connected...");
			while (!receivedCommand)
			{
				await Task.Delay(1);
			}
			receivedCommand = false;
		}

		public async Task connect()
		{
			Log_Manager.write("Connecting to server...");
			try
			{
				await connectServer();
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
			}
			Log_Manager.write("Connected!");

			await scanForPlugs();
		}

		public void begin()
		{
			isUpdating = true;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			update();
#pragma warning restore CS4014
		}

		private async Task update()
		{
			long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			long newTime;
			while (isRunning)
			{
				if (isUpdating)
				{
					newTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
					if ((newTime - time) > TIME_BETWEEN_DECAY)
					{
						time = newTime;
						// decay
					}

					foreach (ButtplugClientDevice toy in client.Devices)
					{
						await remote.vibe(toy);
					}
				}
				await Task.Delay(DELAY_TIME);
			}
		}

		private async Task connectServer()
		{
			try
			{
				connector = new ButtplugWebsocketConnector(new Uri("ws://localhost:12345/buttplug"));
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
			}
			client = new ButtplugClient("Main Client", connector);

			client.DeviceAdded += clientDeviceAdded;
			
			try
			{
				await client.ConnectAsync();
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
			}
		}

		private void clientDeviceAdded(object sender, DeviceAddedEventArgs args)
		{
			Log_Manager.write($"Device ${args.Device.Name} connected");
			remote.buzz(args.Device, 1, 200);

			// TEMP for
			receivedCommand = true;
		}

		public async Task scanForPlugs()
		{
			client.ScanningFinished += (aObj, aScanningFinishedArgs) =>
				Log_Manager.write("Device scanning is finished!");

			if (client.Devices.Length > 0)
			{
				Log_Manager.write("Client currently knows about these devices:");
				foreach (var device in client.Devices)
				{
					Log_Manager.write($"- {device.Name}");
				}
				return;
			}

			Log_Manager.write("Scanning for new plugs...");
			await client.StartScanningAsync();
			await waitForCommand();
			Log_Manager.write("Finishing Scanning...");
			await client.StopScanningAsync();

			if (client.Devices.Length > 0)
			{
				Log_Manager.write("Client currently knows about these devices:");
				foreach (var device in client.Devices)
				{
					Log_Manager.write($"- {device.Name}");
				}
			}
			else
			{
				Log_Manager.write("Client knows about no devices.");
			}
		}

	}
}
