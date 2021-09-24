using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using GVUZ.ServiceModel.Import.Package;


namespace GVUZ.ServiceModel.SQL
{
    public partial class InstitutionExporter
    {
        private XElement CompetitiveGroups()
        {
            #region

            /*
InstitutionExports\InstitutionExport\CompetitiveGroups
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\UID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CampaignUID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\UID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\BenefitKindID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\IsForAllOlympics
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\OlympicDiplomTypes
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\OlympicDiplomTypes\OlympicDiplomTypeID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\OlympicYear
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\OlympicsLevels
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\OlympicsLevels\Olympic
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\OlympicsLevels\Olympic\OlympicID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CommonBenefit\CommonBenefitItem\OlympicsLevels\Olympic\LevelID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\CompetitiveGroupID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Course
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\UID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\CompetitiveGroupItemID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\DirectionID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\EducationLevelID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\NumberBudgetO
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\NumberBudgetOZ
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\NumberBudgetZ
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\NumberPaidO
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\NumberPaidOZ
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\NumberPaidZ
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\NumberQuotaO
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\NumberQuotaOZ
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Items\CompetitiveGroupItem\NumberQuotaZ
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\Name
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\UID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\Items
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\Items\CompetitiveGroupTargetItem
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\Items\CompetitiveGroupTargetItem\UID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\Items\CompetitiveGroupTargetItem\DirectionID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\Items\CompetitiveGroupTargetItem\EducationLevelID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\Items\CompetitiveGroupTargetItem\NumberTargetO
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\Items\CompetitiveGroupTargetItem\NumberTargetOZ
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\Items\CompetitiveGroupTargetItem\NumberTargetZ
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\TargetOrganizations\TargetOrganization\TargetOrganizationName
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\UID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\UID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\BenefitKindID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\IsForAllOlympics
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\OlympicDiplomTypes
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\OlympicDiplomTypes\OlympicDiplomTypeID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\OlympicYear
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\OlympicsLevels
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\OlympicsLevels\Olympic
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\OlympicsLevels\Olympic\OlympicID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestBenefits\EntranceTestBenefitItem\OlympicsLevels\Olympic\LevelID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestSubject
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestSubject\SubjectID
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestSubject\SubjectName
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\Form
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\MinScore
InstitutionExports\InstitutionExport\CompetitiveGroups\CompetitiveGroup\EntranceTestItems\EntranceTestItem\EntranceTestPriority

			*/

            #endregion

            #region SQL

            //_filter.CompetitiveGroups = new[] { "136825", "136826" }; //TEST

            string resul = "";

            //_filter.CompetitiveGroups = new[] {"136790"};
            

            if (_filter != null)
            {
                if (_filter.CompetitiveGroups != null)
                {
                    if (_filter.CompetitiveGroups.Any())
                    {
                        resul = string.Format("AND cg.CompetitiveGroupID IN ('{0}')",
                            string.Join("','", _filter.CompetitiveGroups));
                    }
                }
            }
            string sql = string.Format(@"
----------------------------------------CompetitiveGroup------------------------------------------------------------------0 
                SELECT cg.UID, c.UID AS CampaignUID, cg.CompetitiveGroupID, cg.Course, cg.Name, 
				cg.DirectionID, cg.EducationLevelID, cg.EducationFormID, cg.EducationSourceID, cg.IsFromKrym, cg.IsAdditional, cg.ParentDirectionID
                FROM CompetitiveGroup (NOLOCK)  AS cg
                    LEFT JOIN Campaign (NOLOCK)  AS c
	                    ON c.CampaignID = cg.CampaignID
                WHERE cg.InstitutionID=@InstitutionID {0}

----------------------------------------CommonBenefit---------------------------------------------------------------------1
                SELECT bic.UID, bic.BenefitItemID AS BenefitKindID, bic.IsForAllOlympic, bic.OlympicDiplomTypeID, bic.CompetitiveGroupID,
                    bic.OlympicYear, ot.OlympicID, bic.IsCreative, bic.IsAthletic
                FROM BenefitItemC (NOLOCK)  AS bic
                    LEFT JOIN BenefitItemCOlympicType (NOLOCK)  AS bict
                        ON bict.BenefitItemID = bic.BenefitItemID
                    LEFT JOIN OlympicType (NOLOCK)  AS ot
                        ON ot.OlympicID = bict.OlympicTypeID
                    LEFT JOIN CompetitiveGroup (NOLOCK)  as cg
                        ON cg.CompetitiveGroupID = bic.CompetitiveGroupID
                WHERE cg.InstitutionID = @InstitutionID AND bic.EntranceTestItemID IS NULL

----------------------------------------Items-----------------------------------------------------------------------------2
                SELECT cgi.CompetitiveGroupID,cgi.CompetitiveGroupItemID, 
                    cgi.NumberBudgetO, cgi.NumberBudgetOZ, cgi.NumberBudgetZ, 
                    cgi.NumberPaidO, cgi.NumberPaidOZ, cgi.NumberPaidZ,
                    cgi.NumberQuotaO, cgi.NumberQuotaOZ, cgi.NumberQuotaZ,
                    cgi.NumberTargetO, cgi.NumberTargetOZ, cgi.NumberTargetZ
                FROM CompetitiveGroupItem (NOLOCK)  AS cgi
					INNER JOIN CompetitiveGroup (NOLOCK)  AS cg
						ON cg.CompetitiveGroupID = cgi.CompetitiveGroupID
                WHERE cg.InstitutionID=@InstitutionID

----------------------------------------CompetitiveGroupTargetItems------------------------------------------------------------3 
                SELECT cgt.UID, cgt.Name, cgt.CompetitiveGroupTargetID, cgti.CompetitiveGroupID
                FROM CompetitiveGroupTargetItem (NOLOCK)  AS cgti
					INNER JOIN CompetitiveGroupTarget (NOLOCK)  cgt
						ON cgt.CompetitiveGroupTargetID = cgti.CompetitiveGroupTargetID
					INNER JOIN CompetitiveGroup (NOLOCK)  AS cg
						ON cg.CompetitiveGroupID = cgti.CompetitiveGroupID
                WHERE cgt.InstitutionID=@InstitutionID 
                GROUP BY cgt.UID, cgt.Name, cgt.CompetitiveGroupTargetID, cgti.CompetitiveGroupID 

----------------------------------------TargetOrganizations---------------------------------------------------------------4
                SELECT 
                    cgti.NumberTargetO, cgti.NumberTargetOZ, cgti.NumberTargetZ, cgt.CompetitiveGroupTargetID, cgti.CompetitiveGroupID
                FROM CompetitiveGroupTarget (NOLOCK)  AS cgt
	                INNER JOIN CompetitiveGroupTargetItem (NOLOCK)  AS cgti
		                ON cgti.CompetitiveGroupTargetID = cgt.CompetitiveGroupTargetID
		            INNER JOIN CompetitiveGroupItem (NOLOCK)  AS cgi
		                ON cgi.CompetitiveGroupID = cgti.CompetitiveGroupID
                WHERE cgt.InstitutionID=@InstitutionID

----------------------------------------EntranceTestItems-----------------------------------------------------------------5
                SELECT etic.CompetitiveGroupID, etic.UID, bic.UID AS eUID, bic.BenefitItemID AS BenefitKindID,
                    bic.IsForAllOlympic, bic.OlympicDiplomTypeID, bic.OlympicYear, etic.SubjectID,                                 
                    s.Name AS SubjectName, etic.MinScore,etic.EntranceTestPriority, bic.EntranceTestItemID, etic.IsFirst, etic.IsSecond
                FROM EntranceTestItemC (NOLOCK)  AS etic
                    LEFT JOIN BenefitItemC (NOLOCK)  AS bic
                        ON bic.EntranceTestItemID = etic.EntranceTestItemID
                    LEFT JOIN BenefitItemCOlympicType (NOLOCK)  AS bict
                        ON bict.BenefitItemID = bic.BenefitItemID
                    LEFT JOIN [Subject] (NOLOCK)  AS s
                        ON s.SubjectID = etic.SubjectID
                    LEFT JOIN CompetitiveGroup (NOLOCK)  AS cg
                        ON cg.CompetitiveGroupID = etic.CompetitiveGroupID
                WHERE cg.InstitutionID = @InstitutionID
                GROUP BY etic.CompetitiveGroupID, etic.UID, bic.UID, bic.BenefitItemID, bic.IsForAllOlympic,
                    bic.OlympicDiplomTypeID, bic.OlympicYear, etic.SubjectID,                                       
                    s.Name, etic.MinScore,etic.EntranceTestPriority, bic.EntranceTestItemID, etic.IsFirst, etic.IsSecond

----------------------------------------Olympic--------------------------------------------------------------------------6
              --  SELECT otsl.OlympicID,
		            --CASE
			           -- WHEN otsl.SubjectLevelID IS NULL   THEN ot.OlympicLevelID
			           -- WHEN ot.OlympicLevelID IS NULL THEN otsl.SubjectLevelID 
              --      END AS LevelID
              --  FROM OlympicTypeSubjectLink (NOLOCK)  AS otsl
              --      INNER JOIN OlympicType (NOLOCK)  AS ot
		            --    ON ot.OlympicID = otsl.OlympicID
              --  GROUP BY otsl.OlympicID, otsl.SubjectLevelID, ot.OlympicLevelID
			  Select OlympicTypeID, isnull([OlympicLevelID], 1) as LevelID
			  From OlympicTypeProfile (NOLOCK)

----------------------------------------OlympicID------------------------------------------------------------------------7
                SELECT etic.CompetitiveGroupID, ot.OlympicID                                   
                FROM EntranceTestItemC (NOLOCK)  AS etic
                    LEFT JOIN BenefitItemC (NOLOCK)  AS bic
                        ON bic.EntranceTestItemID = etic.EntranceTestItemID
                    LEFT JOIN BenefitItemCOlympicType (NOLOCK)  AS bict
                        ON bict.BenefitItemID = bic.BenefitItemID
                    LEFT JOIN OlympicType (NOLOCK)  AS ot
                        ON ot.OlympicID = bict.OlympicTypeID
                    LEFT JOIN CompetitiveGroup (NOLOCK)  AS cg
                        ON cg.CompetitiveGroupID = etic.CompetitiveGroupID
                WHERE cg.InstitutionID = @InstitutionID
                AND bic.EntranceTestItemID IS NOT NULL 

----------------------------------------EntranceTestItems-----------------------------------------------------------------8
                SELECT etic.CompetitiveGroupID, etic.UID, etic.SubjectID, etic.SubjectName,
                        etic.MinScore, etic.EntranceTestPriority, bic.EntranceTestItemID, etic.IsFirst, etic.IsSecond
                FROM EntranceTestItemC (NOLOCK)  AS etic
                    LEFT JOIN BenefitItemC (NOLOCK)  AS bic
                        ON bic.EntranceTestItemID = etic.EntranceTestItemID
                    LEFT JOIN [Subject] (NOLOCK)  AS s
                        ON s.SubjectID = etic.SubjectID
                    LEFT JOIN CompetitiveGroup (NOLOCK)  AS cg
                        ON cg.CompetitiveGroupID = etic.CompetitiveGroupID
                WHERE cg.InstitutionID = @InstitutionID
                GROUP BY etic.CompetitiveGroupID, etic.UID, etic.SubjectID, etic.SubjectName, etic.MinScore, 
                    etic.EntranceTestPriority, bic.EntranceTestItemID, etic.IsFirst, etic.IsSecond

", resul);
            #endregion

            var com = getSqlCommand(sql);

            #region Parameters
            com.Parameters.Add(new SqlParameter("InstitutionID", SqlDbType.Int) {Value = this._institutionID});
            #endregion

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            var xCompetitiveGroups = new XElement("CompetitiveGroups");
            try
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow drRow = ds.Tables[0].Rows[i];
                    xCompetitiveGroups.Add(
                        new XElement("CompetitiveGroup",
                            new XElement("UID", Str2Str(drRow["UID"])),
                            new XElement("CampaignUID", Str2Str(drRow["CampaignUID"])),
                            new XElement(// "CommonBenefit",
                                XCommonBenefitItem(ds.Tables[1], ds.Tables[6], Convert.ToInt32(drRow["CompetitiveGroupID"] as Int32?), ds.Tables[7])),
                            new XElement("CompetitiveGroupID", Int2Str(drRow["CompetitiveGroupID"])),
                            new XElement("Course", Int2Str(drRow["Course"])),
                            new XElement(xItems(ds.Tables[2], Convert.ToInt32(drRow["CompetitiveGroupID"] as Int32?))),
                            new XElement("DirectionID", Int2Str(drRow["DirectionID"])),
                            new XElement("DirectionID", Int2Str(drRow["ParentDirectionID"])),
                            new XElement("EducationFormID", Int2Str(drRow["EducationFormID"])),
                            new XElement("EducationLevelID", Int2Str(drRow["EducationLevelID"])),
                            new XElement("EducationSourceID", Int2Str(drRow["EducationSourceID"])),
                            new XElement("IsAdditional", Int2Str(drRow["IsAdditional"])),
                            new XElement("IsFromKrym", Int2Str(drRow["IsFromKrym"])),
                            new XElement("Name", Str2Str(drRow["Name"])),
                            new XElement(xTargetOrganizations(ds.Tables[4], Convert.ToInt32(drRow["CompetitiveGroupID"] as Int32?), ds.Tables[3])),
                                xEntranceTestItems(ds.Tables[5], ds.Tables[8], ds.Tables[6], Convert.ToInt32(drRow["CompetitiveGroupID"] as Int32?), ds.Tables[7])));
                }
            }
            catch (Exception)
            {
                
            }
            return xCompetitiveGroups;
        }

        private XElement XCommonBenefitItem(DataTable cbiTable, DataTable oTable, int competitiveGroupId, DataTable oidTable)
        {
            var xCommonBenefitItem = new XElement("CommonBenefit");

            for (int i = 0; i < cbiTable.Rows.Count; i++)
            {
                DataRow cbi = cbiTable.Rows[i];
                if ((int) cbi["CompetitiveGroupID"] == competitiveGroupId)
                {
                    xCommonBenefitItem.Add(new XElement("CommonBenefitItem",
                        new XElement("UID", Str2Str(cbi["UID"])),
                        new XElement("BenefitKindID", Int2Str(cbi["BenefitKindID"])),
                        new XElement("IsForAllOlympic", Int2Str(cbi["IsForAllOlympic"])),
                        new XElement("OlympicDiplomTypes",
                            new XElement("OlympicDiplomTypeID", Int2Str(cbi["OlympicDiplomTypeID"]))),
                        new XElement("OlympicYear", Str2Str(cbi["OlympicYear"])),
                        new XElement("OlympicsLevels", XOlympic(oTable, competitiveGroupId, Convert.ToBoolean(cbi["IsForAllOlympic"] as bool?), oidTable)),
                        new XElement("IsCreative", Convert.ToBoolean(cbi["IsCreative"] as bool?)),
                        new XElement("IsAthletic", Convert.ToBoolean(cbi["IsAthletic"] as bool?))
                        )
                    );
                }
            }
            return xCommonBenefitItem;
        }

        private XElement xItems(DataTable cgiTable, int competitiveGroupId)
        {
            var xItems = new XElement("Items");

            for (int i = 0; i < cgiTable.Rows.Count; i++)
            {
                DataRow cgi = cgiTable.Rows[i];
                if ((int) cgi["CompetitiveGroupID"] == competitiveGroupId)
                {
                    xItems.Add(new XElement("CompetitiveGroupItem",
                        //new XElement("UID", Str2Str(cgi["UID"])),
                        new XElement("CompetitiveGroupItemID", Int2Str(cgi["CompetitiveGroupItemID"])),
                        //new XElement("DirectionID", Int2Str(cgi["DirectionID"])),
                        //new XElement("EducationLevelID", Int2Str(cgi["EducationLevelID"])),
                        new XElement("NumberBudgetO", Int2Str(cgi["NumberBudgetO"])),
                        new XElement("NumberBudgetOZ", Int2Str(cgi["NumberBudgetOZ"])),
                        new XElement("NumberBudgetZ", Int2Str(cgi["NumberBudgetZ"])),
                        new XElement("NumberPaidO", Int2Str(cgi["NumberPaidO"])),
                        new XElement("NumberPaidOZ", Int2Str(cgi["NumberPaidOZ"])),
                        new XElement("NumberPaidZ", Int2Str(cgi["NumberPaidZ"])),
                        new XElement("NumberQuotaO", Int2Str(cgi["NumberQuotaO"])),
                        new XElement("NumberQuotaOZ", Int2Str(cgi["NumberQuotaOZ"])),
                        new XElement("NumberQuotaZ", Int2Str(cgi["NumberQuotaZ"])),
                        new XElement("NumberTargetO", Int2Str(cgi["NumberTargetO"])),
                        new XElement("NumberTargetOZ", Int2Str(cgi["NumberTargetOZ"])),
                        new XElement("NumberTargetZ", Int2Str(cgi["NumberTargetZ"]))
                        ));
                }
            }
            return xItems;
        }

        private XElement xTargetOrganizations(DataTable toTable, int competitiveGroupId, DataTable cgiTable)
        {
            var xTargetOrganizations = new XElement("TargetOrganizations");
            
            for (int i = 0; i < cgiTable.Rows.Count; i++)
            {
                var xItems = new XElement("Items");
                DataRow cgi = cgiTable.Rows[i];
                if (Convert.ToInt32(cgi["CompetitiveGroupID"] as Int32?) == competitiveGroupId)
                {
                    for (int j = 0; j < toTable.Rows.Count; j++)
                    {
                        DataRow to = toTable.Rows[j];
                        if ((Convert.ToInt32(cgi["CompetitiveGroupTargetID"] as Int32?) == Convert.ToInt32(to["CompetitiveGroupTargetID"] as Int32?))
                            &&
                            (Convert.ToInt32(cgi["CompetitiveGroupID"] as Int32?) == Convert.ToInt32(to["CompetitiveGroupID"] as Int32?))
                            )
                        {
                            xItems.Add(new XElement("CompetitiveGroupTargetItem",
                                //new XElement("UID", O2Str(toTable.Rows[j]["UID"])),
                                //new XElement("DirectionID", O2Str(toTable.Rows[j]["DirectionID"])),
                                //new XElement("EducationLevelID", O2Str(toTable.Rows[j]["EducationLevelID"])),
                                new XElement("NumberTargetO", O2Str(toTable.Rows[j]["NumberTargetO"])),
                                new XElement("NumberTargetOZ", O2Str(toTable.Rows[j]["NumberTargetOZ"])),
                                new XElement("NumberTargetZ", O2Str(toTable.Rows[j]["NumberTargetZ"]))
                                ));
                        }
                    }
                    xTargetOrganizations.Add(new XElement("TargetOrganization",
                        new XElement("UID", O2Str(cgiTable.Rows[i]["UID"])),
                        xItems,
                        new XElement("TargetOrganizationName", O2Str(cgiTable.Rows[i]["Name"]))));
                }
            }
            return xTargetOrganizations;
        }

        private XElement xEntranceTestItems(DataTable etiTable, DataTable jetiTable, DataTable oTable, int competitiveGroupId,
            DataTable oidTable)
        {
            XElement xEntranceTestItems = new XElement("EntranceTestItems");
            for (int j = 0; j < jetiTable.Rows.Count; j++)
            {
                DataRow jeti = jetiTable.Rows[j];
                XElement xEntranceTestBenefits = new XElement("EntranceTestBenefits");

                if ((int)jeti["CompetitiveGroupID"] == competitiveGroupId)
                {
                    for (int i = 0; i < etiTable.Rows.Count; i++)
                    {

                        DataRow eti = etiTable.Rows[i];
                        if (eti["EntranceTestItemID"] as int? != null)
                        {
                            if (eti["EntranceTestItemID"] as int? == jeti["EntranceTestItemID"] as int?)
                            {
                                xEntranceTestBenefits.Add(new XElement("EntranceTestBenefitItem",
                                    new XElement("UID", O2Str(eti["eUID"])),
                                    new XElement("BenefitKindID", O2Str(eti["BenefitKindID"])),
                                    new XElement("IsForAllOlympic", Bool2Str(eti["IsForAllOlympic"])),
                                    new XElement("OlympicDiplomTypes",
                                        new XElement("OlympicDiplomTypeID", O2Str(eti["OlympicDiplomTypeID"]))),
                                    new XElement("OlympicYear", O2Str(eti["OlympicYear"])),
                                    new XElement("OlympicsLevels",
                                        XOlympic(oTable, competitiveGroupId,
                                            Convert.ToBoolean(eti["IsForAllOlympic"] as bool?),
                                            oidTable))));
                            }
                        }
                    }

                    XElement xEntranceTestSubject = new XElement("EntranceTestSubject");
                    if (jeti["SubjectID"] as int? != null)
                    {
                        xEntranceTestSubject.Add(new XElement("SubjectID", O2Str(jeti["SubjectID"])));
                    }
                    if (jeti["SubjectName"].ToString() != "")
                    {
                        xEntranceTestSubject.Add(new XElement("SubjectName", Str2Str(jeti["SubjectName"])));
                    }

                    xEntranceTestItems.Add(new XElement("EntranceTestItem",
                        new XElement("UID", Str2Str(jeti["UID"])),
                        xEntranceTestBenefits
                        ,new XElement(xEntranceTestSubject),
                        //new XElement("Form", Str2Str(jeti["Form"])),
                        new XElement("MinScore", O2Str(jeti["MinScore"])),
                        new XElement("EntranceTestPriority", Int2Str(jeti["EntranceTestPriority"])),
                        new XElement("IsFirst", Bool2Str(jeti["IsFirst"])),
                        new XElement("IsSecond", Bool2Str(jeti["IsSecond"]))
                        ));

                }
            }
            return xEntranceTestItems;
        }

        private XElement XOlympic(DataTable oTable, int competitiveGroupId, bool isForAllOlympic, DataTable oidTable)
        {
            XElement xOlympic = new XElement("Olympic");
            for (int i = 0; i < oidTable.Rows.Count; i++)
            {
                DataRow oi = oidTable.Rows[i];
                if (Convert.ToInt32(oi["CompetitiveGroupID"] as Int32?) == competitiveGroupId)
                {
                    if (!isForAllOlympic)
                    {
                        
                        for (int j = 0; j < oTable.Rows.Count; j++)
                        {
                            DataRow o = oTable.Rows[j];
                            if (Convert.ToInt32(o["OlympicTypeID"] as Int32?) == Convert.ToInt32(oi["OlympicID"] as Int32?))
                            {
                                xOlympic.Add(new XElement("OlympicID", Int2Str(o["OlympicTypeID"])),
                                    new XElement("LevelID", Int2Str(o["LevelID"])));
                            }
                        }
                    }
                }
            }
            return xOlympic;
        }
    }
}