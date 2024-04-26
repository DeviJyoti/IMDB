using System;
using System.Runtime.Serialization;

namespace IMDB.CustomExceptions
{
    public class NoItemFoundException : Exception
    {

        public NoItemFoundException(string message) : base(message)
        {
        }

    }
}