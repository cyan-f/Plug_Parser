using System;
using System.Windows.Forms;
using System.Text;
using Advanced_Combat_Tracker;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

/*
 * File uses example code from Advanced Combat Tracker website.
 */

namespace Plug_Parser_Plugin
{
	public class Plug_Parser : UserControl, IActPluginV1
	{
		public const bool ALLOWING_UNSAFE_UI_EDITS = false;
		private delegate void SafeCallDelegate(Stopwatch s);

		Director director;
		private Button Rescan;

		private Button button1;
		private RichTextBox log1;
		private NumericUpDown vibeOverrideValue;
		private TrackBar trackBar1;
		private Button clearLogButton;
		private System.Windows.Forms.DataVisualization.Charting.Chart vibeStrengthChart;
		private Label EVENT_LOG_LABEL;
		private Label CHART_LABEL;
		private CheckBox checkBox1;
		private Label label1;

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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.EVENT_LOG_LABEL = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.Rescan = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.log1 = new System.Windows.Forms.RichTextBox();
			this.vibeOverrideValue = new System.Windows.Forms.NumericUpDown();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.clearLogButton = new System.Windows.Forms.Button();
			this.vibeStrengthChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.CHART_LABEL = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.vibeOverrideValue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.vibeStrengthChart)).BeginInit();
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
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// Rescan
			// 
			this.Rescan.AccessibleDescription = "Rescans for devices.";
			this.Rescan.AccessibleName = "Rescan";
			this.Rescan.Location = new System.Drawing.Point(517, 19);
			this.Rescan.Name = "Rescan";
			this.Rescan.Size = new System.Drawing.Size(75, 23);
			this.Rescan.TabIndex = 2;
			this.Rescan.Text = "Rescan";
			this.Rescan.UseVisualStyleBackColor = true;
			this.Rescan.Click += new System.EventHandler(this.button1_Click);
			// 
			// button1
			// 
			this.button1.Enabled = false;
			this.button1.Location = new System.Drawing.Point(517, 48);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(113, 35);
			this.button1.TabIndex = 4;
			this.button1.Text = "Stop Scanning";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// log1
			// 
			this.log1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.log1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.log1.DetectUrls = false;
			this.log1.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.log1.Location = new System.Drawing.Point(4, 20);
			this.log1.Margin = new System.Windows.Forms.Padding(4);
			this.log1.Name = "log1";
			this.log1.ReadOnly = true;
			this.log1.Size = new System.Drawing.Size(512, 252);
			this.log1.TabIndex = 5;
			this.log1.TabStop = false;
			this.log1.Text = "...\n1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12\n13\n14\n16\n17\n18\n19";
			this.log1.WordWrap = false;
			this.log1.TextChanged += new System.EventHandler(this.log1_TextChanged);
			// 
			// vibeOverrideValue
			// 
			this.vibeOverrideValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.vibeOverrideValue.Location = new System.Drawing.Point(585, 626);
			this.vibeOverrideValue.Name = "vibeOverrideValue";
			this.vibeOverrideValue.Size = new System.Drawing.Size(51, 20);
			this.vibeOverrideValue.TabIndex = 8;
			this.vibeOverrideValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.vibeOverrideValue.ValueChanged += new System.EventHandler(this.vibeOverrideValue_ValueChanged);
			// 
			// trackBar1
			// 
			this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.trackBar1.AutoSize = false;
			this.trackBar1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.trackBar1.LargeChange = 10;
			this.trackBar1.Location = new System.Drawing.Point(600, 297);
			this.trackBar1.Maximum = 100;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackBar1.Size = new System.Drawing.Size(36, 323);
			this.trackBar1.SmallChange = 5;
			this.trackBar1.TabIndex = 11;
			this.trackBar1.TickFrequency = 5;
			this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
			// 
			// clearLogButton
			// 
			this.clearLogButton.BackColor = System.Drawing.Color.Transparent;
			this.clearLogButton.FlatAppearance.BorderSize = 0;
			this.clearLogButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.clearLogButton.Location = new System.Drawing.Point(79, 0);
			this.clearLogButton.Name = "clearLogButton";
			this.clearLogButton.Size = new System.Drawing.Size(60, 20);
			this.clearLogButton.TabIndex = 12;
			this.clearLogButton.Text = "clear";
			this.clearLogButton.UseVisualStyleBackColor = false;
			this.clearLogButton.Click += new System.EventHandler(this.button2_Click);
			// 
			// vibeStrengthChart
			// 
			this.vibeStrengthChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.vibeStrengthChart.BackColor = System.Drawing.Color.Transparent;
			chartArea4.AxisX.LabelStyle.Enabled = false;
			chartArea4.AxisX.MajorGrid.Enabled = false;
			chartArea4.AxisX.MajorTickMark.Enabled = false;
			chartArea4.AxisX.ScrollBar.BackColor = System.Drawing.Color.Black;
			chartArea4.AxisY.LabelStyle.Enabled = false;
			chartArea4.AxisY.MajorGrid.Enabled = false;
			chartArea4.AxisY.MajorTickMark.Enabled = false;
			chartArea4.AxisY.Maximum = 100D;
			chartArea4.AxisY.Minimum = 0D;
			chartArea4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			chartArea4.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
			chartArea4.CursorX.AutoScroll = false;
			chartArea4.Name = "areaMain";
			chartArea4.Position.Auto = false;
			chartArea4.Position.Height = 100F;
			chartArea4.Position.Width = 100F;
			this.vibeStrengthChart.ChartAreas.Add(chartArea4);
			legend4.Enabled = false;
			legend4.Name = "Legend1";
			this.vibeStrengthChart.Legends.Add(legend4);
			this.vibeStrengthChart.Location = new System.Drawing.Point(4, 309);
			this.vibeStrengthChart.Margin = new System.Windows.Forms.Padding(0);
			this.vibeStrengthChart.MinimumSize = new System.Drawing.Size(594, 252);
			this.vibeStrengthChart.Name = "vibeStrengthChart";
			series4.ChartArea = "areaMain";
			series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
			series4.Color = System.Drawing.SystemColors.ControlLightLight;
			series4.LabelForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			series4.Legend = "Legend1";
			series4.Name = "seriesMain";
			this.vibeStrengthChart.Series.Add(series4);
			this.vibeStrengthChart.Size = new System.Drawing.Size(594, 300);
			this.vibeStrengthChart.TabIndex = 13;
			this.vibeStrengthChart.Click += new System.EventHandler(this.chart1_Click);
			// 
			// CHART_LABEL
			// 
			this.CHART_LABEL.AutoSize = true;
			this.CHART_LABEL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CHART_LABEL.Location = new System.Drawing.Point(45, 285);
			this.CHART_LABEL.Name = "CHART_LABEL";
			this.CHART_LABEL.Size = new System.Drawing.Size(90, 20);
			this.CHART_LABEL.TabIndex = 14;
			this.CHART_LABEL.Text = "Vibrations";
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox1.AutoSize = true;
			this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBox1.Location = new System.Drawing.Point(513, 629);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(66, 17);
			this.checkBox1.TabIndex = 15;
			this.checkBox1.Text = "Override";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(145, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(202, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "NOTE: ONLY SUPPORTS ONE DEVICE";
			// 
			// Plug_Parser
			// 
			this.AccessibleDescription = "";
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.CHART_LABEL);
			this.Controls.Add(this.vibeStrengthChart);
			this.Controls.Add(this.clearLogButton);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.EVENT_LOG_LABEL);
			this.Controls.Add(this.vibeOverrideValue);
			this.Controls.Add(this.log1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.Rescan);
			this.Controls.Add(this.textBox1);
			this.MinimumSize = new System.Drawing.Size(640, 655);
			this.Name = "Plug_Parser";
			this.Size = new System.Drawing.Size(640, 655);
			((System.ComponentModel.ISupportInitialize)(this.vibeOverrideValue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.vibeStrengthChart)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TextBox textBox1;

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
		string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\PluginSample.config.xml");
		SettingsSerializer xmlSettings;

		#region IActPluginV1 Members
		// Bog standard plugin stuff I don't want/have to think about too much.


		public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
		{
			lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
			pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
			this.Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
			xmlSettings = new SettingsSerializer(this); // Create a new settings serializer and pass it this instance
			LoadSettings();

			// Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.
			ActGlobals.oFormActMain.AfterCombatAction += new CombatActionDelegate(oFormActMain_AfterCombatAction);

			lblStatus.Text = "Plugin Started";

			Log_Manager.setLogTarget(log1);
			EVENT_LOG_LABEL.SendToBack();

			initDirector();
		}

		public void DeInitPlugin()
		{
			// Unsubscribe from any events you listen to when exiting!
			ActGlobals.oFormActMain.AfterCombatAction -= oFormActMain_AfterCombatAction;

			SaveSettings();
			lblStatus.Text = "Plugin Exited";
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
			if (vibeStrengthChart.InvokeRequired && !ALLOWING_UNSAFE_UI_EDITS)
			{
				var d = new SafeCallDelegate(updateChartHelper);
				vibeStrengthChart.Invoke(d, new object[] { s });
			}
			else
			{
				var series = vibeStrengthChart.Series["seriesMain"];
				var area = vibeStrengthChart.ChartAreas["areaMain"];

				if (series.Points.Count >= 288)
				{
					series.Points.RemoveAt(0);
				}

				series.Points.AddXY(s.ElapsedMilliseconds, director.getCurrentStrength());
				vibeStrengthChart.ResetAutoValues();
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

		#region Settings
		void LoadSettings()
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
		void SaveSettings()
		{
			FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			XmlTextWriter xWriter = new XmlTextWriter(fs, Encoding.UTF8);
			xWriter.Formatting = Formatting.Indented;
			xWriter.Indentation = 1;
			xWriter.IndentChar = '\t';
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

		#region UI Actions
		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			director.scanForPlugs();
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void log1_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			director.pressedAnyKey();
		}

		private void log1_TextChanged(object sender, EventArgs e)
		{

		}

		private void vibeOverrideValue_ValueChanged(object sender, EventArgs e)
		{
			director.queueVibeOverride(checkBox1.Checked, trackBar1.Value);
			if (trackBar1.Value != (int)vibeOverrideValue.Value)
			{
				if (!Plug_Parser.ALLOWING_UNSAFE_UI_EDITS)
				{
					return;
				}
				trackBar1.Value = (int)vibeOverrideValue.Value;
			}
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			Log_Manager.clear();
		}

		private void chart1_Click(object sender, EventArgs e)
		{

		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			director.queueVibeOverride(checkBox1.Checked, trackBar1.Value);
			if ((int)vibeOverrideValue.Value != trackBar1.Value)
			{
				if (!Plug_Parser.ALLOWING_UNSAFE_UI_EDITS)
				{
					return;
				}
				vibeOverrideValue.Value = trackBar1.Value;
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			director.queueVibeOverride(checkBox1.Checked, trackBar1.Value);
		}

	}
	#endregion
}
