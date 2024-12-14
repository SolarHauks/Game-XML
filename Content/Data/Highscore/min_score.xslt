<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/scores">
        <scores>
            <best_time>
                <xsl:value-of select="temps[1]"/>
            </best_time>
        </scores>
    </xsl:template>

</xsl:stylesheet>