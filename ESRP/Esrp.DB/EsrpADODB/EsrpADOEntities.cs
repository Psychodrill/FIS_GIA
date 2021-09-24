using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esrp.DB.Common;

namespace Esrp.DB.EsrpADODB
{
    public class AllowedEducationalDirection : EntityBase, IEIISIdable
    {

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        } 
        private int? _licensesupplementid;
        public int? LicenseSupplementId
        {
            get { return _licensesupplementid; }
            set
            {
                if (_licensesupplementid != value)
                {
                    SetHasChanges(true);
                }
                _licensesupplementid = value;
            }
        }

        private int? _educationaldirectionid;
        public int? EducationalDirectionId
        {
            get { return _educationaldirectionid; }
            set
            {
                if (_educationaldirectionid != value)
                {
                    SetHasChanges(true);
                }
                _educationaldirectionid = value;
            }
        } 
    }

    public class EducationalDirectionGroup : EntityBase, IEIISIdable
    {

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    SetHasChanges(true);
                }
                _name = value;
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    SetHasChanges(true);
                }
                _code = value;
            }
        }

        private DateTime _createddate;
        public DateTime CreatedDate
        {
            get { return _createddate; }
            set
            {
                if (_createddate != value)
                {
                    SetHasChanges(true);
                }
                _createddate = value;
            }
        }

        private DateTime? _modifieddate;
        public DateTime? ModifiedDate
        {
            get { return _modifieddate; }
            set
            {
                if (_modifieddate != value)
                {
                    SetHasChanges(true);
                }
                _modifieddate = value;
            }
        }

        private bool _isactual;
        public bool IsActual
        {
            get { return _isactual; }
            set
            {
                if (_isactual != value)
                {
                    SetHasChanges(true);
                }
                _isactual = value;
            }
        }
    }

    public class EducationalDirection : EntityBase, IEIISIdable
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    SetHasChanges(true);
                }
                _name = value;
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    SetHasChanges(true);
                }
                _code = value;
            }
        }

        private int? _mappededucationallevelid;
        public int? MappedEducationalLevelId
        {
            get { return _mappededucationallevelid; }
            set
            {
                if (_mappededucationallevelid != value)
                {
                    SetHasChanges(true);
                }
                _mappededucationallevelid = value;
            }
        }

        private int? _educationaldirectiontypeid;
        public int? EducationalDirectionTypeId
        {
            get { return _educationaldirectiontypeid; }
            set
            {
                if (_educationaldirectiontypeid != value)
                {
                    SetHasChanges(true);
                }
                _educationaldirectiontypeid = value;
            }
        } 
        
        private int? _educationaldirectiongroupid;
        public int? EducationalDirectionGroupId
        {
            get { return _educationaldirectiongroupid; }
            set
            {
                if (_educationaldirectiongroupid != value)
                {
                    SetHasChanges(true);
                }
                _educationaldirectiongroupid = value;
            }
        }
         
        private string _period;
        public string Period
        {
            get { return _period; }
            set
            {
                if (_period != value)
                {
                    SetHasChanges(true);
                }
                _period = value;
            }
        }

        private bool _isactual;
        public bool IsActual
        {
            get { return _isactual; }
            set
            {
                if (_isactual != value)
                {
                    SetHasChanges(true);
                }
                _isactual = value;
            }
        }

        private string _directoryname;
        public string DirectoryName
        {
            get { return _directoryname; }
            set
            {
                if (_directoryname != value)
                {
                    SetHasChanges(true);
                }
                _directoryname = value;
            }
        } 
    }

    public class EducationalDirectionType : EntityBase, IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    SetHasChanges(true);
                }
                _code = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    SetHasChanges(true);
                }
                _name = value;
            }
        }

        private string _shortname;
        public string ShortName
        {
            get { return _shortname; }
            set
            {
                if (_shortname != value)
                {
                    SetHasChanges(true);
                }
                _shortname = value;
            }
        }
    }

    public class EducationalLevelEIISMap : EntityBase, IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }


        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    SetHasChanges(true);
                }
                _code = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    SetHasChanges(true);
                }
                _name = value;
            }
        }

        private string _shortname;
        public string ShortName
        {
            get { return _shortname; }
            set
            {
                if (_shortname != value)
                {
                    SetHasChanges(true);
                }
                _shortname = value;
            }
        }

        private int? _mappededucationallevelid;
        public int? MappedEducationalLevelId
        {
            get { return _mappededucationallevelid; }
            set
            {
                if (_mappededucationallevelid != value)
                {
                    SetHasChanges(true);
                }
                _mappededucationallevelid = value;
            }
        }

    }

    public class License : EntityBase, IEIISIdable
    {

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private int? _regionid;
        public int? RegionId
        {
            get { return _regionid; }
            set
            {
                if (_regionid != value)
                {
                    SetHasChanges(true);
                }
                _regionid = value;
            }
        }

        private string _regnumber;
        public string RegNumber
        {
            get { return _regnumber; }
            set
            {
                if (_regnumber != value)
                {
                    SetHasChanges(true);
                }
                _regnumber = value;
            }
        }

        private string _statusname;
        public string StatusName
        {
            get { return _statusname; }
            set
            {
                if (_statusname != value)
                {
                    SetHasChanges(true);
                }
                _statusname = value;
            }
        }

        private bool _istermless;
        public bool IsTermless
        {
            get { return _istermless; }
            set
            {
                if (_istermless != value)
                {
                    SetHasChanges(true);
                }
                _istermless = value;
            }
        }

        private DateTime? _enddate;
        public DateTime? EndDate
        {
            get { return _enddate; }
            set
            {
                if (_enddate != value)
                {
                    SetHasChanges(true);
                }
                _enddate = value;
            }
        }

        private int _organizationid;
        public int OrganizationId
        {
            get { return _organizationid; }
            set
            {
                if (_organizationid != value)
                {
                    SetHasChanges(true);
                }
                _organizationid = value;
            }
        }

        private string _basedocumenttypename;
        public string BaseDocumentTypeName
        {
            get { return _basedocumenttypename; }
            set
            {
                if (_basedocumenttypename != value)
                {
                    SetHasChanges(true);
                }
                _basedocumenttypename = value;
            }
        }

        private string _orderdocumentnumber;
        public string OrderDocumentNumber
        {
            get { return _orderdocumentnumber; }
            set
            {
                if (_orderdocumentnumber != value)
                {
                    SetHasChanges(true);
                }
                _orderdocumentnumber = value;
            }
        }

        private DateTime? _orderdocumentdate;
        public DateTime? OrderDocumentDate
        {
            get { return _orderdocumentdate; }
            set
            {
                if (_orderdocumentdate != value)
                {
                    SetHasChanges(true);
                }
                _orderdocumentdate = value;
            }
        }

        private string _reasonofsuspension;
        public string ReasonOfSuspension
        {
            get { return _reasonofsuspension; }
            set
            {
                if (_reasonofsuspension != value)
                {
                    SetHasChanges(true);
                }
                _reasonofsuspension = value;
            }
        }

        private DateTime? _dateofsuspension;
        public DateTime? DateOfSuspension
        {
            get { return _dateofsuspension; }
            set
            {
                if (_dateofsuspension != value)
                {
                    SetHasChanges(true);
                }
                _dateofsuspension = value;
            }
        }

        private int? _oldlicenseid;
        public int? OldLicenseId
        {
            get { return _oldlicenseid; }
            set
            {
                if (_oldlicenseid != value)
                {
                    SetHasChanges(true);
                }
                _oldlicenseid = value;
            }
        }

        private string _administrativesuspensionorder;
        public string AdministrativeSuspensionOrder
        {
            get { return _administrativesuspensionorder; }
            set
            {
                if (_administrativesuspensionorder != value)
                {
                    SetHasChanges(true);
                }
                _administrativesuspensionorder = value;
            }
        }

        private string _suspensiondecision;
        public string SuspensionDecision
        {
            get { return _suspensiondecision; }
            set
            {
                if (_suspensiondecision != value)
                {
                    SetHasChanges(true);
                }
                _suspensiondecision = value;
            }
        }

        private string _courtrevokingdecision;
        public string CourtRevokingDecision
        {
            get { return _courtrevokingdecision; }
            set
            {
                if (_courtrevokingdecision != value)
                {
                    SetHasChanges(true);
                }
                _courtrevokingdecision = value;
            }
        }

    }

    public class LicenseSupplement : EntityBase, IEIISIdable
    {

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private int _licenseid;
        public int LicenseId
        {
            get { return _licenseid; }
            set
            {
                if (_licenseid != value)
                {
                    SetHasChanges(true);
                }
                _licenseid = value;
            }
        }

        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    SetHasChanges(true);
                }
                _number = value;
            }
        }

        private string _formserialnumber;
        public string FormSerialNumber
        {
            get { return _formserialnumber; }
            set
            {
                if (_formserialnumber != value)
                {
                    SetHasChanges(true);
                }
                _formserialnumber = value;
            }
        }

        private string _formnumber;
        public string FormNumber
        {
            get { return _formnumber; }
            set
            {
                if (_formnumber != value)
                {
                    SetHasChanges(true);
                }
                _formnumber = value;
            }
        } 

        private string _statusname;
        public string StatusName
        {
            get { return _statusname; }
            set
            {
                if (_statusname != value)
                {
                    SetHasChanges(true);
                }
                _statusname = value;
            }
        }

        private DateTime? _enddate;
        public DateTime? EndDate
        {
            get { return _enddate; }
            set
            {
                if (_enddate != value)
                {
                    SetHasChanges(true);
                }
                _enddate = value;
            }
        }

        private int _organizationid;
        public int OrganizationId
        {
            get { return _organizationid; }
            set
            {
                if (_organizationid != value)
                {
                    SetHasChanges(true);
                }
                _organizationid = value;
            }
        }

        private string _basedocumenttypename;
        public string BaseDocumentTypeName
        {
            get { return _basedocumenttypename; }
            set
            {
                if (_basedocumenttypename != value)
                {
                    SetHasChanges(true);
                }
                _basedocumenttypename = value;
            }
        }

        private string _orderdocumentnumber;
        public string OrderDocumentNumber
        {
            get { return _orderdocumentnumber; }
            set
            {
                if (_orderdocumentnumber != value)
                {
                    SetHasChanges(true);
                }
                _orderdocumentnumber = value;
            }
        }

        private DateTime? _orderdocumentdate;
        public DateTime? OrderDocumentDate
        {
            get { return _orderdocumentdate; }
            set
            {
                if (_orderdocumentdate != value)
                {
                    SetHasChanges(true);
                }
                _orderdocumentdate = value;
            }
        }

        private string _reasonofsuspension;
        public string ReasonOfSuspension
        {
            get { return _reasonofsuspension; }
            set
            {
                if (_reasonofsuspension != value)
                {
                    SetHasChanges(true);
                }
                _reasonofsuspension = value;
            }
        }

        private DateTime? _dateofsuspension;
        public DateTime? DateOfSuspension
        {
            get { return _dateofsuspension; }
            set
            {
                if (_dateofsuspension != value)
                {
                    SetHasChanges(true);
                }
                _dateofsuspension = value;
            }
        }

        private int? _oldsupplementid;
        public int? OldSupplementId
        {
            get { return _oldsupplementid; }
            set
            {
                if (_oldsupplementid != value)
                {
                    SetHasChanges(true);
                }
                _oldsupplementid = value;
            }
        }

    }

    public class OrganizationKindEIISMap : EntityBase, IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    SetHasChanges(true);
                }
                _name = value;
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    SetHasChanges(true);
                }
                _code = value;
            }
        }

        private int? _organizationkindid;
        public int? OrganizationKindId
        {
            get { return _organizationkindid; }
            set
            {
                if (_organizationkindid != value)
                {
                    SetHasChanges(true);
                }
                _organizationkindid = value;
            }
        }
    }

    public class OrganizationLimitation : EntityBase, IEIISIdable
    {

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private int _organizationid;
        public int OrganizationId
        {
            get { return _organizationid; }
            set
            {
                if (_organizationid != value)
                {
                    SetHasChanges(true);
                }
                _organizationid = value;
            }
        }

        private string _documentname;
        public string DocumentName
        {
            get { return _documentname; }
            set
            {
                if (_documentname != value)
                {
                    SetHasChanges(true);
                }
                _documentname = value;
            }
        }

        private string _documentnumber;
        public string DocumentNumber
        {
            get { return _documentnumber; }
            set
            {
                if (_documentnumber != value)
                {
                    SetHasChanges(true);
                }
                _documentnumber = value;
            }
        }

        private DateTime? _documentdate;
        public DateTime? DocumentDate
        {
            get { return _documentdate; }
            set
            {
                if (_documentdate != value)
                {
                    SetHasChanges(true);
                }
                _documentdate = value;
            }
        }

    }

    public class Organization2010 : EntityBase, IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return FullName; }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private DateTime _createdate;
        public DateTime CreateDate
        {
            get { return _createdate; }
            set
            {
                if (_createdate != value)
                {
                    SetHasChanges(true);
                }
                _createdate = value;
            }
        }

        private DateTime _updatedate;
        public DateTime UpdateDate
        {
            get { return _updatedate; }
            set
            {
                if (_updatedate != value)
                {
                    SetHasChanges(true);
                }
                _updatedate = value;
            }
        }

        private string _fullname;
        public string FullName
        {
            get { return _fullname; }
            set
            {
                if (_fullname != value)
                {
                    SetHasChanges(true);
                }
                _fullname = value;
            }
        }

        private string _shortname;
        public string ShortName
        {
            get { return _shortname; }
            set
            {
                if (_shortname != value)
                {
                    SetHasChanges(true);
                }
                _shortname = value;
            }
        }

        private int _regionid;
        public int RegionId
        {
            get { return _regionid; }
            set
            {
                if (_regionid != value)
                {
                    SetHasChanges(true);
                }
                _regionid = value;
            }
        }

        private int _typeid;
        public int TypeId
        {
            get { return _typeid; }
            set
            {
                if (_typeid != value)
                {
                    SetHasChanges(true);
                }
                _typeid = value;
            }
        }

        private int _kindid;
        public int KindId
        {
            get { return _kindid; }
            set
            {
                if (_kindid != value)
                {
                    SetHasChanges(true);
                }
                _kindid = value;
            }
        }

        private string _inn;
        public string INN
        {
            get { return _inn; }
            set
            {
                if (_inn != value)
                {
                    SetHasChanges(true);
                }
                _inn = value;
            }
        }

        private string _ogrn;
        public string OGRN
        {
            get { return _ogrn; }
            set
            {
                if (_ogrn != value)
                {
                    SetHasChanges(true);
                }
                _ogrn = value;
            }
        }

        private string _ownerdepartment;
        public string OwnerDepartment
        {
            get { return _ownerdepartment; }
            set
            {
                if (_ownerdepartment != value)
                {
                    SetHasChanges(true);
                }
                _ownerdepartment = value;
            }
        }

        private bool _isprivate;
        public bool IsPrivate
        {
            get { return _isprivate; }
            set
            {
                if (_isprivate != value)
                {
                    SetHasChanges(true);
                }
                _isprivate = value;
            }
        }

        private bool _isfilial;
        public bool IsFilial
        {
            get { return _isfilial; }
            set
            {
                if (_isfilial != value)
                {
                    SetHasChanges(true);
                }
                _isfilial = value;
            }
        }

        private string _directorposition;
        public string DirectorPosition
        {
            get { return _directorposition; }
            set
            {
                if (_directorposition != value)
                {
                    SetHasChanges(true);
                }
                _directorposition = value;
            }
        }

        private string _directorfullname;
        public string DirectorFullName
        {
            get { return _directorfullname; }
            set
            {
                if (_directorfullname != value)
                {
                    SetHasChanges(true);
                }
                _directorfullname = value;
            }
        }

        private bool? _isaccredited;
        public bool? IsAccredited
        {
            get { return _isaccredited; }
            set
            {
                if (_isaccredited != value)
                {
                    SetHasChanges(true);
                }
                _isaccredited = value;
            }
        }

        private string _accreditationsertificate;
        public string AccreditationSertificate
        {
            get { return _accreditationsertificate; }
            set
            {
                if (_accreditationsertificate != value)
                {
                    SetHasChanges(true);
                }
                _accreditationsertificate = value;
            }
        }

        private string _lawaddress;
        public string LawAddress
        {
            get { return _lawaddress; }
            set
            {
                if (_lawaddress != value)
                {
                    SetHasChanges(true);
                }
                _lawaddress = value;
            }
        }

        private string _factaddress;
        public string FactAddress
        {
            get { return _factaddress; }
            set
            {
                if (_factaddress != value)
                {
                    SetHasChanges(true);
                }
                _factaddress = value;
            }
        }

        private string _phonecitycode;
        public string PhoneCityCode
        {
            get { return _phonecitycode; }
            set
            {
                if (_phonecitycode != value)
                {
                    SetHasChanges(true);
                }
                _phonecitycode = value;
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone != value)
                {
                    SetHasChanges(true);
                }
                _phone = value;
            }
        }

        private string _fax;
        public string Fax
        {
            get { return _fax; }
            set
            {
                if (_fax != value)
                {
                    SetHasChanges(true);
                }
                _fax = value;
            }
        }

        private string _email;
        public string EMail
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    SetHasChanges(true);
                }
                _email = value;
            }
        }

        private string _site;
        public string Site
        {
            get { return _site; }
            set
            {
                if (_site != value)
                {
                    SetHasChanges(true);
                }
                _site = value;
            }
        }

        private bool _wasimportedatstart;
        public bool WasImportedAtStart
        {
            get { return _wasimportedatstart; }
            set
            {
                if (_wasimportedatstart != value)
                {
                    SetHasChanges(true);
                }
                _wasimportedatstart = value;
            }
        }

        private int _cnfederalbudget;
        public int CNFederalBudget
        {
            get { return _cnfederalbudget; }
            set
            {
                if (_cnfederalbudget != value)
                {
                    SetHasChanges(true);
                }
                _cnfederalbudget = value;
            }
        }

        private int _cntargeted;
        public int CNTargeted
        {
            get { return _cntargeted; }
            set
            {
                if (_cntargeted != value)
                {
                    SetHasChanges(true);
                }
                _cntargeted = value;
            }
        }

        private int _cnlocalbudget;
        public int CNLocalBudget
        {
            get { return _cnlocalbudget; }
            set
            {
                if (_cnlocalbudget != value)
                {
                    SetHasChanges(true);
                }
                _cnlocalbudget = value;
            }
        }

        private int _cnpaying;
        public int CNPaying
        {
            get { return _cnpaying; }
            set
            {
                if (_cnpaying != value)
                {
                    SetHasChanges(true);
                }
                _cnpaying = value;
            }
        }

        private int _cnfulltime;
        public int CNFullTime
        {
            get { return _cnfulltime; }
            set
            {
                if (_cnfulltime != value)
                {
                    SetHasChanges(true);
                }
                _cnfulltime = value;
            }
        }

        private int _cnevening;
        public int CNEvening
        {
            get { return _cnevening; }
            set
            {
                if (_cnevening != value)
                {
                    SetHasChanges(true);
                }
                _cnevening = value;
            }
        }

        private int _cnpostal;
        public int CNPostal
        {
            get { return _cnpostal; }
            set
            {
                if (_cnpostal != value)
                {
                    SetHasChanges(true);
                }
                _cnpostal = value;
            }
        }

        private int? _rcmodel;
        public int? RCModel
        {
            get { return _rcmodel; }
            set
            {
                if (_rcmodel != value)
                {
                    SetHasChanges(true);
                }
                _rcmodel = value;
            }
        }

        private string _rcdescription;
        public string RCDescription
        {
            get { return _rcdescription; }
            set
            {
                if (_rcdescription != value)
                {
                    SetHasChanges(true);
                }
                _rcdescription = value;
            }
        }

        private int? _mainid;
        public int? MainId
        {
            get { return _mainid; }
            set
            {
                if (_mainid != value)
                {
                    SetHasChanges(true);
                }
                _mainid = value;
            }
        }

        private int? _departmentid;
        public int? DepartmentId
        {
            get { return _departmentid; }
            set
            {
                if (_departmentid != value)
                {
                    SetHasChanges(true);
                }
                _departmentid = value;
            }
        }

        private int _statusid;
        public int StatusId
        {
            get { return _statusid; }
            set
            {
                if (_statusid != value)
                {
                    SetHasChanges(true);
                }
                _statusid = value;
            }
        }

        private int? _neworgid;
        public int? NewOrgId
        {
            get { return _neworgid; }
            set
            {
                if (_neworgid != value)
                {
                    SetHasChanges(true);
                }
                _neworgid = value;
            }
        }

        private int _version;
        public int Version
        {
            get { return _version; }
            set
            {
                if (_version != value)
                {
                    SetHasChanges(true);
                }
                _version = value;
            }
        }

        private DateTime? _datechangestatus;
        public DateTime? DateChangeStatus
        {
            get { return _datechangestatus; }
            set
            {
                if (_datechangestatus != value)
                {
                    SetHasChanges(true);
                }
                _datechangestatus = value;
            }
        }

        private string _reason;
        public string Reason
        {
            get { return _reason; }
            set
            {
                if (_reason != value)
                {
                    SetHasChanges(true);
                }
                _reason = value;
            }
        }

        private bool? _receptiononresultscne;
        public bool? ReceptionOnResultsCNE
        {
            get { return _receptiononresultscne; }
            set
            {
                if (_receptiononresultscne != value)
                {
                    SetHasChanges(true);
                }
                _receptiononresultscne = value;
            }
        }

        private string _islod_guid;
        public string ISLOD_GUID
        {
            get { return _islod_guid; }
            set
            {
                if (_islod_guid != value)
                {
                    SetHasChanges(true);
                }
                _islod_guid = value;
            }
        }

        private string _kpp;
        public string KPP
        {
            get { return _kpp; }
            set
            {
                if (_kpp != value)
                {
                    SetHasChanges(true);
                }
                _kpp = value;
            }
        }

        private string _lettertoreschedulename;
        public string LetterToRescheduleName
        {
            get { return _lettertoreschedulename; }
            set
            {
                if (_lettertoreschedulename != value)
                {
                    SetHasChanges(true);
                }
                _lettertoreschedulename = value;
            }
        }

        private string _lettertoreschedulecontenttype;
        public string LetterToRescheduleContentType
        {
            get { return _lettertoreschedulecontenttype; }
            set
            {
                if (_lettertoreschedulecontenttype != value)
                {
                    SetHasChanges(true);
                }
                _lettertoreschedulecontenttype = value;
            }
        }

        private DateTime? _timeconnectiontosecurenetwork;
        public DateTime? TimeConnectionToSecureNetwork
        {
            get { return _timeconnectiontosecurenetwork; }
            set
            {
                if (_timeconnectiontosecurenetwork != value)
                {
                    SetHasChanges(true);
                }
                _timeconnectiontosecurenetwork = value;
            }
        }

        private DateTime? _timeenterinformationinfis;
        public DateTime? TimeEnterInformationInFIS
        {
            get { return _timeenterinformationinfis; }
            set
            {
                if (_timeenterinformationinfis != value)
                {
                    SetHasChanges(true);
                }
                _timeenterinformationinfis = value;
            }
        }

        private int _connectionschemeid;
        public int ConnectionSchemeId
        {
            get { return _connectionschemeid; }
            set
            {
                if (_connectionschemeid != value)
                {
                    SetHasChanges(true);
                }
                _connectionschemeid = value;
            }
        }

        private int _connectionstatusid;
        public int ConnectionStatusId
        {
            get { return _connectionstatusid; }
            set
            {
                if (_connectionstatusid != value)
                {
                    SetHasChanges(true);
                }
                _connectionstatusid = value;
            }
        }

        private bool? _isagreedtimeconnection;
        public bool? IsAgreedTimeConnection
        {
            get { return _isagreedtimeconnection; }
            set
            {
                if (_isagreedtimeconnection != value)
                {
                    SetHasChanges(true);
                }
                _isagreedtimeconnection = value;
            }
        }

        private bool? _isagreedtimeenterinformation;
        public bool? IsAgreedTimeEnterInformation
        {
            get { return _isagreedtimeenterinformation; }
            set
            {
                if (_isagreedtimeenterinformation != value)
                {
                    SetHasChanges(true);
                }
                _isagreedtimeenterinformation = value;
            }
        }

        private string _directorpositioningenetive;
        public string DirectorPositionInGenetive
        {
            get { return _directorpositioningenetive; }
            set
            {
                if (_directorpositioningenetive != value)
                {
                    SetHasChanges(true);
                }
                _directorpositioningenetive = value;
            }
        }

        private string _directorfullnameingenetive;
        public string DirectorFullNameInGenetive
        {
            get { return _directorfullnameingenetive; }
            set
            {
                if (_directorfullnameingenetive != value)
                {
                    SetHasChanges(true);
                }
                _directorfullnameingenetive = value;
            }
        }

        private string _directorfirstname;
        public string DirectorFirstName
        {
            get { return _directorfirstname; }
            set
            {
                if (_directorfirstname != value)
                {
                    SetHasChanges(true);
                }
                _directorfirstname = value;
            }
        }

        private string _directorlastname;
        public string DirectorLastName
        {
            get { return _directorlastname; }
            set
            {
                if (_directorlastname != value)
                {
                    SetHasChanges(true);
                }
                _directorlastname = value;
            }
        }

        private string _directorpatronymicname;
        public string DirectorPatronymicName
        {
            get { return _directorpatronymicname; }
            set
            {
                if (_directorpatronymicname != value)
                {
                    SetHasChanges(true);
                }
                _directorpatronymicname = value;
            }
        }

        private bool _ouconfirmation;
        public bool OUConfirmation
        {
            get { return _ouconfirmation; }
            set
            {
                if (_ouconfirmation != value)
                {
                    SetHasChanges(true);
                }
                _ouconfirmation = value;
            }
        }

        private bool _updatedbyuser;
        public bool UpdatedByUser
        {
            get { return _updatedbyuser; }
            set
            {
                if (_updatedbyuser != value)
                {
                    SetHasChanges(true);
                }
                _updatedbyuser = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private string _townname;
        public string TownName
        {
            get { return _townname; }
            set
            {
                if (_townname != value)
                {
                    SetHasChanges(true);
                }
                _townname = value;
            }
        }

        private bool _isreligious;
        public bool IsReligious
        {
            get { return _isreligious; }
            set
            {
                if (_isreligious != value)
                {
                    SetHasChanges(true);
                }
                _isreligious = value;
            }
        }

        private bool _islawenforcmentorganization;
        public bool IsLawEnforcmentOrganization
        {
            get { return _islawenforcmentorganization; }
            set
            {
                if (_islawenforcmentorganization != value)
                {
                    SetHasChanges(true);
                }
                _islawenforcmentorganization = value;
            }
        }

    }

    public class OrganizationStatusEIISMap : EntityBase, IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }


        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    SetHasChanges(true);
                }
                _name = value;
            }
        }

        private int? _organizationoperatingstatusid;
        public int? OrganizationOperatingStatusId
        {
            get { return _organizationoperatingstatusid; }
            set
            {
                if (_organizationoperatingstatusid != value)
                {
                    SetHasChanges(true);
                }
                _organizationoperatingstatusid = value;
            }
        }
    }

    public class Region : EntityBase, IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }


        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    SetHasChanges(true);
                }
                _code = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    SetHasChanges(true);
                }
                _name = value;
            }
        }

        private bool _inorganization;
        public bool InOrganization
        {
            get { return _inorganization; }
            set
            {
                if (_inorganization != value)
                {
                    SetHasChanges(true);
                }
                _inorganization = value;
            }
        }

        private bool _incertificate;
        public bool InCertificate
        {
            get { return _incertificate; }
            set
            {
                if (_incertificate != value)
                {
                    SetHasChanges(true);
                }
                _incertificate = value;
            }
        }

        private int _sortindex;
        public int SortIndex
        {
            get { return _sortindex; }
            set
            {
                if (_sortindex != value)
                {
                    SetHasChanges(true);
                }
                _sortindex = value;
            }
        }

        private bool _inorganizationetalon;
        public bool InOrganizationEtalon
        {
            get { return _inorganizationetalon; }
            set
            {
                if (_inorganizationetalon != value)
                {
                    SetHasChanges(true);
                }
                _inorganizationetalon = value;
            }
        }

        private int _federaldistrictid;
        public int FederalDistrictId
        {
            get { return _federaldistrictid; }
            set
            {
                if (_federaldistrictid != value)
                {
                    SetHasChanges(true);
                }
                _federaldistrictid = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }
    }

    public class FounderType : EntityBase, IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    SetHasChanges(true);
                }
                _name = value;
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    SetHasChanges(true);
                }
                _code = value;
            }
        }

        private bool _isdeleted;
        public bool IsDeleted
        {
            get { return _isdeleted; }
            set
            {
                if (_isdeleted != value)
                {
                    SetHasChanges(true);
                }
                _isdeleted = value;
            }
        }
    }

    public class Founder : EntityBase, IEIISIdable
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private int? _typeid;
        public int? TypeId
        {
            get { return _typeid; }
            set
            {
                if (_typeid != value)
                {
                    SetHasChanges(true);
                }
                _typeid = value;
            }
        }

        private int _lawaddressregionid;
        public int LawAddressRegionId
        {
            get { return _lawaddressregionid; }
            set
            {
                if (_lawaddressregionid != value)
                {
                    SetHasChanges(true);
                }
                _lawaddressregionid = value;
            }
        }

        private int _factaddressregionid;
        public int FactAddressRegionId
        {
            get { return _factaddressregionid; }
            set
            {
                if (_factaddressregionid != value)
                {
                    SetHasChanges(true);
                }
                _factaddressregionid = value;
            }
        }

        private string _organizationfullname;
        public string OrganizationFullName
        {
            get { return _organizationfullname; }
            set
            {
                if (_organizationfullname != value)
                {
                    SetHasChanges(true);
                }
                _organizationfullname = value;
            }
        }

        private string _organizationshortname;
        public string OrganizationShortName
        {
            get { return _organizationshortname; }
            set
            {
                if (_organizationshortname != value)
                {
                    SetHasChanges(true);
                }
                _organizationshortname = value;
            }
        }

        private string _personlastname;
        public string PersonLastName
        {
            get { return _personlastname; }
            set
            {
                if (_personlastname != value)
                {
                    SetHasChanges(true);
                }
                _personlastname = value;
            }
        }

        private string _personfirstname;
        public string PersonFirstName
        {
            get { return _personfirstname; }
            set
            {
                if (_personfirstname != value)
                {
                    SetHasChanges(true);
                }
                _personfirstname = value;
            }
        }

        private string _personpatronymic;
        public string PersonPatronymic
        {
            get { return _personpatronymic; }
            set
            {
                if (_personpatronymic != value)
                {
                    SetHasChanges(true);
                }
                _personpatronymic = value;
            }
        }

        private string _phones;
        public string
            Phones
        {
            get { return _phones; }
            set
            {
                if (_phones != value)
                {
                    SetHasChanges(true);
                }
                _phones = value;
            }
        }

        private string _faxes;
        public string Faxes
        {
            get { return _faxes; }
            set
            {
                if (_faxes != value)
                {
                    SetHasChanges(true);
                }
                _faxes = value;
            }
        }

        private string _emails;
        public string Emails
        {
            get { return _emails; }
            set
            {
                if (_emails != value)
                {
                    SetHasChanges(true);
                }
                _emails = value;
            }
        }

        private string _ogrn;
        public string Ogrn
        {
            get { return _ogrn; }
            set
            {
                if (_ogrn != value)
                {
                    SetHasChanges(true);
                }
                _ogrn = value;
            }
        }

        private string _inn;
        public string Inn
        {
            get { return _inn; }
            set
            {
                if (_inn != value)
                {
                    SetHasChanges(true);
                }
                _inn = value;
            }
        }

        private string _kpp;
        public string Kpp
        {
            get { return _kpp; }
            set
            {
                if (_kpp != value)
                {
                    SetHasChanges(true);
                }
                _kpp = value;
            }
        }

        private string _lawaddress;
        public string LawAddress
        {
            get { return _lawaddress; }
            set
            {
                if (_lawaddress != value)
                {
                    SetHasChanges(true);
                }
                _lawaddress = value;
            }
        }

        private string _factaddress;
        public string FactAddress
        {
            get { return _factaddress; }
            set
            {
                if (_factaddress != value)
                {
                    SetHasChanges(true);
                }
                _factaddress = value;
            }
        }

        private string _lawaddressdistrict;
        public string LawAddressDistrict
        {
            get { return _lawaddressdistrict; }
            set
            {
                if (_lawaddressdistrict != value)
                {
                    SetHasChanges(true);
                }
                _lawaddressdistrict = value;
            }
        }

        private string _factaddressdistrict;
        public string FactAddressDistrict
        {
            get { return _factaddressdistrict; }
            set
            {
                if (_factaddressdistrict != value)
                {
                    SetHasChanges(true);
                }
                _factaddressdistrict = value;
            }
        }

        private string _lawaddresstown;
        public string LawAddressTown
        {
            get { return _lawaddresstown; }
            set
            {
                if (_lawaddresstown != value)
                {
                    SetHasChanges(true);
                }
                _lawaddresstown = value;
            }
        }

        private string _factaddresstown;
        public string FactAddressTown
        {
            get { return _factaddresstown; }
            set
            {
                if (_factaddresstown != value)
                {
                    SetHasChanges(true);
                }
                _factaddresstown = value;
            }
        }

        private string _lawaddressstreet;
        public string LawAddressStreet
        {
            get { return _lawaddressstreet; }
            set
            {
                if (_lawaddressstreet != value)
                {
                    SetHasChanges(true);
                }
                _lawaddressstreet = value;
            }
        }

        private string _factaddressstreet;
        public string FactAddressStreet
        {
            get { return _factaddressstreet; }
            set
            {
                if (_factaddressstreet != value)
                {
                    SetHasChanges(true);
                }
                _factaddressstreet = value;
            }
        }

        private string _lawaddresshousenumber;
        public string LawAddressHouseNumber
        {
            get { return _lawaddresshousenumber; }
            set
            {
                if (_lawaddresshousenumber != value)
                {
                    SetHasChanges(true);
                }
                _lawaddresshousenumber = value;
            }
        }

        private string _factaddresshousenumber;
        public string FactAddressHouseNumber
        {
            get { return _factaddresshousenumber; }
            set
            {
                if (_factaddresshousenumber != value)
                {
                    SetHasChanges(true);
                }
                _factaddresshousenumber = value;
            }
        }

        private string _lawaddresspostalcode;
        public string LawAddressPostalCode
        {
            get { return _lawaddresspostalcode; }
            set
            {
                if (_lawaddresspostalcode != value)
                {
                    SetHasChanges(true);
                }
                _lawaddresspostalcode = value;
            }
        }

        private string _factaddresspostalcode;
        public string FactAddressPostalCode
        {
            get { return _factaddresspostalcode; }
            set
            {
                if (_factaddresspostalcode != value)
                {
                    SetHasChanges(true);
                }
                _factaddresspostalcode = value;
            }
        }
    }

    public class OrganizationFounder : EntityBase, IEIISIdable
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private int _organizationid;
        public int OrganizationId
        {
            get { return _organizationid; }
            set
            {
                if (_organizationid != value)
                {
                    SetHasChanges(true);
                }
                _organizationid = value;
            }
        }

        private int _founderid;
        public int FounderId
        {
            get { return _founderid; }
            set
            {
                if (_founderid != value)
                {
                    SetHasChanges(true);
                }
                _founderid = value;
            }
        }
    }

    public class Qualification : EntityBase, IEIISIdable
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    SetHasChanges(true);
                }
                _code = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    SetHasChanges(true);
                }
                _name = value;
            }
        }

        private bool _isactual;
        public bool IsActual
        {
            get { return _isactual; }
            set
            {
                if (_isactual != value)
                {
                    SetHasChanges(true);
                }
                _isactual = value;
            }
        }
    }

    public class AllowedEducationalDirectionQualification : EntityBase, IEIISIdable
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SetHasChanges(true);
                }
                _id = value;
            }
        }

        private string _eiis_id;
        public string Eiis_Id
        {
            get { return _eiis_id; }
            set
            {
                if (_eiis_id != value)
                {
                    SetHasChanges(true);
                }
                _eiis_id = value;
            }
        }

        private int _allowededucationaldirectionid;
        public int AllowedEducationalDirectionId
        {
            get { return _allowededucationaldirectionid; }
            set
            {
                if (_allowededucationaldirectionid != value)
                {
                    SetHasChanges(true);
                }
                _allowededucationaldirectionid = value;
            }
        }

        private int _qualificationid;
        public int QualificationId
        {
            get { return _qualificationid; }
            set
            {
                if (_qualificationid != value)
                {
                    SetHasChanges(true);
                }
                _qualificationid = value;
            }
        }
    }
}
