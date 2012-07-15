package System.Linq.Dynamic;

public class ExpressionParser
{
//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: struct Token
	private final static class Token
	{
		public TokenId id = TokenId.forValue(0);
		public String text;
		public int pos;

		public Token clone()
		{
			Token varCopy = new Token();

			varCopy.id = this.id;
			varCopy.text = this.text;
			varCopy.pos = this.pos;

			return varCopy;
		}
	}

	private enum TokenId
	{
		Unknown,
		End,
		Identifier,
		StringLiteral,
		IntegerLiteral,
		RealLiteral,
		Exclamation,
		Percent,
		Amphersand,
		OpenParen,
		CloseParen,
		Asterisk,
		Plus,
		Comma,
		Minus,
		Dot,
		Slash,
		Colon,
		LessThan,
		Equal,
		GreaterThan,
		Question,
		OpenBracket,
		CloseBracket,
		Bar,
		ExclamationEqual,
		DoubleAmphersand,
		LessThanEqual,
		LessGreater,
		DoubleEqual,
		GreaterThanEqual,
		DoubleBar;

		public int getValue()
		{
			return this.ordinal();
		}

		public static TokenId forValue(int value)
		{
			return values()[value];
		}
	}

	private interface ILogicalSignatures
	{
		void F(boolean x, boolean y);
		void F(Boolean x, Boolean y);
	}

	private interface IArithmeticSignatures
	{
		void F(int x, int y);
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: void F(uint x, uint y);
		void F(int x, int y);
		void F(long x, long y);
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: void F(ulong x, ulong y);
		void F(long x, long y);
		void F(float x, float y);
		void F(double x, double y);
		void F(java.math.BigDecimal x, java.math.BigDecimal y);
		void F(Integer x, Integer y);
		void F(Integer x, Integer y);
		void F(Long x, Long y);
		void F(Long x, Long y);
		void F(Float x, Float y);
		void F(Double x, Double y);
		void F(java.math.BigDecimal x, java.math.BigDecimal y);
	}

	private interface IRelationalSignatures extends IArithmeticSignatures
	{
		void F(String x, String y);
		void F(char x, char y);
		void F(java.util.Date x, java.util.Date y);
		void F(TimeSpan x, TimeSpan y);
		void F(Character x, Character y);
		void F(java.util.Date x, java.util.Date y);
		void F(TimeSpan x, TimeSpan y);
	}

	private interface IEqualitySignatures extends IRelationalSignatures
	{
		void F(boolean x, boolean y);
		void F(Boolean x, Boolean y);
		void F(Guid x, Guid y);
		void F(Guid x, Guid y);
	}

	private interface IAddSignatures extends IArithmeticSignatures
	{
		void F(java.util.Date x, TimeSpan y);
		void F(TimeSpan x, TimeSpan y);
		void F(java.util.Date x, TimeSpan y);
		void F(TimeSpan x, TimeSpan y);
	}

	private interface ISubtractSignatures extends IAddSignatures
	{
		void F(java.util.Date x, java.util.Date y);
		void F(java.util.Date x, java.util.Date y);
	}

	private interface INegationSignatures
	{
		void F(int x);
		void F(long x);
		void F(float x);
		void F(double x);
		void F(java.math.BigDecimal x);
		void F(Integer x);
		void F(Long x);
		void F(Float x);
		void F(Double x);
		void F(java.math.BigDecimal x);
	}

	private interface INotSignatures
	{
		void F(boolean x);
		void F(Boolean x);
	}

	private interface IEnumerableSignatures
	{
		void Where(boolean predicate);
		void Any();
		void Any(boolean predicate);
		void All(boolean predicate);
		void Count();
		void Count(boolean predicate);
		void Min(Object selector);
		void Max(Object selector);
		void Sum(int selector);
		void Sum(Integer selector);
		void Sum(long selector);
		void Sum(Long selector);
		void Sum(float selector);
		void Sum(Float selector);
		void Sum(double selector);
		void Sum(Double selector);
		void Sum(java.math.BigDecimal selector);
		void Sum(java.math.BigDecimal selector);
		void Average(int selector);
		void Average(Integer selector);
		void Average(long selector);
		void Average(Long selector);
		void Average(float selector);
		void Average(Float selector);
		void Average(double selector);
		void Average(Double selector);
		void Average(java.math.BigDecimal selector);
		void Average(java.math.BigDecimal selector);
	}

	private static final java.lang.Class[] predefinedTypes = { Object.class, Boolean.class, Character.class, String.class, Byte.class, Byte.class, Short.class, Short.class, Integer.class, Integer.class, Long.class, Long.class, Float.class, Double.class, java.math.BigDecimal.class, java.util.Date.class, TimeSpan.class, Guid.class, Math.class, Convert.class };

	private static final Expression trueLiteral = Expression.Constant(true);
	private static final Expression falseLiteral = Expression.Constant(false);
	private static final Expression nullLiteral = Expression.Constant(null);

	private static final String keywordIt = "it";
	private static final String keywordIif = "iif";
	private static final String keywordNew = "new";

	private static java.util.HashMap<String, Object> keywords;

	private java.util.HashMap<String, Object> symbols;
	private java.util.Map<String, Object> externals;
	private java.util.HashMap<Expression, String> literals;
	private ParameterExpression it;
	private String text;
	private int textPos;
	private int textLen;
	private char ch;
	private Token token;

	public ExpressionParser(ParameterExpression[] parameters, String expression, Object[] values)
	{
		if (expression == null)
		{
			throw new ArgumentNullException("expression");
		}
		if (keywords == null)
		{
			keywords = CreateKeywords();
		}
		symbols = new java.util.HashMap<String, Object>(StringComparer.OrdinalIgnoreCase);
		literals = new java.util.HashMap<Expression, String>();
		if (parameters != null)
		{
			ProcessParameters(parameters);
		}
		if (values != null)
		{
			ProcessValues(values);
		}
		text = expression;
		textLen = text.length();
		SetTextPos(0);
		NextToken();
	}

	private void ProcessParameters(ParameterExpression[] parameters)
	{
		for (ParameterExpression pe : parameters)
		{
			if (!DotNetToJavaStringHelper.isNullOrEmpty(pe.getName()))
			{
				AddSymbol(pe.getName(), pe);
			}
		}
		if (parameters.length == 1 && DotNetToJavaStringHelper.isNullOrEmpty(parameters[0].getName()))
		{
			it = parameters[0];
		}
	}

	private void ProcessValues(Object[] values)
	{
		for (int i = 0; i < values.length; i++)
		{
			Object value = values[i];
			if (i == values.length - 1 && value instanceof java.util.Map<String, Object>)
			{
				externals = (java.util.Map<String, Object>)value;
			}
			else
			{
				AddSymbol("@" + (new Integer(i)).ToString(System.Globalization.CultureInfo.InvariantCulture), value);
			}
		}
	}

	private void AddSymbol(String name, Object value)
	{
		if (symbols.containsKey(name))
		{
			throw ParseError(Res.DuplicateIdentifier, name);
		}
		symbols.put(name, value);
	}

	public final Expression Parse(java.lang.Class resultType)
	{
		int exprPos = token.pos;
		Expression expr = ParseExpression();
		if (resultType != null)
		{
			if ((expr = PromoteExpression(expr, resultType, true)) == null)
			{
				throw ParseError(exprPos, Res.ExpressionTypeMismatch, GetTypeName(resultType));
			}
		}
		ValidateToken(TokenId.End, Res.SyntaxError);
		return expr;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning disable 0219
	public final Iterable<DynamicOrdering> ParseOrdering()
	{
		java.util.ArrayList<DynamicOrdering> orderings = new java.util.ArrayList<DynamicOrdering>();
		while (true)
		{
			Expression expr = ParseExpression();
			boolean ascending = true;
			if (TokenIdentifierIs("asc") || TokenIdentifierIs("ascending"))
			{
				NextToken();
			}
			else if (TokenIdentifierIs("desc") || TokenIdentifierIs("descending"))
			{
				NextToken();
				ascending = false;
			}
			DynamicOrdering tempVar = new DynamicOrdering();
			tempVar.Selector = expr;
			tempVar.Ascending = ascending;
			orderings.add(tempVar);
			if (token.id != TokenId.Comma)
			{
				break;
			}
			NextToken();
		}
		ValidateToken(TokenId.End, Res.SyntaxError);
		return orderings;
	}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning restore 0219

	// ?: operator
	private Expression ParseExpression()
	{
		int errorPos = token.pos;
		Expression expr = ParseLogicalOr();
		if (token.id == TokenId.Question)
		{
			NextToken();
			Expression expr1 = ParseExpression();
			ValidateToken(TokenId.Colon, Res.ColonExpected);
			NextToken();
			Expression expr2 = ParseExpression();
			expr = GenerateConditional(expr, expr1, expr2, errorPos);
		}
		return expr;
	}

	// ||, or operator
	private Expression ParseLogicalOr()
	{
		Expression left = ParseLogicalAnd();
		while (token.id == TokenId.DoubleBar || TokenIdentifierIs("or"))
		{
			Token op = token.clone();
			NextToken();
			Expression right = ParseLogicalAnd();
			RefObject<Expression> tempRef_left = new RefObject<Expression>(left);
			RefObject<Expression> tempRef_right = new RefObject<Expression>(right);
			CheckAndPromoteOperands(ILogicalSignatures.class, op.text, tempRef_left, tempRef_right, op.pos);
			left = tempRef_left.argvalue;
			right = tempRef_right.argvalue;
			left = Expression.OrElse(left, right);
		}
		return left;
	}

	// &&, and operator
	private Expression ParseLogicalAnd()
	{
		Expression left = ParseComparison();
		while (token.id == TokenId.DoubleAmphersand || TokenIdentifierIs("and"))
		{
			Token op = token.clone();
			NextToken();
			Expression right = ParseComparison();
			RefObject<Expression> tempRef_left = new RefObject<Expression>(left);
			RefObject<Expression> tempRef_right = new RefObject<Expression>(right);
			CheckAndPromoteOperands(ILogicalSignatures.class, op.text, tempRef_left, tempRef_right, op.pos);
			left = tempRef_left.argvalue;
			right = tempRef_right.argvalue;
			left = Expression.AndAlso(left, right);
		}
		return left;
	}

	// =, ==, !=, <>, >, >=, <, <= operators
	private Expression ParseComparison()
	{
		Expression left = ParseAdditive();
		while (token.id == TokenId.Equal || token.id == TokenId.DoubleEqual || token.id == TokenId.ExclamationEqual || token.id == TokenId.LessGreater || token.id == TokenId.GreaterThan || token.id == TokenId.GreaterThanEqual || token.id == TokenId.LessThan || token.id == TokenId.LessThanEqual)
		{
			Token op = token.clone();
			NextToken();
			Expression right = ParseAdditive();
			boolean isEquality = op.id == TokenId.Equal || op.id == TokenId.DoubleEqual || op.id == TokenId.ExclamationEqual || op.id == TokenId.LessGreater;
			if (isEquality && !left.Type.IsValueType && !right.Type.IsValueType)
			{
				if (left.Type != right.Type)
				{
					if (left.Type.IsAssignableFrom(right.Type))
					{
						right = Expression.Convert(right, left.Type);
					}
					else if (right.Type.IsAssignableFrom(left.Type))
					{
						left = Expression.Convert(left, right.Type);
					}
					else
					{
						throw IncompatibleOperandsError(op.text, left, right, op.pos);
					}
				}
			}
			else if (IsEnumType(left.Type) || IsEnumType(right.Type))
			{
				if (left.Type != right.Type)
				{
					Expression e;
					if ((e = PromoteExpression(right, left.Type, true)) != null)
					{
						right = e;
					}
					else if ((e = PromoteExpression(left, right.Type, true)) != null)
					{
						left = e;
					}
					else
					{
						throw IncompatibleOperandsError(op.text, left, right, op.pos);
					}
				}
			}
			else
			{
				RefObject<Expression> tempRef_left = new RefObject<Expression>(left);
				RefObject<Expression> tempRef_right = new RefObject<Expression>(right);
				CheckAndPromoteOperands(isEquality ? IEqualitySignatures.class : IRelationalSignatures.class, op.text, tempRef_left, tempRef_right, op.pos);
				left = tempRef_left.argvalue;
				right = tempRef_right.argvalue;
			}
			switch (op.id)
			{
				case Equal:
				case DoubleEqual:
					left = GenerateEqual(left, right);
					break;
				case ExclamationEqual:
				case LessGreater:
					left = GenerateNotEqual(left, right);
					break;
				case GreaterThan:
					left = GenerateGreaterThan(left, right);
					break;
				case GreaterThanEqual:
					left = GenerateGreaterThanEqual(left, right);
					break;
				case LessThan:
					left = GenerateLessThan(left, right);
					break;
				case LessThanEqual:
					left = GenerateLessThanEqual(left, right);
					break;
			}
		}
		return left;
	}

	// +, -, & operators
	private Expression ParseAdditive()
	{
		Expression left = ParseMultiplicative();
		while (token.id == TokenId.Plus || token.id == TokenId.Minus || token.id == TokenId.Amphersand)
		{
			Token op = token.clone();
			NextToken();
			Expression right = ParseMultiplicative();
			switch (op.id)
			{
				case Plus:
					if (left.Type == String.class || right.Type == String.class)
					{
//C# TO JAVA CONVERTER TODO TASK: There is no 'goto' in Java:
						goto case TokenId.Amphersand;
					}
					RefObject<Expression> tempRef_left = new RefObject<Expression>(left);
					RefObject<Expression> tempRef_right = new RefObject<Expression>(right);
					CheckAndPromoteOperands(IAddSignatures.class, op.text, tempRef_left, tempRef_right, op.pos);
					left = tempRef_left.argvalue;
					right = tempRef_right.argvalue;
					left = GenerateAdd(left, right);
					break;
				case Minus:
					RefObject<Expression> tempRef_left2 = new RefObject<Expression>(left);
					RefObject<Expression> tempRef_right2 = new RefObject<Expression>(right);
					CheckAndPromoteOperands(ISubtractSignatures.class, op.text, tempRef_left2, tempRef_right2, op.pos);
					left = tempRef_left2.argvalue;
					right = tempRef_right2.argvalue;
					left = GenerateSubtract(left, right);
					break;
				case Amphersand:
					left = GenerateStringConcat(left, right);
					break;
			}
		}
		return left;
	}

	// *, /, %, mod operators
	private Expression ParseMultiplicative()
	{
		Expression left = ParseUnary();
		while (token.id == TokenId.Asterisk || token.id == TokenId.Slash || token.id == TokenId.Percent || TokenIdentifierIs("mod"))
		{
			Token op = token.clone();
			NextToken();
			Expression right = ParseUnary();
			RefObject<Expression> tempRef_left = new RefObject<Expression>(left);
			RefObject<Expression> tempRef_right = new RefObject<Expression>(right);
			CheckAndPromoteOperands(IArithmeticSignatures.class, op.text, tempRef_left, tempRef_right, op.pos);
			left = tempRef_left.argvalue;
			right = tempRef_right.argvalue;
			switch (op.id)
			{
				case Asterisk:
					left = Expression.Multiply(left, right);
					break;
				case Slash:
					left = Expression.Divide(left, right);
					break;
				case Percent:
				case Identifier:
					left = Expression.Modulo(left, right);
					break;
			}
		}
		return left;
	}

	// -, !, not unary operators
	private Expression ParseUnary()
	{
		if (token.id == TokenId.Minus || token.id == TokenId.Exclamation || TokenIdentifierIs("not"))
		{
			Token op = token.clone();
			NextToken();
			if (op.id == TokenId.Minus && (token.id == TokenId.IntegerLiteral || token.id == TokenId.RealLiteral))
			{
				token.text = "-" + token.text;
				token.pos = op.pos;
				return ParsePrimary();
			}
			Expression expr = ParseUnary();
			if (op.id == TokenId.Minus)
			{
				RefObject<Expression> tempRef_expr = new RefObject<Expression>(expr);
				CheckAndPromoteOperand(INegationSignatures.class, op.text, tempRef_expr, op.pos);
				expr = tempRef_expr.argvalue;
				expr = Expression.Negate(expr);
			}
			else
			{
				RefObject<Expression> tempRef_expr2 = new RefObject<Expression>(expr);
				CheckAndPromoteOperand(INotSignatures.class, op.text, tempRef_expr2, op.pos);
				expr = tempRef_expr2.argvalue;
				expr = Expression.Not(expr);
			}
			return expr;
		}
		return ParsePrimary();
	}

	private Expression ParsePrimary()
	{
		Expression expr = ParsePrimaryStart();
		while (true)
		{
			if (token.id == TokenId.Dot)
			{
				NextToken();
				expr = ParseMemberAccess(null, expr);
			}
			else if (token.id == TokenId.OpenBracket)
			{
				expr = ParseElementAccess(expr);
			}
			else
			{
				break;
			}
		}
		return expr;
	}

	private Expression ParsePrimaryStart()
	{
		switch (token.id)
		{
			case Identifier:
				return ParseIdentifier();
			case StringLiteral:
				return ParseStringLiteral();
			case IntegerLiteral:
				return ParseIntegerLiteral();
			case RealLiteral:
				return ParseRealLiteral();
			case OpenParen:
				return ParseParenExpression();
			default:
				throw ParseError(Res.ExpressionExpected);
		}
	}

	private Expression ParseStringLiteral()
	{
		ValidateToken(TokenId.StringLiteral);
		char quote = token.text.charAt(0);
		String s = token.text.substring(1, 1 + token.text.length() - 2);
		int start = 0;
		while (true)
		{
			int i = s.indexOf(quote, start);
			if (i < 0)
			{
				break;
			}
			s = s.substring(0, i) + s.substring(i + 1);
			start = i + 1;
		}
		if (quote == '\'')
		{
			if (s.length() != 1)
			{
				throw ParseError(Res.InvalidCharacterLiteral);
			}
			NextToken();
			return CreateLiteral(s.charAt(0), s);
		}
		NextToken();
		return CreateLiteral(s, s);
	}

	private Expression ParseIntegerLiteral()
	{
		ValidateToken(TokenId.IntegerLiteral);
		String text = token.text;
		if (text.charAt(0) != '-')
		{
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: ulong value;
			long value = 0;
			RefObject<Long> tempRef_value = new RefObject<Long>(value);
			boolean tempVar = !Long.TryParse(text, tempRef_value);
				value = tempRef_value.argvalue;
			if (tempVar)
			{
				throw ParseError(Res.InvalidIntegerLiteral, text);
			}
			NextToken();
			if (value <= (long)Integer.MAX_VALUE)
			{
				return CreateLiteral((int)value, text);
			}
			if (value <= (long)Integer.MAX_VALUE)
			{
				return CreateLiteral((int)value, text);
			}
			if (value <= (long)Long.MAX_VALUE)
			{
				return CreateLiteral((long)value, text);
			}
			return CreateLiteral(value, text);
		}
		else
		{
			long value = 0;
			RefObject<Long> tempRef_value2 = new RefObject<Long>(value);
			boolean tempVar2 = !Long.TryParse(text, tempRef_value2);
				value = tempRef_value2.argvalue;
			if (tempVar2)
			{
				throw ParseError(Res.InvalidIntegerLiteral, text);
			}
			NextToken();
			if (value >= Integer.MIN_VALUE && value <= Integer.MAX_VALUE)
			{
				return CreateLiteral((int)value, text);
			}
			return CreateLiteral(value, text);
		}
	}

	private Expression ParseRealLiteral()
	{
		ValidateToken(TokenId.RealLiteral);
		String text = token.text;
		Object value = null;
		char last = text.charAt(text.length() - 1);
		if (last == 'F' || last == 'f')
		{
			float f = 0F;
			RefObject<Float> tempRef_f = new RefObject<Float>(f);
			boolean tempVar = Float.TryParse(text.substring(0, text.length() - 1), tempRef_f);
				f = tempRef_f.argvalue;
			if (tempVar)
			{
				value = f;
			}
		}
		else
		{
			double d = 0;
			RefObject<Double> tempRef_d = new RefObject<Double>(d);
			boolean tempVar2 = Double.TryParse(text, tempRef_d);
				d = tempRef_d.argvalue;
			if (tempVar2)
			{
				value = d;
			}
		}
		if (value == null)
		{
			throw ParseError(Res.InvalidRealLiteral, text);
		}
		NextToken();
		return CreateLiteral(value, text);
	}

	private Expression CreateLiteral(Object value, String text)
	{
		ConstantExpression expr = Expression.Constant(value);
		literals.put(expr, text);
		return expr;
	}

	private Expression ParseParenExpression()
	{
		ValidateToken(TokenId.OpenParen, Res.OpenParenExpected);
		NextToken();
		Expression e = ParseExpression();
		ValidateToken(TokenId.CloseParen, Res.CloseParenOrOperatorExpected);
		NextToken();
		return e;
	}

	private Expression ParseIdentifier()
	{
		ValidateToken(TokenId.Identifier);
		Object value = null;
		if ((value = keywords.get(token.text)) != null)
		{
			if (value instanceof java.lang.Class)
			{
				return ParseTypeAccess((java.lang.Class)value);
			}
			if (value == (Object)keywordIt)
			{
				return ParseIt();
			}
			if (value == (Object)keywordIif)
			{
				return ParseIif();
			}
			if (value == (Object)keywordNew)
			{
				return ParseNew();
			}
			NextToken();
			return (Expression)value;
		}
		if ((value = symbols.get(token.text)) != null || externals != null && (value = externals.get(token.text)) != null)
		{
			Expression expr = (Expression)((value instanceof Expression) ? value : null);
			if (expr == null)
			{
				expr = Expression.Constant(value);
			}
			else
			{
				LambdaExpression lambda = (LambdaExpression)((expr instanceof LambdaExpression) ? expr : null);
				if (lambda != null)
				{
					return ParseLambdaInvocation(lambda);
				}
			}
			NextToken();
			return expr;
		}
		if (it != null)
		{
			return ParseMemberAccess(null, it);
		}
		throw ParseError(Res.UnknownIdentifier, token.text);
	}

	private Expression ParseIt()
	{
		if (it == null)
		{
			throw ParseError(Res.NoItInScope);
		}
		NextToken();
		return it;
	}

	private Expression ParseIif()
	{
		int errorPos = token.pos;
		NextToken();
		Expression[] args = ParseArgumentList();
		if (args.length != 3)
		{
			throw ParseError(errorPos, Res.IifRequiresThreeArgs);
		}
		return GenerateConditional(args[0], args[1], args[2], errorPos);
	}

	private Expression GenerateConditional(Expression test, Expression expr1, Expression expr2, int errorPos)
	{
		if (test.Type != Boolean.class)
		{
			throw ParseError(errorPos, Res.FirstExprMustBeBool);
		}
		if (expr1.Type != expr2.Type)
		{
			Expression expr1as2 = expr2 != nullLiteral ? PromoteExpression(expr1, expr2.Type, true) : null;
			Expression expr2as1 = expr1 != nullLiteral ? PromoteExpression(expr2, expr1.Type, true) : null;
			if (expr1as2 != null && expr2as1 == null)
			{
				expr1 = expr1as2;
			}
			else if (expr2as1 != null && expr1as2 == null)
			{
				expr2 = expr2as1;
			}
			else
			{
				String type1 = expr1 != nullLiteral ? expr1.Type.getName() : "null";
				String type2 = expr2 != nullLiteral ? expr2.Type.getName() : "null";
				if (expr1as2 != null && expr2as1 != null)
				{
					throw ParseError(errorPos, Res.BothTypesConvertToOther, type1, type2);
				}
				throw ParseError(errorPos, Res.NeitherTypeConvertsToOther, type1, type2);
			}
		}
		return Expression.Condition(test, expr1, expr2);
	}

	private Expression ParseNew()
	{
		NextToken();
		ValidateToken(TokenId.OpenParen, Res.OpenParenExpected);
		NextToken();
		java.util.ArrayList<DynamicProperty> properties = new java.util.ArrayList<DynamicProperty>();
		java.util.ArrayList<Expression> expressions = new java.util.ArrayList<Expression>();
		while (true)
		{
			int exprPos = token.pos;
			Expression expr = ParseExpression();
			String propName;
			if (TokenIdentifierIs("as"))
			{
				NextToken();
				propName = GetIdentifier();
				NextToken();
			}
			else
			{
				MemberExpression me = (MemberExpression)((expr instanceof MemberExpression) ? expr : null);
				if (me == null)
				{
					throw ParseError(exprPos, Res.MissingAsClause);
				}
				propName = me.Member.getName();
			}
			expressions.add(expr);
			properties.add(new DynamicProperty(propName, expr.Type));
			if (token.id != TokenId.Comma)
			{
				break;
			}
			NextToken();
		}
		ValidateToken(TokenId.CloseParen, Res.CloseParenOrCommaExpected);
		NextToken();
		java.lang.Class type = DynamicExpression.CreateClass(properties);
		MemberBinding[] bindings = new MemberBinding[properties.size()];
		for (int i = 0; i < bindings.length; i++)
		{
			bindings[i] = Expression.Bind(type.GetProperty(properties.get(i).getName()), expressions.get(i));
		}
		return Expression.MemberInit(Expression.New(type), bindings);
	}

	private Expression ParseLambdaInvocation(LambdaExpression lambda)
	{
		int errorPos = token.pos;
		NextToken();
		Expression[] args = ParseArgumentList();
		MethodBase method = null;
		RefObject<MethodBase> tempRef_method = new RefObject<MethodBase>(method);
		boolean tempVar = FindMethod(lambda.Type, "Invoke", false, args, tempRef_method) != 1;
			method = tempRef_method.argvalue;
		if (tempVar)
		{
			throw ParseError(errorPos, Res.ArgsIncompatibleWithLambda);
		}
		return Expression.invoke(lambda, args);
	}

	private Expression ParseTypeAccess(java.lang.Class type)
	{
		int errorPos = token.pos;
		NextToken();
		if (token.id == TokenId.Question)
		{
			if (!type.IsValueType || IsNullableType(type))
			{
				throw ParseError(errorPos, Res.TypeHasNoNullableForm, GetTypeName(type));
			}
			type = .class.MakeGenericType(type);
			NextToken();
		}
		if (token.id == TokenId.OpenParen)
		{
			Expression[] args = ParseArgumentList();
			MethodBase method = null;
			switch (FindBestMethod(type.getConstructors(), args, method))
			{
				case 0:
					if (args.length == 1)
					{
						return GenerateConversion(args[0], type, errorPos);
					}
					throw ParseError(errorPos, Res.NoMatchingConstructor, GetTypeName(type));
				case 1:
					return Expression.New((java.lang.reflect.Constructor)method, args);
				default:
					throw ParseError(errorPos, Res.AmbiguousConstructorInvocation, GetTypeName(type));
			}
		}
		ValidateToken(TokenId.Dot, Res.DotOrOpenParenExpected);
		NextToken();
		return ParseMemberAccess(type, null);
	}

	private Expression GenerateConversion(Expression expr, java.lang.Class type, int errorPos)
	{
		java.lang.Class exprType = expr.Type;
		if (exprType == type)
		{
			return expr;
		}
		if (exprType.IsValueType && type.IsValueType)
		{
			if ((IsNullableType(exprType) || IsNullableType(type)) && GetNonNullableType(exprType) == GetNonNullableType(type))
			{
				return Expression.Convert(expr, type);
			}
			if ((IsNumericType(exprType) || IsEnumType(exprType)) && (IsNumericType(type)) || IsEnumType(type))
			{
				return Expression.ConvertChecked(expr, type);
			}
		}
		if (exprType.IsAssignableFrom(type) || type.IsAssignableFrom(exprType) || exprType.IsInterface || type.IsInterface)
		{
			return Expression.Convert(expr, type);
		}
		throw ParseError(errorPos, Res.CannotConvertValue, GetTypeName(exprType), GetTypeName(type));
	}

	private Expression ParseMemberAccess(java.lang.Class type, Expression instance)
	{
		if (instance != null)
		{
			type = instance.Type;
		}
		int errorPos = token.pos;
		String id = GetIdentifier();
		NextToken();
		if (token.id == TokenId.OpenParen)
		{
			if (instance != null && type != String.class)
			{
				java.lang.Class enumerableType = FindGenericType(Iterable<>.class, type);
				if (enumerableType != null)
				{
					java.lang.Class elementType = enumerableType.GetGenericArguments()[0];
					return ParseAggregate(instance, elementType, id, errorPos);
				}
			}
			Expression[] args = ParseArgumentList();
			MethodBase mb = null;
			switch (FindMethod(type, id, instance == null, args, mb))
			{
				case 0:
					throw ParseError(errorPos, Res.NoApplicableMethod, id, GetTypeName(type));
				case 1:
					java.lang.reflect.Method method = (java.lang.reflect.Method)mb;
					if (!IsPredefinedType(method.DeclaringType))
					{
						throw ParseError(errorPos, Res.MethodsAreInaccessible, GetTypeName(method.DeclaringType));
					}
					if (method.ReturnType == void.class)
					{
						throw ParseError(errorPos, Res.MethodIsVoid, id, GetTypeName(method.DeclaringType));
					}
					return Expression.Call(instance, (java.lang.reflect.Method)method, args);
				default:
					throw ParseError(errorPos, Res.AmbiguousMethodInvocation, id, GetTypeName(type));
			}
		}
		else
		{
			MemberInfo member = FindPropertyOrField(type, id, instance == null);
			if (member == null)
			{
				throw ParseError(errorPos, Res.UnknownPropertyOrField, id, GetTypeName(type));
			}
			return member instanceof PropertyInfo ? Expression.Property(instance, (PropertyInfo)member) : Expression.Field(instance, (java.lang.reflect.Field)member);
		}
	}

	private static java.lang.Class FindGenericType(java.lang.Class generic, java.lang.Class type)
	{
		while (type != null && type != Object.class)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
			{
				return type;
			}
			if (generic.IsInterface)
			{
				for (java.lang.Class intfType : type.GetInterfaces())
				{
					java.lang.Class found = FindGenericType(generic, intfType);
					if (found != null)
					{
						return found;
					}
				}
			}
			type = type.getSuperclass();
		}
		return null;
	}

	private Expression ParseAggregate(Expression instance, java.lang.Class elementType, String methodName, int errorPos)
	{
		ParameterExpression outerIt = it;
		ParameterExpression innerIt = Expression.Parameter(elementType, "");
		it = innerIt;
		Expression[] args = ParseArgumentList();
		it = outerIt;
		MethodBase signature = null;
		RefObject<MethodBase> tempRef_signature = new RefObject<MethodBase>(signature);
		boolean tempVar = FindMethod(IEnumerableSignatures.class, methodName, false, args, tempRef_signature) != 1;
			signature = tempRef_signature.argvalue;
		if (tempVar)
		{
			throw ParseError(errorPos, Res.NoApplicableAggregate, methodName);
		}
		java.lang.Class[] typeArgs;
		if (signature.getName().equals("Min") || signature.getName().equals("Max"))
		{
			typeArgs = new java.lang.Class[] { elementType, args[0].Type };
		}
		else
		{
			typeArgs = new java.lang.Class[] { elementType };
		}
		if (args.length == 0)
		{
			args = new Expression[] { instance };
		}
		else
		{
			args = new Expression[] { instance, Expression.Lambda(args[0], innerIt) };
		}
		return Expression.Call(Enumerable.class, signature.getName(), typeArgs, args);
	}

	private Expression[] ParseArgumentList()
	{
		ValidateToken(TokenId.OpenParen, Res.OpenParenExpected);
		NextToken();
		Expression[] args = token.id != TokenId.CloseParen ? ParseArguments() : new Expression[0];
		ValidateToken(TokenId.CloseParen, Res.CloseParenOrCommaExpected);
		NextToken();
		return args;
	}

	private Expression[] ParseArguments()
	{
		java.util.ArrayList<Expression> argList = new java.util.ArrayList<Expression>();
		while (true)
		{
			argList.add(ParseExpression());
			if (token.id != TokenId.Comma)
			{
				break;
			}
			NextToken();
		}
		return argList.toArray(new Expression[]{});
	}

	private Expression ParseElementAccess(Expression expr)
	{
		int errorPos = token.pos;
		ValidateToken(TokenId.OpenBracket, Res.OpenParenExpected);
		NextToken();
		Expression[] args = ParseArguments();
		ValidateToken(TokenId.CloseBracket, Res.CloseBracketOrCommaExpected);
		NextToken();
		if (expr.Type.IsArray)
		{
			if (expr.Type.GetArrayRank() != 1 || args.length != 1)
			{
				throw ParseError(errorPos, Res.CannotIndexMultiDimArray);
			}
			Expression index = PromoteExpression(args[0], Integer.class, true);
			if (index == null)
			{
				throw ParseError(errorPos, Res.InvalidIndex);
			}
			return Expression.ArrayIndex(expr, index);
		}
		else
		{
			MethodBase mb = null;
			switch (FindIndexer(expr.Type, args, mb))
			{
				case 0:
					throw ParseError(errorPos, Res.NoApplicableIndexer, GetTypeName(expr.Type));
				case 1:
					return Expression.Call(expr, (java.lang.reflect.Method)mb, args);
				default:
					throw ParseError(errorPos, Res.AmbiguousIndexerInvocation, GetTypeName(expr.Type));
			}
		}
	}

	private static boolean IsPredefinedType(java.lang.Class type)
	{
		for (java.lang.Class t : predefinedTypes)
		{
			if (t == type)
			{
				return true;
			}
		}
		return false;
	}

	private static boolean IsNullableType(java.lang.Class type)
	{
		return type.IsGenericType && type.GetGenericTypeDefinition() == .class;
	}

	private static java.lang.Class GetNonNullableType(java.lang.Class type)
	{
		return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
	}

	private static String GetTypeName(java.lang.Class type)
	{
		java.lang.Class baseType = GetNonNullableType(type);
		String s = baseType.getName();
		if (type != baseType)
		{
			s += '?';
		}
		return s;
	}

	private static boolean IsNumericType(java.lang.Class type)
	{
		return GetNumericTypeKind(type) != 0;
	}

	private static boolean IsSignedIntegralType(java.lang.Class type)
	{
		return GetNumericTypeKind(type) == 2;
	}

	private static boolean IsUnsignedIntegralType(java.lang.Class type)
	{
		return GetNumericTypeKind(type) == 3;
	}

	private static int GetNumericTypeKind(java.lang.Class type)
	{
		type = GetNonNullableType(type);
		if (type.IsEnum)
		{
			return 0;
		}
		switch (java.lang.Class.GetTypeCode(type))
		{
			case TypeCode.Char:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return 1;
			case TypeCode.SByte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
				return 2;
			case TypeCode.Byte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
				return 3;
			default:
				return 0;
		}
	}

	private static boolean IsEnumType(java.lang.Class type)
	{
		return GetNonNullableType(type).IsEnum;
	}

	private void CheckAndPromoteOperand(java.lang.Class signatures, String opName, RefObject<Expression> expr, int errorPos)
	{
		Expression[] args = new Expression[] { expr.argvalue };
		MethodBase method = null;
		RefObject<MethodBase> tempRef_method = new RefObject<MethodBase>(method);
		boolean tempVar = FindMethod(signatures, "F", false, args, tempRef_method) != 1;
			method = tempRef_method.argvalue;
		if (tempVar)
		{
			throw ParseError(errorPos, Res.IncompatibleOperand, opName, GetTypeName(args[0].Type));
		}
		expr.argvalue = args[0];
	}

	private void CheckAndPromoteOperands(java.lang.Class signatures, String opName, RefObject<Expression> left, RefObject<Expression> right, int errorPos)
	{
		Expression[] args = new Expression[] { left.argvalue, right.argvalue };
		MethodBase method = null;
		RefObject<MethodBase> tempRef_method = new RefObject<MethodBase>(method);
		boolean tempVar = FindMethod(signatures, "F", false, args, tempRef_method) != 1;
			method = tempRef_method.argvalue;
		if (tempVar)
		{
			throw IncompatibleOperandsError(opName, left.argvalue, right.argvalue, errorPos);
		}
		left.argvalue = args[0];
		right.argvalue = args[1];
	}

	private RuntimeException IncompatibleOperandsError(String opName, Expression left, Expression right, int pos)
	{
		return ParseError(pos, Res.IncompatibleOperands, opName, GetTypeName(left.Type), GetTypeName(right.Type));
	}

	private MemberInfo FindPropertyOrField(java.lang.Class type, String memberName, boolean staticAccess)
	{
		BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
		for (java.lang.Class t : SelfAndBaseTypes(type))
		{
			MemberInfo[] members = t.FindMembers(MemberTypes.Property | MemberTypes.Field, flags, java.lang.Class.FilterNameIgnoreCase, memberName);
			if (members.length != 0)
			{
				return members[0];
			}
		}
		return null;
	}

	private int FindMethod(java.lang.Class type, String methodName, boolean staticAccess, Expression[] args, RefObject<MethodBase> method)
	{
		BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
		for (java.lang.Class t : SelfAndBaseTypes(type))
		{
			MemberInfo[] members = t.FindMembers(MemberTypes.Method, flags, java.lang.Class.FilterNameIgnoreCase, methodName);
			int count = FindBestMethod(members.<MethodBase>Cast(), args, method);
			if (count != 0)
			{
				return count;
			}
		}
		method.argvalue = null;
		return 0;
	}

	private int FindIndexer(java.lang.Class type, Expression[] args, RefObject<MethodBase> method)
	{
		for (java.lang.Class t : SelfAndBaseTypes(type))
		{
			MemberInfo[] members = t.GetDefaultMembers();
			if (members.length != 0)
			{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				Iterable<MethodBase> methods = members.<PropertyInfo>OfType().Select(p => (MethodBase)p.GetGetMethod()).Where(m => m != null);
				int count = FindBestMethod(methods, args, method);
				if (count != 0)
				{
					return count;
				}
			}
		}
		method.argvalue = null;
		return 0;
	}

	private static Iterable<java.lang.Class> SelfAndBaseTypes(java.lang.Class type)
	{
		if (type.IsInterface)
		{
			java.util.ArrayList<java.lang.Class> types = new java.util.ArrayList<java.lang.Class>();
			AddInterface(types, type);
			return types;
		}
		return SelfAndBaseClasses(type);
	}

	private static Iterable<java.lang.Class> SelfAndBaseClasses(java.lang.Class type)
	{
		while (type != null)
		{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
			yield return type;
			type = type.getSuperclass();
		}
	}

	private static void AddInterface(java.util.ArrayList<java.lang.Class> types, java.lang.Class type)
	{
		if (!types.contains(type))
		{
			types.add(type);
			for (java.lang.Class t : type.GetInterfaces())
			{
				AddInterface(types, t);
			}
		}
	}

	private static class MethodData
	{
		public MethodBase MethodBase;
		public ParameterInfo[] Parameters;
		public Expression[] Args;
	}

	private int FindBestMethod(Iterable<MethodBase> methods, Expression[] args, RefObject<MethodBase> method)
	{
		MethodData tempVar = new MethodData();
		tempVar.MethodBase = m;
		tempVar.Parameters = m.GetParameters();
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		MethodData[] applicable = methods.Select(m => tempVar).Where(m => IsApplicable(m, args)).toArray();
		if (applicable.length > 1)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			applicable = applicable.Where(m => applicable.All(n => m == n || IsBetterThan(args, m, n))).toArray();
		}
		if (applicable.length == 1)
		{
			MethodData md = applicable[0];
			for (int i = 0; i < args.length; i++)
			{
				args[i] = md.Args[i];
			}
			method.argvalue = md.MethodBase;
		}
		else
		{
			method.argvalue = null;
		}
		return applicable.length;
	}

	private boolean IsApplicable(MethodData method, Expression[] args)
	{
		if (method.Parameters.length != args.length)
		{
			return false;
		}
		Expression[] promotedArgs = new Expression[args.length];
		for (int i = 0; i < args.length; i++)
		{
			ParameterInfo pi = method.Parameters[i];
			if (pi.IsOut)
			{
				return false;
			}
			Expression promoted = PromoteExpression(args[i], pi.ParameterType, false);
			if (promoted == null)
			{
				return false;
			}
			promotedArgs[i] = promoted;
		}
		method.Args = promotedArgs;
		return true;
	}

	private Expression PromoteExpression(Expression expr, java.lang.Class type, boolean exact)
	{
		if (expr.Type == type)
		{
			return expr;
		}
		if (expr instanceof ConstantExpression)
		{
			ConstantExpression ce = (ConstantExpression)expr;
			if (ce == nullLiteral)
			{
				if (!type.IsValueType || IsNullableType(type))
				{
					return Expression.Constant(null, type);
				}
			}
			else
			{
				String text = null;
				if ((text = literals.get(ce)) != null)
				{
					java.lang.Class target = GetNonNullableType(type);
					Object value = null;
					switch (java.lang.Class.GetTypeCode(ce.Type))
					{
						case TypeCode.Int32:
						case TypeCode.UInt32:
						case TypeCode.Int64:
						case TypeCode.UInt64:
							value = ParseNumber(text, target);
							break;
						case TypeCode.Double:
							if (target == java.math.BigDecimal.class)
							{
								value = ParseNumber(text, target);
							}
							break;
						case TypeCode.String:
							value = ParseEnum(text, target);
							break;
					}
					if (value != null)
					{
						return Expression.Constant(value, type);
					}
				}
			}
		}
		if (IsCompatibleWith(expr.Type, type))
		{
			if (type.IsValueType || exact)
			{
				return Expression.Convert(expr, type);
			}
			return expr;
		}
		return null;
	}

	private static Object ParseNumber(String text, java.lang.Class type)
	{
		switch (java.lang.Class.GetTypeCode(GetNonNullableType(type)))
		{
			case TypeCode.SByte:
				byte sb = 0;
				RefObject<Byte> tempRef_sb = new RefObject<Byte>(sb);
				boolean tempVar = Byte.TryParse(text, tempRef_sb);
					sb = tempRef_sb.argvalue;
				if (tempVar)
				{
					return sb;
				}
				break;
			case TypeCode.Byte:
				byte b = 0;
				RefObject<Byte> tempRef_b = new RefObject<Byte>(b);
				boolean tempVar2 = Byte.TryParse(text, tempRef_b);
					b = tempRef_b.argvalue;
				if (tempVar2)
				{
					return b;
				}
				break;
			case TypeCode.Int16:
				short s = 0;
				RefObject<Short> tempRef_s = new RefObject<Short>(s);
				boolean tempVar3 = Short.TryParse(text, tempRef_s);
					s = tempRef_s.argvalue;
				if (tempVar3)
				{
					return s;
				}
				break;
			case TypeCode.UInt16:
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: ushort us;
				short us = 0;
				RefObject<Short> tempRef_us = new RefObject<Short>(us);
				boolean tempVar4 = Short.TryParse(text, tempRef_us);
					us = tempRef_us.argvalue;
				if (tempVar4)
				{
					return us;
				}
				break;
			case TypeCode.Int32:
				int i = 0;
				RefObject<Integer> tempRef_i = new RefObject<Integer>(i);
				boolean tempVar5 = Integer.TryParse(text, tempRef_i);
					i = tempRef_i.argvalue;
				if (tempVar5)
				{
					return i;
				}
				break;
			case TypeCode.UInt32:
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: uint ui;
				int ui = 0;
				RefObject<Integer> tempRef_ui = new RefObject<Integer>(ui);
				boolean tempVar6 = Integer.TryParse(text, tempRef_ui);
					ui = tempRef_ui.argvalue;
				if (tempVar6)
				{
					return ui;
				}
				break;
			case TypeCode.Int64:
				long l = 0;
				RefObject<Long> tempRef_l = new RefObject<Long>(l);
				boolean tempVar7 = Long.TryParse(text, tempRef_l);
					l = tempRef_l.argvalue;
				if (tempVar7)
				{
					return l;
				}
				break;
			case TypeCode.UInt64:
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: ulong ul;
				long ul = 0;
				RefObject<Long> tempRef_ul = new RefObject<Long>(ul);
				boolean tempVar8 = Long.TryParse(text, tempRef_ul);
					ul = tempRef_ul.argvalue;
				if (tempVar8)
				{
					return ul;
				}
				break;
			case TypeCode.Single:
				float f = 0F;
				RefObject<Float> tempRef_f = new RefObject<Float>(f);
				boolean tempVar9 = Float.TryParse(text, tempRef_f);
					f = tempRef_f.argvalue;
				if (tempVar9)
				{
					return f;
				}
				break;
			case TypeCode.Double:
				double d = 0;
				RefObject<Double> tempRef_d = new RefObject<Double>(d);
				boolean tempVar10 = Double.TryParse(text, tempRef_d);
					d = tempRef_d.argvalue;
				if (tempVar10)
				{
					return d;
				}
				break;
			case TypeCode.Decimal:
				java.math.BigDecimal e = new java.math.BigDecimal(0);
				RefObject<java.math.BigDecimal> tempRef_e = new RefObject<java.math.BigDecimal>(e);
				boolean tempVar11 = java.math.BigDecimal.TryParse(text, tempRef_e);
					e = tempRef_e.argvalue;
				if (tempVar11)
				{
					return e;
				}
				break;
		}
		return null;
	}

	private static Object ParseEnum(String name, java.lang.Class type)
	{
		if (type.IsEnum)
		{
			MemberInfo[] memberInfos = type.FindMembers(MemberTypes.Field, BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static, java.lang.Class.FilterNameIgnoreCase, name);
			if (memberInfos.length != 0)
			{
				return ((java.lang.reflect.Field)memberInfos[0]).GetValue(null);
			}
		}
		return null;
	}

	private static boolean IsCompatibleWith(java.lang.Class source, java.lang.Class target)
	{
		if (source == target)
		{
			return true;
		}
		if (!target.IsValueType)
		{
			return target.IsAssignableFrom(source);
		}
		java.lang.Class st = GetNonNullableType(source);
		java.lang.Class tt = GetNonNullableType(target);
		if (st != source && tt == target)
		{
			return false;
		}
		TypeCode sc = st.IsEnum ? TypeCode.Object : java.lang.Class.GetTypeCode(st);
		TypeCode tc = tt.IsEnum ? TypeCode.Object : java.lang.Class.GetTypeCode(tt);
		switch (sc)
		{
			case TypeCode.SByte:
				switch (tc)
				{
					case TypeCode.SByte:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return true;
				}
				break;
			case TypeCode.Byte:
				switch (tc)
				{
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return true;
				}
				break;
			case TypeCode.Int16:
				switch (tc)
				{
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return true;
				}
				break;
			case TypeCode.UInt16:
				switch (tc)
				{
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return true;
				}
				break;
			case TypeCode.Int32:
				switch (tc)
				{
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return true;
				}
				break;
			case TypeCode.UInt32:
				switch (tc)
				{
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return true;
				}
				break;
			case TypeCode.Int64:
				switch (tc)
				{
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return true;
				}
				break;
			case TypeCode.UInt64:
				switch (tc)
				{
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return true;
				}
				break;
			case TypeCode.Single:
				switch (tc)
				{
					case TypeCode.Single:
					case TypeCode.Double:
						return true;
				}
				break;
			default:
				if (st == tt)
				{
					return true;
				}
				break;
		}
		return false;
	}

	private static boolean IsBetterThan(Expression[] args, MethodData m1, MethodData m2)
	{
		boolean better = false;
		for (int i = 0; i < args.length; i++)
		{
			int c = CompareConversions(args[i].Type, m1.Parameters[i].ParameterType, m2.Parameters[i].ParameterType);
			if (c < 0)
			{
				return false;
			}
			if (c > 0)
			{
				better = true;
			}
		}
		return better;
	}

	// Return 1 if s -> t1 is a better conversion than s -> t2
	// Return -1 if s -> t2 is a better conversion than s -> t1
	// Return 0 if neither conversion is better
	private static int CompareConversions(java.lang.Class s, java.lang.Class t1, java.lang.Class t2)
	{
		if (t1 == t2)
		{
			return 0;
		}
		if (s == t1)
		{
			return 1;
		}
		if (s == t2)
		{
			return -1;
		}
		boolean t1t2 = IsCompatibleWith(t1, t2);
		boolean t2t1 = IsCompatibleWith(t2, t1);
		if (t1t2 && !t2t1)
		{
			return 1;
		}
		if (t2t1 && !t1t2)
		{
			return -1;
		}
		if (IsSignedIntegralType(t1) && IsUnsignedIntegralType(t2))
		{
			return 1;
		}
		if (IsSignedIntegralType(t2) && IsUnsignedIntegralType(t1))
		{
			return -1;
		}
		return 0;
	}

	private Expression GenerateEqual(Expression left, Expression right)
	{
		return Expression.Equal(left, right);
	}

	private Expression GenerateNotEqual(Expression left, Expression right)
	{
		return Expression.NotEqual(left, right);
	}

	private Expression GenerateGreaterThan(Expression left, Expression right)
	{
		if (left.Type == String.class)
		{
			return Expression.GreaterThan(GenerateStaticMethodCall("Compare", left, right), Expression.Constant(0));
		}
		return Expression.GreaterThan(left, right);
	}

	private Expression GenerateGreaterThanEqual(Expression left, Expression right)
	{
		if (left.Type == String.class)
		{
			return Expression.GreaterThanOrEqual(GenerateStaticMethodCall("Compare", left, right), Expression.Constant(0));
		}
		return Expression.GreaterThanOrEqual(left, right);
	}

	private Expression GenerateLessThan(Expression left, Expression right)
	{
		if (left.Type == String.class)
		{
			return Expression.LessThan(GenerateStaticMethodCall("Compare", left, right), Expression.Constant(0));
		}
		return Expression.LessThan(left, right);
	}

	private Expression GenerateLessThanEqual(Expression left, Expression right)
	{
		if (left.Type == String.class)
		{
			return Expression.LessThanOrEqual(GenerateStaticMethodCall("Compare", left, right), Expression.Constant(0));
		}
		return Expression.LessThanOrEqual(left, right);
	}

	private Expression GenerateAdd(Expression left, Expression right)
	{
		if (left.Type == String.class && right.Type == String.class)
		{
			return GenerateStaticMethodCall("Concat", left, right);
		}
		return Expression.Add(left, right);
	}

	private Expression GenerateSubtract(Expression left, Expression right)
	{
		return Expression.Subtract(left, right);
	}

	private Expression GenerateStringConcat(Expression left, Expression right)
	{
		return Expression.Call(null, String.class.GetMethod("Concat", new Object[] { Object.class, Object.class }), new Expression[] { left, right });
	}

	private java.lang.reflect.Method GetStaticMethod(String methodName, Expression left, Expression right)
	{
		return left.Type.GetMethod(methodName, new Object[] { left.Type, right.Type });
	}

	private Expression GenerateStaticMethodCall(String methodName, Expression left, Expression right)
	{
		return Expression.Call(null, GetStaticMethod(methodName, left, right), new Expression[] { left, right });
	}

	private void SetTextPos(int pos)
	{
		textPos = pos;
		ch = textPos < textLen ? text.charAt(textPos) : '\0';
	}

	private void NextChar()
	{
		if (textPos < textLen)
		{
			textPos++;
		}
		ch = textPos < textLen ? text.charAt(textPos) : '\0';
	}

	private void NextToken()
	{
		while (Character.isWhitespace(ch))
		{
			NextChar();
		}
		TokenId t;
		int tokenPos = textPos;
		switch (ch)
		{
			case '!':
				NextChar();
				if (ch == '=')
				{
					NextChar();
					t = TokenId.ExclamationEqual;
				}
				else
				{
					t = TokenId.Exclamation;
				}
				break;
			case '%':
				NextChar();
				t = TokenId.Percent;
				break;
			case '&':
				NextChar();
				if (ch == '&')
				{
					NextChar();
					t = TokenId.DoubleAmphersand;
				}
				else
				{
					t = TokenId.Amphersand;
				}
				break;
			case '(':
				NextChar();
				t = TokenId.OpenParen;
				break;
			case ')':
				NextChar();
				t = TokenId.CloseParen;
				break;
			case '*':
				NextChar();
				t = TokenId.Asterisk;
				break;
			case '+':
				NextChar();
				t = TokenId.Plus;
				break;
			case ',':
				NextChar();
				t = TokenId.Comma;
				break;
			case '-':
				NextChar();
				t = TokenId.Minus;
				break;
			case '.':
				NextChar();
				t = TokenId.Dot;
				break;
			case '/':
				NextChar();
				t = TokenId.Slash;
				break;
			case ':':
				NextChar();
				t = TokenId.Colon;
				break;
			case '<':
				NextChar();
				if (ch == '=')
				{
					NextChar();
					t = TokenId.LessThanEqual;
				}
				else if (ch == '>')
				{
					NextChar();
					t = TokenId.LessGreater;
				}
				else
				{
					t = TokenId.LessThan;
				}
				break;
			case '=':
				NextChar();
				if (ch == '=')
				{
					NextChar();
					t = TokenId.DoubleEqual;
				}
				else
				{
					t = TokenId.Equal;
				}
				break;
			case '>':
				NextChar();
				if (ch == '=')
				{
					NextChar();
					t = TokenId.GreaterThanEqual;
				}
				else
				{
					t = TokenId.GreaterThan;
				}
				break;
			case '?':
				NextChar();
				t = TokenId.Question;
				break;
			case '[':
				NextChar();
				t = TokenId.OpenBracket;
				break;
			case ']':
				NextChar();
				t = TokenId.CloseBracket;
				break;
			case '|':
				NextChar();
				if (ch == '|')
				{
					NextChar();
					t = TokenId.DoubleBar;
				}
				else
				{
					t = TokenId.Bar;
				}
				break;
			case '"':
			case '\'':
				char quote = ch;
				do
				{
					NextChar();
					while (textPos < textLen && ch != quote)
					{
						NextChar();
					}
					if (textPos == textLen)
					{
						throw ParseError(textPos, Res.UnterminatedStringLiteral);
					}
					NextChar();
				} while (ch == quote);
				t = TokenId.StringLiteral;
				break;
			default:
				if (Character.isLetter(ch) || ch == '@' || ch == '_')
				{
					do
					{
						NextChar();
					} while (Character.isLetterOrDigit(ch) || ch == '_');
					t = TokenId.Identifier;
					break;
				}
				if (Character.isDigit(ch))
				{
					t = TokenId.IntegerLiteral;
					do
					{
						NextChar();
					} while (Character.isDigit(ch));
					if (ch == '.')
					{
						t = TokenId.RealLiteral;
						NextChar();
						ValidateDigit();
						do
						{
							NextChar();
						} while (Character.isDigit(ch));
					}
					if (ch == 'E' || ch == 'e')
					{
						t = TokenId.RealLiteral;
						NextChar();
						if (ch == '+' || ch == '-')
						{
							NextChar();
						}
						ValidateDigit();
						do
						{
							NextChar();
						} while (Character.isDigit(ch));
					}
					if (ch == 'F' || ch == 'f')
					{
						NextChar();
					}
					break;
				}
				if (textPos == textLen)
				{
					t = TokenId.End;
					break;
				}
				throw ParseError(textPos, Res.InvalidCharacter, ch);
		}
		token.id = t;
		token.text = text.substring(tokenPos, textPos);
		token.pos = tokenPos;
	}

	private boolean TokenIdentifierIs(String id)
	{
		return token.id == TokenId.Identifier && String.equals(id, token.text, StringComparison.OrdinalIgnoreCase);
	}

	private String GetIdentifier()
	{
		ValidateToken(TokenId.Identifier, Res.IdentifierExpected);
		String id = token.text;
		if (id.length() > 1 && id.charAt(0) == '@')
		{
			id = id.substring(1);
		}
		return id;
	}

	private void ValidateDigit()
	{
		if (!Character.isDigit(ch))
		{
			throw ParseError(textPos, Res.DigitExpected);
		}
	}

	private void ValidateToken(TokenId t, String errorMessage)
	{
		if (token.id != t)
		{
			throw ParseError(errorMessage);
		}
	}

	private void ValidateToken(TokenId t)
	{
		if (token.id != t)
		{
			throw ParseError(Res.SyntaxError);
		}
	}

	private RuntimeException ParseError(String format, Object... args)
	{
		return ParseError(token.pos, format, args);
	}

	private RuntimeException ParseError(int pos, String format, Object... args)
	{
		return new ParseException(String.format(System.Globalization.CultureInfo.CurrentCulture, format, args), pos);
	}

	private static java.util.HashMap<String, Object> CreateKeywords()
	{
		java.util.HashMap<String, Object> d = new java.util.HashMap<String, Object>(StringComparer.OrdinalIgnoreCase);
		d.put("true", trueLiteral);
		d.put("false", falseLiteral);
		d.put("null", nullLiteral);
		d.put(keywordIt, keywordIt);
		d.put(keywordIif, keywordIif);
		d.put(keywordNew, keywordNew);
		for (java.lang.Class type : predefinedTypes)
		{
			d.put(type.getName(), type);
		}
		return d;
	}
}