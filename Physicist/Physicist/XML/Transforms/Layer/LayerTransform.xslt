<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    xmlns:types="PhysicistTypes"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

  <xsl:param name="xoffset"></xsl:param>
  <xsl:param name="yoffset"></xsl:param>

  <xsl:template match="Position | Location">
    <xsl:copy>
      <xsl:attribute name="x">
        <xsl:value-of select="@x + $xoffset"/>
      </xsl:attribute>
      <xsl:attribute name="y">
        <xsl:value-of select="@y + $yoffset"/>
      </xsl:attribute>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>
