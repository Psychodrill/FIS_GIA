using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.DB
{
    partial class Region : IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }
    }

    partial class OrganizationStatusEIISMap : IEIISIdable, IEntityWithNaturalKey
    { 
        public string NaturalKey
        {
            get { return Name; }
        }
    }

    partial class OrganizationKindEIISMap : IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }
    }

    partial class EducationalLevelEIISMap : IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }
    }

    partial class EducationalDirectionType : IEIISIdable, IEntityWithNaturalKey
    {
        public string NaturalKey
        {
            get { return Name; }
        }
    }

    partial class Organization2010 : IEIISIdable, IEntityWithNaturalKey
    {
        public virtual string NaturalKey
        {
            get { return FullName; }
        }
    }

    partial class FounderType : IEIISIdable, IEntityWithNaturalKey
    {
        public virtual string NaturalKey
        {
            get { return Name; }
        }
    }

    partial class Founder :IEIISIdable
    {
    }

    partial class OrganizationFounder : IEIISIdable
    {
    }

    partial class OrganizationLimitation : IEIISIdable
    {
    }

    partial class EducationalDirectionGroup : IEIISIdable
    {
    }

    partial class EducationalDirection : IEIISIdable
    {
    }

    partial class License : IEIISIdable
    {
    }

    partial class LicenseSupplement : IEIISIdable
    {
    }

    partial class AllowedEducationalDirection : IEIISIdable
    {
    }
}
