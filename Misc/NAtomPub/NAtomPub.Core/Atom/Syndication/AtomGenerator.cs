using System;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomGenerator
    {
        public Uri Uri { get; internal set; }
        public Single Version { get; internal set; }
        public String Value { get; internal set; }
    }
}