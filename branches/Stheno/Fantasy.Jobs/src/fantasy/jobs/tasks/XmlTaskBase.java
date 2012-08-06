package fantasy.jobs.tasks;

import org.jdom2.output.Format;

import fantasy.*;
import fantasy.jobs.*;

public abstract class XmlTaskBase extends ObjectWithSite implements ITask
{

	
	private org.jdom2.output.Format _format;
	protected final org.jdom2.output.Format getFormat()
	{
		if (_format == null)
		{
			_format = Format.getPrettyFormat()
				.setEncoding(this.Encoding).
				setEscapeStrategy(new org.jdom2.output.EscapeStrategy(){

				@Override
				public boolean shouldEscape(char arg0) {
					return EscapeStrategy.indexOf(arg0) >= 0;
				}})
				.setExpandEmptyElements(this.ExpandEmptyElements)
				.setIndent(this.Indent)
				.setLineSeparator(this.LineSeperator)
				.setOmitDeclaration(this.OmitDeclaration)
				.setOmitEncoding(this.OmitEncoding)
				.setTextMode(this.TextMode);
			
		}

		return _format;
	}


	public abstract void Execute() throws Exception;


	@TaskMember(name="encoding", description="The configured output encoding.")
	public String Encoding = "UTF-8";
	

	@TaskMember(name="escapeStrategy", description="Characters are escaping.")
	public String EscapeStrategy = "";
	
	@TaskMember(name="expandEmptyElements", description="Whether empty elements are expanded.")
	public boolean ExpandEmptyElements = false;
	
	@TaskMember(name="indent", description="The indent string in use.")
	public String Indent = "    ";
	
	@TaskMember(name="lineSeperator", description="The current line separator.")
	public String LineSeperator = System.getProperty("line.separator");
	

	@TaskMember(name="omitDeclaration", description="Whether the XML declaration will be omitted.")
	public boolean OmitDeclaration;
	
	@TaskMember(name="omitEncoding", description="Whether the XML declaration encoding will be omitted.")
	public boolean OmitEncoding;
	
	@TaskMember(name="textMode", description="The current text output style.")
	public org.jdom2.output.Format.TextMode TextMode = org.jdom2.output.Format.TextMode.PRESERVE;


}