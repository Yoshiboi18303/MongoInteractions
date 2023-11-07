using System;

namespace MongoInteractions.Exceptions
{
    /// <summary>
    /// Something was not ready.
    /// </summary>
    public class NotReadyException : Exception
    {
        internal NotReadyException() : base() { }
        internal NotReadyException(string message) : base(message) { }
        internal NotReadyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
