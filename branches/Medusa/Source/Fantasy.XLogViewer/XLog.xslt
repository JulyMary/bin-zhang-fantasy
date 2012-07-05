<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl sc"
                xmlns:sc="urn:xsltscript"
>
    <xsl:output method="html" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="sc">
    <msxsl:assembly name="System.Web"/>
    <![CDATA[ 
    
        
        public string CreateAnchors(string text)
        {
         
            if (!string.IsNullOrEmpty(text))
            {
                string rs = ParseFile(ParseUri(text));
                return rs.Replace("\r", "").Replace("\n", "<br/>");
            }
            else
            {
                return text;
            }
        }
        
        private string ParseFile(string text)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(([a-z]:\\)|(\\\\))\S*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.StringBuilder rs = new System.Text.StringBuilder();

            int pos = 0;

            foreach (System.Text.RegularExpressions.Match match in regex.Matches(text))
            {
                string normal = text.Substring(pos, match.Index - pos);
                rs.Append(normal);
                Uri uri;
                if(  Uri.TryCreate(match.Value, UriKind.Absolute, out uri))
                {
                    string anchor = string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", uri.ToString(), match.Value);
                    rs.Append(anchor); 
                }
                else
                {
                    rs.Append(match.Value);
                }
                
                pos = match.Index + match.Length;
            }

            if (pos < text.Length)
            {
                rs.Append(text.Substring(pos));
            }
            return rs.ToString();
        }
        
        private string ParseUri(string text)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"((mailto\:)|([a-z]\w*\:\/\/))\S*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.StringBuilder rs = new System.Text.StringBuilder();

            int pos = 0;

            foreach (System.Text.RegularExpressions.Match match in regex.Matches(text))
            {
                string normal = text.Substring(pos, match.Index - pos);
                normal = System.Web.HttpUtility.HtmlEncode(normal);
                rs.Append(normal);
                string anchor = string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", match.Value);
                rs.Append(anchor); 
                pos = match.Index + match.Length;
            }

            if (pos < text.Length)
            {
                rs.Append(text.Substring(pos));
            }
            return rs.ToString();
           
        }
        
        public string ToLocalTime(string text)
        {
            return DateTime.Parse(text).ToLocalTime().ToString();
        }
    ]]>
  </msxsl:script>

  <xsl:param name="executeLocation"/>
  <xsl:template match="/">
    <html xmlns="http://www.w3.org/1999/xhtml" >
      <head>
        <title>xlog viewer</title>
        <style type="text/css">
          .message
          {
          font-size : 10pt;
          font-family : Courier New;
          }
          .alternative
          {
          font-size : 10pt;
          font-family : Courier New;
          background-color: #e6eed5;
          }

          .start
          {
          font-size : 10pt;
          font-family : Courier New;
          background-color: #ff5050;
          }

          .logtable
          {
          width:100%;
          border: thin solid #9bbb59;
          border-collapse:collapse;
          }
          td
          {
          border: 1px solid #9bbb59;
          padding:3px;
          }

          .head
          {
          background-color: #9bbb59;
          font-size: 14pt;
          font-weight: bold;
          font-family : Courier New;
          text-align : center;
          }
        </style>
        <script language="javascript" type="text/javascript">


          <xsl:text disable-output-escaping="yes">
         //&lt;[CDATA[
          <![CDATA[ 
              function toggleItems(name, checked) {
                var rules = document.styleSheets[0].rules;
                for (var i = 0; i < rules.length; i++) {
                    if (rules[i].selectorText == name) {
                        rules[i].style.display = checked ? "" : "none";
                        break;
                    }
                }
              }
              function toggleException(details) {
              
                var style = details.style;
                style.display = style.display == "none" ? "" : "none";
          
          }]]>
          
              // ]]&gt;
          </xsl:text>
         
        </script>
      </head>
      <body style="font-size: 9pt;font-family : Courier New; " alink="0">

        <!--<p>
          <input id="showErrors" type="checkbox" checked="checked"   onclick="toggleItems('.error', this.checked)" />Errors
          <input id="showMessage" type="checkbox" checked="checked" onclick="toggleItems('.message', this.checked)"/>Messages
          <input id="showWarning" type="checkbox" checked="checked" onclick="toggleItems('.warning', this.checked)" />Warnings
        </p>-->

        <table class="logtable" cellpadding="0" cellspacing="0">
          <thead>
            <tr class="head" >
              <td></td>
              <td width="100px" style="white-space: nowrap">Time</td>
              <td width="100px" style="white-space: nowrap">Category</td>
              <td width="100px" style="white-space: nowrap">Importance</td>
              <td width="100%">Message</td>
            </tr>
          </thead>
         
          <xsl:for-each select="xlog/*">
            <tr>
              <xsl:attribute name="class">
                <xsl:choose>
                  <xsl:when test ="name()='start'">start</xsl:when>
                  <xsl:when test="position() mod 2 = 1">message</xsl:when>
                  <xsl:when test="position() mod 2 = 0">alternative</xsl:when>
                </xsl:choose>
              </xsl:attribute>
              <td>
                
                <img width="16px" height="16px">
                  <xsl:attribute name="src">
                    <xsl:value-of select="$executeLocation"/>
                    <xsl:choose>
                        <xsl:when test="name()='message' or name()='start'">message.ico</xsl:when>
                        <xsl:when test="name()='error'">error.ico</xsl:when>
                        <xsl:when test="name()='warning'">warning.ico</xsl:when>
                      </xsl:choose>
                  </xsl:attribute>
                </img>
              </td>
              <td style="white-space: nowrap">
                <xsl:value-of select="sc:ToLocalTime(@time)"/>
              </td>
              <td style="white-space: nowrap">
                <xsl:value-of select="@category"/>
              </td>
              <td style="white-space: nowrap">
                <xsl:value-of select="@importance"/>
              </td>
              <td>
                <xsl:value-of select="sc:CreateAnchors(@text)" disable-output-escaping="yes" />
                <xsl:for-each select="exception">
                  <xsl:variable name="detailsId" select="generate-id()"/>
                  <div style="text-align: right"><a style="color:red">
                    <xsl:attribute name="href">
                      <xsl:value-of select="concat('javascript:toggleException(' , $detailsId, ')') "/>
                    </xsl:attribute>
                    Exception
                  </a></div>
                  <div style="display:none">
                    <xsl:attribute name="id" >
                      <xsl:value-of select="$detailsId"/>
                    </xsl:attribute>
                    <xsl:call-template name="showException" />
                    <xsl:for-each select="exception">
                      <div xmlns="http://www.w3.org/1999/xhtml" style="margin-left:10px">
                        <span style="font-weight: bold;">InnerException:</span>
                        <br/>
                      <xsl:call-template name="showException" />
                      </div>
                    </xsl:for-each>
                  </div>
                </xsl:for-each>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
  <xsl:template name="showException" >

      <xsl:for-each select="@*">
          <span style="font-weight: bold;">
              <xsl:value-of select="name()"/>
              <xsl:text>: </xsl:text>
          </span>
          <xsl:value-of select="sc:CreateAnchors(.)"
                        disable-output-escaping="yes"/>
          <br/>
      </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
