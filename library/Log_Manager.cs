using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Plug_Parser_Plugin
{
	class Log_Manager
	{
		private const bool RESTRICTING_LINE_COUNT = false;

		private delegate void SafeCallDelegate(string text);
		private delegate void SafeClearDelegate();

		private static RichTextBox eventLog;

		public static void setLogTarget(RichTextBox l)
		{
			eventLog = l;
		}

		public static void write(string s)
		{
			if (eventLog == null)
			{
				return;
			}

			if (eventLog.InvokeRequired && !usercontrolPlugParser.ALLOWING_UNSAFE_UI_EDITS)
			{
				var d = new SafeCallDelegate(write);
				eventLog.Invoke(d, new object[] { s });
			}
			else
			{

				if (((eventLog.Lines.Length >= 20) || (eventLog.Text == "")) && RESTRICTING_LINE_COUNT)
				{
					eventLog.Text = s;
				}
				else
				{
					eventLog.AppendText("\n" + s);
				}
			}
		}

		public static void clear()
		{
			if (eventLog == null)
			{
				return;
			}

			if (eventLog.InvokeRequired && !usercontrolPlugParser.ALLOWING_UNSAFE_UI_EDITS)
			{
				var d = new SafeClearDelegate(clear);
				eventLog.Invoke(d);
			}
			else
			{
				eventLog.Text = "";
			}
		}
	}
}
