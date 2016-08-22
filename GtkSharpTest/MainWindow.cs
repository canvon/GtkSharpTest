using System;
using System.IO;
using Gtk;

public partial class MainWindow: Gtk.Window
{
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnButtonDoSomethingClicked(object sender, EventArgs e)
	{
		MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok,
		                                         "Hello, world!");
		dialog.Run();
		dialog.Destroy();
	}

	protected void OnButtonReadLogClicked(object sender, EventArgs e)
	{
		using (TextReader reader = new StreamReader("/var/log/syslog")) {
			TextBuffer buf = this.textviewLogContents.Buffer;
			TextIter iter = buf.EndIter;

			string line;
			while ((line = reader.ReadLine()) != null)
			{
				buf.Insert(ref iter, line);
			}
		}
	}
}
