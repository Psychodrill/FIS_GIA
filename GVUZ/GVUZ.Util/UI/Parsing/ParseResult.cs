using System;

namespace GVUZ.Util.UI.Parsing
{
    public class ParseResult
    {
        public ParseResult()
        {   
        }

        public ParseResult(ParseResultStatus status)
        {
            Status = status;
        }

        public string Message { get; set; }
        public ParseResultStatus Status { get; private set; }
        public Exception Exception { get; set; }
    }

    public enum ParseResultStatus
    {
        Completed,
        Cancelled,
        Exception
    }
}