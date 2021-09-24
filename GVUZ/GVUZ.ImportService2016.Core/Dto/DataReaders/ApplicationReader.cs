using GVUZ.ImportService2015.Dto.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2015.Dto.DataReaders
{
    public class ApplicationReader : IDataReader
    {
        private PackageData packageData; // {get; set;}
        private int counter = 0;

        //Func<object, object>[] convertTable;
        //Func<object, bool>[] constraintsTable;

        PackageDataApplication[] Rows;
        PackageDataApplication Row;

        public ApplicationReader(PackageData packageData)
        {
            this.packageData = packageData;
            Rows = packageData.Applications;
        }



        #region "нужно переопределить, чтобы использовать SqlBulkCopy"
        public object GetValue(int i)
        {
            switch (i)
            {
                case 1: // EntrantId
                    return 9578;
                case 2: // RegistrationDate
                    return Row.RegistrationDate;
                case 3: // InstitutionId
                    return packageData.InstitutionId;
                case 4: // ApproveInstitutionCount
                    return false; // <-- 
                case 5: //NeedHostel
                    return Row.NeedHostel;

                case 6: // FirstHigherEducation
                    return true; // <--
                case 7: // ApprovePersonalData
                    return true;
                case 8: // FamiliarWithLicenseAndRules
                    return true;
                case 9: // FamiliarWithAdmissionType
                    return true;
                case 10: // FamiliarWithOriginalDocumentDeliveryDate
                    return true;

                case 11: // StatusID
                    return 1;
                case 12: // WizardStepID
                    return 0;
                case 13: // ViolationID
                    return 0;

                case 14: // StatusDecision
                case 15: // LastCheckDate
                case 16: // ViolationErrors
                case 17: // PublishDate
                    return null;

                case 18: // SourceID
                    return 2;
                case 19: // ApplicationNumber
                    return Row.ApplicationNumber;
                case 20: // OriginalDocumentsReceived
                    return false;
                case 21: // OrderCompetitiveGroupID
                case 22: // OrderOfAdmissionID
                case 23: // OrderCompetitiveGroupItemID
                case 24: // OrderCalculatedRating
                case 25: // OrderCalculatedBenefitID
                case 26: // OrderEducationFormID
                case 27: // OrderEducationSourceID
                    return null;
                case 28: // LastDenyDate
                    return null;
                case 29: // UID
                    return Row.UID;
                case 30: // IsRequiresBudgetO
                case 31: // IsRequiresBudgetOZ
                case 32: // IsRequiresBudgetZ
                case 33: // IsRequiresPaidO
                case 34: // IsRequiresPaidOZ
                case 35: // IsRequiresPaidZ
                    return false;
                case 36: // CreatedDate
                case 37: // ModifiedDate
                    return DateTime.Now;
                case 38: // OriginalDocumentsReceivedDate
                case 39: // LastEgeDocumentsCheckDate
                case 40: // OrderCompetitiveGroupTargetID
                    return null;
                case 41: // IsRequiresTargetO
                case 42: // IsRequiresTargetOZ
                case 43: // IsRequiresTargetZ
                    return false;
                case 44: // ApplicationGUID
                case 45: // Priority
                    return null;

                case 46: // EntrantUID
                    return Row.Entrant.UID;
                case 47: //ImportPackageID
                    return packageData.ImportPackageId;
                default:
                    return null;
            }

            // ApplicationID







        }

        public bool Read()
        {
            if (counter>=packageData.Applications.Count())
                return false;

            Row = Rows[counter];
            counter++;

            var invalidRow = false;

            // check Row?
            //for (int i = 0; i < _currentLineValues.Length; i++)
            //{
            //    if (!_constraintsTable[i](_currentLine))
            //    {
            //        invalidRow = true;
            //        break;
            //    }
            //}

            return !invalidRow || Read();
        }

        public int FieldCount
        {
            get { return 48; }
        }


        public void Dispose()
        {
            // nothing 
        }
        #endregion

        #region "не используются"

        public void Close()
        {
            throw new NotImplementedException();
        }

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed
        {
            get { throw new NotImplementedException(); }
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

       

        public int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }



       

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }



        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }

    
}
