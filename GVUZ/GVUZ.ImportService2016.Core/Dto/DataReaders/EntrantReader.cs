using GVUZ.ImportService2015.Dto.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2015.Dto.DataReaders
{
    public class EntrantReader : IDataReader
    {
        private PackageData packageData; // {get; set;}
        private int counter = 0;

        //Func<object, object>[] convertTable;
        //Func<object, bool>[] constraintsTable;

        //PackageDataApplication[] currentLineValues;
        PackageDataApplicationEntrant[] Rows;
        PackageDataApplicationEntrant Row;

        public EntrantReader(PackageData packageData)
        {
            this.packageData = packageData;

            var allEntrants = packageData.ApplicationsToImport().Select(t => t.Entrant);
            Rows = allEntrants.GroupBy(t=> t.UID).Select(g=>g.First()).ToArray();
            //Rows = packageData.ApplicationsToImport().Select(t => t.Entrant).GroupBy(t=> t.UID).Select(g=>g.First()).ToArray();
        }

        #region "нужно переопределить, чтобы использовать SqlBulkCopy"
        public object GetValue(int i)
        {
            switch (i)
            {
                case 1: // [IdentityDocumentID]
                    return null;
                case 2: // [CustomInformation]
                    return Row.CustomInformation;
                case 3: // [SNILS]
                    return Row.Snils;
                case 4: // [RegistrationAddressID]
                    return null;
                case 5: // [FactAddressID]
                    return null;
                case 6: // [MobilePhone]
                    return null;
                case 7: // [Email]
                    return null;
                case 8: // [UID]
                    return Row.UID;
                case 9: // [CreatedDate]
                case 10: // [ModifiedDate]
                    return DateTime.Now;
                case 11: // [InstitutionID]
                    return packageData.InstitutionId;
                case 12: // [LastName]
                    return Row.LastName;
                case 13: // [FirstName]
                    return Row.LastName;
                case 14: // [MiddleName]
                    return Row.MiddleName;
                case 15: // [GenderID]
                    return Row.GenderID;
                case 16:
                    return packageData.ImportPackageId;
                default:
                    return null;
            }

        }

        public bool Read()
        {
            if (counter >= Rows.Count())
                return false;

            Row = Rows[counter];
            counter++;

            var invalidRow = false;
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
            get { return 17; }
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
