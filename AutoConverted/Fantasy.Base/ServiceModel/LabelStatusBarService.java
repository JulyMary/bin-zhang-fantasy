package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public class LabelStatusBarService implements IStatusBarService
{

	private Label _label;
	private String _status;
	public LabelStatusBarService(Label label)
	{
		if ((label == null))
		{
			throw new ArgumentNullException("label");
		}
		this._label = label;
		if (!this._label.IsHandleCreated)
		{
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
			this._label.HandleCreated += new EventHandler(Label_HandleCreated);
		}

	}

	private void Label_HandleCreated(Object sender, EventArgs e)
	{
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		this._label.HandleCreated -= new EventHandler(Label_HandleCreated);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
		this._label.invoke(new MethodInvoker(()=>{
			this._label.setText(this._status);
	}
   private ));
	}


	public void SetStatus(String status)
	{
		this._status = status;
		if (_label.IsHandleCreated)
		{
			if (_label.InvokeRequired)
			{
				MethodInvoker<String> del = new MethodInvoker<String>(SetStatus);
				_label.invoke(del, status);
			}
			else
			{
				this._label.setText(status);

			}
		}

	}


	public final class StatusBarExtensions
	{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SetStatus(this IStatusBarService bar, string format, params object[] args)
		public static void SetStatus(IStatusBarService bar, String format, Object... args)
		{
			bar.SetStatus(String.format(format, args));
		}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SafeSetStatus(this IStatusBarService bar, string format, params object[] args)
		public static void SafeSetStatus(IStatusBarService bar, String format, Object... args)
		{
			if (bar != null)
			{
				bar.SetStatus(String.format(format, args));
			}
		}
	}

}