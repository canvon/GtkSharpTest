using System;
using System.IO;
using Gtk;

namespace GtkSharpTest
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class LogViewWidget : Gtk.Bin
	{
		private string _LogName;
		public string LogName {
			get {
				return _LogName;
			}
			set {
				_LogName = value;
				this.labelLogName.Text = string.Format("Log {0}", _LogName);
			}
		}

		private string _LogFilePath;
		public string LogFilePath {
			get {
				return _LogFilePath;
			}
			set {
				_LogFilePath = value;
			}
		}

		public LogViewWidget()
		{
			this.Build();
		}

		protected void OnButtonReadLogClicked(object sender, EventArgs e)
		{
			ReadLogSafe();
		}

		public void ReadLog()
		{
			TextBuffer buf = this.textviewLogContents.Buffer;
			TextIter iter;

			try {
				buf.BeginUserAction();

				// Always clear first, so that when the user clicks the error message away
				// he won't be able to confuse previous content to be new/current content.
				buf.Clear();

				if (string.IsNullOrEmpty(_LogFilePath))
					throw new InvalidOperationException(
						string.Format("No proper log file path set: \"{0}\"", _LogFilePath));

				using (TextReader reader = new StreamReader(_LogFilePath)) {
					iter = buf.EndIter;

					string line;
					while ((line = reader.ReadLine()) != null)
					{
						buf.Insert(ref iter, line + Environment.NewLine);
					}
				}
			}
			finally {
				buf.EndUserAction();
			}

			// Scroll to bottom, so that possibly the last line is visible.
			buf.PlaceCursor(iter);
			this.textviewLogContents.ScrollToIter(iter, 0, true, 0, 0);
		}

		public bool ReadLogSafe()
		{
			try {
				ReadLog();

				return true;
			}
			catch (Exception ex) {
				MessageDialog dialog = new MessageDialog(
					null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,
					"Error reading log {0} from \"{1}\":" + Environment.NewLine + "{2}",
					new object[]{_LogName, _LogFilePath, ex.Message});
				dialog.Run();
				dialog.Destroy();

				return false;
			}
		}
	}
}

