namespace Ege.Hsc.Logic.Models.Blanks
{
    using System;

    public class Blank
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public string Url { get; set; }

        public string ParticipantHash { get; set; }

        public string ParticipantDocumentNumber { get; set; }

        public Guid ParticipantRbdId { get; set; }
    }
}