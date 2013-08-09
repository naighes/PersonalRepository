using System.Collections.Generic;
using System.Linq;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomFeed : AtomFeedBase
    {
        public IEnumerable<AtomEntry> Entries
        {
            get { return new List<AtomEntry>(_entries); }
        }

        private readonly IList<AtomEntry> _entries = new List<AtomEntry>();

        internal void AddEntry(AtomEntry entry)
        {
            _entries.Add(entry);
        }

        public override void Validate()
        {
            base.Validate();

            if (!HasAuthors)
            {
                if (!Entries.All(entry => entry.HasAuthors))
                    throw new AtomValidationException(@"atom:feed elements MUST contain one or more atom:author elements, unless all of the atom:feed element's child atom:entry elements contain at least one atom:author element.");
            }

            if (_icons.Count > 1)
                throw new AtomValidationException("atom:feed elements MUST NOT contain more than one atom:icon element.");

            if (Logos.Count > 1)
                throw new AtomValidationException("atom:feed elements MUST NOT contain more than one atom:logo element.");

            foreach (var entry in Entries)
                entry.Validate();
        }
    }
}