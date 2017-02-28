using System;
using System.Collections.Generic;
using System.Linq;

namespace NAtomPub.Core.Atom.Syndication
{
    public abstract class AtomEntryBase
    {
        public String Id
        {
            get { return _identifiers.LastOrDefault(); }
        }
        private readonly IList<String> _identifiers = new List<String>();

        internal void AddId(String id)
        {
            _identifiers.Add(id);
        }

        public DateTime? Updated
        {
            get { return _updated.LastOrDefault(); }
        }
        private readonly IList<DateTime?> _updated = new List<DateTime?>();

        internal void AddUpdated(DateTime? date)
        {
            _updated.Add(date);
        }

        public AtomTextConstruct Title
        {
            get { return _titles.LastOrDefault(); }
        }
        private readonly IList<AtomTextConstruct> _titles = new List<AtomTextConstruct>();

        internal void AddTitle(AtomTextConstruct title)
        {
            _titles.Add(title);
        }

        public AtomTextConstruct Rights
        {
            get { return _rights.LastOrDefault(); }
        }
        private readonly IList<AtomTextConstruct> _rights = new List<AtomTextConstruct>();

        internal void AddRights(AtomTextConstruct rights)
        {
            _rights.Add(rights);
        }

        public IEnumerable<AtomPersonConstruct> Authors
        {
            get { return new List<AtomPersonConstruct>(_authors); }
        }

        internal void AddAuthor(AtomPersonConstruct author)
        {
            _authors.Add(author);
        }

        internal Boolean HasAuthors
        {
            get { return _authors.Any(); }
        }
        private readonly IList<AtomPersonConstruct> _authors = new List<AtomPersonConstruct>();

        public IEnumerable<AtomCategory> Categories
        {
            get { return new List<AtomCategory>(_categories); }
        }
        private readonly IList<AtomCategory> _categories = new List<AtomCategory>();

        internal void AddCategory(AtomCategory category)
        {
            _categories.Add(category);
        }

        public IEnumerable<AtomPersonConstruct> Contributors
        {
            get { return new List<AtomPersonConstruct>(_contributors); }
        }
        private readonly IList<AtomPersonConstruct> _contributors = new List<AtomPersonConstruct>();

        public IEnumerable<AtomLink> Links
        {
            get { return new List<AtomLink>(_links); }
        }

        internal void AddLink(AtomLink link)
        {
            _links.Add(link);
        }
        private readonly IList<AtomLink> _links = new List<AtomLink>();

        internal void AddContributor(AtomPersonConstruct contributor)
        {
            _contributors.Add(contributor);
        }

        public AtomTextConstruct Subtitle
        {
            get { return _subtitles.LastOrDefault(); }
        }
        private readonly IList<AtomTextConstruct> _subtitles = new List<AtomTextConstruct>();

        internal void AddSubtitle(AtomTextConstruct subtitle)
        {
            _subtitles.Add(subtitle);
        }

        public virtual void Validate()
        {
            if (_identifiers.Count != 1)
                throw new AtomValidationException("atom:feed elements MUST contain exactly one atom:id element.");

            if (_links.Where(link => link.Rel == "alternate")
                      .GroupBy(link => new {link.HRefLang, link.Type})
                      .Any(g => g.Count() > 1))
                throw new AtomValidationException("atom:feed elements MUST NOT contain more than one atom:link element with a rel attribute value of \"alternate\" that has the same combination of type and hreflang attribute values.");

            if (_rights.Count > 1)
                throw new AtomValidationException("atom:feed elements MUST NOT contain more than one atom:rights element.");

            if (_subtitles.Count > 1)
                throw new AtomValidationException("atom:feed elements MUST NOT contain more than one atom:subtitle element.");

            if (_titles.Count != 1)
                throw new AtomValidationException("atom:feed elements MUST contain exactly one atom:title element.");

            if (_updated.Count != 1)
                throw new AtomValidationException("atom:feed elements MUST contain exactly one atom:updated element.");

            foreach (var category in Categories)
                category.Validate();

            foreach (var link in Links)
                link.Validate();
        }
    }
}