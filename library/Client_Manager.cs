﻿using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Plug_Parser_Plugin
{
	class Client_Manager
	{
		private const int DELAY_TIME = 50;
		private const int TIME_BETWEEN_DECAY = 1000; // milliseconds
		private const int MAX_DEVICES = 1;

		private bool receivedCommand = false;
		private bool isUpdating = false;
		private bool isRunning = false;
		private bool isScanning = false;

		// 0 = max quality
		private long chartQuality = 0;
		
		// TODO convert to lists/arrays of objects/structs/data
		private ButtplugWebsocketConnector connector;
		private ButtplugClient client;
		private Vibe_Remote remote;

		public Client_Manager()
		{
			receivedCommand = false;
			isUpdating = false;
			isRunning = true;
			chartQuality = 0;

			remote = new Vibe_Remote();

			Log_Manager.write("Created Manager");
		}

		// Controls
		public void queueAction(string action)
		{
			switch (action)
			{
				case C_Actions.YOU_HIT:
					//remote.buzz(40, 100);
					remote.spike(2.0, 200);
					break;

				case C_Actions.YOU_HEALED:
					break;

				case C_Actions.YOU_KILLED:
					//remote.accelerate(2.0);
					remote.buzz(90, 250);
					break;
				case C_Actions.YOU_KILLED_ENOUGH:
					remote.buzz(100, 500);
					break;
				default:
					remote.buzz(70, 100);
					break;
			}
		}

		#region Setters
		public void setUpdating(bool u)
		{
			isUpdating = u;
		}

		public void setRunning(bool r)
		{
			isRunning = r;
		}

		public void setOverriding(bool isOverriding, double strength)
		{
			if (isOverriding)
			{
				remote.setOverride(strength);
			}
			else
			{
				remote.unsetOverride();
			}
		}

		public void setChartQuality(long q)
		{
			chartQuality = q;
		}
		#endregion

		#region Getters
		public double getCurrentStrength()
		{
			if (chartQuality > 0)
			{
				return remote.getPreviousStrength();
			}
			return remote.updateStrength();
		}
		#endregion


		public void stopScanning()
		{
			client.StopScanningAsync();
			Log_Manager.write("Stopped scanning for new devices.");
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
				Log_Manager.write("Failed to connect to server.");
				return;
			}
			catch (Exception e)
			{
				Log_Manager.write(e.Message);
				Log_Manager.write("Failed to connect to server.");
				return;
			}
			Log_Manager.write(C_Messages.CLIENT_CONNECTED);

			await scanForPlugs();
		}

		public async Task scanForPlugs()
		{
			if (!isScanning)
			{
				if (client.Connected)
				{
					isScanning = true;
					await Connection_Manager.connectDevicesToClient(client);
					isScanning = false;
				}
				else
				{
					Log_Manager.write(C_Messages.DEVICE_SCAN_FAIL + C_Messages.CLIENT_NOT_CONNECTED);
				}
			}
			else
			{
				Log_Manager.write("Already scanning for new devices.");
			}
		}

		private void clientDeviceAdded(object sender, DeviceAddedEventArgs args)
		{
			Log_Manager.write($"Device ${args.Device.Name} connected");
			remote.buzz(50, 240);
		}

		public void begin()
		{
			if (!client.Connected)
			{
				return;
			}
			isUpdating = true;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			update();
#pragma warning restore CS4014
		}

		private async Task update()
		{
			while (isRunning)
			{
				if (isUpdating)
				{
					remote.updateStrength();

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

			await Connection_Manager.connectClientToServer(client);
			client.DeviceAdded += clientDeviceAdded;
		}

		public async Task disconnect()
		{
			isRunning = false;
			isUpdating = false;

			Log_Manager.write("Disconnecting...");
			try
			{
				await client.DisconnectAsync();
			}
			catch (Exception e)
			{
				Log_Manager.write(e.Message);
				Log_Manager.write(e.InnerException.Message);
				Log_Manager.write(e.InnerException.InnerException.Message);
				Log_Manager.write("Could not disconnect from server.");
				return;
			}

			Log_Manager.write("Disconnected.");
		}

	}
}