using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Esrp.DB;
using Esrp.DB.Common;

namespace Esrp.EIISIntegration.Import
{
    internal class EIISObject: EntityBase
    { 
        public string Eiis_Id
        {
            get { return PrimaryKeyFields.First().Value; }
            set { throw new NotSupportedException(); }
        }

        public IEnumerable<EIISField> DataFields { get; protected set; }
        public IEnumerable<EIISPrimaryKeyField> PrimaryKeyFields { get; protected set; }
        public IEnumerable<EIISForeignKeyField> ForeignKeyFields { get; protected set; }

        private List<EIISField> allFields;
        public IEnumerable<EIISField> AllFields
        {
            get
            {
                if (allFields == null)
                {
                    allFields = new List<EIISField>();
                    foreach (EIISField primaryKeyField in PrimaryKeyFields)
                    {
                        allFields.Add(primaryKeyField);
                    }
                    foreach (EIISField foreignKeyField in ForeignKeyFields)
                    {
                        allFields.Add(foreignKeyField);
                    }
                    allFields.AddRange(DataFields);
                }
                return allFields;
            }
        }

        public string AllFieldsString
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (EIISField field in AllFields)
                {
                    result.Append(field.Code).Append("=").Append(field.Value).AppendLine();
                }
                return result.ToString();
            }
        }

        protected EIISObject()
        {
        }

        public EIISObject(XmlNode objectNode)
        {
            if (objectNode == null)
                throw new ArgumentNullException("objectNode");
            Parse(objectNode);
        }

        public bool FieldExists(string fieldCode)
        {
            EIISField field = AllFields.FirstOrDefault(obj => obj.Code == fieldCode);
            return (field != null);
        }

        public EIISField GetField(string fieldCode)
        {
            EIISField field = AllFields.FirstOrDefault(obj => obj.Code == fieldCode);
            if (field == null)
                throw new Exception(String.Format("Поле {0} не найдено", fieldCode));

            return field;
        }

        public string GetFieldStringValue(string fieldCode)
        {
            string result = GetField(fieldCode).Value;
            if (result != null)
            {
                if ((fieldCode == "GOSREGNUM") && (result.Length > 18))
                {
                    var b = 0;
                }
            }

            return GetField(fieldCode).Value;
        }

        public DateTime? GetFieldDateTimeValue(string fieldCode)
        {
            return GetField(fieldCode).ValueDateTime;
        }

        public bool? GetFieldBooleanValue(string fieldCode)
        {
            return GetField(fieldCode).ValueBool;
        }

        private void Parse(XmlNode objectNode)
        {
            List<EIISPrimaryKeyField> primaryKeyFields = new List<EIISPrimaryKeyField>();
            foreach (XmlNode primaryNode in objectNode.SelectNodes("primary"))
            {
                primaryKeyFields.Add(new EIISPrimaryKeyField(primaryNode.Attributes["code"].Value, primaryNode.InnerText));
            }
            PrimaryKeyFields = primaryKeyFields;

            List<EIISField> dataFields = new List<EIISField>();
            foreach (XmlNode columnNode in objectNode.SelectNodes("column"))
            {
                dataFields.Add(new EIISField(columnNode.Attributes["code"].Value, columnNode.InnerText));
            }
            DataFields = dataFields;

            List<EIISForeignKeyField> foreignKeyFields = new List<EIISForeignKeyField>();
            foreach (XmlNode referenceNode in objectNode.SelectNodes("reference"))
            {
                foreignKeyFields.Add(new EIISForeignKeyField(referenceNode.Attributes["code"].Value, referenceNode.InnerText, referenceNode.Attributes["object"].Value));
            }
            ForeignKeyFields = foreignKeyFields;
        }
    }

    internal class ExtendedEIISObject : EIISObject, IEIISIdable
    {
        public ExtendedEIISObject()
        {
        }

        public ExtendedEIISObject(EIISObject obj)
        {
            DataFields = obj.DataFields;
            PrimaryKeyFields = obj.PrimaryKeyFields;
            ForeignKeyFields = obj.ForeignKeyFields;
        } 

        public int Id
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
    }

    internal class EIISField
    {
        public EIISField(string code, string value)
        {
            Code = code;
            Value = value;
            if (!String.IsNullOrEmpty(Value))
            {
                Value = Value.Trim();
            }

            if ((!String.IsNullOrEmpty(value)) && (value.ToLower() == "null"))
            {
                Value = null;
            }
        }
        public string Code { get; private set; }
        public string Value { get; private set; }

        public DateTime? ValueDateTime
        {
            get
            {
                if (String.IsNullOrEmpty(Value))
                    return null;
                DateTime result;
                if (DateTime.TryParse(Value, out result))
                    return result;
                return null;
            }
        }

        public bool? ValueBool
        {
            get
            {
                if (String.IsNullOrEmpty(Value))
                    return null;
                if ((Value == "1") || (Value.ToLower() == "true"))
                    return true;
                if ((Value == "0") || (Value.ToLower() == "false"))
                    return false;

                return null;
            }
        }
    }

    internal class EIISPrimaryKeyField : EIISField
    {
        public EIISPrimaryKeyField(string code, string value)
            : base(code, value)
        {
        }
    }

    internal class EIISForeignKeyField : EIISField
    {
        public EIISForeignKeyField(string code, string value, string referencedObjectCode)
            : base(code, value)
        {
            ReferencedObjectCode = referencedObjectCode;
        }
        public string ReferencedObjectCode { get; private set; }
    }
}
