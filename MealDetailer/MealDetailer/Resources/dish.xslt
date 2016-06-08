<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
  <xsl:output method="html" encoding="utf-8" indent="no"/>

  <xsl:template match="/">
    <html>
      <body>
        <h2>
          <xsl:value-of select="./report/food/@name"/>
        </h2>

        <table border="1">
          <tr bgcolor="#eeeeee">
            <th>Name</th>
            <th>Amount in 100g</th>
          </tr>
          <xsl:for-each select="./report/food/nutrients/nutrient">
            <tr>
              <td>
                <xsl:value-of select="@name"/>
              </td>
              <td>
                <xsl:value-of select="@value"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>