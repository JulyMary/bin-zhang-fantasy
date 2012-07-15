package Fantasy.Jobs.Expressions;

public class Expression
{
	private ParseTree _tree;

	public Expression(String input)
	{
		Parser parser = new Parser(new Scanner());
		this._tree = parser.Parse(input);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		this._tree.InvokeFunction += new EventHandler<InvokeFunctionEventArgs>(Tree_InvokeFunction);


	}

	private void Tree_ResolveType(Object sender, ResolveTypeEventArgs e)
	{
		this.OnResolveType(e);
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<ResolveTypeEventArgs> ResolveType;

	protected void OnResolveType(ResolveTypeEventArgs e)
	{
		if (this.ResolveType != null)
		{
			this.ResolveType(this, e);
		}
	}

	public final boolean getSuccess()
	{
		return this._tree.Errors.size() == 0;
	}

	public final ParseError[] getErrors()
	{
		return this._tree.Errors.toArray();
	}

	private void Tree_InvokeFunction(Object sender, InvokeFunctionEventArgs e)
	{
		this.OnInvokeFunction(e);
	}


	public final Object Eval()
	{
		return this._tree.Eval();
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<InvokeFunctionEventArgs> InvokeFunction;

	protected void OnInvokeFunction(InvokeFunctionEventArgs e)
	{
		if(this.InvokeFunction != null)
		{
			this.InvokeFunction(this, e);
		}
	}







}