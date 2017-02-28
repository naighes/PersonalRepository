using System;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomContent
    {
        public String Type
        {
            get { return _type; }
            internal set { _type = value; }
        }
        private String _type = "text";

        public Uri Src { get; internal set; }
        public String Value { get; internal set; }
    }
}