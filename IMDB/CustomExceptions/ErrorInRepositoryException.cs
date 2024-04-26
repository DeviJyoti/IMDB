using System;

namespace IMDB.CustomExceptions
{
    public class ErrorInRepositoryException:Exception
    {
        public ErrorInRepositoryException(string message):base(message)
        {
            
        }
    }
}
