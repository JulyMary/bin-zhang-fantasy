package Fantasy.Jobs.Expressions;

public class Token
{
	private int startpos;
	private int endpos;
	private String text;
	private Object value;

	public final int getStartPos()
	{
		return startpos;
	}
	public final void setStartPos(int value)
	{
		startpos = value;
	}

	public final int getLength()
	{
		return endpos - startpos;
	}

	public final int getEndPos()
	{
		return endpos;
	}
	public final void setEndPos(int value)
	{
		endpos = value;
	}

	public final String getText()
	{
		return text;
	}
	public final void setText(String value)
	{
		text = value;
	}

	public final Object getValue()
	{
		return value;
	}
	public final void setValue(Object value)
	{
		this.value = value;
	}

	public TokenType Type = TokenType.forValue(0);

	public Token()
	{
		this(0, 0);
	}

	public Token(int start, int end)
	{
		Type = TokenType._UNDETERMINED_;
		startpos = start;
		endpos = end;
		setText(""); // must initialize with empty string, may cause null reference exceptions otherwise
		setValue(null);
	}

	public final void UpdateRange(Token token)
	{
		if (token.getStartPos() < startpos)
		{
			startpos = token.getStartPos();
		}
		if (token.getEndPos() > endpos)
		{
			endpos = token.getEndPos();
		}
	}

	@Override
	public String toString()
	{
		if (getText() != null)
		{
			return Type.toString() + " '" + getText() + "'";
		}
		else
		{
			return Type.toString();
		}
	}
}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
	///#endregion
