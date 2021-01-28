using System;

namespace AutoReport
{
    public class UnsafeLocationException : ApplicationException
    {
        public UnsafeLocationException(string message) : base(message)
        {
        }
    }
}