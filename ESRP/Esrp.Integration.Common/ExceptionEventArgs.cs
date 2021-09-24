﻿using System;

namespace Esrp.Integration.Common
{
    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }
        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
