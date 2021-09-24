using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using FogSoft.Helpers;
using FogSoft.Web.Mvc;
using GVUZ.Data.Helpers;
using GVUZ.DAL.Helpers;
using GVUZ.DAL.Dapper.Repository.Model.Olympics;
using GVUZ.DAL.Dapper.Repository.Model.OlympicDiplomant;
using GVUZ.Data.Model;
using GVUZ.DAL.Dapper.Repository.Model;

namespace GVUZ.Model.Entrants.UniDocuments
{

    public static class EntrantDocumentMatrix
    {
        public const string Field_IsNostrificated = "IsNostrificated";

        public struct FieledOnForm
        {
            public bool show;
            public bool required;
        }

        private static Dictionary<string, int> FieldList;
        private static Dictionary<string, int> DocList;

        //private static FieledOnForm[,] matrix=new FieledOnForm[40,30]; 
        private static int fields = 75;
        private static int docs = 40;
        private static byte[,] M = new byte[fields, docs];

        // 0 - отсутствует в документе
        // 1 - присутствуте, но не обязательно
        // 2 - обязательно.

        private static void SetAllDocForFiled(int f, byte value)
        {
            for (int i = 0; i < docs; i++) { M[f, i] = value; }
        }

        public static byte getFieledForDoc(string field, int docid)
        {
            byte v = 0;
            try
            {
                v = M[FieldList[field], docid];
            }

#warning Empty catch. 
            catch (Exception) {
            }
            return v;
        }

        public static int getDocId(string DocName)
        {
            if (DocList == null) { Init(); }
            return DocList[DocName];
        }

        // метод для проверки видимости того или иного контрола на форме документа
        // вызывается из EntrantDocumentEdit.ascx
        public static bool isFieledInDoc(string field, int docid, int? entrantId = null)
        {
            if (FieldList == null)
                Init();
            int v = getFieledForDoc(field, docid);
            if (!entrantId.HasValue)
                return (v > 0);
            else
            {
                if (field == Field_IsNostrificated)
                {
                    if (v > 0)
                    {
                        bool isForeigner = new ApplicationRepository().GetEntrantIsForeigner(entrantId.Value);
                        return isForeigner;
                    }
                    else
                        return false;
                }
                else
                    return (v > 0);
            }
        }

        public static bool Editable(string field, int docid)
        {
            if (FieldList == null) { Init(); }
            int v = getFieledForDoc(field, docid);
            return (v == 2);
        }

        public static void Init()
        {
            FieldList = new Dictionary<string, int>();
            DocList = new Dictionary<string, int>();

            FieldList.Add("DocumentTypeID", 0); // Тип документа,	DocumentType.Name
            FieldList.Add("DocumentName", 1);   // Название документа	, EntrantDocumentCustom. DocumentTypeNameText
            FieldList.Add("UID", 2);                    // UID	, EntrantDocument.UID
            FieldList.Add("IdentityDocumentTypeID", 3); //Вид документа , IdentityDocumentType.Name
            FieldList.Add("DocumentSeries", 4); //Серия документа		, EntrantDocument.DocumentSeries	
            FieldList.Add("DocumentNumber", 5); //Номер документа		, EntrantDocument.DocumentNumber
            FieldList.Add("DocumentDate", 6);       //Дата выдачи			, EntrantDocument.DocumentDate
            FieldList.Add("SubdivisionCode", 7);        //Код подразделения , EntrantDocumentIdentity.SubDivisionCode
            FieldList.Add("DocumentOrganization", 8);       //Кем выдан, EntrantDocument.DocumentOrganization
            FieldList.Add("GenderTypeID", 9);       //Пол , GenderType.Name, Entrant.GenderID 
            FieldList.Add("NationalityTypeID", 10);     //Гражданство	, CountryType.Name, EntrantDocumentIdentity. NationalityTypeID
            FieldList.Add("BirthDate", 11);     //Дата рождения	, EntrantDocumentIdentity.BirthDate
            FieldList.Add("BirthPlace", 12);        //Место рождения, EntrantDocumentIdentity.BirthPlace
            FieldList.Add("AttachmentID", 13);      //Ссылка на документ, EntrantDocument.AttachmentID
            FieldList.Add("Series", 14);        //Серия	-	EntrantDocument.DocumentSeries
            FieldList.Add("Number", 15);        //Номер	-	EntrantDocument.DocumentNumber
            FieldList.Add("RegistrationNumber", 16);// Рег. Номер.	-	EntrantDocumentEdu. RegistrationNumber
            FieldList.Add("QualificationName", 17);// Квалификация	Direction.QualificationName	EntrantDocumentEdu.QualificationName
            FieldList.Add("SpecialityName", 18);// Направление подготовки, 	EntrantDocumentEdu.SpecialityName , 	IsNULL(Direction.NewCode,’’) / IsNULL(Direction.Code,’’) + ' ' + IsNULL(Direction.Name,’’) FROM Direction WHERE Direction.QualificationName = @Qual
            FieldList.Add("DocumentOU", 19);// ОУ, выдавшее документ	-	EntrantDocument.DocumentOrganization
            FieldList.Add("GPA", 20);// Средний балл	-	EntrantDocumentEdu.GPA
            FieldList.Add("OlympicDiplomaTypeID", 21);// Тип диплома (ОШ),	EntrantDocumentOlympic.DiplomaTypeID, OlympicDiplomType.Name
            FieldList.Add("OlympicTotalDiplomaTypeID", 22);// Тип диплома (ВОШ)	,EntrantDocumentOlympicTotal.DiplomaTypeID, OlympicDiplomType.Name
            FieldList.Add("SubjectDetails", 23);// Предметы	ApplicationEntranceTestDocument.SubjectID, Subject.Name
            FieldList.Add("OlympicDate", 24);// Дата проведения	EntrantDocumentOlympicTotal.OlympicDate
            FieldList.Add("OlympicPlace", 25);// Место проведения	-	EntrantDocumentOlympicTotal.OlympicPlace
            FieldList.Add("OlympicID", 26);// Олимпиада	EntrantDocumentOlympic.OlympicID, OlympicType.Name
            FieldList.Add("OlympicProfSubject", 27);// Профильные дисциплины, -,	SELECT S.name FROM Subject S INNER JOIN OlympicTypeSubjectLink OTSL ON S.SubjectID = OTSL.SubjectID WHERE OTSL.OlympicID = @Olymp 	
            FieldList.Add("OlympicLevelID", 28);// Уровень олимпиады,	-, OlympicType.OlympicLevelID	
            FieldList.Add("OlympicYear", 29);// Год олимпиады, -, 	OlympicType.OlympicYear	
            FieldList.Add("OrganizerName", 30);// Организатор,	- ,OlympicType.OrganizerName
            FieldList.Add("AdditionalInfo", 31);// Дополнительные сведения, EntrantDocumentCustom.AdditionalInfo 
            FieldList.Add("DocOrganization", 32);// Выдана	, EntrantDocument.DocumentOrganization, -	
            FieldList.Add("CertificateNumber", 33);// Номер свидетельства, EntrantDocument.DocumentNumber, -	
            FieldList.Add("GetEgeDataByNumber", 34);// Получить данные по номеру	-	-
            FieldList.Add("TypographicNumber", 35);// Типографский номер	-	EntrantDocumentEge.TypographicNumber
            FieldList.Add("CertificateYear", 36);// Год выдачи	-	Неизв. - EntrantDocument.DocumentDate Показвать тока год
            FieldList.Add("Subject", 37);// Дисциплина	,ApplicationEntranceTestDocument.SubjectID , Subject.Name
            FieldList.Add("SubjectEgeSertificate", 38);// Дисциплина (из сертификата ЕГЭ)	,	ApplicationEntranceTestDocument.SubjectID, Subject.Name
            FieldList.Add("ResultValue", 39);// Балл ,	ApplicationEntranceTestDocument.ResultValue, -	
            FieldList.Add("ResultValueEgeSertificate", 40);// Балл (из сертификата ЕГЭ)	ApplicationEntranceTestDocument.ResultValue, -	
            FieldList.Add("DocOrg", 41);// Кем выдана	, EntrantDocument.DocumentOrganization, -	
            FieldList.Add("DisabilityTypeID", 42);// Группа	,	EntrantDocumentDisability.DisabilityTypeID  , DisabilityType.Name
            FieldList.Add("OlympicCheck", 43);// Группа	,	EntrantDocumentDisability.DisabilityTypeID  , DisabilityType.Name			
            FieldList.Add("OlympicFormNumber", 44);// Класс обучения призера ОШ
            FieldList.Add("OlympicName", 45); //Наименование олимпиады
            FieldList.Add("OlympicProfile", 46);// Профиль олимпиады
            FieldList.Add("CountryID", 47);// Член сборной команды
            FieldList.Add("CompatriotCategoryID", 48);// Категория
            FieldList.Add("OrphanCategoryID", 49);// Категория
            FieldList.Add("VeteranCategoryID", 50);// Категория
            //FieldList.Add("DocumentName", 51);// Наименование документа

            FieldList.Add("LastName", 52);// фамилия
            FieldList.Add("FirstName", 53);// имя
            FieldList.Add("MiddleName", 54);// отчество

            FieldList.Add(Field_IsNostrificated, 55);// Нострифицирован 	-	EntrantDocumentEdu.IsNostrificated

            FieldList.Add("OlympicSubject", 56);// Профильная дисциплина олимпиады
            FieldList.Add("OlympicEgeSubject", 57);// Соответствующий профильной дисциплине общеобразовательный предмет

            FieldList.Add("ParentsLostCategoryID", 58);// Категория
            FieldList.Add("StateEmployeeCategoryID", 59);// Категория
            FieldList.Add("RadiationWorkCategoryID", 60);// Категория

            FieldList.Add("StateServicePreparation", 61);// Осуществляет подготовку к военной или иной государственной службе (преимущественное право)

            FieldList.Add("CompatriotStatus", 62); //Статус соотечественника

            FieldList.Add("InstitutionAddress",63);//Адрес образовательной организации
            FieldList.Add("FacultyName", 64);//Наименование факультета
            FieldList.Add("InstitutionName", 65);//Наименование учебного заведения
            FieldList.Add("BeginDate", 66);//Дата поступления в учебное заведение
            FieldList.Add("EndDate", 67);//Дата окончания учебного заведения
            FieldList.Add("EducationFormID", 68);//Форма обучения
            

            DocList.Add("Документ, удостоверяющий личность", 1);
            DocList.Add("Свидетельство о результатах ЕГЭ", 2);
            DocList.Add("Аттестат о среднем (полном) общем образовании", 3);
            DocList.Add("Диплом о высшем профессиональном образовании", 4);
            DocList.Add("Диплом о среднем профессиональном образовании", 5);
            DocList.Add("Диплом о начальном профессиональном образовании", 6);
            DocList.Add("Диплом о неполном высшем профессиональном образовании", 7);
            DocList.Add("Академическая справка", 8);
            DocList.Add("Диплом победителя/призера олимпиады школьников", 9);
            DocList.Add("Диплом победителя/призера всероссийской олимпиады школьников", 10);
            DocList.Add("Справка об установлении инвалидности", 11);
            DocList.Add("Заключение психолого-медико-педагогической комиссии", 12);
            DocList.Add("Заключение об отсутствии противопоказаний для обучения", 13);
            DocList.Add("Военный билет", 14);
            DocList.Add("Иной документ", 15);
            DocList.Add("Аттестат об основном общем образовании", 16);
            DocList.Add("Справка ГИА", 17);
            DocList.Add("Справка об обучении в другом ВУЗе", 18);
            DocList.Add("Иной документ об образовании", 19);
            DocList.Add("Диплом чемпиона/призера Олимпийских игр", 20);
            DocList.Add("Диплом чемпиона/призера Паралимпийских игр", 21);
            DocList.Add("Диплом чемпиона/призера Сурдлимпийских игр", 22);
            DocList.Add("Диплом чемпиона мира", 23);
            DocList.Add("Диплом чемпиона Европы", 24);
            DocList.Add("Диплом об окончании аспирантуры (адъюнктуры)", 25);
            DocList.Add("Диплом кандидата наук", 26);
            DocList.Add("Диплом победителя/призера IV этапа всеукраинской ученической олимпиады", 27);
            DocList.Add("Диплом об участии в международной олимпиаде", 28);
            DocList.Add("Документ, подтверждающий принадлежность к соотечественникам за рубежом", 29);
            DocList.Add("Документ, подтверждающий принадлежность к детям-сиротам и детям, оставшимся без попечения родителей", 30);
            DocList.Add("Документ, подтверждающий принадлежность к ветеранам боевых действий", 31);
            DocList.Add("Документ, подтверждающий наличие только одного родителя - инвалида I группы и принадлежность к числу малоимущих семей", 32);
            DocList.Add("Документ, подтверждающий принадлежность родителей и опекунов к погибшим в связи с исполнением служебных обязанностей", 33);
            DocList.Add("Документ, подтверждающий принадлежность к сотрудникам государственных органов Российской Федерации", 34);
            DocList.Add("Документ, подтверждающий участие в работах на радиационных объектах или воздействие радиации", 35);

            M[0, 1] = 2; M[0, 2] = 2; M[0, 3] = 2; M[0, 4] = 2; M[0, 5] = 2; M[0, 6] = 2; M[0, 7] = 2; M[0, 8] = 2; M[0, 9] = 2; M[0, 10] = 2; M[0, 11] = 2; M[0, 12] = 2; M[0, 13] = 2; M[0, 14] = 2; M[0, 15] = 2; M[0, 16] = 2; M[0, 17] = 2; M[0, 18] = 2; M[0, 19] = 2; M[0, 20] = 2; M[0, 21] = 2; M[0, 22] = 2; M[0, 23] = 2; M[0, 24] = 2; M[0, 25] = 2; M[0, 26] = 2;
            M[1, 1] = 0; M[1, 2] = 0; M[1, 3] = 0; M[1, 4] = 0; M[1, 5] = 0; M[1, 6] = 0; M[1, 7] = 0; M[1, 8] = 0; M[1, 9] = 0; M[1, 10] = 0; M[1, 11] = 0; M[1, 12] = 0; M[1, 13] = 0; M[1, 14] = 0; M[1, 15] = 2; M[1, 16] = 0; M[1, 17] = 0; M[1, 18] = 0; M[1, 19] = 2; M[1, 20] = 2; M[1, 21] = 2; M[1, 22] = 2; M[1, 23] = 2; M[1, 24] = 2; M[1, 25] = 0; M[1, 26] = 0;
            M[2, 1] = 1; M[2, 2] = 1; M[2, 3] = 1; M[2, 4] = 1; M[2, 5] = 1; M[2, 6] = 1; M[2, 7] = 1; M[2, 8] = 1; M[2, 9] = 1; M[2, 10] = 1; M[2, 11] = 1; M[2, 12] = 1; M[2, 13] = 1; M[2, 14] = 1; M[2, 15] = 1; M[2, 16] = 1; M[2, 17] = 1; M[2, 18] = 1; M[2, 19] = 1; M[2, 20] = 1; M[2, 21] = 1; M[2, 22] = 1; M[2, 23] = 1; M[2, 24] = 1; M[2, 25] = 1; M[2, 26] = 1;
            M[3, 1] = 2; M[3, 2] = 0; M[3, 3] = 0; M[3, 4] = 0; M[3, 5] = 0; M[3, 6] = 0; M[3, 7] = 0; M[3, 8] = 0; M[3, 9] = 0; M[3, 10] = 0; M[3, 11] = 0; M[3, 12] = 0; M[3, 13] = 0; M[3, 14] = 0; M[3, 15] = 0; M[3, 16] = 0; M[3, 17] = 0; M[3, 18] = 0; M[3, 19] = 0; M[3, 20] = 0; M[3, 21] = 0; M[3, 22] = 0; M[3, 23] = 0; M[3, 24] = 0; M[3, 25] = 0; M[3, 26] = 0;
            M[4, 1] = 2; M[4, 2] = 0; M[4, 3] = 1; M[4, 4] = 0; M[4, 5] = 0; M[4, 6] = 0; M[4, 7] = 0; M[4, 8] = 0; M[4, 9] = 1; M[4, 10] = 1; M[4, 11] = 1; M[4, 12] = 0; M[4, 13] = 0; M[4, 14] = 1; M[4, 15] = 1; M[4, 16] = 1; M[4, 17] = 0; M[4, 18] = 0; M[4, 19] = 1; M[4, 20] = 1; M[4, 21] = 1; M[4, 22] = 1; M[4, 23] = 1; M[4, 24] = 1; M[4, 25] = 0; M[4, 26] = 0;
            M[5, 1] = 2; M[5, 2] = 0; M[5, 3] = 2; M[5, 4] = 0; M[5, 5] = 0; M[5, 6] = 0; M[5, 7] = 0; M[5, 8] = 0; M[5, 9] = 2; M[5, 10] = 2; M[5, 11] = 2; M[5, 12] = 0; M[5, 13] = 0; M[5, 14] = 2; M[5, 15] = 1; M[5, 16] = 2; M[5, 17] = 2; M[5, 18] = 2; M[5, 19] = 1; M[5, 20] = 1; M[5, 21] = 1; M[5, 22] = 1; M[5, 23] = 1; M[5, 24] = 1; M[5, 25] = 0; M[5, 26] = 0;
            M[6, 1] = 2; M[6, 2] = 0; M[6, 3] = 2; M[6, 4] = 2; M[6, 5] = 2; M[6, 6] = 2; M[6, 7] = 2; M[6, 8] = 2; M[6, 9] = 0; M[6, 10] = 0; M[6, 11] = 2; M[6, 12] = 2; M[6, 13] = 2; M[6, 14] = 2; M[6, 15] = 2; M[6, 16] = 2; M[6, 17] = 2; M[6, 18] = 2; M[6, 19] = 2; M[6, 20] = 2; M[6, 21] = 2; M[6, 22] = 2; M[6, 23] = 2; M[6, 24] = 2; M[6, 25] = 2; M[6, 26] = 2;
            M[7, 1] = 1; M[7, 2] = 0; M[7, 3] = 0; M[7, 4] = 0; M[7, 5] = 0; M[7, 6] = 0; M[7, 7] = 0; M[7, 8] = 0; M[7, 9] = 0; M[7, 10] = 0; M[7, 11] = 0; M[7, 12] = 0; M[7, 13] = 0; M[7, 14] = 0; M[7, 15] = 0; M[7, 16] = 0; M[7, 17] = 0; M[7, 18] = 0; M[7, 19] = 0; M[7, 20] = 0; M[7, 21] = 0; M[7, 22] = 0; M[7, 23] = 0; M[7, 24] = 0; M[7, 25] = 0; M[7, 26] = 0;
            M[8, 1] = 1; M[8, 2] = 0; M[8, 3] = 2; M[8, 4] = 0; M[8, 5] = 0; M[8, 6] = 0; M[8, 7] = 0; M[8, 8] = 0; M[8, 9] = 0; M[8, 10] = 0; M[8, 11] = 2; M[8, 12] = 2; M[8, 13] = 2; M[8, 14] = 2; M[8, 15] = 2; M[8, 16] = 2; M[8, 17] = 2; M[8, 18] = 2; M[8, 19] = 2; M[8, 20] = 2; M[8, 21] = 2; M[8, 22] = 2; M[8, 23] = 2; M[8, 24] = 2; M[8, 25] = 0; M[8, 26] = 0;
            M[8, 17] = 2;
            M[9, 1] = 2; M[9, 2] = 0; M[9, 3] = 0; M[9, 4] = 0; M[9, 5] = 0; M[9, 6] = 0; M[9, 7] = 0; M[9, 8] = 0; M[9, 9] = 0; M[9, 10] = 0; M[9, 11] = 0; M[9, 12] = 0; M[9, 13] = 0; M[9, 14] = 0; M[9, 15] = 0; M[9, 16] = 0; M[9, 17] = 0; M[9, 18] = 0; M[9, 19] = 0; M[9, 20] = 0; M[9, 21] = 0; M[9, 22] = 0; M[9, 23] = 0; M[9, 24] = 0; M[9, 25] = 0; M[9, 26] = 0;
            M[10, 1] = 2; M[10, 2] = 0; M[10, 3] = 0; M[10, 4] = 0; M[10, 5] = 0; M[10, 6] = 0; M[10, 7] = 0; M[10, 8] = 0; M[10, 9] = 0; M[10, 10] = 0; M[10, 11] = 0; M[10, 12] = 0; M[10, 13] = 0; M[10, 14] = 0; M[10, 15] = 0; M[10, 16] = 0; M[10, 17] = 0; M[10, 18] = 0; M[10, 19] = 0; M[10, 20] = 0; M[10, 21] = 0; M[10, 22] = 0; M[10, 23] = 0; M[10, 24] = 0; M[10, 25] = 0; M[10, 26] = 0;
            M[11, 1] = 2; M[11, 2] = 0; M[11, 3] = 0; M[11, 4] = 0; M[11, 5] = 0; M[11, 6] = 0; M[11, 7] = 0; M[11, 8] = 0; M[11, 9] = 0; M[11, 10] = 0; M[11, 11] = 0; M[11, 12] = 0; M[11, 13] = 0; M[11, 14] = 0; M[11, 15] = 0; M[11, 16] = 0; M[11, 17] = 0; M[11, 18] = 0; M[11, 19] = 0; M[11, 20] = 0; M[11, 21] = 0; M[11, 22] = 0; M[11, 23] = 0; M[11, 24] = 0; M[11, 25] = 0; M[11, 26] = 0;
            M[12, 1] = 1; M[12, 2] = 0; M[12, 3] = 0; M[12, 4] = 0; M[12, 5] = 0; M[12, 6] = 0; M[12, 7] = 0; M[12, 8] = 0; M[12, 9] = 0; M[12, 10] = 0; M[12, 11] = 0; M[12, 12] = 0; M[12, 13] = 0; M[12, 14] = 0; M[12, 15] = 0; M[12, 16] = 0; M[12, 17] = 0; M[12, 18] = 0; M[12, 19] = 0; M[12, 20] = 0; M[12, 21] = 0; M[12, 22] = 0; M[12, 23] = 0; M[12, 24] = 0; M[12, 25] = 0; M[12, 26] = 0;
            M[13, 1] = 1; M[13, 2] = 1; M[13, 3] = 1; M[13, 4] = 1; M[13, 5] = 1; M[13, 6] = 1; M[13, 7] = 1; M[13, 8] = 1; M[13, 9] = 1; M[13, 10] = 1; M[13, 11] = 1; M[13, 12] = 1; M[13, 13] = 1; M[13, 14] = 1; M[13, 15] = 1; M[13, 16] = 1; M[13, 17] = 1; M[13, 18] = 1; M[13, 19] = 1; M[13, 20] = 1; M[13, 21] = 1; M[13, 22] = 1; M[13, 23] = 1; M[13, 24] = 1; M[13, 25] = 1; M[13, 26] = 1;
            M[14, 1] = 0; M[14, 2] = 0; M[14, 3] = 1; M[14, 4] = 1; M[14, 5] = 1; M[14, 6] = 1; M[14, 7] = 1; M[14, 8] = 1; M[14, 9] = 2; M[14, 10] = 2; M[14, 11] = 1; M[14, 12] = 0; M[14, 13] = 0; M[14, 14] = 1; M[14, 15] = 1; M[14, 16] = 1; M[14, 17] = 0; M[14, 18] = 0; M[14, 19] = 1; M[14, 20] = 1; M[14, 21] = 1; M[14, 22] = 1; M[14, 23] = 1; M[14, 24] = 1; M[14, 25] = 1; M[14, 26] = 1;
            M[15, 1] = 1; M[15, 2] = 0; M[15, 3] = 2; M[15, 4] = 2; M[15, 5] = 2; M[15, 6] = 2; M[15, 7] = 2; M[15, 8] = 2; M[15, 9] = 2; M[15, 10] = 2; M[15, 11] = 0; M[15, 12] = 2; M[15, 13] = 2; M[15, 14] = 2; M[15, 15] = 1; M[15, 16] = 0; M[15, 17] = 2; M[15, 18] = 1; M[15, 19] = 1; M[15, 20] = 1; M[15, 21] = 1; M[15, 22] = 1; M[15, 23] = 1; M[15, 24] = 1; M[15, 25] = 2; M[15, 26] = 2;
            M[16, 1] = 0; M[16, 2] = 0; M[16, 3] = 0; M[16, 4] = 1; M[16, 5] = 1; M[16, 6] = 1; M[16, 7] = 1; M[16, 8] = 1; M[16, 9] = 0; M[16, 10] = 0; M[16, 11] = 0; M[16, 12] = 0; M[16, 13] = 0; M[16, 14] = 0; M[16, 15] = 0; M[16, 16] = 0; M[16, 17] = 0; M[16, 18] = 0; M[16, 19] = 0; M[16, 20] = 0; M[16, 21] = 0; M[16, 22] = 0; M[16, 23] = 0; M[16, 24] = 0; M[16, 25] = 1; M[16, 26] = 1;
            //M[17, 1] = 0; M[17, 2] = 0; M[17, 3] = 0; M[17, 4] = 1; M[17, 5] = 1; M[17, 6] = 1; M[17, 7] = 1; M[17, 8] = 1; M[17, 9] = 0; M[17, 10] = 0; M[17, 11] = 0; M[17, 12] = 0; M[17, 13] = 0; M[17, 14] = 0; M[17, 15] = 0; M[17, 16] = 0; M[17, 17] = 0; M[17, 18] = 0; M[17, 19] = 0; M[17, 20] = 0; M[17, 21] = 0; M[17, 22] = 0; M[17, 23] = 0; M[17, 24] = 0; M[17, 25] = 1; M[17, 26] = 1;
            M[18, 1] = 0; M[18, 2] = 0; M[18, 3] = 0; M[18, 4] = 1; M[18, 5] = 1; M[18, 6] = 1; M[18, 7] = 1; M[18, 8] = 1; M[18, 9] = 0; M[18, 10] = 0; M[18, 11] = 0; M[18, 12] = 0; M[18, 13] = 0; M[18, 14] = 0; M[18, 15] = 0; M[18, 16] = 0; M[18, 17] = 0; M[18, 18] = 0; M[18, 19] = 0; M[18, 20] = 0; M[18, 21] = 0; M[18, 22] = 0; M[18, 23] = 0; M[18, 24] = 0; M[18, 25] = 1; M[18, 26] = 1;
            M[19, 1] = 0; M[19, 2] = 0; M[19, 3] = 0; M[19, 4] = 2; M[19, 5] = 2; M[19, 6] = 2; M[19, 7] = 2; M[19, 8] = 2; M[19, 9] = 0; M[19, 10] = 0; M[19, 11] = 0; M[19, 12] = 0; M[19, 13] = 0; M[19, 14] = 0; M[19, 15] = 0; M[19, 16] = 0; M[19, 17] = 0; M[19, 18] = 0; M[19, 19] = 0; M[19, 20] = 0; M[19, 21] = 0; M[19, 22] = 0; M[19, 23] = 0; M[19, 24] = 0; M[19, 25] = 2; M[19, 26] = 2;
            M[20, 1] = 0; M[20, 2] = 0; M[20, 3] = 1; M[20, 4] = 1; M[20, 5] = 1; M[20, 6] = 1; M[20, 7] = 1; M[20, 8] = 1; M[20, 9] = 0; M[20, 10] = 0; M[20, 11] = 0; M[20, 12] = 0; M[20, 13] = 0; M[20, 14] = 0; M[20, 15] = 0; M[20, 16] = 1; M[20, 17] = 0; M[20, 18] = 0; M[20, 19] = 0; M[20, 20] = 0; M[20, 21] = 0; M[20, 22] = 0; M[20, 23] = 0; M[20, 24] = 0; M[20, 25] = 1; M[20, 26] = 1;
            M[21, 1] = 0; M[21, 2] = 0; M[21, 3] = 0; M[21, 4] = 0; M[21, 5] = 0; M[21, 6] = 0; M[21, 7] = 0; M[21, 8] = 0; M[21, 9] = 2; M[21, 10] = 2; M[21, 11] = 0; M[21, 12] = 0; M[21, 13] = 0; M[21, 14] = 0; M[21, 15] = 0; M[21, 16] = 0; M[21, 17] = 0; M[21, 18] = 0; M[21, 19] = 0; M[21, 20] = 0; M[21, 21] = 0; M[21, 22] = 0; M[21, 23] = 0; M[21, 24] = 0; M[21, 25] = 0; M[21, 26] = 0;
            M[22, 1] = 0; M[22, 2] = 0; M[22, 3] = 0; M[22, 4] = 0; M[22, 5] = 0; M[22, 6] = 0; M[22, 7] = 0; M[22, 8] = 0; M[22, 9] = 0; M[22, 10] = 2; M[22, 11] = 0; M[22, 12] = 0; M[22, 13] = 0; M[22, 14] = 0; M[22, 15] = 0; M[22, 16] = 0; M[22, 17] = 0; M[22, 18] = 0; M[22, 19] = 0; M[22, 20] = 0; M[22, 21] = 0; M[22, 22] = 0; M[22, 23] = 0; M[22, 24] = 0; M[22, 25] = 0; M[22, 26] = 0;
            M[23, 1] = 0; M[23, 2] = 0; M[23, 3] = 0; M[23, 4] = 0; M[23, 5] = 0; M[23, 6] = 0; M[23, 7] = 0; M[23, 8] = 0; M[23, 9] = 0; M[23, 10] = 2; M[23, 11] = 0; M[23, 12] = 0; M[23, 13] = 0; M[23, 14] = 0; M[23, 15] = 0; M[23, 16] = 0; M[23, 17] = 0; M[23, 18] = 0; M[23, 19] = 0; M[23, 20] = 0; M[23, 21] = 0; M[23, 22] = 0; M[23, 23] = 0; M[23, 24] = 0; M[23, 25] = 0; M[23, 26] = 0;
            M[24, 1] = 0; M[24, 2] = 0; M[24, 3] = 0; M[24, 4] = 0; M[24, 5] = 0; M[24, 6] = 0; M[24, 7] = 0; M[24, 8] = 0; M[24, 9] = 0; M[24, 10] = 1; M[24, 11] = 0; M[24, 12] = 0; M[24, 13] = 0; M[24, 14] = 0; M[24, 15] = 0; M[24, 16] = 0; M[24, 17] = 0; M[24, 18] = 0; M[24, 19] = 0; M[24, 20] = 0; M[24, 21] = 0; M[24, 22] = 0; M[24, 23] = 0; M[24, 24] = 0; M[24, 25] = 0; M[24, 26] = 0;
            M[25, 1] = 0; M[25, 2] = 0; M[25, 3] = 0; M[25, 4] = 0; M[25, 5] = 0; M[25, 6] = 0; M[25, 7] = 0; M[25, 8] = 0; M[25, 9] = 0; M[25, 10] = 1; M[25, 11] = 0; M[25, 12] = 0; M[25, 13] = 0; M[25, 14] = 0; M[25, 15] = 0; M[25, 16] = 0; M[25, 17] = 0; M[25, 18] = 0; M[25, 19] = 0; M[25, 20] = 0; M[25, 21] = 0; M[25, 22] = 0; M[25, 23] = 0; M[25, 24] = 0; M[25, 25] = 0; M[25, 26] = 0;
            M[26, 1] = 0; M[26, 2] = 0; M[26, 3] = 0; M[26, 4] = 0; M[26, 5] = 0; M[26, 6] = 0; M[26, 7] = 0; M[26, 8] = 0; M[26, 9] = 2; M[26, 10] = 2; M[26, 11] = 0; M[26, 12] = 0; M[26, 13] = 0; M[26, 14] = 0; M[26, 15] = 0; M[26, 16] = 0; M[26, 17] = 0; M[26, 18] = 0; M[26, 19] = 0; M[26, 20] = 0; M[26, 21] = 0; M[26, 22] = 0; M[26, 23] = 0; M[26, 24] = 0; M[26, 25] = 0; M[26, 26] = 0;
            M[27, 1] = 0; M[27, 2] = 0; M[27, 3] = 0; M[27, 4] = 0; M[27, 5] = 0; M[27, 6] = 0; M[27, 7] = 0; M[27, 8] = 0; M[27, 9] = 2; M[27, 10] = 2; M[27, 11] = 0; M[27, 12] = 0; M[27, 13] = 0; M[27, 14] = 0; M[27, 15] = 0; M[27, 16] = 0; M[27, 17] = 0; M[27, 18] = 0; M[27, 19] = 0; M[27, 20] = 0; M[27, 21] = 0; M[27, 22] = 0; M[27, 23] = 0; M[27, 24] = 0; M[27, 25] = 0; M[27, 26] = 0;
            M[28, 1] = 0; M[28, 2] = 0; M[28, 3] = 0; M[28, 4] = 0; M[28, 5] = 0; M[28, 6] = 0; M[28, 7] = 0; M[28, 8] = 0; M[28, 9] = 2; M[28, 10] = 2; M[28, 11] = 0; M[28, 12] = 0; M[28, 13] = 0; M[28, 14] = 0; M[28, 15] = 0; M[28, 16] = 0; M[28, 17] = 0; M[28, 18] = 0; M[28, 19] = 0; M[28, 20] = 0; M[28, 21] = 0; M[28, 22] = 0; M[28, 23] = 0; M[28, 24] = 0; M[28, 25] = 0; M[28, 26] = 0;
            M[29, 1] = 1; M[29, 2] = 0; M[29, 3] = 0; M[29, 4] = 0; M[29, 5] = 0; M[29, 6] = 0; M[29, 7] = 0; M[29, 8] = 0; M[29, 9] = 2; M[29, 10] = 2; M[29, 11] = 0; M[29, 12] = 0; M[29, 13] = 0; M[29, 14] = 0; M[29, 15] = 0; M[29, 16] = 0; M[29, 17] = 0; M[29, 18] = 0; M[29, 19] = 0; M[29, 20] = 0; M[29, 21] = 0; M[29, 22] = 0; M[29, 23] = 0; M[29, 24] = 0; M[29, 25] = 0; M[29, 26] = 0;
            M[30, 1] = 0; M[30, 2] = 0; M[30, 3] = 0; M[30, 4] = 0; M[30, 5] = 0; M[30, 6] = 0; M[30, 7] = 0; M[30, 8] = 0; M[30, 9] = 2; M[30, 10] = 2; M[30, 11] = 0; M[30, 12] = 0; M[30, 13] = 0; M[30, 14] = 0; M[30, 15] = 0; M[30, 16] = 0; M[30, 17] = 0; M[30, 18] = 0; M[30, 19] = 0; M[30, 20] = 0; M[30, 21] = 0; M[30, 22] = 0; M[30, 23] = 0; M[30, 24] = 0; M[30, 25] = 0; M[30, 26] = 0;
            M[31, 1] = 0; M[31, 2] = 0; M[31, 3] = 0; M[31, 4] = 0; M[31, 5] = 0; M[31, 6] = 0; M[31, 7] = 0; M[31, 8] = 0; M[31, 9] = 0; M[31, 10] = 0; M[31, 11] = 0; M[31, 12] = 0; M[31, 13] = 0; M[31, 14] = 0; M[31, 15] = 1; M[31, 16] = 0; M[31, 17] = 0; M[31, 18] = 0; M[31, 19] = 1; M[31, 20] = 1; M[31, 21] = 0; M[31, 22] = 1; M[31, 23] = 1; M[31, 24] = 1; M[31, 25] = 0; M[31, 26] = 0;
            M[32, 1] = 0; M[32, 2] = 0; M[32, 3] = 0; M[32, 4] = 0; M[32, 5] = 0; M[32, 6] = 0; M[32, 7] = 0; M[32, 8] = 0; M[32, 9] = 0; M[32, 10] = 0; M[32, 11] = 0; M[32, 12] = 2; M[32, 13] = 2; M[32, 14] = 0; M[32, 15] = 0; M[32, 16] = 0; M[32, 17] = 0; M[32, 18] = 0; M[32, 19] = 0; M[32, 20] = 0; M[32, 21] = 0; M[32, 22] = 0; M[32, 23] = 0; M[32, 24] = 0; M[32, 25] = 0; M[32, 26] = 0;
            M[33, 1] = 0; M[33, 2] = 2; M[33, 3] = 0; M[33, 4] = 0; M[33, 5] = 0; M[33, 6] = 0; M[33, 7] = 0; M[33, 8] = 0; M[33, 9] = 0; M[33, 10] = 0; M[33, 11] = 0; M[33, 12] = 0; M[33, 13] = 0; M[33, 14] = 0; M[33, 15] = 0; M[33, 16] = 0; M[33, 17] = 0; M[33, 18] = 0; M[33, 19] = 0; M[33, 20] = 0; M[33, 21] = 0; M[33, 22] = 0; M[33, 23] = 0; M[33, 24] = 0; M[33, 25] = 0; M[33, 26] = 0;
            M[34, 1] = 0; M[34, 2] = 1; M[34, 3] = 0; M[34, 4] = 0; M[34, 5] = 0; M[34, 6] = 0; M[34, 7] = 0; M[34, 8] = 0; M[34, 9] = 0; M[34, 10] = 0; M[34, 11] = 0; M[34, 12] = 0; M[34, 13] = 0; M[34, 14] = 0; M[34, 15] = 0; M[34, 16] = 0; M[34, 17] = 0; M[34, 18] = 0; M[34, 19] = 0; M[34, 20] = 0; M[34, 21] = 0; M[34, 22] = 0; M[34, 23] = 0; M[34, 24] = 0; M[34, 25] = 0; M[34, 26] = 0;
            M[35, 1] = 0; M[35, 2] = 1; M[35, 3] = 0; M[35, 4] = 0; M[35, 5] = 0; M[35, 6] = 0; M[35, 7] = 0; M[35, 8] = 0; M[35, 9] = 0; M[35, 10] = 0; M[35, 11] = 0; M[35, 12] = 0; M[35, 13] = 0; M[35, 14] = 0; M[35, 15] = 0; M[35, 16] = 0; M[35, 17] = 0; M[35, 18] = 0; M[35, 19] = 0; M[35, 20] = 0; M[35, 21] = 0; M[35, 22] = 0; M[35, 23] = 0; M[35, 24] = 0; M[35, 25] = 0; M[35, 26] = 0;
            M[36, 1] = 0; M[36, 2] = 2; M[36, 3] = 0; M[36, 4] = 0; M[36, 5] = 0; M[36, 6] = 0; M[36, 7] = 0; M[36, 8] = 0; M[36, 9] = 0; M[36, 10] = 0; M[36, 11] = 0; M[36, 12] = 0; M[36, 13] = 0; M[36, 14] = 0; M[36, 15] = 0; M[36, 16] = 0; M[36, 17] = 0; M[36, 18] = 0; M[36, 19] = 0; M[36, 20] = 0; M[36, 21] = 0; M[36, 22] = 0; M[36, 23] = 0; M[36, 24] = 0; M[36, 25] = 0; M[36, 26] = 0;
            M[37, 1] = 0;
            M[38, 2] = 2; M[38, 3] = 2; M[38, 16] = 2; M[38, 17] = 2;
            M[39, 1] = 0; M[39, 2] = 0; M[39, 3] = 0; M[39, 4] = 0; M[39, 5] = 0; M[39, 6] = 0; M[39, 7] = 0; M[39, 8] = 0; M[39, 9] = 0; M[39, 10] = 0; M[39, 11] = 0; M[39, 12] = 0; M[39, 13] = 0; M[39, 14] = 0; M[39, 15] = 0; M[39, 16] = 0; M[39, 17] = 2; M[39, 18] = 0; M[39, 19] = 0; M[39, 20] = 0; M[39, 21] = 0; M[39, 22] = 0; M[39, 23] = 0; M[39, 24] = 0; M[39, 25] = 0; M[39, 26] = 0;
            M[40, 1] = 0; M[40, 2] = 2; M[40, 3] = 2; M[40, 4] = 0; M[40, 5] = 0; M[40, 6] = 0; M[40, 7] = 0; M[40, 8] = 0; M[40, 9] = 0; M[40, 10] = 0; M[40, 11] = 0; M[40, 12] = 0; M[40, 13] = 0; M[40, 14] = 0; M[40, 15] = 0; M[40, 16] = 2; M[40, 17] = 2; M[40, 18] = 0; M[40, 19] = 0; M[40, 20] = 0; M[40, 21] = 0; M[40, 22] = 0; M[40, 23] = 0; M[40, 24] = 0; M[40, 25] = 0; M[40, 26] = 0;
            M[41, 1] = 0; M[41, 2] = 0; M[41, 3] = 0; M[41, 4] = 0; M[41, 5] = 0; M[41, 6] = 0; M[41, 7] = 0; M[41, 8] = 0; M[41, 9] = 0; M[41, 10] = 0; M[41, 11] = 2; M[41, 12] = 0; M[41, 13] = 0; M[41, 14] = 0; M[41, 15] = 0; M[41, 16] = 0; M[41, 17] = 2; M[41, 18] = 0; M[41, 19] = 0; M[41, 20] = 0; M[41, 21] = 0; M[41, 22] = 0; M[41, 23] = 0; M[41, 24] = 0; M[41, 25] = 0; M[41, 26] = 0;
            M[42, 1] = 0; M[42, 2] = 0; M[42, 3] = 0; M[42, 4] = 0; M[42, 5] = 0; M[42, 6] = 0; M[42, 7] = 0; M[42, 8] = 0; M[42, 9] = 0; M[42, 10] = 0; M[42, 11] = 2; M[42, 12] = 0; M[42, 13] = 0; M[42, 14] = 0; M[42, 15] = 0; M[42, 16] = 0; M[42, 17] = 0; M[42, 18] = 0; M[42, 19] = 0; M[42, 20] = 0; M[42, 21] = 0; M[42, 22] = 0; M[42, 23] = 0; M[42, 24] = 0; M[42, 25] = 0; M[42, 26] = 0;

            int f = 0;

            f = 0; SetAllDocForFiled(f, 1);
            f = 2; SetAllDocForFiled(f, 1);
            f = 3; SetAllDocForFiled(f, 0); M[f, 1] = 2;
            f = 4; M[f, 1] = 2; M[f, 8] = 2; M[f, 26] = 2; M[f, 4] = 2; M[f, 6] = 2; M[f, 7] = 2; M[f, 5] = 2; M[f, 25] = 2; M[f, 15] = 1;
            f = 5; M[f, 1] = 2; M[f, 8] = 2; M[f, 26] = 2; M[f, 4] = 2; M[f, 6] = 2; M[f, 7] = 2; M[f, 5] = 2; M[f, 25] = 2; M[f, 15] = 1;
            f = 6; M[f, 1] = 2; M[f, 8] = 2; M[f, 26] = 2; M[f, 4] = 2; M[f, 6] = 2; M[f, 7] = 2; M[f, 5] = 2; M[f, 25] = 2;
            f = 7; M[f, 1] = 2;
            f = 9; M[f, 1] = 2;
            f = 10; M[f, 1] = 2;
            f = 11; M[f, 1] = 2;
            f = 12; M[f, 1] = 2;
            f = 13; M[f, 1] = 2;
            f = 16; M[f, 8] = 1; M[f, 26] = 1; M[f, 4] = 1; M[f, 6] = 1; M[f, 7] = 1; M[f, 5] = 1; M[f, 25] = 1;
            f = 17; M[f, 8] = 1; M[f, 26] = 1; M[f, 4] = 1; M[f, 6] = 1; M[f, 7] = 1; M[f, 5] = 1; M[f, 25] = 1;
            f = 18; M[f, 8] = 1; M[f, 26] = 1; M[f, 4] = 1; M[f, 6] = 1; M[f, 7] = 1; M[f, 5] = 1; M[f, 25] = 1;
            f = 19; M[f, 8] = 2; M[f, 26] = 2; M[f, 4] = 2; M[f, 6] = 2; M[f, 7] = 2; M[f, 5] = 2; M[f, 25] = 2;
            f = 20; M[f, 8] = 1; M[f, 26] = 1; M[f, 4] = 1; M[f, 6] = 1; M[f, 7] = 1; M[f, 5] = 1; M[f, 25] = 1;

            //Нострицикация для документов об образовании (типы 3,4,5,6,7,8,16,18,19,25,26)
            f = 55; M[f, 3] = 2; M[f, 4] = 2; M[f, 5] = 2; M[f, 6] = 2; M[f, 7] = 2; M[f, 8] = 2; M[f, 16] = 2; M[f, 18] = 2; M[f, 19] = 2; M[f, 25] = 2; M[f, 26] = 2;

            //Осуществляет подготовку к военной или иной государственной службе (преимущественное право) (типы 3,5,6,16)
            f = 61; M[f, 3] = 2; M[f, 5] = 2; M[f, 6] = 2; M[f, 16] = 2;

            // 9 документ
            M[21, 9] = 2; M[23, 9] = 0; M[24, 9] = 0; M[25, 9] = 0;
            M[26, 9] = 1; M[27, 9] = 1; M[43, 9] = 1; M[44, 9] = 1; M[56, 9] = 1; M[57, 9] = 1;

            // 10 документ
            M[21, 10] = 2; M[23, 10] = 0; M[24, 10] = 0; M[25, 10] = 0;
            M[26, 10] = 1; M[27, 10] = 1; M[43, 10] = 1; M[44, 10] = 1;

            // 27 документ
            M[45, 27] = 2; M[4, 27] = 2; M[5, 27] = 2; M[21, 27] = 2;
            M[46, 27] = 2; M[24, 27] = 1; M[25, 27] = 1; M[13, 27] = 1;

            // 28 документ
            M[45, 28] = 2; M[4, 28] = 2; M[5, 28] = 2; M[47, 28] = 2; 
            M[46, 28] = 1; M[24, 28] = 1; M[25, 28] = 1; M[13, 28] = 1;

            // 29 документ
            M[1, 29] = 2; M[48, 29] = 2; M[4, 29] = 2; M[5, 29] = 2; M[6, 29] = 2; M[8, 29] = 2; M[13, 29] = 1; M[62, 29] = 2;

            // 30 документ
            M[1, 30] = 2; M[49, 30] = 2; M[4, 30] = 2; M[5, 30] = 2; M[6, 30] = 2; M[8, 30] = 2; M[13, 30] = 1;

            // 31 документ
            M[1, 31] = 2; M[50, 31] = 2; M[4, 31] = 2; M[5, 31] = 2; M[6, 31] = 2; M[8, 31] = 2; M[13, 31] = 1;

            //новые документы (2017 год)
            // 32 документ
            M[1, 32] = 2; M[4, 32] = 2; M[5, 32] = 2; M[6, 32] = 2; M[8, 32] = 2; M[13, 32] = 1;

            // 33 документ
            M[1, 33] = 2; M[4, 33] = 2; M[5, 33] = 2; M[6, 33] = 2; M[8, 33] = 2; M[13, 33] = 1; M[58, 33] = 1;

            // 34 документ
            M[1, 34] = 2; M[4, 34] = 2; M[5, 34] = 2; M[6, 34] = 2; M[8, 34] = 2; M[13, 34] = 1; M[59, 34] = 1;

            // 35 документ
            M[1, 35] = 2; M[4, 35] = 2; M[5, 35] = 2; M[6, 35] = 2; M[8, 35] = 2; M[13, 35] = 1; M[60, 35] = 1;

            // фио для документа типа 1 
            M[52, 1] = 2; M[53, 1] = 2; M[54, 1] = 2;


            //M[17, 3] = 1; M[47, 3] = 1; M[63, 3] = 1; M[64, 3] = 1; M[65, 3] = 1; M[66, 3] = 1; M[67, 3] = 1; M[68, 3] = 1;
            M[17, 4] = 1; M[47, 4] = 1; M[63, 4] = 1; M[64, 4] = 1; M[65, 4] = 1; M[66, 4] = 1; M[67, 4] = 1; M[68, 4] = 1;
            M[17, 5] = 1; M[47, 5] = 1; M[63, 5] = 1; M[64, 5] = 1; M[65, 5] = 1; M[66, 5] = 1; M[67, 5] = 1; M[68, 5] = 1;
            M[17, 6] = 1; M[47, 6] = 1; M[63, 6] = 1; M[64, 6] = 1; M[65, 6] = 1; M[66, 6] = 1; M[67, 6] = 1; M[68, 6] = 1;
            M[17, 7] = 1; M[47, 7] = 1; M[63, 7] = 1; M[64, 7] = 1; M[65, 7] = 1; M[66, 7] = 1; M[67, 7] = 1; M[68, 7] = 1;
            M[17, 8] = 1; M[47, 8] = 1; M[63, 8] = 1; M[64, 8] = 1; M[65, 8] = 1; M[66, 8] = 1; M[67, 8] = 1; M[68, 8] = 1;
            
            //M[17, 16] = 1; M[47, 16] = 1; M[63, 16] = 1; M[64, 16] = 1; M[65, 16] = 1; M[66, 16] = 1; M[67, 16] = 1; M[68, 16] = 1;
            M[17, 18] = 1; M[47, 18] = 1; M[63, 18] = 1; M[64, 18] = 1; M[65, 18] = 1; M[66, 18] = 1; M[67, 18] = 1; M[68, 18] = 1;
            M[17, 19] = 1; M[47, 19] = 1; M[63, 19] = 1; M[64, 19] = 1; M[65, 19] = 1; M[66, 19] = 1; M[67, 19] = 1; M[68, 19] = 1;
            M[17, 25] = 1; M[47, 25] = 1; M[63, 25] = 1; M[64, 25] = 1; M[65, 25] = 1; M[66, 25] = 1; M[67, 25] = 1; M[68, 25] = 1;
            M[17, 26] = 1; M[47, 26] = 1; M[63, 26] = 1; M[64, 26] = 1; M[65, 26] = 1; M[66, 26] = 1; M[67, 26] = 1; M[68, 26] = 1;
        }
    }

    public class UniDocumentViewModel
    {

        public UniDocumentViewModel()
        {
            ListIdentityDocumentType = new List<IdentityDocumentType>();
        }

        public EntrantDocumentViewModel UniD { get; set; }
        public int MaxFileSize { get; set; }

        public List<IdentityDocumentType> ListIdentityDocumentType { get; set; }
    }

    public class IdentityDocumentType
    {
        public int IdentityDocumentTypeId { get; set; }
        public string IdentityDocumentTypeName { get; set; }
        public bool IsRussianNationality { get; set; }
    }

    public class EntrantDocumentViewModel
    {

        public EntrantDocumentViewModel() { }
        //--------------------------------------------------------------------------------//
        [XmlIgnore]
        [ScriptIgnore]
        public int ApplicationID { get; set; }

        [DisplayName("UID")]
        [StringLength(200)]
        public string UID { get; set; }

        public int EntrantID { get; set; }
        public int EntrantDocumentID { get; set; }

        [DisplayName("Тип документа")]
        [LocalRequired]
        public int DocumentTypeID { get; set; }
        [DisplayName("Тип документа")]
        public string DocumentTypeName { get; set; }

        [DisplayName("Наименование документа")]
        [LocalRequired]
        [StringLength(500)]
        public string DocumentName { get; set; }

        [DisplayName("Наименование олимпиады")]
        [StringLength(255)]
        public string OlympicName { get; set; }

        [DisplayName("Серия документа")]
        [LocalRequired]
        [StringLength(20)]
        public string DocumentSeries { get; set; }

        [DisplayName("Номер документа")]
        [LocalRequired]
        [StringLength(50)]
        public string DocumentNumber { get; set; }

        [DisplayName("Серия и номер документа")]
        [LocalRequired]
        public string DocumentSeriesNumber { get { return (DocumentSeries + " " + DocumentNumber); } }

        [DisplayName("Дата выдачи")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Date(">today-100y")]
        [Date("<=today")]
        [LocalRequired]
        public DateTime? DocumentDate { get; set; }

        [LocalRequired]
        [DisplayName(@"Год выдачи")]
        [LocalRange(2000, 2100)]
        public int? DocumentYear
        {
            get { return DocumentDate.HasValue ? DocumentDate.Value.Year : (int?)null; }
            set { DocumentDate = value.HasValue ? new DateTime(value.Value, 1, 1) : (DateTime?)null; }
        }

        [DisplayName("Кем выдан")]
        [StringLength(500)]
        public string DocumentOrganization { get; set; }

        [DisplayName("Кем выдана")]
        [StringLength(500)]
        public string DocumentOrganizationW { get; set; }
        //[DisplayName("Кем выдана")]
        //[StringLength(500)]
        //public string DocumentOrganization { get; set; }

        [DisplayName("Ссылка на документ")]
        public Guid? AttachmentFileID { get; set; }
        [DisplayName("Ссылка на документ")]
        public int? AttachmentID { get; set; }
        public string AttachmentName { get; set; }

        [DisplayName("Серия")]
        [LocalRequired]
        [StringLength(20)]
        public string Series { get; set; }

        [DisplayName("Номер")]
        [LocalRequired]
        [StringLength(50)]
        public string Number { get; set; }

        [DisplayName("Номер свидетельства")]
        [LocalRequired]
        [StringLength(50)]
        public string CertificateNumber { get; set; }   // EntrantDocument.DocumentNumber

        [ScriptIgnore]
        public int MaxFileSize { get { return AppSettings.Get("MaxPostFileLength", 10000); } }

        public EntrantDocumentIdentityViewModel EntDocIdentity { get; set; }
        public EntrantDocumentEduViewModel EntDocEdu { get; set; }
        public EntrantDocumentOlympicViewModel EntDocOlymp { get; set; }
        public EntrantDocumentCustomViewModel EntDocCustom { get; set; }
        public EntrantDocumentDisabilityViewModel EntDocDis { get; set; }
        public EntrantDocumentSubjectBallViewModel EntDocSubBall { get; set; }
        public EntrantDocumentEgeViewModel EntDocEge { get; set; }
        public EntrantDocumentOtherViewModel EntDocOther { get; set; }
        public string Description
        {
            get { return DocumentTypeName + " " + DocumentSeries + " № " + DocumentNumber + " " + (DocumentDate != null ? ("от " + DocumentDate.Value.ToString("dd.MM.yyyy")) : ""); }
        }
    }

    public class EntrantDocumentIdentityViewModel
    {

        public EntrantDocumentIdentityViewModel() { }

        public int EntrantDocumentID { get; set; }

        [DisplayName("Вид документа удостоверяющий личность")]
        [LocalRequired]
        public int IdentityDocumentTypeID { get; set; }
        [ScriptIgnore]
        public string IdentityDocumentTypeName { get; set; }
        [ScriptIgnore]
        public IEnumerable IdentityDocumentList { get; set; }

        [ScriptIgnore]
        public bool IdentityDocumentTypeEdit { get; set; }

        [DisplayName("Пол")]
        [LocalRequired]
        public int GenderTypeID { get; set; }
        public string GenderTypeName { get; set; }
        [ScriptIgnore]
        public IEnumerable GenderList { get; set; }

        [DisplayName("Гражданство")]
        [LocalRequired]
        public int NationalityTypeID { get; set; }
        [ScriptIgnore]
        public string NationalityTypeName { get; set; }
        [ScriptIgnore]
        public IEnumerable NationalityList { get; set; }

        [DisplayName("Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Date("<today-14y")]
        [Date(">today-100y")]
        [LocalRequired]
        public DateTime BirthDate { get; set; }

        [DisplayName("Место рождения")]
        [StringLength(200)]
        public string BirthPlace { get; set; }

        [DisplayName("Код подразделения")]
        [StringLength(200)]
        public string SubdivisionCode { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }
    }

    public class EntrantDocumentEduViewModel
    {

        public EntrantDocumentEduViewModel() { }

        public int EntrantDocumentID { get; set; }

        //         FieldList.Add("RegistrationNumber", 16);
        [DisplayName("Рег. Номер")]
        [StringLength(20)]
        public string RegistrationNumber { get; set; }

        [DisplayName("Адрес организации")]
        [StringLength(255)]
        public string InstitutionAddress { get; set; }

        [DisplayName("Наименование факультета")]
        [StringLength(255)]
        public string FacultyName { get; set; }

        [DisplayName("Наименование образовательной организации")]
        [StringLength(255)]
        public string InstitutionName { get; set; }

        [DisplayName("Дата поступления")]
        public DateTime? BeginDate { get; set; }

        [DisplayName("Дата окончания")]
        public DateTime? EndDate { get; set; }


        [DisplayName("Форма обучения")]
        public byte? EducationFormID { get; set; }

        public string EducationForm
        {
            get
            {
                var educationForm = new OlympicsRepository().GetEducationFormById(EducationFormID);
                return educationForm != null ? educationForm.Name : "";
            }
        }

        IEnumerable<SelectListItem> educationForms;
        public IEnumerable<SelectListItem> EducationForms
        {
            get
            {
                if (educationForms == null)
                {
                    educationForms = new OlympicsRepository().GetEducationFormsAll().Select(s =>
                      new SelectListItem
                      {
                          Value = s.EducationFormID.ToString(),
                          Text = s.Name
                      });
                    educationForms = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(educationForms);
                }
                return educationForms;
            }
        }

        [DisplayName("Страна")]
        public int? CountryID { get; set; }
        public string Country
        {
            get
            {
                var country = new OlympicsRepository().GetCountryTypeById(CountryID);
                return country != null ? country.Name : "";
            }
        }

        IEnumerable<SelectListItem> countries;
        public IEnumerable<SelectListItem> Countries
        {
            get
            {
                if (countries == null)
                {
                    countries = new OlympicsRepository().GetCountryTypeAll()/*.Where(s => s.CountryID == 1 || s.CountryID == 209)*/.Select(s =>
                      new SelectListItem
                      {
                          Value = s.CountryID.ToString(),
                          Text = s.Name
                      });
                    countries = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(countries);
                }
                return countries;
            }
        }

        // Квалификация	Direction.QualificationName	EntrantDocumentEdu.QualificationName
        [DisplayName("Квалификация")]
        [StringLength(255)]
        public string QualificationName { get; set; }

        // Направление подготовки, 	EntrantDocumentEdu.SpecialityName , 	IsNULL(Direction.NewCode,’’) / IsNULL(Direction.Code,’’) + ' ' + IsNULL(Direction.Name,’’) FROM Direction WHERE Direction.QualificationName = @Qual
        [DisplayName("Направление подготовки")]
        [StringLength(255)]
        public string SpecialityName { get; set; }

        // ОУ, выдавшее документ	-	EntrantDocument.DocumentOrganization
        [DisplayName("ОУ, выдавшее документ")]
        [LocalRequired]
        [StringLength(500)]
        public string DocumentOU { get; set; }

        // Средний балл	-	EntrantDocumentEdu.GPA
        [DisplayName("Средний балл")]
        public float? GPA { get; set; }

        [DisplayName("Нострифицирован (признан на территории РФ)")]
        public bool? IsNostrificated { get; set; }

        [DisplayName("Осуществляет подготовку к военной или иной государственной службе (преимущественное право)")]
        public bool? StateServicePreparation { get; set; }

        public string IsNostrificatedStr { get { return IsNostrificated.HasValue ? (IsNostrificated.Value ? "Да" : "Нет") : String.Empty; } }

        public string StateServicePreparationStr { get { return StateServicePreparation.HasValue ? (StateServicePreparation.Value ? "Да" : "Нет") : String.Empty; } }
    }

    public class SubjectBriefData
    {
        public int SubjectID { get; set; }
        [DisplayName("Дисциплина")]
        public string SubjectName { get; set; }
        public bool IsEge { get; set; }
    }

    public class EntrantDocumentOlympicViewModel
    {
        public EntrantDocumentOlympicViewModel()
        {
            OlympicDetails = new OlympicData();
        }

        private int documentTypeID;
        public EntrantDocumentOlympicViewModel(int documentTypeID)
            : this()
        {
            this.documentTypeID = documentTypeID;
        }

        [ScriptIgnore]
        public string DiplomaTypeName { get; set; }

        //для отображения текста во вьюхе
        [ScriptIgnore]
        public OlympicData OlympicDetails { get; set; }

        //---------------------------------------------------------------

        IEnumerable<SelectListItem> formNumber;
        int[] forms = new int[] { 7, 8, 9, 10, 11 };
        public IEnumerable<SelectListItem> FormNumber
        {
            get
            {
                if (formNumber == null)
                {
                    formNumber = forms.Select(s => new SelectListItem
                    {
                        Value = s.ToString(),
                        Text = s.ToString() + " класс"
                    });
                    formNumber = new List<SelectListItem>(){ new SelectListItem
                    {
                        Value = "0",
                        Text = "Не выбрано"
                    }}.Concat(formNumber);

                    if (ForVosh)
                        formNumber = formNumber.Where(s => s.Value != "7" && s.Value != "8");
                }
                return formNumber;
            }
        }

        //---------------------------------------------------------------

        public bool ForVosh { get { return documentTypeID == 10; } }

        //---------------------------------------------------------------

        IEnumerable<GroupedSelectListItem> olympics;
        public IEnumerable<GroupedSelectListItem> Olympics
        {
            get
            {
                if (olympics == null)
                {
                    OlympicTypeEnum olympicTypeEnum = OlympicTypeEnum.NotVosh;
                    if (ForVosh)
                        olympicTypeEnum = OlympicTypeEnum.Vosh;

                    olympics = new OlympicsRepository().GetOlympicTypeAll(olympicTypeEnum).Select(s =>
                    new GroupedSelectListItem
                    {
                        GroupKey = s.OlympicYear.ToString(),
                        GroupName = s.OlympicYear.ToString() + "   ==================================",
                        Value = s.OlympicID.ToString(),
                        Text = s.Name
                    });
                    olympics = new List<GroupedSelectListItem>(){ new GroupedSelectListItem
                    {
                        Value = "0",
                        Text = "Не выбрано"
                    }}.Concat(olympics);
                }
                return olympics;
            }
        }

        //---------------------------------------------------------------

        IEnumerable<SelectListItem> diploms;
        public IEnumerable<SelectListItem> Diploms
        {
            get
            {
                if (diploms == null)
                    diploms = new List<SelectListItem>()
                    {
                        new SelectListItem { Value = "0", Text = "Не выбрано" },
                        new SelectListItem { Value = "1", Text = "Победитель" },
                        new SelectListItem { Value = "2", Text = "Призер" }
                    };
                return diploms;
            }
        }

        //---------------------------------------------------------------

        [DisplayName("Тип диплома")]
        public int? DiplomaTypeID { get; set; }

        [DisplayName("Олимпиада")]
        public int? OlympicID { get; set; }

        [DisplayName("Профиль")]
        public int? OlympicTypeProfileID { get; set; }

        [DisplayName("Класс")]
        public int? FormNumberID { get; set; }

        [DisplayName("Профильная дисциплина")]
        public int? ProfileSubjectID { get; set; }

        public string ProfileSubjectName { get; set; }

        [DisplayName("Общеобразовательный предмет")]
        public int? EgeSubjectID { get; set; }

        public string EgeSubjectName { get; set; } 

        IEnumerable<SelectListItem> egeSubjects;
        public IEnumerable<SelectListItem> EgeSubjects
        {
            get
            {
                if (egeSubjects == null)
                {
                    egeSubjects = new OlympicsRepository().GetEgeSubjects()
                        .Select(x => new SelectListItem() { Value = x.SubjectID.ToString(), Text = x.Name });
                    egeSubjects = new List<SelectListItem>(){ new SelectListItem
                    {
                        Value = "0",
                        Text = "Не выбрано"
                    }}.Concat(egeSubjects);
                } 
                return egeSubjects;
            }
        }

        IEnumerable<SelectListItem> profileSubjects;
        public IEnumerable<SelectListItem> ProfileSubjects
        {
            get
            {
                if (profileSubjects == null)
                {
                    if (OlympicTypeProfileID != null)
                    {
                        profileSubjects = new OlympicsRepository().GetSubjectsByOlympicTypeProfile(OlympicTypeProfileID.Value)
                            .Select(x => new SelectListItem() { Value = x.SubjectID.ToString(), Text = x.Name });
                        if (profileSubjects.Count() == 1)
                        {
                            profileSubjects.First().Selected = true;
                        }
                        profileSubjects = new List<SelectListItem>(){ new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }}.Concat(profileSubjects);
                    }
                    else
                    {
                        profileSubjects = new List<SelectListItem>(){ new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }};
                    }
                } 
                return profileSubjects;
            }
        }

        //---------------------------------------------------------------

        OlympicTypeProfile profile = null;
        public OlympicTypeProfile Profile
        {
            get
            {
                if (profile == null && OlympicTypeProfileID != null)
                    profile = new OlympicDiplomantRepository().GetOlympicTypeProfileById((int)OlympicTypeProfileID);
                return profile;
            }
        }

        public string ProfileText
        {
            get
            {
                return Profile != null && Profile.OlympicProfile != null ? Profile.OlympicProfile.ProfileName : "";
            }
        }

        public string Level
        {
            get
            {
                return Profile != null && Profile.OlympicLevel != null ? Profile.OlympicLevel.Name : "";
            }
        }

        public string Year
        {
            get
            {
                return Profile != null && Profile.OlympicType != null ? Profile.OlympicType.OlympicYear.ToString() : "";
            }
        }

        public string Organizer
        {
            get
            {
                return Profile != null ? Profile.OrganizerName : "";
            }
        }

        public bool? Approved { get; set; }
        [DisplayName("Проверка достоверности сведений")]
        public string ApprovedText
        {
            get
            {
                return Approved.GetValueOrDefault() ? "Результаты подтверждены" : "Результаты не подтверждены";
            }
        }
    }

    public class OlympicData
    {
        public int OlympicID { get; set; }
        public string OlympicName { get; set; }

        [DisplayName("Профиль")]
        public string SubjectNames { get; set; }

        [DisplayName("Уровень олимпиады")]
        public string LevelName { get; set; }

        [DisplayName("Организатор")]
        public string OrganizerName { get; set; }

        [DisplayName("Год олимпиады")]
        public int OlympicYear { get; set; }
    }

    public class EntrantDocumentCustomViewModel
    {
        public EntrantDocumentCustomViewModel() { }

        [DisplayName("Дополнительные сведения")]
        [StringLength(8000)]
        public string AdditionalInfo { get; set; }

        //[DisplayName("Наименование документа")]
        //[LocalRequired]
        //[StringLength(500)]
        //public string DocumentName { get; set; }
    }

    public class EntrantDocumentDisabilityViewModel
    {

        //[LocalRequired]
        [DisplayName("Категория")]

        public int DisabilityTypeID { get; set; }

        public string DisabilityType
        {
            get
            {
                var disabilityType = new OlympicsRepository().GetDisabilityTypeById(DisabilityTypeID);
                return disabilityType != null ? disabilityType.Name : "";
            }
        }



        public IEnumerable<SelectListItem> disabilityTypes;

        public IEnumerable<SelectListItem> DisabilityTypes
        {
            get
            {
                if (disabilityTypes == null)
                {
                    disabilityTypes = new OlympicsRepository().GetDisabilityTypeAll().Select(s => new SelectListItem
                    {
                        Value = s.DisabilityID.ToString(),
                        Text = s.Name
                    });
                    disabilityTypes = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(disabilityTypes);
                }
                return disabilityTypes;
            }
        }



        [ScriptIgnore]
        public object[] DisabilityList { get; set; }
    }

    public class EntrantDocumentSubjectBallViewModel
    {

        [ScriptIgnore]
        public SubjectBall FieldInfo { get; set; }
        [ScriptIgnore]
        public List<SubjectBriefData> SubjectList;
        public IEnumerable<SubjectBall> SubjectBalls { get; set; }

    }

    public class EntrantDocumentEgeViewModel
    {

        [StringLength(8)]
        [DisplayName(@"Типографский номер")]
        [LocalRegex("\\d{7,8}")]
        public string TypographicNumber { get; set; }

    }

    public class SubjectBall
    {
        [DisplayName(@"Дисциплина")]
        public int SubjectID { get; set; }

        [DisplayName(@"Дисциплина")]
        public string SubjectName { get; set; }

        [DisplayName(@"Балл")]
        public int? Value { get; set; }
    }

    public class EntrantDocumentOtherViewModel
    {
        public EntrantDocumentOtherViewModel()
            : base()
        {
        }

        private int documentTypeID;
        public EntrantDocumentOtherViewModel(int documentTypeID)
            : this()
        {
            this.documentTypeID = documentTypeID;
        }

        [DisplayName("Тип диплома")]
        public byte DiplomaTypeID { get; set; }

        IEnumerable<SelectListItem> diploms;
        public IEnumerable<SelectListItem> Diploms
        {
            get
            {
                if (diploms == null)
                    diploms = new List<SelectListItem>()
                    {
                        new SelectListItem { Value = "0", Text = "Не выбрано" },
                        new SelectListItem { Value = "1", Text = "Победитель" },
                        new SelectListItem { Value = "2", Text = "Призер" }
                    };
                return diploms;
            }
        }

        [DisplayName("Профиль олимпиады")]
        [StringLength(255)]
        public string OlympicProfile { get; set; }

        [DisplayName("Дата проведения")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Date(">today-100y")]
        [Date("<=today")]
        public DateTime? OlympicDate { get; set; }

        [DisplayName("Место проведения")]
        [StringLength(255)]
        public string OlympicPlace { get; set; }

        [DisplayName("Страна")]
        public int CountryID { get; set; }
        public string Country
        {
            get
            {
                var country = new OlympicsRepository().GetCountryTypeById(CountryID);
                return country != null ? country.Name : "";
            }
        }

        IEnumerable<SelectListItem> countries;
        public IEnumerable<SelectListItem> Countries
        {
            get
            {
                if (countries == null)
                {
                    countries = new OlympicsRepository().GetCountryTypeAll().Where(s => s.CountryID == 1 || s.CountryID == 209).Select(s =>
                      new SelectListItem
                      {
                          Value = s.CountryID.ToString(),
                          Text = s.Name
                      });
                    countries = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(countries);
                }
                return countries;
            }
        }

        [DisplayName("Категория")]
        public int CompatriotCategoryID { get; set; }
        public string CompatriotCategory
        {
            get
            {
                var compatriotCategory = new OlympicsRepository().GetCompatriotCategoryById(CompatriotCategoryID);
                return compatriotCategory != null ? compatriotCategory.Name : "";
            }
        }

        IEnumerable<SelectListItem> сompatriotCategories;
        public IEnumerable<SelectListItem> CompatriotCategories
        {
            get
            {
                if (сompatriotCategories == null)
                {
                    сompatriotCategories = new OlympicsRepository().GetCompatriotCategoryAll().Select(s =>
                      new SelectListItem
                      {
                          Value = s.CompatriotCategoryID.ToString(),
                          Text = s.Name
                      });
                    сompatriotCategories = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(сompatriotCategories);
                }
                return сompatriotCategories;
            }
        }

        [DisplayName("Статус соотечественника")]
        //[LocalRequired]
        [StringLength(255)]
        public string CompatriotStatus { get; set; }

        [DisplayName("Категория")]
        public int OrphanCategoryID { get; set; }
        public string OrphanCategory
        {
            get
            {
                var orphanCategory = new OlympicsRepository().GetOrphanCategoryById(OrphanCategoryID);
                return orphanCategory != null ? orphanCategory.Name : "";
            }
        }

        IEnumerable<SelectListItem> orphanCategories;
        public IEnumerable<SelectListItem> OrphanCategories
        {
            get
            {
                if (orphanCategories == null)
                {
                    orphanCategories = new OlympicsRepository().GetOrphanCategoryAll().Select(s =>
                      new SelectListItem
                      {
                          Value = s.OrphanCategoryID.ToString(),
                          Text = s.Name
                      });
                    orphanCategories = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(orphanCategories);
                }
                return orphanCategories;
            }
        }

        [DisplayName("Категория")]
        public int VeteranCategoryID { get; set; }
        public string VeteranCategory
        {
            get
            {
                var veteranCategory = new OlympicsRepository().GetVeteranCategoryById(VeteranCategoryID);
                return veteranCategory != null ? veteranCategory.Name : "";
            }
        }

        IEnumerable<SelectListItem> veteranCategories;
        public IEnumerable<SelectListItem> VeteranCategories
        {
            get
            {
                if (veteranCategories == null)
                {
                    veteranCategories = new OlympicsRepository().GetVeteranCategoryAll().Select(s =>
                      new SelectListItem
                      {
                          Value = s.VeteranCategoryID.ToString(),
                          Text = s.Name
                      });
                    veteranCategories = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(veteranCategories);
                }
                return veteranCategories;
            }
        }

        [DisplayName("Категория")]
        public int ParentsLostCategoryID { get; set; }
        public string ParentsLostCategory
        {
            get
            {
                var ParentsLostCategory = new OlympicsRepository().GetParentsLostCategoryById(ParentsLostCategoryID);
                return ParentsLostCategory != null ? ParentsLostCategory.Name : "";
            }
        }

        IEnumerable<SelectListItem> parentsLostCategories;
        public IEnumerable<SelectListItem> ParentsLostCategories
        {
            get
            {
                if (parentsLostCategories == null)
                {
                    parentsLostCategories = new OlympicsRepository().GetParentsLostCategoryAll().Select(s =>
                      new SelectListItem
                      {
                          Value = s.ParentsLostCategoryID.ToString(),
                          Text = s.Name
                      });
                    parentsLostCategories = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(parentsLostCategories);
                }
                return parentsLostCategories;
            }
        }

        [DisplayName("Категория")]
        public int StateEmployeeCategoryID { get; set; }
        public string StateEmployeeCategory
        {
            get
            {
                var StateEmployeeCategory = new OlympicsRepository().GetStateEmployeeCategoryById(StateEmployeeCategoryID);
                return StateEmployeeCategory != null ? StateEmployeeCategory.Name : "";
            }
        }

        IEnumerable<SelectListItem> stateEmployeeCategories;
        public IEnumerable<SelectListItem> StateEmployeeCategories
        {
            get
            {
                if (stateEmployeeCategories == null)
                {
                    stateEmployeeCategories = new OlympicsRepository().GetStateEmployeeCategoryAll().Select(s =>
                      new SelectListItem
                      {
                          Value = s.StateEmployeeCategoryID.ToString(),
                          Text = s.Name
                      });
                    stateEmployeeCategories = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(stateEmployeeCategories);
                }
                return stateEmployeeCategories;
            }
        }

        [DisplayName("Категория")]
        public int RadiationWorkCategoryID { get; set; }
        public string RadiationWorkCategory
        {
            get
            {
                var RadiationWorkCategory = new OlympicsRepository().GetRadiationWorkCategoryById(RadiationWorkCategoryID);
                return RadiationWorkCategory != null ? RadiationWorkCategory.Name : "";
            }
        }

        IEnumerable<SelectListItem> radiationWorkCategories;
        public IEnumerable<SelectListItem> RadiationWorkCategories
        {
            get
            {
                if (radiationWorkCategories == null)
                {
                    radiationWorkCategories = new OlympicsRepository().GetRadiationWorkCategoryAll().Select(s =>
                      new SelectListItem
                      {
                          Value = s.RadiationWorkCategoryID.ToString(),
                          Text = s.Name
                      });
                    radiationWorkCategories = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Value = "0",
                            Text = "Не выбрано"
                        }
                    }.Concat(radiationWorkCategories);
                }
                return radiationWorkCategories;
            }
        }       
    }
}
