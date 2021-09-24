using System;

namespace FbsWebViewModel.CNEC
{
    public class HistoryCheckViewCommon
    {
        public long CheckId { get; set; }
        public long? BatchId { get; set; }
        public DateTime CreateDate { get; set; }
        public long OwnerId { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Status { get; set; }
        public long RowNumber { get; set; }
    }
}