<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/scores">
        <scores>
            <xsl:apply-templates select="temps">
                <xsl:sort select="." data-type="number" order="ascending"/>
            </xsl:apply-templates>
        </scores>
    </xsl:template>

    <xsl:template match="temps">
        <temps>
            <xsl:value-of select="."/>
        </temps>
    </xsl:template>

</xsl:stylesheet>