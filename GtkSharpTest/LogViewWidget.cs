using System;
using System.IO;
using Gtk;

namespace GtkSharpTest
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class LogViewWidget : Gtk.Bin
	{
		public LogViewWidget()
		{
			this.Build();
		}

		protected void OnButtonReadLogClicked(object sender, EventArgs e)
		{
			using (TextReader reader = new StreamReader("/var/log/syslog")) {
				TextBuffer buf = this.textviewLogContents.Buffer;
				TextIter iter = buf.EndIter;

				string line;
				while ((line = reader.ReadLine()) != null)
				{
					buf.Insert(ref iter, line + Environment.NewLine);
				}
			}
		}
	}
}

