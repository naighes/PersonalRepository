using System;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomLink
    {
        public String Rel
        {
            get { return _rel; }
            internal set { _rel = value; }
        }
        private String _rel = "alternate";

        public String Type { get; internal set; }
        public Uri HRef { get; internal set; }
        public String HRefLang { get; internal set; }
        public String Title { get; internal set; }
        public Int64 Length { get; internal set; }

        public void Validate()
        {
            if (HRef == null)
                throw new AtomValidationException("atom:link elements MUST have an href attribute.");

            if (String.IsNullOrWhiteSpace(_rel))
                throw new AtomValidationException("The value of \"rel\" MUST be a string that is non-empty.");
        }
    }
}