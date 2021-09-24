using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.DAL.Dapper.Model.CompetitiveGroups;
using System.Linq;
using GVUZ.DAL.Dapper.Model.TargetOrganization;
using GVUZ.DAL.Dapper.Model.Dictionary;
using GVUZ.DAL.Dapper.Model.Benefit;
using GVUZ.DAL.Dto;
using System;
using GVUZ.DAL.Dapper.Model.LevelBudgets;



namespace GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups
{
    public class CompetitiveGroupProperty
    {

        private int propertyTypeCode;

        //public CompetitiveGroupProperty(DateTime value)
        //{
        //    //this.PropertyValue = value.ToShortDateString();
        //}



        //public CompetitiveGroupProperty(string value, int typeCode)
        //{
        //    this.PropertyValue = value;
        //    this.propertyTypeCode = typeCode;
        //}

        public string PropertyName
        {
            get
            {
                switch (propertyTypeCode)
                {
                    case 1:
                        return "Срок обучения в месяцах";
                    case 2:
                        return "Дата начала обучения";
                    case 3:
                        return "Дата окончания обучения";
                    default:
                        throw new ArgumentException(message: "invalid property name", paramName: nameof(propertyTypeCode));
                }
            }
            //set
            //{
                
            //}
        }
        public int PropertyTypeCode
        { 
            get
            {
                return propertyTypeCode;
            } 
            set 
            {
                propertyTypeCode= value;
            }
        }

        
        public string PropertyValue { get; set; }

    }
}