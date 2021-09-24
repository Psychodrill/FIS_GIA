<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:output method="xml" encoding="utf-8"/>
	<xsl:template match="/">
		<xsl:processing-instruction name="mso-application">progid="Excel.Sheet"</xsl:processing-instruction>
		<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" xmlns:html="http://www.w3.org/TR/REC-html40">
			<ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
				<WindowHeight>10000</WindowHeight>
				<WindowWidth>15000</WindowWidth>
				<WindowTopX>0</WindowTopX>
				<WindowTopY>0</WindowTopY>
				<ProtectStructure>False</ProtectStructure>
				<ProtectWindows>False</ProtectWindows>
			</ExcelWorkbook>
			<Styles>
				<Style ss:ID="Default" ss:Name="Normal">
					<Alignment ss:Vertical="Bottom"/>
					<Borders/>
					<Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Color="#000000"/>
					<Interior/>
					<NumberFormat/>
					<Protection/>
				</Style>
				<Style ss:ID="s60" ss:Name="TableHeader">
					<Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<Font ss:FontName="Verdana" x:CharSet="204" x:Family="Swiss" ss:Size="9" ss:Color="#333399" ss:Bold="1"/>
					<Interior ss:Color="#FDE9D9" ss:Pattern="Solid"/>
				</Style>
				<Style ss:ID="s70" ss:Name="Data_0">
					<Alignment ss:Vertical="Bottom" ss:WrapText="1"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<Font ss:FontName="Verdana" x:CharSet="204" x:Family="Swiss" ss:Color="#000000"/>
					<Interior ss:Color="#EAF1DD" ss:Pattern="Solid"/>
				</Style>
				<Style ss:ID="s71" ss:Name="Data_0_date">
					<Alignment ss:Vertical="Bottom" ss:WrapText="1"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<Font ss:FontName="Verdana" x:CharSet="204" x:Family="Swiss" ss:Color="#000000"/>
					<Interior ss:Color="#EAF1DD" ss:Pattern="Solid"/>
					<NumberFormat ss:Format="[$-FC19]dd\ mmmm\ yyyy\ \г\.;@"/>
				</Style>
				<Style ss:ID="s72" ss:Name="Data_1">
					<Alignment ss:Vertical="Bottom" ss:WrapText="1"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<Font ss:FontName="Verdana" x:CharSet="204" x:Family="Swiss" ss:Color="#000000"/>
					<Interior ss:Color="#DBEEF3" ss:Pattern="Solid"/>
				</Style>
				<Style ss:ID="s73" ss:Name="Data_1_date">
					<Alignment ss:Vertical="Bottom" ss:WrapText="1"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<Font ss:FontName="Verdana" x:CharSet="204" x:Family="Swiss" ss:Color="#000000"/>
					<Interior ss:Color="#DBEEF3" ss:Pattern="Solid"/>
					<NumberFormat ss:Format="[$-FC19]dd\ mmmm\ yyyy\ \г\.;@"/>
				</Style>
                <Style ss:ID="s75">
                    <Alignment ss:Vertical="Bottom" ss:WrapText="1"/>
                </Style>
                <Style ss:ID="s76" ss:Parent="s60">
                    <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
                    <Borders>
                        <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
                        <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
                        <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
                        <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
                    </Borders>
                    <Font ss:FontName="Verdana" x:CharSet="204" x:Family="Swiss" ss:Size="9"
                     ss:Color="#333399" ss:Bold="1"/>
                    <Interior ss:Color="#FDE9D9" ss:Pattern="Solid"/>
                </Style>
				<Style ss:ID="header">
					<Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="20"
					 ss:Color="#000000"/>
				</Style>
			</Styles>
			<xsl:for-each select="Tables/Table">
			<Worksheet>
				<xsl:attribute name="ss:Name">
					<xsl:value-of select="position()"/>
				</xsl:attribute>
				<Table>
					<xsl:if test="@name='[Отчет о регистрации пользователей за 24 часа]'">
                        <Column ss:AutoFitWidth="0" ss:Width="87"/>
                        <Column ss:AutoFitWidth="0" ss:Width="113.25"/>
                        <Column ss:AutoFitWidth="0" ss:Width="68.25"/>
                        <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                        <Column ss:AutoFitWidth="0" ss:Width="82.5"/>
                        <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                        <Column ss:AutoFitWidth="0" ss:Width="80.25"/>
                        <Column ss:AutoFitWidth="0" ss:Width="77.25"/>
						<!--<Column ss:AutoFitWidth="0" ss:Width="77.25"/>
						<Column ss:AutoFitWidth="0" ss:Width="93"/>
						<Column ss:AutoFitWidth="0" ss:Width="88.5"/>
						<Column ss:AutoFitWidth="0" ss:Width="82.5"/>
						<Column ss:AutoFitWidth="0" ss:Width="65.25" ss:Span="1"/>
						<Column ss:Index="7" ss:AutoFitWidth="0" ss:Width="84.75"/>
						<Column ss:AutoFitWidth="0" ss:Width="78.75"/>-->
					</xsl:if>
					<xsl:if test="position()=2">
						<Column ss:Width="180"/>
						<Column ss:Width="36"/>
						<Column ss:Width="123"/>
						<Column ss:Width="130.5"/>
						<Column ss:Width="111.75"/>
						<Column ss:Width="116.25"/>
						<Column ss:Width="114.75"/>
					</xsl:if>
					<xsl:if test="position()=3">
						<Column ss:Width="110.25"/>
						<Column ss:Width="213"/>
						<Column ss:Width="157.5" ss:Span="1"/>
						<Column ss:Width="111"/>
						<Column ss:Width="116.25"/>
						<Column ss:Width="140.25"/>
						<Column ss:Width="204.75"/>
						<Column ss:Width="209.25"/>
						<Column ss:Width="233.25"/>
					</xsl:if>
					<xsl:if test="position()=4">
						<Column ss:Width="99.75"/>
						<Column ss:Width="10.25"/>
						<Column ss:Width="126"/>
						<Column ss:Width="201.75"/>
						<Column ss:Width="165.75"/>
						<Column ss:Width="92.25"/>
						<Column ss:Width="167.25"/>
						<Column ss:Width="131.25"/>
						<Column ss:Width="116.25"/>
						<Column ss:Width="191.25"/>
						<Column ss:Width="155.25"/>
						<Column ss:Width="206.25"/>
						<Column ss:Width="195.75"/>
						<Column ss:Width="171.75"/>
					</xsl:if>
					<xsl:if test="position()=5">
						<Column ss:Width="32.25"/>
						<Column ss:Width="266.25"/>
						<Column ss:Width="154.5"/>
						<Column ss:Width="266.25"/>
						<Column ss:Width="170.25"/>
					</xsl:if>
					<xsl:if test="position()=6">
					   <Column ss:AutoFitWidth="0" ss:Width="75.75"/>
					   <Column ss:AutoFitWidth="0" ss:Width="69.75"/>
					   <Column ss:AutoFitWidth="0" ss:Width="190.5"/>
					   <Column ss:AutoFitWidth="0" ss:Width="255.75"/>
					   <Column ss:AutoFitWidth="0" ss:Width="137.25"/>
					   <Column ss:AutoFitWidth="0" ss:Width="118.5"/>
					</xsl:if>
					<xsl:call-template name="Table">
						<xsl:with-param name="tableNode" select="."/>
					</xsl:call-template>
				</Table>
			</Worksheet>
			</xsl:for-each>
		</Workbook>
	</xsl:template>
	<!--таблица-->
	<xsl:template name="Table" xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
		<xsl:param name="tableNode"/>
		<!--распечатываем название секции-->
		<Row ss:AutoFitHeight="0" ss:Height="30" ss:StyleID="header">
			<Cell>
				<Data ss:Type="String"><xsl:value-of select="$tableNode/@name"/></Data>
			</Cell>
		</Row>
		<!--распечатываем иерархический заголовок таблицы-->
		<xsl:call-template name="HierarhTableHeader">
			<xsl:with-param name="HeadersNode" select="$tableNode/Headers"/>
		</xsl:call-template>

		<!--распечатываем заголовок таблицы-->
		<xsl:call-template name="TableHeader">
			<xsl:with-param name="columnsNode" select="$tableNode/Columns"/>
		</xsl:call-template>
		<!--распечатываем данные-->
		<xsl:call-template name="DataNode">
			<xsl:with-param name="columnsNode" select="$tableNode/Columns"/>
			<xsl:with-param name="dataNode" select="$tableNode/Data"/>
		</xsl:call-template>
		<Row/>
	</xsl:template>

	<!--Иерархический заголовок таблицы-->
	<xsl:template name="HierarhTableHeader" xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
		<xsl:param name="HeadersNode"/>
		<xsl:for-each select="$HeadersNode/Level">
			<Row>
				<xsl:for-each select="Cell">
					<xsl:variable name="MergeAcross" select="-1+@columnSpan"/>
					<Cell ss:MergeAcross="{$MergeAcross}" ss:StyleID="s60" >
						<Data ss:Type="String">
							<xsl:value-of select="@name"/>
						</Data>
					</Cell>
				</xsl:for-each>
			</Row>
		</xsl:for-each>
	</xsl:template>
	
	
	<!--Заголовок таблицы-->
	<xsl:template name="TableHeader" xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
		<xsl:param name="columnsNode"/>
		<Row ss:Height="22.5" ss:StyleID="s75">
			<xsl:for-each select="$columnsNode/Column">
				<Cell ss:StyleID="s76">
					<Data ss:Type="String">
						<xsl:value-of select="@userName"/>
					</Data>
				</Cell>
			</xsl:for-each>
		</Row>
	</xsl:template>
	<!--распечатываем данные таблицы-->
	<xsl:template name="DataNode" xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
		<xsl:param name="columnsNode"/>
		<xsl:param name="dataNode"/>
		<xsl:for-each select="$dataNode/Row">
			<Row>
				<xsl:variable name="rowNumberType" select="position() mod 2"/>
				<xsl:for-each select="./Cell">
					<xsl:variable name="cellNumber" select="position()"/>
					<xsl:variable name="cellType">
							<xsl:call-template name="GetExcelCellType">
								<xsl:with-param name="dotNetType" select="$columnsNode/Column[position()=$cellNumber]/@type"/>
							</xsl:call-template>
						</xsl:variable>
					<xsl:variable name="cellStyleID">
						<xsl:choose>
							<xsl:when test="$cellType!='DateTime' and $rowNumberType=0">s70</xsl:when>
							<xsl:when test="$cellType='DateTime' and $rowNumberType=0">s71</xsl:when>
							<xsl:when test="$cellType!='DateTime' and $rowNumberType!=0">s72</xsl:when>
							<xsl:when test="$cellType='DateTime' and $rowNumberType!=0">s73</xsl:when>
						</xsl:choose>
					</xsl:variable>
					<Cell ss:StyleID="{$cellStyleID}">
						<xsl:if test="."><xsl:if test="string-length(.)>0">
							<Data ss:Type="{$cellType}">
								<xsl:value-of select="."/>
							</Data>
						</xsl:if></xsl:if>
					</Cell>
				</xsl:for-each>
			</Row>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="GetExcelCellType">
		<xsl:param name="dotNetType"/>
		<xsl:choose>
			<xsl:when test="$dotNetType='System.String'">String</xsl:when>
			<xsl:when test="$dotNetType='System.DateTime'">DateTime</xsl:when>
			<xsl:when test="$dotNetType='System.Int32'">Number</xsl:when>
			<xsl:when test="$dotNetType='System.Decimal'">Number</xsl:when>
			<xsl:otherwise>String</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
