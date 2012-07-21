package fantasy.jobs.expressions;



public enum TokenType
{

		//Non terminal tokens:
		_NONE_(0),
		_UNDETERMINED_(1),

		//Non terminal tokens:
		Start(2),
		OrExpr(3),
		AndExpr(4),
		NotExpr(5),
		CompareExpr(6),
		AddExpr(7),
		MultExpr(8),
		CastExpr(9),
		NegetiveExpr(10),
		Atom(11),
		Number(12),
		Function(13),
		Arguments(14),

		//Terminal tokens:
		EOF(15),
		STRING(16),
		INTEGER(17),
		DECIMAL(18),
		HEX(19),
		PLUSMINUS(20),
		NEGETIVE(21),
		MULTDIV(22),
		BROPEN(23),
		BRCLOSE(24),
		IDENTITY(25),
		AND(26),
		OR(27),
		NOT(28),
		COMPARER(29),
		COMMA(30),
		DOT(31),
		BOOL(32),
		WHITESPACE(33),
		
		//
		Absurd(Integer.MAX_VALUE);

	private int intValue;
	private static java.util.HashMap<Integer, TokenType> mappings;
	private synchronized static java.util.HashMap<Integer, TokenType> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, TokenType>();
		}
		return mappings;
	}

	private TokenType(int value)
	{
		intValue = value;
		TokenType.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static TokenType forValue(int value)
	{
		return getMappings().get(value);
	}
}