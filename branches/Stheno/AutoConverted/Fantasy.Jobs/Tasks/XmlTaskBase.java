package Fantasy.Jobs.Tasks;

public abstract class XmlTaskBase extends ObjectWithSite implements ITask
{

	public XmlTaskBase()
	{
		this.setEncoding("utf-8");
		this.setConformanceLevel(getConformanceLevel().Document);
		this.setIndent(false);
		this.setIndentChars("  ");
		this.setNamespaceHandling(getNamespaceHandling().Default);
		this.setNewLineChars("\r\n");
		this.setNewLineHandling(getNewLineHandling().Replace);
		this.setNewLineOnAttributes(false);
		this.setOmitXmlDeclaration(false);
	}

	private XmlWriterSettings _xmlWriterSettings;
	protected final XmlWriterSettings getXmlWriterSettings()
	{
		if (_xmlWriterSettings == null)
		{
			XmlWriterSettings tempVar = new XmlWriterSettings();
			tempVar.CloseOutput = true;
			tempVar.Encoding = System.Text.Encoding.GetEncoding(this.getEncoding());
			tempVar.Indent = this.getIndent();
			tempVar.IndentChars = this.getIndentChars();
			tempVar.CheckCharacters = this.getCheckCharacters();
			tempVar.ConformanceLevel = this.getConformanceLevel();
			tempVar.NewLineChars = this.getNewLineChars();
			tempVar.NewLineHandling = this.getNewLineHandling();
			tempVar.NewLineOnAttributes = this.getNewLineOnAttributes();
			tempVar.OmitXmlDeclaration = this.getOmitXmlDeclaration();
			tempVar.NamespaceHandling = this.getNamespaceHandling();
			_xmlWriterSettings = tempVar;
		}

		return _xmlWriterSettings;
	}


	public abstract boolean Execute();


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("encoding", Description="The type of text encoding to use.")]
	private String privateEncoding;
	public final String getEncoding()
	{
		return privateEncoding;
	}
	public final void setEncoding(String value)
	{
		privateEncoding = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("checkCharacters", Description="Sets a valude indicating whether to do character checking.")]
	private boolean privateCheckCharacters;
	public final boolean getCheckCharacters()
	{
		return privateCheckCharacters;
	}
	public final void setCheckCharacters(boolean value)
	{
		privateCheckCharacters = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("conformanceLevel", Description="The level of conformance which the XmlWriter compiles with.")]
	private ConformanceLevel privateConformanceLevel;
	public final ConformanceLevel getConformanceLevel()
	{
		return privateConformanceLevel;
	}
	public final void setConformanceLevel(ConformanceLevel value)
	{
		privateConformanceLevel = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("indent", Description="A value indicating whether to indent elements.")]
	private boolean privateIndent;
	public final boolean getIndent()
	{
		return privateIndent;
	}
	public final void setIndent(boolean value)
	{
		privateIndent = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("indentChars", Description=" The character string to use when indenting. This setting is used when the 'indent' property is set to true.")]
	private String privateIndentChars;
	public final String getIndentChars()
	{
		return privateIndentChars;
	}
	public final void setIndentChars(String value)
	{
		privateIndentChars = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("namespaceHandling", Description="A value indicates whether the XmlWriter should remove duplicate namespace declarations when writing XML content. The default behavior is for the writer to output all namespace declarations that are present in the writer's namespace resolver.")]
	private NamespaceHandling privateNamespaceHandling;
	public final NamespaceHandling getNamespaceHandling()
	{
		return privateNamespaceHandling;
	}
	public final void setNamespaceHandling(NamespaceHandling value)
	{
		privateNamespaceHandling = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("newLineChars", Description="The character string to use for line break.")]
	private String privateNewLineChars;
	public final String getNewLineChars()
	{
		return privateNewLineChars;
	}
	public final void setNewLineChars(String value)
	{
		privateNewLineChars = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("newLineHandling", Description="A value indicating whether to normalize line breaks in the output.")]
	private NewLineHandling privateNewLineHandling;
	public final NewLineHandling getNewLineHandling()
	{
		return privateNewLineHandling;
	}
	public final void setNewLineHandling(NewLineHandling value)
	{
		privateNewLineHandling = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("newLineOnAttributes", Description="A value indicating whether ot write attributes on a new line.")]
	private boolean privateNewLineOnAttributes;
	public final boolean getNewLineOnAttributes()
	{
		return privateNewLineOnAttributes;
	}
	public final void setNewLineOnAttributes(boolean value)
	{
		privateNewLineOnAttributes = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("omitXmlDeclaration", Description="A value indicating whether ot write an XML declaration.")]
	private boolean privateOmitXmlDeclaration;
	public final boolean getOmitXmlDeclaration()
	{
		return privateOmitXmlDeclaration;
	}
	public final void setOmitXmlDeclaration(boolean value)
	{
		privateOmitXmlDeclaration = value;
	}

}