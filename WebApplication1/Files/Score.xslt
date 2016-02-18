<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <html>
      <body>
        <h2>Past Score</h2>
        <table border="1">
          <tr bgcolor="#9acd32">
            <th style="text-align:left">Date</th>
            <th style="text-align:left">Score</th>
          </tr>
          <xsl:for-each select="IndividualScore">
            <tr>
              <td>
                <xsl:value-of select="score" />
              </td>
              <td>
                <xsl:value-of select="title" />
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>