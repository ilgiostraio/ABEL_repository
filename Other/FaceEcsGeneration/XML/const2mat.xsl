<?xml version="1.0" encoding="utf-8" ?>
<!--this transform is for reading constants into MATLAB-->
<xsl:transform version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="utf-8" indent="yes"/>
  <xsl:strip-space elements="*" />
  <xsl:template match="/">
    <!--
    <xsl:variable name="a" select="//@name"></xsl:variable>-->
    <xsl:element name="constants">
      <xsl:for-each select="constants/constant">
        <xsl:element name="{@name}">
          <xsl:text>[</xsl:text>
          <xsl:for-each select="column">
		  <xsl:text>[</xsl:text>
            <xsl:for-each select="value">
              <xsl:value-of select="self::value"/>
              <xsl:text>;</xsl:text>
            </xsl:for-each>
			<xsl:text>]</xsl:text>
            <xsl:text> </xsl:text>
          </xsl:for-each>
          <xsl:text>]</xsl:text>
        </xsl:element>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>
</xsl:transform>
