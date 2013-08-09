using System;
using System.Collections.Generic;
using System.Linq;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomFeedBase : AtomEntryBase
    {
        public AtomGenerator Generator { get; internal set; }

        public Uri Icon
        {
            get { return _icons.LastOrDefault(); }
        }

        protected readonly IList<Uri> _icons = new List<Uri>();

        internal void AddIcon(Uri uri)
        {
            _icons.Add(uri);
        }

        public Uri Logo
        {
            get { return Logos.LastOrDefault(); }
        }

        protected readonly IList<Uri> Logos = new List<Uri>();

        internal void AddLogo(Uri uri)
        {
            Logos.Add(uri);
        }
    }
}