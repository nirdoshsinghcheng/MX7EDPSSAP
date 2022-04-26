using System;

namespace MX7EDPSSAP.Infrastructure
{
    public class NoRecordFoundException : Exception
    {
        public readonly int errorCode = 100;
        public NoRecordFoundException()
        {
        }
        public NoRecordFoundException(string message) : base(message)
        {
        }
    }

    public class InvalidParametersException : Exception
    {
        public readonly int errorCode = 101;
        public InvalidParametersException()
        {
        }
        public InvalidParametersException(string message) : base(message)
        {
        }
    }

    public class RecordAlreadyExistException : Exception
    {
        public readonly int errorCode = 102;
        public RecordAlreadyExistException()
        {
        }
        public RecordAlreadyExistException(string message) : base(message)
        {
        }
    }

    public class InsertOrUpdateFailedException : Exception
    {
        public readonly int errorCode = 103;
        public InsertOrUpdateFailedException()
        {
        }
        public InsertOrUpdateFailedException(string message) : base(message)
        {
        }
    }
}