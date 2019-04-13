using Buttplug.Client;
using Buttplug.Core.Logging;
using Buttplug.Client.Connectors.WebsocketConnector;
using Buttplug.Server.Managers.UWPBluetoothManager;
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
		
		private bool isUpdating = false;
		private bool isRunning = false;
		private bool isScanning = false;

		// 0 = max quality
		private long chartQuality = 0;
		
		// TODO convert to lists/arrays of objects/structs/data
		private ButtplugWebsocketConnector webConnector;
		private ButtplugEmbeddedConnector embeddedConnector;
		private ButtplugClient client;
		private Vibe_Remote remote;

		// TODO set this in ctor.
		private bool usingEmbeddedServer = true;

		public Client_Manager()
		{
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
					remote.buzz(40);
					break;

				case C_Actions.YOU_HEALED:
					break;

				case C_Actions.YOU_KILLED:
					remote.buzz(90);
					break;
				case C_Actions.YOU_KILLED_ENOUGH:
					remote.buzz(100);
					break;
				default:
					remote.buzz(70);
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
			remote.buzz(50);
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
			if (usingEmbeddedServer)
			{
				await connectEmbeddedServer();
			}
			else
			{
				await connectRemoteServer();
			}
		}

		private async Task connectEmbeddedServer()
		{
			embeddedConnector = new ButtplugEmbeddedConnector("Buttplugin Server");
			embeddedConnector.Server.AddDeviceSubtypeManager((IButtplugLogManager aMgr)
				=> { return new UWPBluetoothManager(aMgr); });

			client = new ButtplugClient("Buttplugin Client", embeddedConnector);

			await Connection_Manager.connectClientToServer(client);
			client.DeviceAdded += clientDeviceAdded;
		}

		private async Task connectRemoteServer()
		{
			try
			{
				webConnector = new ButtplugWebsocketConnector(new Uri("ws://localhost:12345/buttplug"));
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
			}
			client = new ButtplugClient("Main Client", webConnector);

			await Connection_Manager.connectClientToServer(client);
			client.DeviceAdded += clientDeviceAdded;
		}


		public void setServerType(bool isEmbedded)
		{
			usingEmbeddedServer = isEmbedded;
			if (usingEmbeddedServer)
			{
				Log_Manager.write("Set to use embedded server. Reconnect to apply changes.");
			}
			else
			{
				Log_Manager.write("Set to use remote server. Reconnect to apply changes.");
			}
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
