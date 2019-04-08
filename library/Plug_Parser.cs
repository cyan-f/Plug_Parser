using Advanced_Combat_Tracker;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

/*
 * File uses example code from Advanced Combat Tracker website.
 */

namespace Plug_Parser_Plugin
{
	public class Plug_Parser : UserControl, IActPluginV1
	{
		public const bool ALLOWING_UNSAFE_UI_EDITS = false;
		private delegate void SafeCallDelegate(Stopwatch s);
		private long chartQuality; // 0 = max
		private bool changedChartQuality = false;

		// 0 = no update
		// 1 = update trackbar
		// 2 = update updownbox
		private long updateOverrideUI = 0;

		Director director;

		private Button buttonRescan;
		private TextBox textBox1;
		private Button buttonStopScanning;
		private RichTextBox logEvents;
		private NumericUpDown numericupdownOverrideValue;
		private TrackBar sliderVibeOverride;
		private Button buttonClearLog;
		private System.Windows.Forms.DataVisualization.Charting.Chart chartVibeStrength;
		private Label EVENT_LOG_LABEL;
		private Label CHART_LABEL;
		private CheckBox checkboxOverride;
		private Label label1;
		private Button buttonStart;
		private CheckBox checkboxChartQuality;

		#region Designer Created Code (Avoid editing)
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.EVENT_LOG_LABEL = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.buttonRescan = new System.Windows.Forms.Button();
			this.buttonStopScanning = new System.Windows.Forms.Button();
			this.logEvents = new System.Windows.Forms.RichTextBox();
			this.numericupdownOverrideValue = new System.Windows.Forms.NumericUpDown();
			this.sliderVibeOverride = new System.Windows.Forms.TrackBar();
			this.buttonClearLog = new System.Windows.Forms.Button();
			this.chartVibeStrength = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.CHART_LABEL = new System.Windows.Forms.Label();
			this.checkboxOverride = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonStart = new System.Windows.Forms.Button();
			this.checkboxChartQuality = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownOverrideValue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sliderVibeOverride)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartVibeStrength)).BeginInit();
			this.SuspendLayout();
			// 
			// EVENT_LOG_LABEL
			// 
			this.EVENT_LOG_LABEL.AutoSize = true;
			this.EVENT_LOG_LABEL.BackColor = System.Drawing.Color.Transparent;
			this.EVENT_LOG_LABEL.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EVENT_LOG_LABEL.Location = new System.Drawing.Point(4, 2);
			this.EVENT_LOG_LABEL.Name = "EVENT_LOG_LABEL";
			this.EVENT_LOG_LABEL.Size = new System.Drawing.Size(69, 17);
			this.EVENT_LOG_LABEL.TabIndex = 9;
			this.EVENT_LOG_LABEL.Text = "Event Log";
			this.EVENT_LOG_LABEL.Click += new System.EventHandler(this.label1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(4, 629);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(431, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.TabStop = false;
			this.textBox1.Text = "Sample TextBox that has its value stored to the settings file automatically.";
			this.textBox1.Visible = false;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// buttonRescan
			// 
			this.buttonRescan.AccessibleDescription = "Rescans for devices.";
			this.buttonRescan.AccessibleName = "Rescan";
			this.buttonRescan.Location = new System.Drawing.Point(517, 56);
			this.buttonRescan.Name = "buttonRescan";
			this.buttonRescan.Size = new System.Drawing.Size(114, 32);
			this.buttonRescan.TabIndex = 2;
			this.buttonRescan.Text = "Rescan";
			this.buttonRescan.UseVisualStyleBackColor = true;
			this.buttonRescan.Click += new System.EventHandler(this.buttonRescan_Click);
			// 
			// buttonStopScanning
			// 
			this.buttonStopScanning.Location = new System.Drawing.Point(517, 92);
			this.buttonStopScanning.Name = "buttonStopScanning";
			this.buttonStopScanning.Size = new System.Drawing.Size(114, 32);
			this.buttonStopScanning.TabIndex = 4;
			this.buttonStopScanning.Text = "Stop Scanning";
			this.buttonStopScanning.UseVisualStyleBackColor = true;
			this.buttonStopScanning.Click += new System.EventHandler(this.buttonStopScanning_Click);
			// 
			// logEvents
			// 
			this.logEvents.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.logEvents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.logEvents.DetectUrls = false;
			this.logEvents.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.logEvents.Location = new System.Drawing.Point(4, 20);
			this.logEvents.Margin = new System.Windows.Forms.Padding(4);
			this.logEvents.Name = "logEvents";
			this.logEvents.ReadOnly = true;
			this.logEvents.Size = new System.Drawing.Size(512, 252);
			this.logEvents.TabIndex = 5;
			this.logEvents.TabStop = false;
			this.logEvents.Text = "...\n1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12\n13\n14\n16\n17\n18\n19";
			this.logEvents.WordWrap = false;
			this.logEvents.TextChanged += new System.EventHandler(this.logEvents_TextChanged);
			// 
			// numericupdownOverrideValue
			// 
			this.numericupdownOverrideValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericupdownOverrideValue.Location = new System.Drawing.Point(585, 626);
			this.numericupdownOverrideValue.Name = "numericupdownOverrideValue";
			this.numericupdownOverrideValue.Size = new System.Drawing.Size(51, 20);
			this.numericupdownOverrideValue.TabIndex = 8;
			this.numericupdownOverrideValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericupdownOverrideValue.ValueChanged += new System.EventHandler(this.numericupdownOverrideValue_ValueChanged);
			// 
			// sliderVibeOverride
			// 
			this.sliderVibeOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.sliderVibeOverride.AutoSize = false;
			this.sliderVibeOverride.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.sliderVibeOverride.LargeChange = 10;
			this.sliderVibeOverride.Location = new System.Drawing.Point(600, 297);
			this.sliderVibeOverride.Maximum = 100;
			this.sliderVibeOverride.Name = "sliderVibeOverride";
			this.sliderVibeOverride.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.sliderVibeOverride.Size = new System.Drawing.Size(36, 323);
			this.sliderVibeOverride.SmallChange = 5;
			this.sliderVibeOverride.TabIndex = 11;
			this.sliderVibeOverride.TickFrequency = 5;
			this.sliderVibeOverride.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.sliderVibeOverride.Scroll += new System.EventHandler(this.sliderVibeOverride_Scroll);
			// 
			// buttonClearLog
			// 
			this.buttonClearLog.BackColor = System.Drawing.Color.Transparent;
			this.buttonClearLog.FlatAppearance.BorderSize = 0;
			this.buttonClearLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonClearLog.Location = new System.Drawing.Point(79, 0);
			this.buttonClearLog.Name = "buttonClearLog";
			this.buttonClearLog.Size = new System.Drawing.Size(60, 20);
			this.buttonClearLog.TabIndex = 12;
			this.buttonClearLog.Text = "Clear";
			this.buttonClearLog.UseVisualStyleBackColor = false;
			this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
			// 
			// chartVibeStrength
			// 
			this.chartVibeStrength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chartVibeStrength.BackColor = System.Drawing.Color.Transparent;
			this.chartVibeStrength.BorderlineColor = System.Drawing.Color.Transparent;
			chartArea1.AxisX.LabelStyle.Enabled = false;
			chartArea1.AxisX.MajorGrid.Enabled = false;
			chartArea1.AxisX.MajorTickMark.Enabled = false;
			chartArea1.AxisX.ScrollBar.BackColor = System.Drawing.Color.Black;
			chartArea1.AxisY.LabelStyle.Enabled = false;
			chartArea1.AxisY.MajorGrid.Enabled = false;
			chartArea1.AxisY.MajorTickMark.Enabled = false;
			chartArea1.AxisY.Maximum = 100D;
			chartArea1.AxisY.Minimum = 0D;
			chartArea1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
			chartArea1.CursorX.AutoScroll = false;
			chartArea1.Name = "areaMain";
			chartArea1.Position.Auto = false;
			chartArea1.Position.Height = 100F;
			chartArea1.Position.Width = 100F;
			this.chartVibeStrength.ChartAreas.Add(chartArea1);
			this.chartVibeStrength.Cursor = System.Windows.Forms.Cursors.Default;
			legend1.Enabled = false;
			legend1.Name = "Legend1";
			this.chartVibeStrength.Legends.Add(legend1);
			this.chartVibeStrength.Location = new System.Drawing.Point(4, 309);
			this.chartVibeStrength.Margin = new System.Windows.Forms.Padding(0);
			this.chartVibeStrength.MinimumSize = new System.Drawing.Size(594, 252);
			this.chartVibeStrength.Name = "chartVibeStrength";
			series1.BackImageTransparentColor = System.Drawing.Color.DarkCyan;
			series1.BackSecondaryColor = System.Drawing.Color.DarkCyan;
			series1.BorderColor = System.Drawing.Color.DarkCyan;
			series1.BorderWidth = 3;
			series1.ChartArea = "areaMain";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
			series1.Color = System.Drawing.Color.Cyan;
			series1.LabelForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			series1.Legend = "Legend1";
			series1.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			series1.MarkerBorderWidth = 1100;
			series1.MarkerSize = 10;
			series1.Name = "seriesMain";
			this.chartVibeStrength.Series.Add(series1);
			this.chartVibeStrength.Size = new System.Drawing.Size(594, 300);
			this.chartVibeStrength.TabIndex = 13;
			this.chartVibeStrength.Click += new System.EventHandler(this.chartVibeStrength_Click);
			// 
			// CHART_LABEL
			// 
			this.CHART_LABEL.AutoSize = true;
			this.CHART_LABEL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CHART_LABEL.Location = new System.Drawing.Point(3, 285);
			this.CHART_LABEL.Name = "CHART_LABEL";
			this.CHART_LABEL.Size = new System.Drawing.Size(90, 20);
			this.CHART_LABEL.TabIndex = 14;
			this.CHART_LABEL.Text = "Vibrations";
			// 
			// checkboxOverride
			// 
			this.checkboxOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkboxOverride.AutoSize = true;
			this.checkboxOverride.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkboxOverride.Location = new System.Drawing.Point(513, 629);
			this.checkboxOverride.Name = "checkboxOverride";
			this.checkboxOverride.Size = new System.Drawing.Size(66, 17);
			this.checkboxOverride.TabIndex = 15;
			this.checkboxOverride.Text = "Override";
			this.checkboxOverride.UseVisualStyleBackColor = true;
			this.checkboxOverride.CheckedChanged += new System.EventHandler(this.checkboxOverride_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(145, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(223, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "NOTE: ONLY SOLO DEVICES SUPPORTED";
			// 
			// buttonStart
			// 
			this.buttonStart.Location = new System.Drawing.Point(517, 20);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(114, 32);
			this.buttonStart.TabIndex = 17;
			this.buttonStart.Text = "Start";
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// checkboxChartQuality
			// 
			this.checkboxChartQuality.AutoSize = true;
			this.checkboxChartQuality.Checked = true;
			this.checkboxChartQuality.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkboxChartQuality.Location = new System.Drawing.Point(100, 288);
			this.checkboxChartQuality.Name = "checkboxChartQuality";
			this.checkboxChartQuality.Size = new System.Drawing.Size(58, 17);
			this.checkboxChartQuality.TabIndex = 18;
			this.checkboxChartQuality.Text = "Quality";
			this.checkboxChartQuality.UseVisualStyleBackColor = true;
			this.checkboxChartQuality.CheckedChanged += new System.EventHandler(this.checkboxChartQuality_CheckedChanged);
			// 
			// Plug_Parser
			// 
			this.AccessibleDescription = "";
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.checkboxChartQuality);
			this.Controls.Add(this.buttonStart);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkboxOverride);
			this.Controls.Add(this.CHART_LABEL);
			this.Controls.Add(this.chartVibeStrength);
			this.Controls.Add(this.buttonClearLog);
			this.Controls.Add(this.sliderVibeOverride);
			this.Controls.Add(this.EVENT_LOG_LABEL);
			this.Controls.Add(this.numericupdownOverrideValue);
			this.Controls.Add(this.logEvents);
			this.Controls.Add(this.buttonStopScanning);
			this.Controls.Add(this.buttonRescan);
			this.Controls.Add(this.textBox1);
			this.MinimumSize = new System.Drawing.Size(640, 655);
			this.Name = "Plug_Parser";
			this.Size = new System.Drawing.Size(640, 655);
			((System.ComponentModel.ISupportInitialize)(this.numericupdownOverrideValue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sliderVibeOverride)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartVibeStrength)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		#endregion

		public Plug_Parser()
		{
			try
			{
				InitializeComponent();
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
			}

			Control.CheckForIllegalCrossThreadCalls = true;
		}

		Label lblStatus;    // The status label that appears in ACT's Plugin tab
		readonly string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\PluginSample.config.xml");
		SettingsSerializer xmlSettings;

		#region IActPluginV1 Members
		// Bog standard plugin stuff I don't want/have to think about too much.


		public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
		{
			lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
			pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
			this.Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
			xmlSettings = new SettingsSerializer(this); // Create a new settings serializer and pass it this instance
			loadSettings();

			// Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.
			ActGlobals.oFormActMain.AfterCombatAction += new CombatActionDelegate(oFormActMain_AfterCombatAction);

			lblStatus.Text = "Plugin Started";

			// Custom
			Log_Manager.setLogTarget(logEvents);
			EVENT_LOG_LABEL.SendToBack();

			initDirector();
		}

		public void DeInitPlugin()
		{
			// Unsubscribe from any events you listen to when exiting!
			ActGlobals.oFormActMain.AfterCombatAction -= oFormActMain_AfterCombatAction;

			saveSettings();
			lblStatus.Text = "Plugin Exited";
		}
		#endregion

		#region Settings
		void loadSettings()
		{
			xmlSettings.AddControlSetting(textBox1.Name, textBox1);

			if (File.Exists(settingsFile))
			{
				FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				XmlTextReader xReader = new XmlTextReader(fs);

				try
				{
					while (xReader.Read())
					{
						if (xReader.NodeType == XmlNodeType.Element)
						{
							if (xReader.LocalName == "SettingsSerializer")
							{
								xmlSettings.ImportFromXml(xReader);
							}
						}
					}
				}
				catch (Exception ex)
				{
					lblStatus.Text = "Error loading settings: " + ex.Message;
				}
				xReader.Close();
			}
		}
		void saveSettings()
		{
			FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			XmlTextWriter xWriter = new XmlTextWriter(fs, Encoding.UTF8)
			{
				Formatting = Formatting.Indented,
				Indentation = 1,
				IndentChar = '\t'
			};
			xWriter.WriteStartDocument(true);
			xWriter.WriteStartElement("Config");    // <Config>
			xWriter.WriteStartElement("SettingsSerializer");    // <Config><SettingsSerializer>
			xmlSettings.ExportToXml(xWriter);   // Fill the SettingsSerializer XML
			xWriter.WriteEndElement();  // </SettingsSerializer>
			xWriter.WriteEndElement();  // </Config>
			xWriter.WriteEndDocument(); // Tie up loose ends (shouldn't be any)
			xWriter.Flush();    // Flush the file buffer to disk
			xWriter.Close();
		}
		#endregion

		// Infinite background loop.
		// TODO buffer point adding
		public async Task updateChart()
		{
			await Task.Delay(1000);

			Stopwatch s = new Stopwatch();
			s.Start();
			while (true)
			{
				updateChartHelper(s);
				await Task.Delay(4);
			}

		}
		public void updateChartHelper(Stopwatch s)
		{
			// Switch to UI thread.
			if (chartVibeStrength.InvokeRequired && !ALLOWING_UNSAFE_UI_EDITS)
			{
				var d = new SafeCallDelegate(updateChartHelper);
				chartVibeStrength.Invoke(d, new object[] { s });
			}

			else
			{
				// Update override control UI
				switch (updateOverrideUI)
				{
					case 1:
						sliderVibeOverride.Value = (int)numericupdownOverrideValue.Value;
						updateOverrideUI = 0;
						break;
					case 2:
						numericupdownOverrideValue.Value = sliderVibeOverride.Value;
						updateOverrideUI = 0;
						break;
					default:
						break;
				}


				// Update Chart
				var series = chartVibeStrength.Series["seriesMain"];
				var area = chartVibeStrength.ChartAreas["areaMain"];

				if (series.Points.Count >= 288)
				{
					series.Points.RemoveAt(0);
				}

				double strength = director.getCurrentStrength();

				// Smooth graph if quality, stepped line if not.
				if (chartQuality > 0)
				{
					if (changedChartQuality)
					{
						chartVibeStrength.Series[0].ChartType = 
							System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
						changedChartQuality = false;
					}

					strength = Math.Round(strength / 5) * 5; // Based on Lovense Hush limits
				}
				if (changedChartQuality)
				{
					chartVibeStrength.Series[0].ChartType =
							System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
					changedChartQuality = false;
				}

				// Add points
				series.Points.AddXY(s.ElapsedMilliseconds, strength);
				chartVibeStrength.ResetAutoValues();
			}


		}

		private void initDirector()
		{
			try
			{
				director = new Director();
				director.begin();
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
			}

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			updateChart();
#pragma warning restore CS4014
		}

		// Only parses data for easier use in Director
		void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
		{
			// I know this is scuffed, sorry.
			string attacker = actionInfo.attacker;
			string victim = actionInfo.victim;
			string attackType = actionInfo.theAttackType;
			string damagetype = actionInfo.theDamageType;
			string swingType = Enum.GetName(typeof(SwingTypeEnum), actionInfo.swingType);
			string special = actionInfo.special;
			long damage = actionInfo.damage;
			bool wasCrit = actionInfo.critical;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			director.addAction(attacker, victim, attackType, damagetype, swingType, special, damage, wasCrit);
#pragma warning restore CS4014
		}

		#region UI Actions
		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void buttonRescan_Click(object sender, EventArgs e)
		{
			director.scanForPlugs();
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void log1_Click(object sender, EventArgs e)
		{

		}

		private void buttonStopScanning_Click(object sender, EventArgs e)
		{
			director.pressedAnyKey();
		}

		private void logEvents_TextChanged(object sender, EventArgs e)
		{

		}

		private void numericupdownOverrideValue_ValueChanged(object sender, EventArgs e)
		{
			director.queueVibeOverride(checkboxOverride.Checked, (double) numericupdownOverrideValue.Value);
			updateOverrideUI = 1;
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void buttonClearLog_Click(object sender, EventArgs e)
		{
			Log_Manager.clear();
		}

		private void chartVibeStrength_Click(object sender, EventArgs e)
		{

		}

		private void sliderVibeOverride_Scroll(object sender, EventArgs e)
		{
			director.queueVibeOverride(checkboxOverride.Checked, sliderVibeOverride.Value);
			updateOverrideUI = 2;
		}

		private void checkboxOverride_CheckedChanged(object sender, EventArgs e)
		{
			director.queueVibeOverride(checkboxOverride.Checked, sliderVibeOverride.Value);
		}

		private void buttonStart_Click(object sender, EventArgs e)
		{

		}

		private void checkboxChartQuality_CheckedChanged(object sender, EventArgs e)
		{
			if (checkboxChartQuality.Checked)
			{
				director.setChartQuality(0);
				chartQuality = 0;
			}
			else
			{
				director.setChartQuality(1);
				chartQuality = 1;
			}

			changedChartQuality = true;
		}
	}
	#endregion
}
