package System.Linq.Dynamic;

// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.


public final class DynamicQueryable
{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IQueryable<T> Where<T>(this IQueryable<T> source, string predicate, params object[] values)
	public static <T> IQueryable<T> Where(IQueryable<T> source, String predicate, Object... values)
	{
		return (IQueryable<T>)Where((IQueryable)source, predicate, values);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IQueryable Where(this IQueryable source, string predicate, params object[] values)
	public static IQueryable Where(IQueryable source, String predicate, Object... values)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}
		LambdaExpression lambda = DynamicExpression.ParseLambda(source.ElementType, Boolean.class, predicate, values);
		return source.Provider.CreateQuery(Expression.Call(Queryable.class, "Where", new java.lang.Class[] { source.ElementType }, source.Expression, Expression.Quote(lambda)));
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IQueryable Select(this IQueryable source, string selector, params object[] values)
	public static IQueryable Select(IQueryable source, String selector, Object... values)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (selector == null)
		{
			throw new ArgumentNullException("selector");
		}
		LambdaExpression lambda = DynamicExpression.ParseLambda(source.ElementType, null, selector, values);
		return source.Provider.CreateQuery(Expression.Call(Queryable.class, "Select", new java.lang.Class[] { source.ElementType, lambda.Body.Type }, source.Expression, Expression.Quote(lambda)));
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, params object[] values)
	public static <T> IQueryable<T> OrderBy(IQueryable<T> source, String ordering, Object... values)
	{
		return (IQueryable<T>)OrderBy((IQueryable)source, ordering, values);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IQueryable OrderBy(this IQueryable source, string ordering, params object[] values)
	public static IQueryable OrderBy(IQueryable source, String ordering, Object... values)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (ordering == null)
		{
			throw new ArgumentNullException("ordering");
		}
		ParameterExpression[] parameters = new ParameterExpression[] { Expression.Parameter(source.ElementType, "") };
		ExpressionParser parser = new ExpressionParser(parameters, ordering, values);
		Iterable<DynamicOrdering> orderings = parser.ParseOrdering();
		Expression queryExpr = source.Expression;
		String methodAsc = "OrderBy";
		String methodDesc = "OrderByDescending";
		for (DynamicOrdering o : orderings)
		{
			queryExpr = Expression.Call(Queryable.class, o.Ascending ? methodAsc : methodDesc, new java.lang.Class[] { source.ElementType, o.Selector.Type }, queryExpr, Expression.Quote(Expression.Lambda(o.Selector, parameters)));
			methodAsc = "ThenBy";
			methodDesc = "ThenByDescending";
		}
		return source.Provider.CreateQuery(queryExpr);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IQueryable Take(this IQueryable source, int count)
	public static IQueryable Take(IQueryable source, int count)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return source.Provider.CreateQuery(Expression.Call(Queryable.class, "Take", new java.lang.Class[] { source.ElementType }, source.Expression, Expression.Constant(count)));
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IQueryable Skip(this IQueryable source, int count)
	public static IQueryable Skip(IQueryable source, int count)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return source.Provider.CreateQuery(Expression.Call(Queryable.class, "Skip", new java.lang.Class[] { source.ElementType }, source.Expression, Expression.Constant(count)));
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IQueryable GroupBy(this IQueryable source, string keySelector, string elementSelector, params object[] values)
	public static IQueryable GroupBy(IQueryable source, String keySelector, String elementSelector, Object... values)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (keySelector == null)
		{
			throw new ArgumentNullException("keySelector");
		}
		if (elementSelector == null)
		{
			throw new ArgumentNullException("elementSelector");
		}
		LambdaExpression keyLambda = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
		LambdaExpression elementLambda = DynamicExpression.ParseLambda(source.ElementType, null, elementSelector, values);
		return source.Provider.CreateQuery(Expression.Call(Queryable.class, "GroupBy", new java.lang.Class[] { source.ElementType, keyLambda.Body.Type, elementLambda.Body.Type }, source.Expression, Expression.Quote(keyLambda), Expression.Quote(elementLambda)));
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static bool Any(this IQueryable source)
	public static boolean Any(IQueryable source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return (boolean)source.Provider.Execute(Expression.Call(Queryable.class, "Any", new java.lang.Class[] { source.ElementType }, source.Expression));
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static int Count(this IQueryable source)
	public static int Count(IQueryable source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return (int)source.Provider.Execute(Expression.Call(Queryable.class, "Count", new java.lang.Class[] { source.ElementType }, source.Expression));
	}
}