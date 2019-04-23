<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="UTF-8" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />

	<xsl:template match="/">
		<html>
			<head>
				<style type="text/css">
					.html,
						body
					{
						overflow-x: hidden;
					}
					
					div.title
					{
						font-weight: bold;
						padding-bottom: 5px;
					}

					div.title a:visited,
						div.title a:hover,
						div.title a:active,
						div.title a:link
					{
						color: #1752b6;
						font-family: Microsoft Sans Serif;
						font-size: 12;
						font-weight: bold;
					}

					div.description
					{
						padding-bottom: 5px;
						width: 100%;
					}

					div.pubDate
					{
						font-size: smaller;
						padding-bottom: 20px;
						width: 100%;
					}
				</style>
			</head>
			<body>
				<xsl:for-each select="rss/channel/item">
					<div class="title">
						<a>
							<xsl:attribute name="href">
								<xsl:value-of select="link" />
							</xsl:attribute>
							<xsl:value-of select="title" />
						</a>
					</div>
					<div class="pubDate">
						<xsl:value-of select="pubDate" />
					</div>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>