using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GVUZ.Model.Institutions
{
    /// <summary>
    ///     Результат поиска института (используется в ПГУ)
    /// </summary>
    [DebuggerDisplay("{Name} ({ParentID} / {AdmissionStructureID})")]
    public partial class InstitutionSearchResult
    {
        private List<InstitutionSearchResult> _children;

        public decimal? Competition
        {
            get
            {
                if (!PlaceCount.HasValue)
                    return null;
                if (!ApplicationCount.HasValue)
                    return 0;
                return Math.Round((decimal) ApplicationCount/(decimal) PlaceCount, 2);
            }
        }

        public bool ShouldHasChildren
        {
            get { return ItemLevel != (short) AdmissionItemLevel.AdmissionType; }
        }

        public bool HasLoadedChildren
        {
            get { return _children != null && _children.Count > 0; }
        }

        public List<InstitutionSearchResult> Children
        {
            get { return _children ?? (_children = new List<InstitutionSearchResult>()); }
        }
    }
}