namespace SCH.Shared.Exceptions
{
    using System;

    public abstract class SCHException : Exception
    {
        public required SCHExceptionTypes SCHExceptionType { get; set; }

        public SCHException(string message) : base(message)
        {
        }

        public SCHException(
            string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
