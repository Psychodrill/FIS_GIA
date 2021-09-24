namespace GVUZ.Model.Entrants.Documents
{
    public enum EntrantDocumentType
    { 
        //1	Документ, удостоверяющий личность
        IdentityDocument = 1,
        //2	Свидетельство о результатах ЕГЭ
        EgeDocument = 2,
        //3	Аттестат о среднем (полном) общем образовании
        SchoolCertificateDocument = 3,
        //4	Диплом о высшем профессиональном образовании
        HighEduDiplomaDocument = 4,
        //5	Диплом о среднем профессиональном образовании
        MiddleEduDiplomaDocument = 5,
        //6	Диплом о начальном профессиональном образовании
        BasicDiplomaDocument = 6,
        //7	Диплом о неполном высшем профессиональном образовании
        IncomplHighEduDiplomaDocument = 7,
        //8	Академическая справка
        AcademicDiplomaDocument = 8,
        //9	Диплом победителя/призера олимпиады школьников
        OlympicDocument = 9,
        //10	Диплом победителя/призера всероссийской олимпиады школьников
        OlympicTotalDocument = 10,
        //11	Справка об установлении инвалидности
        DisabilityDocument = 11,
        //12	Заключение психолого-медико-педагогической комиссии
        MedicalDocument = 12,
        //13	Заключение об отсутствии противопоказаний для обучения
        AllowEducationDocument = 13,
        //14	Военный билет
        MilitaryCardDocument = 14,
        //15	Иной документ
        CustomDocument = 15,
        //16	Аттестат об основном общем образовании
        SchoolCertificateBasicDocument = 16,
        //17 Справка ГИА
        GiaDocument = 17,
        //18 Справка об обучении в другом ВУЗе
        StudentDocument = 18,
        //19 Иной документ об образовании
        EduCustomDocument = 19,

        /// <summary>
        /// 20	Диплом чемпиона/призера Олимпийских игр
        /// </summary>
        SportOlympicChampion = 20,
        /// <summary>
        /// 21	Диплом чемпиона/призера Паралимпийских игр 
        /// </summary>
        SportParaolympicChampion = 21,
        /// <summary>
        /// 22	Диплом чемпиона/призера Сурдлимпийских игр
        /// </summary>
        SportDeaflympicChampion = 22,
        /// <summary>
        /// 23	Диплом чемпиона мира
        /// </summary>
        SportWorldChampion = 23,
        /// <summary>
        /// 24	Диплом чемпиона Европы
        /// </summary>
        SportEuropeChampion = 24,

        /// <summary>
        /// 25 Диплом об окончании аспирантуры (адъюнкатуры)
        /// </summary>
        PostGraduateDiplomaDocument = 25,

        /// <summary>
        /// 26 Диплом кандидата наук
        /// </summary>
        PhDDiplomaDocument = 26,

        /// <summary>
        /// 27	Диплом победителя/призера IV этапа всеукраинской ученической олимпиады
        /// </summary>
        UkraineOlympic = 27,

        /// <summary>
        /// 28	Документ об участии в международной олимпиаде
        /// </summary>
        InternationalOlympic = 28,

        /// <summary>
        /// 29	Документ, подтверждающий принадлежность к соотечественникам за рубежом
        /// </summary>
        CompatriotDocument = 29,

        /// <summary>
        /// 30	Документ, подтверждающий принадлежность к детям-сиротам и детям, оставшимся без попечения родителей
        /// </summary>
        OrphanDocument = 30,

        /// <summary>
        /// 31	Документ, подтверждающий принадлежность к ветеранам боевых действий
        /// </summary>
        VeteranDocument = 31,

        /// <summary>
        /// 32	Документ, подтверждающий наличие только одного родителя - инвалида I группы и принадлежность к числу малоимущих семей
        /// </summary>
        PauperDocument = 32,

        /// <summary>
        /// 33	Документ, подтверждающий принадлежность родителей и опекунов к погибшим в связи с исполнением служебных обязанностей
        /// </summary>
        ParentsLostDocument = 33,

        /// <summary>
        /// 34	Документ, подтверждающий принадлежность к сотрудникам государственных органов Российской Федерации
        /// </summary>
        StateEmployeeDocument = 34,

        /// <summary>
        /// 35	Документ, подтверждающий участие в работах на радиационных объектах или воздействие радиации
        /// </summary>
        RadiationWorkDocument = 35,
    }
}