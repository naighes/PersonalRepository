using System;
using System.Xml;

namespace NAtomPub.Core.Atom.Syndication
{
    public class AtomReader
    {
        public AtomEntryBase Read(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            AtomEntryBase entry = null;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("entry"))
                {
                    entry = new AtomEntry();
                    VisitEntryElement(reader, entry as AtomEntry);
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("feed"))
                {
                    entry = new AtomFeed();
                    VisitFeedElement(reader, entry as AtomFeed);
                }
            }

            return entry;
        }

        private static void VisitFeedElement(XmlReader reader, AtomFeed feed)
        {
            while (reader.Read())
            {
                TryReadCommonProperties(reader, feed);

                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                switch (reader.Name)
                {
                    case "subtitle":
                        VisitTextConstruct(reader, feed, "subtitle", (c, e) => e.AddSubtitle(c));
                        break;
                    case "rights":
                        VisitTextConstruct(reader, feed, "rights", (c, e) => e.AddRights(c));
                        break;
                    case "icon":
                        VisitIconElement(reader, feed);
                        break;
                    case "logo":
                        VisitLogoElement(reader, feed);
                        break;
                    case "generator":
                        VisitGeneratorElement(reader, feed);
                        break;
                    case "entry":
                        VisitEntryElement(reader, feed);
                        break;
                }
            }
        }

        private static void TryReadCommonProperties(XmlReader reader, AtomEntryBase entry)
        {
            if (reader.NodeType != XmlNodeType.Element)
                return;

            switch (reader.Name)
            {
                case "id":
                    VisitIdElement(reader, entry);
                    break;
                case "title":
                    VisitTextConstruct(reader, entry, "title", (c, e) => e.AddTitle(c));
                    break;
                case "updated":
                    VisitDateTime(reader, entry, "updated", (d, e) => e.AddUpdated(d));
                    break;
                case "category":
                    VisitCategoryElement(reader, entry);
                    break;
                case "link":
                    VisitLinkElement(reader, entry);
                    break;
                case "author":
                    VisitPersonConstruct(reader, entry, "author", (p, e) => e.AddAuthor(p));
                    break;
                case "contributor":
                    VisitPersonConstruct(reader, entry, "contributor", (p, e) => e.AddContributor(p));
                    break;
            }
        }

        private static void VisitLogoElement(XmlReader reader, AtomFeedBase feed)
        {
            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.Name.Equals("logo") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Text)
                    continue;

                Uri u;
                Uri.TryCreate(reader.Value, UriKind.Absolute, out u);
                feed.AddLogo(u);
            }
        }

        private static void VisitIconElement(XmlReader reader, AtomFeedBase feed)
        {
            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.Name.Equals("icon") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Text)
                    continue;

                Uri u;
                Uri.TryCreate(reader.Value, UriKind.Absolute, out u);
                feed.AddIcon(u);
            }
        }

        private static void VisitPersonConstruct(XmlReader reader,
                                                 AtomEntryBase entry,
                                                 String nodeName,
                                                 Action<AtomPersonConstruct, AtomEntryBase> action)
        {
            if (reader.IsEmptyElement)
                return;

            var personConstruct = new AtomPersonConstruct();

            while (reader.Read())
            {
                if (reader.Name.Equals(nodeName) && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                switch (reader.Name)
                {
                    case "name":
                        VisitPersonConstructNameElement(reader, personConstruct);
                        break;
                    case "uri":
                        VisitPersonConstructUriElement(reader, personConstruct);
                        break;
                    case "email":
                        VisitPersonConstructEmailElement(reader, personConstruct);
                        break;
                }
            }

            action(personConstruct, entry);
        }

        private static void VisitCategoryElement(XmlReader reader, AtomEntryBase entry)
        {
            var category = new AtomCategory
                {
                    Term = reader.GetAttribute("term"),
                    Scheme = reader.GetAttribute("scheme"),
                    Label = reader.GetAttribute("label")
                };
            entry.AddCategory(category);

            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
                if (reader.Name.Equals("category") && reader.NodeType == XmlNodeType.EndElement)
                    break;
        }

        private static void VisitEntryElement(XmlReader reader, AtomFeed feed)
        {
            var entry = new AtomEntry();
            VisitEntryElement(reader, entry);
            feed.AddEntry(entry);
        }

        private static void VisitEntryElement(XmlReader reader, AtomEntry entry)
        {
            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.Name.Equals("entry") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                TryReadCommonProperties(reader, entry);

                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                switch (reader.Name)
                {
                    case "content":
                        VisitContentElement(reader, entry);
                        break;
                    case "published":
                        VisitDateTime(reader, entry, "published", (d, e) => ((AtomEntry)e).AddPublished(d));
                        break;
                    case "source":
                        VisitSourceElement(reader, entry);
                        break;
                    case "summary":
                        VisitTextConstruct(reader, entry, "summary", (t, e) => ((AtomEntry)e).AddSummary(t));
                        break;
                }
            }
        }

        private static void VisitSourceElement(XmlReader reader, AtomEntry entry)
        {
            if (reader.IsEmptyElement)
                return;

            var source = new AtomFeedBase();

            while (reader.Read())
            {
                if (reader.Name.Equals("source") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                TryReadCommonProperties(reader, source);
            }

            entry.AddSource(source);
        }

        private static void VisitPersonConstructEmailElement(XmlReader reader, AtomPersonConstruct author)
        {
            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.Name.Equals("email") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType == XmlNodeType.Text)
                    author.Email = reader.Value;
            }
        }

        private static void VisitPersonConstructUriElement(XmlReader reader, AtomPersonConstruct author)
        {
            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.Name.Equals("uri") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Text)
                    continue;

                Uri uri;
                Uri.TryCreate(reader.Value, UriKind.Absolute, out uri);
                author.Uri = uri;
            }
        }

        private static void VisitPersonConstructNameElement(XmlReader reader, AtomPersonConstruct author)
        {
            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.Name.Equals("name") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType == XmlNodeType.Text)
                    author.Name = reader.Value;
            }
        }

        private static void VisitContentElement(XmlReader reader, AtomEntry entry)
        {
            Uri uri;
            Uri.TryCreate(reader.GetAttribute("src"), UriKind.Absolute, out uri);
            var content = new AtomContent
                {
                    Type = reader.GetAttribute("type"),
                    Src = uri
                };

            entry.AddContent(content);

            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.Name.Equals("content") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType == XmlNodeType.Text)
                    entry.Content.Value = reader.Value;
            }
        }

        private static void VisitLinkElement(XmlReader reader, AtomEntryBase entry)
        {
            var rel = reader.GetAttribute("rel");
            var type = reader.GetAttribute("type");
            var hreflang = reader.GetAttribute("hreflang");
            var title = reader.GetAttribute("title");
            var length = reader.GetAttribute("length");
            Int64 l;
            Int64.TryParse(length, out l);
            Uri href;
            Uri.TryCreate(reader.GetAttribute("href"), UriKind.Absolute, out href);

            var link = new AtomLink
                {
                    Rel = rel,
                    Type = type,
                    HRef = href,
                    HRefLang = hreflang,
                    Title = title,
                    Length = l
                };

            entry.AddLink(link);

            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
                if (reader.Name.Equals("link") && reader.NodeType == XmlNodeType.EndElement)
                    break;
        }

        private static void VisitGeneratorElement(XmlReader reader, AtomFeedBase feed)
        {
            Uri uri;
            Uri.TryCreate(reader.GetAttribute("uri"), UriKind.Absolute, out uri);
            Single version;
            Single.TryParse(reader.GetAttribute("version"), out version);

            feed.Generator = new AtomGenerator
                {
                    Version = version,
                    Uri = uri
                };

            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.Name.Equals("generator") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType == XmlNodeType.Text)
                    feed.Generator.Value = reader.Value;
            }
        }

        private static void VisitDateTime(XmlReader reader,
                                          AtomEntryBase entry,
                                          String nodeName,
                                          Action<DateTime, AtomEntryBase> action)
        {
            while (reader.Read())
            {
                if (reader.Name.Equals(nodeName) && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType == XmlNodeType.Text)
                {
                    var date = DateTime.Parse(reader.Value);
                    action(date, entry);
                }
            }
        }

        private static void VisitTextConstruct(XmlReader reader,
                                               AtomEntryBase entry,
                                               String nodeName,
                                               Action<AtomTextConstruct, AtomEntryBase> action)
        {
            AtomTextConstructType t;
            var type = reader.GetAttribute("type");
            Enum.TryParse(type, true, out t);

            while (reader.Read())
            {
                if (reader.Name.Equals(nodeName) && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Text)
                    continue;

                var construct = new AtomTextConstruct
                    {
                        Value = reader.Value,
                        Type = t
                    };
                action(construct, entry);
            }
        }

        private static void VisitIdElement(XmlReader reader, AtomEntryBase entry)
        {
            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.Name.Equals("id") && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType == XmlNodeType.Text)
                    entry.AddId(reader.Value);
            }
        }
    }
}