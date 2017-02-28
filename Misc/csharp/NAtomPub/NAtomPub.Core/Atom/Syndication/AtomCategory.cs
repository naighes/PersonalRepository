using System;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomCategory
    {
        public String Scheme { get; internal set; }
        public String Term { get; internal set; }

        public String Label { get; internal set; }

        public void Validate()
        {
            if (String.IsNullOrWhiteSpace(Term))
                throw new AtomValidationException("Category elements MUST have a \"term\" attribute.");
        }
    }
}