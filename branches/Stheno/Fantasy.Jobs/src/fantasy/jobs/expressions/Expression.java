package fantasy.jobs.expressions;

public class Expression
{
	private ParseTree _tree;

	public Expression(String input)
	{
		Parser parser = new Parser(new Scanner());
		this._tree = parser.Parse(input);

	}

	public final boolean getSuccess()
	{
		return this._tree.Errors.size() == 0;
	}

	public final ParseError[] getErrors()
	{
		return this._tree.Errors.toArray(new ParseError[0]);
	}


	public final Object Eval() throws Exception
	{
		return this._tree.Eval();
	}

}