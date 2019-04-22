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
		private int chartUpdatesPerSecond = 144;
		private int chartUpdateDelay = 1000 / 144;

		// 0 = no update
		// 1 = update trackbar
		// 2 = update updownbox
		private long updateOverrideUI = 0;

		Director director;

		private Button buttonRescan;
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
		private Button buttonReconnect;
		private CheckBox checkboxChartQuality;
		private CheckBox checkEmbeddedServer;
		private CheckBox checkLogCombatEvents;
		private Button buttonCheckTrigger;
		private TextBox textNewTrigger;
		private Button buttonAddTrigger;
		private TabControl tabControls;
		private TabPage tabTriggerSettings;
		private TabPage tabVibeSettings;
		private Label labelRegex;
		private CheckedListBox checkedListBoxTriggerReactions;
		private TabPage tabPlayerInfo;
		private NumericUpDown numericupdownUpdatesPerSecond;
		private Label labelUpdatesPerSecond;
		private Label label7;
		private Label label6;
		private NumericUpDown numericupdownLengthOfKillstreak;
		private Label label4;
		private NumericUpDown numericupdownStrengthOnKill;
		private Label label3;
		private NumericUpDown numericupdownAmplitudeOnKillstreak;
		private Label label2;
		private NumericUpDown numericupdownStrengthOnHealHit;
		private Button buttonSafeword;
		private Button buttonGreenLight;
		private Button buttonResetVibeState;
		private Label label5;
		private NumericUpDown numericupdownBuzzDuration;

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

#pragma warning disable IDE1006 // Naming Styles
							   /// <summary> 
							   /// Required method for Designer support - do not modify 
							   /// the contents of this method with the code editor.
							   /// </summary>
		private void InitializeComponent()
#pragma warning restore IDE1006 // Naming Styles
		{
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.EVENT_LOG_LABEL = new System.Windows.Forms.Label();
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
			this.buttonReconnect = new System.Windows.Forms.Button();
			this.checkboxChartQuality = new System.Windows.Forms.CheckBox();
			this.checkEmbeddedServer = new System.Windows.Forms.CheckBox();
			this.checkLogCombatEvents = new System.Windows.Forms.CheckBox();
			this.buttonCheckTrigger = new System.Windows.Forms.Button();
			this.textNewTrigger = new System.Windows.Forms.TextBox();
			this.buttonAddTrigger = new System.Windows.Forms.Button();
			this.tabControls = new System.Windows.Forms.TabControl();
			this.tabVibeSettings = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.numericupdownLengthOfKillstreak = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.numericupdownStrengthOnKill = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.numericupdownAmplitudeOnKillstreak = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.numericupdownStrengthOnHealHit = new System.Windows.Forms.NumericUpDown();
			this.tabPlayerInfo = new System.Windows.Forms.TabPage();
			this.tabTriggerSettings = new System.Windows.Forms.TabPage();
			this.labelRegex = new System.Windows.Forms.Label();
			this.checkedListBoxTriggerReactions = new System.Windows.Forms.CheckedListBox();
			this.numericupdownUpdatesPerSecond = new System.Windows.Forms.NumericUpDown();
			this.labelUpdatesPerSecond = new System.Windows.Forms.Label();
			this.buttonSafeword = new System.Windows.Forms.Button();
			this.buttonGreenLight = new System.Windows.Forms.Button();
			this.buttonResetVibeState = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.numericupdownBuzzDuration = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownOverrideValue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sliderVibeOverride)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartVibeStrength)).BeginInit();
			this.tabControls.SuspendLayout();
			this.tabVibeSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownLengthOfKillstreak)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownStrengthOnKill)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownAmplitudeOnKillstreak)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownStrengthOnHealHit)).BeginInit();
			this.tabTriggerSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownUpdatesPerSecond)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownBuzzDuration)).BeginInit();
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
			// buttonRescan
			// 
			this.buttonRescan.AccessibleDescription = "Rescans for devices.";
			this.buttonRescan.AccessibleName = "Rescan";
			this.buttonRescan.Location = new System.Drawing.Point(517, 58);
			this.buttonRescan.Name = "buttonRescan";
			this.buttonRescan.Size = new System.Drawing.Size(114, 32);
			this.buttonRescan.TabIndex = 2;
			this.buttonRescan.Text = "Rescan";
			this.buttonRescan.UseVisualStyleBackColor = true;
			this.buttonRescan.Click += new System.EventHandler(this.buttonRescan_Click);
			// 
			// buttonStopScanning
			// 
			this.buttonStopScanning.Location = new System.Drawing.Point(517, 96);
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
			this.logEvents.Text = "Log start";
			this.logEvents.WordWrap = false;
			this.logEvents.TextChanged += new System.EventHandler(this.logEvents_TextChanged);
			// 
			// numericupdownOverrideValue
			// 
			this.numericupdownOverrideValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericupdownOverrideValue.Location = new System.Drawing.Point(935, 285);
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
			this.sliderVibeOverride.Location = new System.Drawing.Point(992, 297);
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
			this.chartVibeStrength.MinimumSize = new System.Drawing.Size(590, 252);
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
			this.chartVibeStrength.Size = new System.Drawing.Size(986, 300);
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
			this.checkboxOverride.Location = new System.Drawing.Point(821, 286);
			this.checkboxOverride.Name = "checkboxOverride";
			this.checkboxOverride.Size = new System.Drawing.Size(108, 17);
			this.checkboxOverride.TabIndex = 15;
			this.checkboxOverride.Text = "Override Enabled";
			this.checkboxOverride.UseVisualStyleBackColor = true;
			this.checkboxOverride.CheckedChanged += new System.EventHandler(this.checkboxOverride_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(145, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(412, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "NOTE: Only solo, vibrating-only devices have been tested and are officially suppo" +
    "rted.";
			// 
			// buttonReconnect
			// 
			this.buttonReconnect.Location = new System.Drawing.Point(517, 20);
			this.buttonReconnect.Name = "buttonReconnect";
			this.buttonReconnect.Size = new System.Drawing.Size(114, 32);
			this.buttonReconnect.TabIndex = 17;
			this.buttonReconnect.Text = "Reconnect";
			this.buttonReconnect.UseVisualStyleBackColor = true;
			this.buttonReconnect.Click += new System.EventHandler(this.buttonReconnect_Click);
			// 
			// checkboxChartQuality
			// 
			this.checkboxChartQuality.AutoSize = true;
			this.checkboxChartQuality.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkboxChartQuality.Checked = true;
			this.checkboxChartQuality.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkboxChartQuality.Location = new System.Drawing.Point(100, 288);
			this.checkboxChartQuality.Name = "checkboxChartQuality";
			this.checkboxChartQuality.Size = new System.Drawing.Size(76, 17);
			this.checkboxChartQuality.TabIndex = 18;
			this.checkboxChartQuality.Text = "Smoothing";
			this.checkboxChartQuality.UseVisualStyleBackColor = true;
			this.checkboxChartQuality.CheckedChanged += new System.EventHandler(this.checkboxChartQuality_CheckedChanged);
			// 
			// checkEmbeddedServer
			// 
			this.checkEmbeddedServer.AutoSize = true;
			this.checkEmbeddedServer.Checked = true;
			this.checkEmbeddedServer.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkEmbeddedServer.Location = new System.Drawing.Point(517, 232);
			this.checkEmbeddedServer.Name = "checkEmbeddedServer";
			this.checkEmbeddedServer.Size = new System.Drawing.Size(133, 17);
			this.checkEmbeddedServer.TabIndex = 19;
			this.checkEmbeddedServer.Text = "Use Embedded Server";
			this.checkEmbeddedServer.UseVisualStyleBackColor = true;
			this.checkEmbeddedServer.CheckedChanged += new System.EventHandler(this.checkEmbeddedServer_CheckedChanged);
			// 
			// checkLogCombatEvents
			// 
			this.checkLogCombatEvents.AutoSize = true;
			this.checkLogCombatEvents.Location = new System.Drawing.Point(517, 255);
			this.checkLogCombatEvents.Name = "checkLogCombatEvents";
			this.checkLogCombatEvents.Size = new System.Drawing.Size(119, 17);
			this.checkLogCombatEvents.TabIndex = 20;
			this.checkLogCombatEvents.Text = "Log Combat Events";
			this.checkLogCombatEvents.UseVisualStyleBackColor = true;
			this.checkLogCombatEvents.CheckedChanged += new System.EventHandler(this.checkLogCombatEvents_CheckedChanged);
			// 
			// buttonCheckTrigger
			// 
			this.buttonCheckTrigger.Location = new System.Drawing.Point(6, 188);
			this.buttonCheckTrigger.Name = "buttonCheckTrigger";
			this.buttonCheckTrigger.Size = new System.Drawing.Size(114, 32);
			this.buttonCheckTrigger.TabIndex = 21;
			this.buttonCheckTrigger.Text = "Check Triggers";
			this.buttonCheckTrigger.UseVisualStyleBackColor = true;
			this.buttonCheckTrigger.Click += new System.EventHandler(this.buttonCheckTrigger_Click);
			// 
			// textNewTrigger
			// 
			this.textNewTrigger.Location = new System.Drawing.Point(58, 8);
			this.textNewTrigger.Name = "textNewTrigger";
			this.textNewTrigger.Size = new System.Drawing.Size(109, 20);
			this.textNewTrigger.TabIndex = 22;
			this.textNewTrigger.TextChanged += new System.EventHandler(this.textNewTrigger_TextChanged);
			// 
			// buttonAddTrigger
			// 
			this.buttonAddTrigger.Location = new System.Drawing.Point(6, 150);
			this.buttonAddTrigger.Name = "buttonAddTrigger";
			this.buttonAddTrigger.Size = new System.Drawing.Size(114, 32);
			this.buttonAddTrigger.TabIndex = 23;
			this.buttonAddTrigger.Text = "Add Trigger";
			this.buttonAddTrigger.UseVisualStyleBackColor = true;
			// 
			// tabControls
			// 
			this.tabControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControls.Controls.Add(this.tabVibeSettings);
			this.tabControls.Controls.Add(this.tabPlayerInfo);
			this.tabControls.Controls.Add(this.tabTriggerSettings);
			this.tabControls.Location = new System.Drawing.Point(656, 20);
			this.tabControls.MinimumSize = new System.Drawing.Size(360, 252);
			this.tabControls.Name = "tabControls";
			this.tabControls.SelectedIndex = 0;
			this.tabControls.Size = new System.Drawing.Size(374, 252);
			this.tabControls.TabIndex = 24;
			// 
			// tabVibeSettings
			// 
			this.tabVibeSettings.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tabVibeSettings.Controls.Add(this.label5);
			this.tabVibeSettings.Controls.Add(this.numericupdownBuzzDuration);
			this.tabVibeSettings.Controls.Add(this.label7);
			this.tabVibeSettings.Controls.Add(this.label6);
			this.tabVibeSettings.Controls.Add(this.numericupdownLengthOfKillstreak);
			this.tabVibeSettings.Controls.Add(this.label4);
			this.tabVibeSettings.Controls.Add(this.numericupdownStrengthOnKill);
			this.tabVibeSettings.Controls.Add(this.label3);
			this.tabVibeSettings.Controls.Add(this.numericupdownAmplitudeOnKillstreak);
			this.tabVibeSettings.Controls.Add(this.label2);
			this.tabVibeSettings.Controls.Add(this.numericupdownStrengthOnHealHit);
			this.tabVibeSettings.Location = new System.Drawing.Point(4, 22);
			this.tabVibeSettings.Name = "tabVibeSettings";
			this.tabVibeSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tabVibeSettings.Size = new System.Drawing.Size(366, 226);
			this.tabVibeSettings.TabIndex = 1;
			this.tabVibeSettings.Text = "Vibe Settings";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.label7.Location = new System.Drawing.Point(69, 160);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(28, 15);
			this.label7.TabIndex = 31;
			this.label7.Text = "kills";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.ForeColor = System.Drawing.SystemColors.Control;
			this.label6.Location = new System.Drawing.Point(6, 144);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(99, 13);
			this.label6.TabIndex = 30;
			this.label6.Text = "Killstreak Threshold";
			// 
			// numericupdownLengthOfKillstreak
			// 
			this.numericupdownLengthOfKillstreak.Location = new System.Drawing.Point(29, 160);
			this.numericupdownLengthOfKillstreak.Name = "numericupdownLengthOfKillstreak";
			this.numericupdownLengthOfKillstreak.Size = new System.Drawing.Size(40, 20);
			this.numericupdownLengthOfKillstreak.TabIndex = 29;
			this.numericupdownLengthOfKillstreak.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.numericupdownLengthOfKillstreak.ValueChanged += new System.EventHandler(this.numericupdownKillstreakThreshold_ValueChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.SystemColors.Control;
			this.label4.Location = new System.Drawing.Point(6, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Buzz Strength on Kill";
			// 
			// numericupdownStrengthOnKill
			// 
			this.numericupdownStrengthOnKill.Location = new System.Drawing.Point(29, 68);
			this.numericupdownStrengthOnKill.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numericupdownStrengthOnKill.Name = "numericupdownStrengthOnKill";
			this.numericupdownStrengthOnKill.Size = new System.Drawing.Size(40, 20);
			this.numericupdownStrengthOnKill.TabIndex = 4;
			this.numericupdownStrengthOnKill.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numericupdownStrengthOnKill.ValueChanged += new System.EventHandler(this.numericupdownStrengthOnKill_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.SystemColors.Control;
			this.label3.Location = new System.Drawing.Point(6, 98);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(163, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Increased Amplitude on Killstreak";
			// 
			// numericupdownAmplitudeOnKillstreak
			// 
			this.numericupdownAmplitudeOnKillstreak.Location = new System.Drawing.Point(29, 114);
			this.numericupdownAmplitudeOnKillstreak.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
			this.numericupdownAmplitudeOnKillstreak.Name = "numericupdownAmplitudeOnKillstreak";
			this.numericupdownAmplitudeOnKillstreak.Size = new System.Drawing.Size(40, 20);
			this.numericupdownAmplitudeOnKillstreak.TabIndex = 2;
			this.numericupdownAmplitudeOnKillstreak.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericupdownAmplitudeOnKillstreak.ValueChanged += new System.EventHandler(this.numericupdownAmplitudeOnKillstreak_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.Control;
			this.label2.Location = new System.Drawing.Point(6, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(131, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Buzz Strength on Hit/Heal";
			// 
			// numericupdownStrengthOnHealHit
			// 
			this.numericupdownStrengthOnHealHit.Location = new System.Drawing.Point(29, 21);
			this.numericupdownStrengthOnHealHit.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numericupdownStrengthOnHealHit.Name = "numericupdownStrengthOnHealHit";
			this.numericupdownStrengthOnHealHit.Size = new System.Drawing.Size(40, 20);
			this.numericupdownStrengthOnHealHit.TabIndex = 0;
			this.numericupdownStrengthOnHealHit.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
			this.numericupdownStrengthOnHealHit.ValueChanged += new System.EventHandler(this.numericupdownOnHealHit_ValueChanged);
			// 
			// tabPlayerInfo
			// 
			this.tabPlayerInfo.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tabPlayerInfo.Location = new System.Drawing.Point(4, 22);
			this.tabPlayerInfo.Name = "tabPlayerInfo";
			this.tabPlayerInfo.Size = new System.Drawing.Size(366, 226);
			this.tabPlayerInfo.TabIndex = 2;
			this.tabPlayerInfo.Text = "Player Info.";
			// 
			// tabTriggerSettings
			// 
			this.tabTriggerSettings.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tabTriggerSettings.Controls.Add(this.labelRegex);
			this.tabTriggerSettings.Controls.Add(this.checkedListBoxTriggerReactions);
			this.tabTriggerSettings.Controls.Add(this.buttonCheckTrigger);
			this.tabTriggerSettings.Controls.Add(this.textNewTrigger);
			this.tabTriggerSettings.Controls.Add(this.buttonAddTrigger);
			this.tabTriggerSettings.Location = new System.Drawing.Point(4, 22);
			this.tabTriggerSettings.Name = "tabTriggerSettings";
			this.tabTriggerSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tabTriggerSettings.Size = new System.Drawing.Size(366, 226);
			this.tabTriggerSettings.TabIndex = 0;
			this.tabTriggerSettings.Text = "Custom Triggers";
			// 
			// labelRegex
			// 
			this.labelRegex.AutoSize = true;
			this.labelRegex.BackColor = System.Drawing.Color.Transparent;
			this.labelRegex.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelRegex.ForeColor = System.Drawing.SystemColors.Control;
			this.labelRegex.Location = new System.Drawing.Point(6, 9);
			this.labelRegex.Name = "labelRegex";
			this.labelRegex.Size = new System.Drawing.Size(46, 15);
			this.labelRegex.TabIndex = 25;
			this.labelRegex.Text = "Regex:";
			// 
			// checkedListBoxTriggerReactions
			// 
			this.checkedListBoxTriggerReactions.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.checkedListBoxTriggerReactions.ColumnWidth = 80;
			this.checkedListBoxTriggerReactions.FormattingEnabled = true;
			this.checkedListBoxTriggerReactions.Items.AddRange(new object[] {
            "Reaction",
            "Items",
            "For",
            "Demo.",
            "Purposes",
            "Only",
            ".",
            "If",
            "You\'re",
            "Seeing",
            "This",
            "Cyan",
            "Fucked",
            "Up"});
			this.checkedListBoxTriggerReactions.Location = new System.Drawing.Point(6, 34);
			this.checkedListBoxTriggerReactions.MultiColumn = true;
			this.checkedListBoxTriggerReactions.Name = "checkedListBoxTriggerReactions";
			this.checkedListBoxTriggerReactions.Size = new System.Drawing.Size(180, 109);
			this.checkedListBoxTriggerReactions.TabIndex = 24;
			// 
			// numericupdownUpdatesPerSecond
			// 
			this.numericupdownUpdatesPerSecond.Location = new System.Drawing.Point(298, 286);
			this.numericupdownUpdatesPerSecond.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericupdownUpdatesPerSecond.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericupdownUpdatesPerSecond.Name = "numericupdownUpdatesPerSecond";
			this.numericupdownUpdatesPerSecond.Size = new System.Drawing.Size(44, 20);
			this.numericupdownUpdatesPerSecond.TabIndex = 25;
			this.numericupdownUpdatesPerSecond.Value = new decimal(new int[] {
            144,
            0,
            0,
            0});
			this.numericupdownUpdatesPerSecond.ValueChanged += new System.EventHandler(this.numericupdownUpdatesPerSecond_ValueChanged);
			// 
			// labelUpdatesPerSecond
			// 
			this.labelUpdatesPerSecond.AutoSize = true;
			this.labelUpdatesPerSecond.Location = new System.Drawing.Point(188, 289);
			this.labelUpdatesPerSecond.Name = "labelUpdatesPerSecond";
			this.labelUpdatesPerSecond.Size = new System.Drawing.Size(109, 13);
			this.labelUpdatesPerSecond.TabIndex = 26;
			this.labelUpdatesPerSecond.Text = "Updates Per Second:";
			// 
			// buttonSafeword
			// 
			this.buttonSafeword.FlatAppearance.BorderColor = System.Drawing.Color.Firebrick;
			this.buttonSafeword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSafeword.Location = new System.Drawing.Point(517, 156);
			this.buttonSafeword.Name = "buttonSafeword";
			this.buttonSafeword.Size = new System.Drawing.Size(114, 32);
			this.buttonSafeword.TabIndex = 27;
			this.buttonSafeword.Text = "Red Light";
			this.buttonSafeword.UseVisualStyleBackColor = true;
			this.buttonSafeword.Click += new System.EventHandler(this.buttonSafeword_Click);
			// 
			// buttonGreenLight
			// 
			this.buttonGreenLight.FlatAppearance.BorderColor = System.Drawing.Color.Green;
			this.buttonGreenLight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonGreenLight.Location = new System.Drawing.Point(517, 194);
			this.buttonGreenLight.Name = "buttonGreenLight";
			this.buttonGreenLight.Size = new System.Drawing.Size(114, 32);
			this.buttonGreenLight.TabIndex = 28;
			this.buttonGreenLight.Text = "Green Light";
			this.buttonGreenLight.UseVisualStyleBackColor = true;
			this.buttonGreenLight.Click += new System.EventHandler(this.buttonGreenLight_Click);
			// 
			// buttonResetVibeState
			// 
			this.buttonResetVibeState.Location = new System.Drawing.Point(714, 285);
			this.buttonResetVibeState.Name = "buttonResetVibeState";
			this.buttonResetVibeState.Size = new System.Drawing.Size(101, 20);
			this.buttonResetVibeState.TabIndex = 34;
			this.buttonResetVibeState.Text = "Reset Vibe State";
			this.buttonResetVibeState.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.ForeColor = System.Drawing.SystemColors.Control;
			this.label5.Location = new System.Drawing.Point(193, 5);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(103, 13);
			this.label5.TabIndex = 33;
			this.label5.Text = "Buzz Duration (millis)";
			// 
			// numericupdownBuzzDuration
			// 
			this.numericupdownBuzzDuration.Location = new System.Drawing.Point(216, 21);
			this.numericupdownBuzzDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericupdownBuzzDuration.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericupdownBuzzDuration.Name = "numericupdownBuzzDuration";
			this.numericupdownBuzzDuration.Size = new System.Drawing.Size(40, 20);
			this.numericupdownBuzzDuration.TabIndex = 32;
			this.numericupdownBuzzDuration.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.numericupdownBuzzDuration.ValueChanged += new System.EventHandler(this.numericupdownBuzzDuration_ValueChanged);
			// 
			// Plug_Parser
			// 
			this.AccessibleDescription = "";
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.buttonResetVibeState);
			this.Controls.Add(this.buttonGreenLight);
			this.Controls.Add(this.buttonSafeword);
			this.Controls.Add(this.labelUpdatesPerSecond);
			this.Controls.Add(this.numericupdownUpdatesPerSecond);
			this.Controls.Add(this.tabControls);
			this.Controls.Add(this.checkLogCombatEvents);
			this.Controls.Add(this.checkEmbeddedServer);
			this.Controls.Add(this.checkboxChartQuality);
			this.Controls.Add(this.buttonReconnect);
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
			this.MinimumSize = new System.Drawing.Size(720, 626);
			this.Name = "Plug_Parser";
			this.Size = new System.Drawing.Size(1034, 626);
			((System.ComponentModel.ISupportInitialize)(this.numericupdownOverrideValue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sliderVibeOverride)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartVibeStrength)).EndInit();
			this.tabControls.ResumeLayout(false);
			this.tabVibeSettings.ResumeLayout(false);
			this.tabVibeSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownLengthOfKillstreak)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownStrengthOnKill)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownAmplitudeOnKillstreak)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownStrengthOnHealHit)).EndInit();
			this.tabTriggerSettings.ResumeLayout(false);
			this.tabTriggerSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownUpdatesPerSecond)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericupdownBuzzDuration)).EndInit();
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

			ActGlobals.oFormActMain.OnLogLineRead += new LogLineEventDelegate(oFormActMain_OnLogLineRead);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			initPP();
#pragma warning restore CS4014
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
			//xmlSettings.AddControlSetting(textBox1.Name, textBox1);

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
			// Hides messy-looking chart on startup.
			await Task.Delay(1000);

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (true)
			{
				updateChartHelper(stopwatch);
				await Task.Delay(chartUpdateDelay);
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

		private async Task initPP()
		{
			// Remove WIP/on-hold tab pages.
			tabControls.TabPages.Remove(tabPlayerInfo);
			tabControls.TabPages.Remove(tabTriggerSettings);

			PP_Settings.buzzStrengthOnHitOrHeal = (double)numericupdownStrengthOnHealHit.Value;
			PP_Settings.buzzDuration = (double)numericupdownBuzzDuration.Value;
			PP_Settings.amplitudeOnKillstreak = (double)numericupdownAmplitudeOnKillstreak.Value;
			PP_Settings.strengthOnKill = (double)numericupdownStrengthOnKill.Value;
			PP_Settings.lengthOfKillstreak = (double)numericupdownLengthOfKillstreak.Value;

			try
			{
				director = new Director();
				await director.begin();
			}
			catch (FileNotFoundException e)
			{
				Log_Manager.write("ERROR: Cannot find " + e.FileName);
			}

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			updateChart();
#pragma warning restore CS4014
		}

		
		void oFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
		{
			director.parseLogLine(logInfo.logLine);
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
			director.stopScanning();
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

		private void buttonReconnect_Click(object sender, EventArgs e)
		{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			director.reconnect();
#pragma warning restore CS4014
		}

		private void checkEmbeddedServer_CheckedChanged(object sender, EventArgs e)
		{
			director.setServerType(checkEmbeddedServer.Checked);
		}

		private void checkLogCombatEvents_CheckedChanged(object sender, EventArgs e)
		{
			director.setLoggingCombatEvents(checkLogCombatEvents.Checked);
		}

		private void buttonCheckTrigger_Click(object sender, EventArgs e)
		{
			director.checkTrigger();
		}

		private void textNewTrigger_TextChanged(object sender, EventArgs e)
		{

		}

		private void numericupdownUpdatesPerSecond_ValueChanged(object sender, EventArgs e)
		{
			chartUpdatesPerSecond = (int) numericupdownUpdatesPerSecond.Value;
			chartUpdateDelay = 1000 / chartUpdatesPerSecond;
		}

		private void numericupdownOnHealHit_ValueChanged(object sender, EventArgs e)
		{
			PP_Settings.buzzStrengthOnHitOrHeal = (double)numericupdownStrengthOnHealHit.Value;
		}

		private void numericupdownStrengthOnKill_ValueChanged(object sender, EventArgs e)
		{
			PP_Settings.strengthOnKill = (double)numericupdownStrengthOnKill.Value;
		}

		private void numericupdownAmplitudeOnKillstreak_ValueChanged(object sender, EventArgs e)
		{
			PP_Settings.amplitudeOnKillstreak = (double)numericupdownAmplitudeOnKillstreak.Value;
		}

		private void numericupdownKillstreakThreshold_ValueChanged(object sender, EventArgs e)
		{
			PP_Settings.lengthOfKillstreak = (double)numericupdownLengthOfKillstreak.Value;
		}

		private void buttonSafeword_Click(object sender, EventArgs e)
		{
			director.safeword();
		}

		private void buttonGreenLight_Click(object sender, EventArgs e)
		{
			director.greenLight();
		}

		private void numericupdownBuzzDuration_ValueChanged(object sender, EventArgs e)
		{
			PP_Settings.buzzDuration = (double)numericupdownBuzzDuration.Value;
		}
	}
	#endregion
}
