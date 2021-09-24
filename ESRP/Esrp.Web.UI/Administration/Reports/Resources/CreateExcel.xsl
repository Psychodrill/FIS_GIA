<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
    <xsl:output method="xml" encoding="utf-8"/>
    <xsl:template match="/">
        <xsl:processing-instruction name="mso-application">progid="Excel.Sheet"</xsl:processing-instruction>
        <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" xmlns:html="http://www.w3.org/TR/REC-html40">
            <ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
                <WindowHeight>10000</WindowHeight>
                <WindowWidth>20000</WindowWidth>
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
                        <xsl:if test="@code='ReportUserStatusTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="87"/>
                            <Column ss:AutoFitWidth="0" ss:Width="113.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="68.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="82.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="80.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="77.25"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportUserStatusWithAccredTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="95"/>
                            <Column ss:AutoFitWidth="0" ss:Width="113.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="68.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="82.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="80.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="77.25"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportUserStatusWithAccredTVF_New'">
                            <Column ss:AutoFitWidth="0" ss:Width="95"/>
                            <Column ss:AutoFitWidth="0" ss:Width="113.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="68.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="68.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="82.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="80.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="77.25"/>
                        </xsl:if>

                        <xsl:if test="@code='ReportTotalChecksTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="130.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="127.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="121.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="120"/>
                            <Column ss:AutoFitWidth="0" ss:Width="124.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="135.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="120"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportChecksByPeriodTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="130.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="127.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="121.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="120"/>
                            <Column ss:AutoFitWidth="0" ss:Width="124.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="135.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="120"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportTotalChecksTVF_New'">
                            <Column ss:AutoFitWidth="0" ss:Width="130.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="127.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="121.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="120"/>
                            <Column ss:AutoFitWidth="0" ss:Width="124.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="135.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="120"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportOrgsStatusWithAccredTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="95.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="113.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="84.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="82.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="79.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="80.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="77.25"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportCertificateLoadTVF'">
                            <Column ss:Width="110.25"/>
                            <Column ss:Width="213"/>
                            <Column ss:Width="157.5"/>
                            <Column ss:Width="157.5"/>
                            <Column ss:Width="157.5"/>
                            <Column ss:Width="111"/>
                            <Column ss:Width="116.25"/>
                            <Column ss:Width="140.25"/>
                            <Column ss:Width="204.75"/>
                            <Column ss:Width="209.25"/>
                            <Column ss:Width="233.25"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportCheckStatisticsTVF'">
                            <Column ss:Width="99.75" />
                            <Column ss:Width="215.25" />
                            <Column ss:Width="126" />
                            <Column ss:Width="201.75" />
                            <Column ss:Width="165.75" />
                            <Column ss:Width="92.25" />
                            <Column ss:Width="167.25" />
                            <Column ss:Width="131.25" />
                            <Column ss:Width="116.25" />
                            <Column ss:Width="191.25" />
                            <Column ss:Width="155.25" />
                            <Column ss:Width="206.25" />
                            <Column ss:Width="195.75" />
                            <Column ss:Width="171.75" />
                            <Column ss:Width="194.25"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportTopCheckingOrganizationsTVF'">
                            <Column ss:Width="32.25"/>
                            <Column ss:Width="266.25"/>
                            <Column ss:Width="154.5"/>
                            <Column ss:Width="266.25"/>
                            <Column ss:Width="170.25"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportPotentialAbusersTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="75.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="69.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="190.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="255.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="137.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="118.5"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportCheckedCNEsTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="93.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="114.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="82.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="97.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="120.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="106.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="105.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="118.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="106.5"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportCheckedCNEsAggregatedTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="83.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="91.5"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportOrgsInfoByRegionTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="222.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="189.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="84"/>
                            <Column ss:AutoFitWidth="0" ss:Width="73.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="77.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="47.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="38.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="78"/>
                            <Column ss:Index="11" ss:AutoFitWidth="0" ss:Width="83.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="123"/>
                            <Column ss:AutoFitWidth="0" ss:Width="78"/>
                            <Column ss:AutoFitWidth="0" ss:Width="82.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="80.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="120"/>
                            <Column ss:AutoFitWidth="0" ss:Width="60"/>
                            <Column ss:AutoFitWidth="0" ss:Width="147"/>
                            <Column ss:AutoFitWidth="0" ss:Width="126"/>
                            <Column ss:AutoFitWidth="0" ss:Width="64.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="78"/>
                            <Column ss:Width="48.75" ss:Span="5"/>
                            <Column ss:Index="28" ss:AutoFitWidth="0" ss:Width="96"/>
                            <Column ss:AutoFitWidth="0" ss:Width="93.75"/>
                            <Column ss:Width="48.75" ss:Span="8"/>
                            <Column ss:Index="39" ss:AutoFitWidth="0" ss:Width="55.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="63.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="69.75"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportEditedOrgsTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="318"/>
                            <Column ss:AutoFitWidth="0" ss:Width="69.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="39.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="40.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="59.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="54.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="57.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="92.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="117.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="78.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="86.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="91.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="194.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="136.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="134.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="63.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="115.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="71.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="92.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="81"/>
                            <Column ss:AutoFitWidth="0" ss:Width="84.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="91.5"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportOrgsInfoTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="318"/>
                            <Column ss:AutoFitWidth="0" ss:Width="69.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="39.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="40.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="59.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="54.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="57.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="92.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="117.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="78.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="86.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="91.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="194.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="136.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="134.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="63.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="115.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="71.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="92.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="81"/>
                            <Column ss:AutoFitWidth="0" ss:Width="84.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="91.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="98.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="85.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="101.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="104.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="95.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="98.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="114.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="132.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="133.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="176.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="153"/>
                            <Column ss:AutoFitWidth="0" ss:Width="183"/>
                            <Column ss:AutoFitWidth="0" ss:Width="148.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="146.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="136.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="90.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="83.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="99"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportNotRegistredOrgsTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="317.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="223.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="108"/>
                            <Column ss:AutoFitWidth="0" ss:Width="45.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="56.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="66.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="61.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="92.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="117.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="141.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="78"/>
                            <Column ss:AutoFitWidth="0" ss:Width="110.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="196.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="242.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="174.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="156.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="51.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="74.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="128.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="69"/>
                            <Column ss:AutoFitWidth="0" ss:Width="84.75"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportRegistrationShortTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="156.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="122.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="119.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="105.75"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportCheckedCNEsDetailedTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="96"/>
                            <Column ss:AutoFitWidth="0" ss:Width="117.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="77.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="297.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="140.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="117.75"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportChecksByOrgsTVF'">
                            <Column ss:AutoFitWidth="0" ss:Width="168"/>
                            <Column ss:AutoFitWidth="0" ss:Width="270.75"/>
                            <Column ss:Index="4" ss:AutoFitWidth="0" ss:Width="113.25"/>
                            <Column ss:AutoFitWidth="0" ss:Width="70.5"/>
                            <Column ss:AutoFitWidth="0" ss:Width="72.75"/>
                            <Column ss:AutoFitWidth="0" ss:Width="75"/>
                        </xsl:if>
                        <xsl:if test="@code='ReportXMLSubordinateOrg'">
                           <Column ss:AutoFitWidth="0" ss:Width="50"/>
                           <Column ss:AutoFitWidth="0" ss:Width="300"/>
                           <Column ss:AutoFitWidth="0" ss:Width="50"/>
                           <Column ss:AutoFitWidth="0" ss:Width="150"/>
                           <Column ss:AutoFitWidth="0" ss:Width="150"/>
                           <Column ss:AutoFitWidth="0" ss:Width="150"/>
                           <Column ss:AutoFitWidth="0" ss:Width="75"/>
                           <Column ss:AutoFitWidth="0" ss:Width="100"/>
                           <Column ss:AutoFitWidth="0" ss:Width="75"/>
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
                <Data ss:Type="String">
                    <xsl:value-of select="$tableNode/@name"/>
                </Data>
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
                        <xsl:if test=".">
                            <xsl:if test="string-length(.)>0">
                                <Data ss:Type="{$cellType}">
                                    <xsl:value-of select="."/>
                                </Data>
                            </xsl:if>
                        </xsl:if>
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
