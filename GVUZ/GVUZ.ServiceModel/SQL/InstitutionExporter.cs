using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;
using System.Xml;
using System.Xml.Linq;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.Helper.Import;

namespace GVUZ.ServiceModel.SQL {

	public partial class InstitutionExporter {
		private int _institutionID;
		private bool _bIncludeFilials;
		private InstitutionInformationFilter _filter;
		private System.Data.SqlClient.SqlConnection con;

		public InstitutionExporter(int institutionID, bool bIncludeFilials=false, InstitutionInformationFilter filter=null) {
			_institutionID=institutionID;
			_bIncludeFilials=bIncludeFilials;
			
			if(filter==null) {
				//_filter=new InstitutionInformationFilter();
			} else {
				_filter=filter;
			}
			con= new SqlConnection(SQL.ConnectionString);
		}
		public XElement GetInsitututionsData() {
			XElement el=null;
			try {
				con.Open();
				el=GetInstitutionExports();
			} catch(Exception) {

			} finally {
				con.Close();
			}
			return el;
		}

		private XElement GetInstitutionExports() {
			var doc=new XDocument(new XElement("InstitutionExports"));
			var xInstitutionExports=doc.Root;
			xInstitutionExports.Add(
				new XAttribute(XNamespace.Xmlns+"xsd","http://www.w3.org/2001/XMLSchema"),
				new XAttribute(XNamespace.Xmlns+"xsi","http://www.w3.org/2001/XMLSchema-instance"));

			xInstitutionExports.Add(new XElement("InstitutionID",_institutionID));

			var xInstitutionExport=GetInstitutionExport();

            if (xInstitutionExport.HasAttributes)
            {
                doc = new XDocument(xInstitutionExport);
            }
            else
            {
                xInstitutionExports.Add(xInstitutionExport);
            }
	
			return doc.Root;
		}

	    private XElement GetInstitutionExport()
	    {
	        XElement xInstitutionExport = new XElement("InstitutionExport");

	        //var xPreparatoryCourses=PreparatoryCourses();
	        try
	        {
	            if (_filter == null)
	            {
	                var xInstitutionDetails = InstitutionDetails();
	                xInstitutionExport.Add(xInstitutionDetails);

	                var xAllowedDirections = AllowedDirections();
	                xInstitutionExport.Add(xAllowedDirections);

	                var xCampaigns = Campaigns();
	                xInstitutionExport.Add(xCampaigns);

	                var xAdmissionVolume = AdmissionVolume();
	                xInstitutionExport.Add(xAdmissionVolume);

	                var xDistributedAdmissionVolume = DistributedAdmissionVolume();
	                xInstitutionExport.Add(xDistributedAdmissionVolume);

	                var xInstitutionAchievements = InstitutionAchievements();
	                xInstitutionExport.Add(xInstitutionAchievements);

	                var xCompetitiveGroups = CompetitiveGroups();
	                xInstitutionExport.Add(xCompetitiveGroups);

	                //var xApplications=Applications();
	                //xInstitutionExport.Add(xApplications);

	                var xOrdersOfAdmission = OrdersOfAdmission();
	                xInstitutionExport.Add(xOrdersOfAdmission);

	                var xStructure = Structure();
	                xInstitutionExport.Add(xStructure);

	                var xRecommendedLists = RecommendedLists();
	                xInstitutionExport.Add(xRecommendedLists);

	            }
	            else
	            {
	                if (_filter.InstitutionDetails != null)
	                {
	                    var xInstitutionDetails = InstitutionDetails();
	                    xInstitutionExport.Add(xInstitutionDetails);
	                }
	                if (_filter.AllowedDirections != null)
	                {
	                    var xAllowedDirections = AllowedDirections();
	                    xInstitutionExport.Add(xAllowedDirections);
	                }
	                if (_filter.Campaigns != null)
	                {
	                    var xCampaigns = Campaigns();
	                    xInstitutionExport.Add(xCampaigns);
	                }
	                if (_filter.AdmissionVolume != null)
	                {
	                    var xAdmissionVolume = AdmissionVolume();
	                    xInstitutionExport.Add(xAdmissionVolume);
	                }
	                if (_filter.DistributedAdmissionVolume != null)
	                {
	                    var xDistributedAdmissionVolume = DistributedAdmissionVolume();
	                    xInstitutionExport.Add(xDistributedAdmissionVolume);
	                }
	                if (_filter.InstitutionAchievements != null)
	                {
	                    var xInstitutionAchievements = InstitutionAchievements();
	                    xInstitutionExport.Add(xInstitutionAchievements);
	                }
	                if (_filter.CompetitiveGroups != null)
	                {
	                    var xCompetitiveGroups = CompetitiveGroups();
	                    xInstitutionExport.Add(xCompetitiveGroups);
	                }

	                if (_filter.Applications != null)
	                {
	                    var xApplications = Applications();
	                    xInstitutionExport.Add(xApplications);
	                }

	                if (_filter.OrdersOfAdmission != null || _filter.ApplicationsInOrders != null)
	                {
	                    var xOrdersOfAdmission = OrdersOfAdmission();
	                    xInstitutionExport.Add(xOrdersOfAdmission);
	                }
	                if (_filter.Structure != null)
	                {
	                    var xStructure = Structure();
	                    xInstitutionExport.Add(xStructure);
	                }

	                if (_filter.RecommendedLists != null)
	                {
	                    var xRecommendedLists = RecommendedLists();
	                    xInstitutionExport.Add(xRecommendedLists);
	                }
	            }
	        }
	        catch (InstitutionExporterException ex)
	        {
	            return XmlImportHelper.GenerateErrorElement(ex.Message);
	        }
	        catch (Exception ex)
	        {
	            return new XElement(ex.Message);
	        }
	        return xInstitutionExport;
	    }

	    private SqlCommand getSqlCommand(string sql){

			SqlCommand com=new SqlCommand(sql,con);
			return com;		
		}
		private string O2Str(object o) {
			string s="";
			if(o!=null && o!=DBNull.Value) {
				s=Convert.ToString(o);
			}
			return s;
		}
		private string Decimal2Str(object o) {
			string s="";
			if(o!=null&&o!=DBNull.Value && (o is decimal)) {
				s=((decimal)o).ToString("#0");
			}
			return s;
		}
		private string Str2Str(object o) { 
			string s="";
			if(o!=null&&o!=DBNull.Value) {
				s=o.ToString();
			}
			return s;		
		}
		private string Int2Str(object o) {
			string s="";
			if(o!=null&&o!=DBNull.Value) {
				s=Convert.ToString(o);
				//s=((int)o).ToString();
			}
			return s;
		}
		private string Bool2Str(object o) {
			string s="";
			if(o!=null&&o!=DBNull.Value) {
				s=((bool)o).ToString().ToLower();
			}
			return s;
		}
		private string Date2Str(object o) {
			string s="";
			if(o!=null && o!=DBNull.Value) {
				s=((DateTime)o).ToString("yyyy-MM-ddTHH:mm:ss"); // 2014-08-04T00:00:00
			}
			return s;
		}
	}

	public static class XElementExt {
		public static void AddX(this XElement X, string name, string value) {
			if(!String.IsNullOrEmpty(value)) {
				X.Add(new XElement(name, value));
			}
		}
		public static void AddX(this XElement X, string name, object value) {
			if(value!=null && value!=DBNull.Value) {
				if(value is string) {
					X.Add(new XElement(name,(string)value));
					return;
				}
				if(value is int) {
					X.Add(new XElement(name,Convert.ToString(value)));
					return;
				}
				if(value is DateTime) {
					X.Add(new XElement(name,((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss")));
					return;
				}
				if(value is bool){
					X.Add(new XElement(name,((bool)value).ToString().ToLower()));				
					return;
				}
				if(value is decimal) {
					X.Add(new XElement(name,((decimal)value).ToString("#0")));
					return;
				}
				X.Add(new XElement(name,Convert.ToString(value)));
			}
		}	
	}
}
