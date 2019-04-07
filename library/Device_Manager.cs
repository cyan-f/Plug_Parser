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
		private bool isOverwriting;

		private double overwriteStrength; // 0 - 100

		private Power_Calculator powerCalculator;
		

		private ButtplugWebsocketConnector connector;
		private ButtplugClient client;

		public Device_Manager()
		{
			receivedCommand = false;
			isUpdating = false;
			isRunning = true;
			isOverwriting = false;

			overwriteStrength = 0;

			powerCalculator = new Power_Calculator();

			Log_Manager.write("Created Manager");
		}

		public void setUpdating(bool u)
		{
			isUpdating = u;
		}

		public void setRunning(bool r)
		{
			isRunning = r;
		}

		public void setOverwriting(bool isO, double power)
		{
			isOverwriting = isO;
			overwriteStrength = power;
		}

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
						powerCalculator.decay();
					}

					await vibrate();
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

		// Buzzes device briefly for confirmation of connection.
		private void connectedBuzz()
		{
			buzz(1, 200);
		}

		/*
		 * Vibrates every toy connected to client.
		 * 
		 * power: vibration strength in range 0-1, where 1 is full power
		 * duration: amount of time to vibrate at given strength in milliseconds
		 *		If duration is set to 0, the duration is indefinite, until overwritten
		 *		
		 * Notes:
		 *		Lovense Hush accepts input from 0.00 to 1.00, but only changes strength in steps of 0.05.
		 *		So, it, in practice, only has 20 vibration levels.
		 *		ie. 0.00-0.04; 0.05-0.09; 0.10-0.14; ... 
		*/
		public void buzz(double power, int duration)
		{
			Parallel.ForEach(client.Devices, (toy) =>
			{
				toy.SendVibrateCmd(power).Wait();
				if (duration > 0)
				{
					Thread.Sleep(duration);
					toy.SendVibrateCmd(0).Wait();
				}
			});
		}


		public async Task vibrate()
		{
			double power = 0;

			if (isOverwriting)
			{
				power = overwriteStrength / 100;
			}
			else
			{
				power = getCurrentPower();
			}

			foreach (ButtplugClientDevice toy in client.Devices)
			{
				await toy.SendVibrateCmd(power);
			}
		}

		public double getCurrentPower()
		{
			if (isOverwriting)
			{
				return overwriteStrength / 100;
			}
			return 0; // TODO
		}

		public double getCurrentStrength()
		{
			if (isOverwriting)
			{
				return overwriteStrength;
			}
			return 0; // TODO ;
		}

		public void adjustRate(double f)
		{
			powerCalculator.adjustPeriodFactor(f);
		}

	}
}
