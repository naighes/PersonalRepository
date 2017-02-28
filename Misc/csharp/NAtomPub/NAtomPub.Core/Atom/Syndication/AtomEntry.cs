using System;
using System.Collections.Generic;
using System.Linq;
using NAtomPub.Core.Extensions;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomEntry : AtomEntryBase
    {
        public AtomContent Content
        {
            get { return _content.LastOrDefault(); }
        }
        private readonly IList<AtomContent> _content = new List<AtomContent>();

        internal void AddContent(AtomContent content)
        {
            _content.Add(content);
        }

        public DateTime? Published
        {
            get { return _published.LastOrDefault(); }
        }
        private readonly IList<DateTime?> _published = new List<DateTime?>();

        internal void AddPublished(DateTime? published)
        {
            _published.Add(published);
        }

        public AtomTextConstruct Summary
        {
            get { return _summary.LastOrDefault(); }
        }
        private readonly IList<AtomTextConstruct> _summary = new List<AtomTextConstruct>();

        internal void AddSummary(AtomTextConstruct summary)
        {
            _summary.Add(summary);
        }

        public AtomFeedBase Source
        {
            get { return _source.LastOrDefault(); }
        }

        private readonly IList<AtomFeedBase> _source = new List<AtomFeedBase>();

        internal void AddSource(AtomFeedBase source)
        {
            _source.Add(source);
        }

        public override void Validate()
        {
            base.Validate();

            if (_content.Count > 1)
                throw new AtomValidationException("atom:entry elements MUST NOT contain more than one atom:content element.");

            if (_content.Count == 0 && Links.Count(link => link.Rel == "alternate") <= 0)
                throw new AtomValidationException("atom:entry elements that contain no child atom:content element MUST contain at least one atom:link element with a rel attribute value of \"alternate\".");

            if (_published.Count > 1)
                throw new AtomValidationException("atom:entry elements MUST NOT contain more than one atom:published element.");

            if (_source.Count > 1)
                throw new AtomValidationException("atom:entry elements MUST NOT contain more than one atom:source element.");

            if (_summary.Count > 1)
                throw new AtomValidationException("atom:entry elements MUST NOT contain more than one atom:summary element.");

            if (Content != null && Content.Src != null && _summary.Count <= 0)
                throw new AtomValidationException("atom:entry elements MUST contain an atom:summary element when the atom:entry contains an atom:content that has a \"src\" attribute (and is thus empty).");

            if (Content != null && Content.Value.IsBase64String())
                throw new AtomValidationException("atom:entry elements MUST contain an atom:summary element if the atom:entry contains content that is encoded in Base64.");
        }
    }
}