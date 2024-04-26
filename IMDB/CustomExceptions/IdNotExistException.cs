using System;

namespace IMDB.CustomExceptions
{
    public class IdNotExistException:Exception
    {
        public IdNotExistException(string message) : base(message)
        {
            
        }
    }
}
