using System;

namespace ForexSignals.Data.Exceptions
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException(string message) : base(message)
        {

        }
    }
}
