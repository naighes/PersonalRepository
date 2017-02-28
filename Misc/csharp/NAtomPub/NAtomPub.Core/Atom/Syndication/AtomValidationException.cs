using System;
using System.Runtime.Serialization;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomValidationException : Exception
    {
        public AtomValidationException()
        {
        }

        public AtomValidationException(String message)
            : base(message)
        {
        }

        public AtomValidationException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AtomValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}