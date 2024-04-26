using System;

namespace IMDB.CustomExceptions
{
    public class InvalidRequestObjectException:Exception
    {
        public InvalidRequestObjectException(string message):base(message)
        {
            
        }
    }
}
