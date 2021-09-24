using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsHashedCNEsReplicator
{
    public class Consts
    {
        public static int[] SubjectIds = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 
                    12, 13, 14 };

        public static string[] SubjectNames = new string[] { "русский язык", "математика", "физика", "химия",
                    "биология", "история России", "география", "английский язык", "немецкий язык", "французский язык", 
                    "обществознание", "литература", "испанский язык", "информатика" };

        public static int DeniedCNEsCSVFieldsCount = 3;
        public static int CNEsCSVFieldsCount = 41;
        public enum CNECSVStructure
        {
            /// <summary>
            /// 0
            /// </summary>
            CertificateNumber,
            /// <summary>
            /// 1
            /// </summary>
            EducationInstitutionCode,
            /// <summary>
            /// 2
            /// </summary>
            Sex,
            /// <summary>
            /// 3
            /// </summary>
            LastName,
            /// <summary>
            /// 4
            /// </summary>
            FirstName,
            /// <summary>
            /// 5
            /// </summary>
            PatronymicName,
            /// <summary>
            /// 6
            /// </summary>
            Skip,
            /// <summary>
            /// 7
            /// </summary>
            PassportSeria,
            /// <summary>
            /// 8
            /// </summary>
            PassportNumber,
            /// <summary>
            /// 9
            /// </summary>
            Class,
            /// <summary>
            /// 10
            /// </summary>
            BeginMarks,
            /// <summary>
            /// 37
            /// </summary>
            EndMarks = 37, //зарезервированно под оценки 10-37,
            /// <summary>
            /// 38
            /// </summary>
            EntrantNumber,
            /// <summary>
            /// 39
            /// </summary>
            RegionId,
            /// <summary>
            /// 40
            /// </summary>
            TypographicNumber
        }


        public enum DeniedCNECSVStructure
        {
            /// <summary>
            /// 0
            /// </summary>
            CertificateNumber,
            /// <summary>
            /// 1
            /// </summary>
            Comment,
            /// <summary>
            /// 2
            /// </summary>
            NewCertificateNumber
        }
    }
}
