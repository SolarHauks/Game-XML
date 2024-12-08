<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" indent="yes"/>
    
    <xsl:template match="//temps">
        <scores>
            <best_time>
                <xsl:sort data-type="number"/>
                <xsl:for-each select="//temps">
                    
                    <xsl:if test="position()=1"> <xsl:value-of select="."/> </xsl:if>
                </xsl:for-each>
            </best_time>
        </scores>
    </xsl:template>

</xsl:stylesheet>