package System.Linq.Dynamic;

public final class DynamicExpression
{
	public static Expression Parse(java.lang.Class resultType, String expression, Object... values)
	{
		ExpressionParser parser = new ExpressionParser(null, expression, values);
		return parser.Parse(resultType);
	}

	public static LambdaExpression ParseLambda(java.lang.Class itType, java.lang.Class resultType, String expression, Object... values)
	{
		return ParseLambda(new ParameterExpression[] { Expression.Parameter(itType, "") }, resultType, expression, values);
	}

	public static LambdaExpression ParseLambda(ParameterExpression[] parameters, java.lang.Class resultType, String expression, Object... values)
	{
		ExpressionParser parser = new ExpressionParser(parameters, expression, values);
		return Expression.Lambda(parser.Parse(resultType), parameters);
	}

	public static <T, S> Expression<Func<T, S>> ParseLambda(String expression, Object... values)
	{
		return (Expression<Func<T, S>>)ParseLambda(T.class, S.class, expression, values);
	}

	public static java.lang.Class CreateClass(DynamicProperty... properties)
	{
		return ClassFactory.Instance.GetDynamicClass(properties);
	}

	public static java.lang.Class CreateClass(Iterable<DynamicProperty> properties)
	{
		return ClassFactory.Instance.GetDynamicClass(properties);
	}
}