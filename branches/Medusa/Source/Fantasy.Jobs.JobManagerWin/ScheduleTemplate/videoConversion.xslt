<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl s"
                xmlns="urn:schemas-fantasy-com:jobs"
                xmlns:s="urn:schemas-fantasy-com:jobservice-schedule"
>

  <xsl:output method="xml" indent="yes" encoding="utf-8"/>
  <xsl:template match="/s:params">
    <jobStart template="24-7"
              startupTarget="ScheduleConvert">
      <properties>
        <application>24-7 Manager</application>
        <user><xsl:value-of select="s:author"/></user>
        <name>
          <xsl:value-of select="s:name"/> : <xsl:value-of select="s:scheduleTime"/>
        </name>
        <priority>
          <xsl:value-of select="s:priority"/>
        </priority>
        <waitAll>
          <xsl:value-of select="s:waitAll"/>
        </waitAll>
      </properties>
      <items>
        <src>
          <xsl:attribute name="name">
            <xsl:value-of select="s:custom/s:src"/>
          </xsl:attribute>
        </src>
      </items>
    </jobStart>
  </xsl:template>
</xsl:stylesheet>
