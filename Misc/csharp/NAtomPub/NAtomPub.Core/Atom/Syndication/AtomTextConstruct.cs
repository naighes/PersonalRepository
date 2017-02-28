using System;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomTextConstruct
    {
        public String Value { get; internal set; }

        public AtomTextConstructType Type
        {
            get { return _type; }
            internal set { _type = value; }
        }
        private AtomTextConstructType _type = AtomTextConstructType.Text;
    }
}