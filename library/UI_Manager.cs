using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Plug_Parser_Plugin
{
	class UI_Manager
	{
		private delegate void SafeCallDelegate();

		// Update targets
		private Chart chartVibeStrength;
		private NumericUpDown numericOverrideValue;
		private TrackBar barOverrideValue;

		// Queued data
		private Queue<DataPoint> newPoints;

		private bool sliderOverrideChanged = false;
		private bool numericOverrideChanged = false;
		private bool smoothingChart = true;

		private long maxPointsPerSeries = 288;

		public UI_Manager(Chart chart, NumericUpDown nudOverride, TrackBar barOverride)
		{
			newPoints = new Queue<DataPoint>();

			chartVibeStrength = chart;
			numericOverrideValue = nudOverride;
			barOverrideValue = barOverride;

		}

		public void update()
		{
			// Switch to UI thread if necessary.
			if (chartVibeStrength.InvokeRequired)
			{
				var d = new SafeCallDelegate(update);
				chartVibeStrength.Invoke(d, new object[] {});
			}

			else
			{
				// Update override control UI
				if (numericOverrideChanged) {
					barOverrideValue.Value = (int)numericOverrideValue.Value;
					numericOverrideChanged = false;
				}
				else if (sliderOverrideChanged) { 
					numericOverrideValue.Value = barOverrideValue.Value;
					sliderOverrideChanged = false;
				}

				updateChart();
				
			}
		}

		private void updateChart()
		{
			var series = chartVibeStrength.Series["seriesMain"];
			var area = chartVibeStrength.ChartAreas["areaMain"];

			// Check smoothing
			bool usingSpline = chartVibeStrength.Series[0].ChartType == SeriesChartType.Spline;
			if (smoothingChart && !usingSpline)
			{
				chartVibeStrength.Series[0].ChartType = SeriesChartType.Spline;
			}
			else if (!smoothingChart && usingSpline)
			{
				chartVibeStrength.Series[0].ChartType = SeriesChartType.StepLine;
			}

			// Add points
			foreach (DataPoint point in newPoints)
			{
				if (!smoothingChart)
				{
					for (int i = 0; i < point.YValues.Length; i++)
					{
						point.YValues[i] = Math.Round(point.YValues[i] / Device_Properties.getAccuracy())
							* Device_Properties.getAccuracy();
					}
				}
				series.Points.Add(point);
			}

			// Restrict points count
			while (series.Points.Count >= maxPointsPerSeries)
			{
				series.Points.RemoveAt(0);
			}

			// Forces normally automatic update to component.
			chartVibeStrength.ResetAutoValues();
		}

		public void enqueuePoint(long timeInMillis, double strength)
		{
			DataPoint point = new DataPoint(timeInMillis, strength);
			newPoints.Enqueue(point);
		}
	}

}
