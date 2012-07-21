package fantasy.jobs.expressions;

public class ParseError
{
	private String message;
	private int code;
	private int line;
	private int col;
	private int pos;
	private int length;

	public final int getCode()
	{
		return code;
	}
	public final int getLine()
	{
		return line;
	}
	public final int getColumn()
	{
		return col;
	}
	public final int getPosition()
	{
		return pos;
	}
	public final int getLength()
	{
		return length;
	}
	public final String getMessage()
	{
		return message;
	}

	public ParseError(String message, int code, ParseNode node)
	{
		this(message, code, 0, node.Token.getStartPos(), node.Token.getStartPos(), node.Token.getLength());
	}

	public ParseError(String message, int code, int line, int col, int pos, int length)
	{
		this.message = message;
		this.code = code;
		this.line = line;
		this.col = col;
		this.pos = pos;
		this.length = length;
	}
}

