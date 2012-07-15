package Fantasy.Jobs;

import Fantasy.ServiceModel.*;

public class LogTraceListener extends TraceListener
{
	private ILogger _logger;

	public LogTraceListener()
	{

	}

	private boolean _disposed = false;

	private Regex _regex = new Regex("\\{\\d+(:[^\\}]*)?\\}", RegexOptions.Multiline);


	public LogTraceListener(ILogger logger)
	{
		this._logger = logger;
	}



	@Override
	public void Write(String message)
	{
		if (!this._disposed && _logger != null)
		{
			message = this.RemoveFormat(message);
			_logger.LogMessage("Trace", message);
		}
	}

	private String RemoveFormat(String message)
	{
		synchronized (_regex)
		{
			return _regex.Replace(message, "");
		}
	}

	@Override
	public void WriteLine(String message)
	{

		if (!this._disposed && _logger != null)
		{
			message = this.RemoveFormat(message);
			_logger.LogMessage("Trace", message);
		}
	}

	@Override
	protected void dispose(boolean disposing)
	{
		_disposed = true;
		super.dispose(disposing);
	}
}