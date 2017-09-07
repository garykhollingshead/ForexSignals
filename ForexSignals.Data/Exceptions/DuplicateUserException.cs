using System;

namespace ForexSignals.Data.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException(string message):base(message)
        {
            
        }
    }
}
