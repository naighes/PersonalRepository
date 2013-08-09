using NAtomPub.Core.Atom.Syndication;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using Xunit;

namespace NAtomPub.Core.Tests
{
    public class AtomTests
    {
        private readonly AtomReader _atomReader = new AtomReader();

        [Fact]
        public void FeedParsing()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <updated>2005-07-31T12:29:29Z</updated>
     <icon>http://example.org/icons/avatar.gif</icon>
     <logo>http://example.org/logos/logo.png</logo>
     <id>tag:example.org,2003:3</id>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example.org/"" title=""example"" length=""1234"" />
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
       <source>
         <id>tag:example.org,2003:2</id>
         <title type=""text"">Source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
       <summary>
            Atom is cool
       </summary>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);

            Assert.Equal("tag:example.org,2003:3", feed.Id);

            Assert.Equal("dive into mark", feed.Title.Value);
            Assert.Equal(AtomTextConstructType.Text, feed.Title.Type);

            Assert.Equal(DateTime.Parse("2005-07-31T12:29:29Z"), feed.Updated);

            Assert.Equal(1.0f, feed.Generator.Version);
            Assert.Equal(new Uri("http://www.example.com/", UriKind.Absolute), feed.Generator.Uri);
            Assert.Equal("Example Toolkit", feed.Generator.Value.Trim());

            Assert.Equal(1, feed.Categories.Count());
            Assert.Equal("joke", feed.Categories.ToList()[0].Term);
            Assert.Equal("http://example.org/extra-cats/", feed.Categories.ToList()[0].Scheme);
            Assert.Equal("cats", feed.Categories.ToList()[0].Label);
            Assert.Equal(new Uri("http://example.org/icons/avatar.gif", UriKind.Absolute), feed.Icon);
            Assert.Equal(new Uri("http://example.org/logos/logo.png", UriKind.Absolute), feed.Logo);

            Assert.Equal(0, feed.Authors.Count());

            Assert.Equal(1, feed.Contributors.Count());
            Assert.Equal("Mario Rossi", feed.Contributors.ToList()[0].Name);

            Assert.Equal(2, feed.Links.Count());

            Assert.Equal(new Uri("http://example.org/"), feed.Links.ToList()[0].HRef);
            Assert.Equal("alternate", feed.Links.ToList()[0].Rel);
            Assert.Equal("text/html", feed.Links.ToList()[0].Type);
            Assert.Equal("example", feed.Links.ToList()[0].Title);
            Assert.Equal(1234, feed.Links.ToList()[0].Length);

            Assert.Equal(new Uri("http://example.org/feed.atom"), feed.Links.ToList()[1].HRef);
            Assert.Equal("self", feed.Links.ToList()[1].Rel);
            Assert.Equal("application/atom+xml", feed.Links.ToList()[1].Type);

            Assert.Equal("Copyright (c) 2003, Mark Pilgrim", feed.Rights.Value);
            Assert.Equal(@"A <em>lot</em> of effort went into making this effortless", feed.Subtitle.Value.Trim());

            Assert.Equal(1, feed.Entries.Count());

            Assert.Equal("tag:example.org,2003:3.2397", feed.Entries.ToList()[0].Id);
            Assert.Equal("Atom draft-07 snapshot", feed.Entries.ToList()[0].Title.Value);
            Assert.Equal(DateTime.Parse("2005-07-31T12:29:29Z"), feed.Entries.ToList()[0].Updated);
            Assert.Equal(DateTime.Parse("2003-12-13T08:29:29-04:00"), feed.Entries.ToList()[0].Published);

            Assert.Equal(1, feed.Entries.ToList()[0].Categories.Count());
            Assert.Equal("buzz", feed.Entries.ToList()[0].Categories.ToList()[0].Term);
            Assert.Equal("http://example.org/extra-dogs/", feed.Entries.ToList()[0].Categories.ToList()[0].Scheme);
            Assert.Equal("dogs", feed.Entries.ToList()[0].Categories.ToList()[0].Label);

            Assert.Equal(1, feed.Entries.ToList()[0].Authors.Count());
            Assert.Equal("Mark Pilgrim", feed.Entries.ToList()[0].Authors.ToList()[0].Name);
            Assert.Equal(new Uri("http://example.org/", UriKind.Absolute), feed.Entries.ToList()[0].Authors.ToList()[0].Uri);
            Assert.Equal("f8dy@example.com", feed.Entries.ToList()[0].Authors.ToList()[0].Email);

            Assert.Equal(2, feed.Entries.ToList()[0].Contributors.Count());
            Assert.Equal("Sam Ruby", feed.Entries.ToList()[0].Contributors.ToList()[0].Name);
            Assert.Equal("Joe Gregorio", feed.Entries.ToList()[0].Contributors.ToList()[1].Name);

            Assert.Equal(2, feed.Entries.ToList()[0].Links.Count());

            Assert.Equal(new Uri("http://example.org/2005/04/02/atom"), feed.Entries.ToList()[0].Links.ToList()[0].HRef);
            Assert.Equal("alternate", feed.Entries.ToList()[0].Links.ToList()[0].Rel);
            Assert.Equal("text/html", feed.Entries.ToList()[0].Links.ToList()[0].Type);

            Assert.Equal(new Uri("http://example.org/audio/ph34r_my_podcast.mp3"), feed.Entries.ToList()[0].Links.ToList()[1].HRef);
            Assert.Equal("enclosure", feed.Entries.ToList()[0].Links.ToList()[1].Rel);
            Assert.Equal("audio/mpeg", feed.Entries.ToList()[0].Links.ToList()[1].Type);

            Assert.Equal("xhtml", feed.Entries.ToList()[0].Content.Type);

            Assert.Equal("tag:example.org,2003:2", feed.Entries.ToList()[0].Source.Id);
            Assert.Equal("Source feed", feed.Entries.ToList()[0].Source.Title.Value);
            Assert.Equal(AtomTextConstructType.Text, feed.Entries.ToList()[0].Source.Title.Type);
            Assert.Equal(DateTime.Parse("2005-06-29T12:29:29Z"), feed.Entries.ToList()[0].Source.Updated);
            Assert.Equal("Atom is cool", feed.Entries.ToList()[0].Summary.Value.Trim());
        }

        [Fact]
        public void ValidatingFeedWithoutAuthorWhithAtLeastOneEntryIsAuthoeless()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <updated>2005-07-31T12:29:29Z</updated>
     <id>tag:example.org,2003:3</id>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void JustOneIconForFeedIsAllowedForValidFeed()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <updated>2005-07-31T12:29:29Z</updated>
     <icon>http://example.org/icons/avatar.gif</icon>
     <icon>http://example.org/icons/secondicon.gif</icon>
     <id>tag:example.org,2003:3</id>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void JustOneLogoForFeedIsAllowedForValidFeed()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <logo>http://example.org/icons/secondicon.gif</logo>
     <id>tag:example.org,2003:3</id>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void NoIdMeansInvalidFeed()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void MoreThanOneLinkWithRelAlternateThatHasSameCombinationOfTypeAndHreflangMeansInvalidFeed()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example2.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void MoreThanOneRightsMeansInvalidFeed()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <rights>Copyright (c) 2004, Nicola Baldi</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void MoreThanOneSubtitleMeansInvalidFeed()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void MoreThanOneTitleMeansInvalidFeed()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <title type=""text"">duplicated</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void MoreThanOneUpdatedMeansInvalidFeed()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <updated>2005-08-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void MoreThanOneContentMeansInvalidEntry()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Duplicated]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void MoreThanOnePublishedMeansInvalidEntry()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <published>2003-11-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void AtLeastOneLinkWithAlternateIsNeededWhenContentIsMissing()
        {
            const String invalidRawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
     </entry>
   </feed>";

            var invalidFeed = GetFeed(invalidRawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => invalidFeed.Validate());

            const String validRawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
     </entry>
   </feed>";

            var validFeed = GetFeed(validRawXml, _atomReader);
            Assert.DoesNotThrow(() => validFeed.Validate());
        }

        [Fact]
        public void MoreThanOneSourceMeansInvalidEntry()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
       <source>
         <id>tag:example.org,2003:2</id>
         <title type=""text"">Source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
       <source>
         <id>tag:example.org,2003:1</id>
         <title type=""text"">Duplicated source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void MoreThanOneSummaryMeansInvalidEntry()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
       <source>
         <id>tag:example.org,2003:2</id>
         <title type=""text"">Source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
       <summary>
            Atom is cool
       </summary>
       <summary>
            Duplicated summary
       </summary>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void AtomSummaryIsRequiredWhenContentHasSrcAttribute()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""image/gif"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"" src=""http://diveintomark.org/image.gif"" />
       <source>
         <id>tag:example.org,2003:2</id>
         <title type=""text"">Source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void AtomSummaryIsRequiredWhenContentIsBase64Encoded()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category term=""joke"" scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""image/gif"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">QXRvbSBpcyBjb29s</content>
       <source>
         <id>tag:example.org,2003:2</id>
         <title type=""text"">Source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void CategoriesMustHaveTermAttribute()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category scheme=""http://example.org/extra-cats/"" label=""cats"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""image/gif"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">Hello world</content>
       <source>
         <id>tag:example.org,2003:2</id>
         <title type=""text"">Source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void LinksMustHaveHRefAttribute()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel=""alternate"" type=""text/html""
      hreflang=""en"" />
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category scheme=""http://example.org/extra-cats/"" label=""cats"" term=""joke"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""image/gif"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">Hello world</content>
       <source>
         <id>tag:example.org,2003:2</id>
         <title type=""text"">Source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void LinksCannotHaveEmptyRelAttribute()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
   <feed xmlns=""http://www.w3.org/2005/Atom"">
     <title type=""text"">dive into mark</title>
     <subtitle type=""html"">
       A &lt;em&gt;lot&lt;/em&gt; of effort
       went into making this effortless
     </subtitle>
     <contributor>
       <name>Mario Rossi</name>
     </contributor>
     <id>tag:example.org,2003:3</id>
     <updated>2005-07-31T12:29:29Z</updated>
     <logo>http://example.org/icons/avatar.gif</logo>
     <link rel="""" type=""text/html""
      hreflang=""en"" href=""http://example1.org/""/>
     <link rel=""self"" type=""application/atom+xml""
      href=""http://example.org/feed.atom""/>
     <rights>Copyright (c) 2003, Mark Pilgrim</rights>
     <generator uri=""http://www.example.com/"" version=""1.0"">
       Example Toolkit
     </generator>
     <category scheme=""http://example.org/extra-cats/"" label=""cats"" term=""joke"" />
     <entry>
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""image/gif"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">Hello world</content>
       <source>
         <id>tag:example.org,2003:2</id>
         <title type=""text"">Source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
     </entry>
   </feed>";

            var feed = GetFeed(rawXml, _atomReader);
            Assert.Throws<AtomValidationException>(() => feed.Validate());
        }

        [Fact]
        public void SingleEntryParsing()
        {
            const String rawXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
     <entry xmlns=""http://www.w3.org/2005/Atom"">
       <title>Atom draft-07 snapshot</title>
       <link rel=""alternate"" type=""text/html""
        href=""http://example.org/2005/04/02/atom""/>
       <link rel=""enclosure"" type=""audio/mpeg"" length=""1337""
        href=""http://example.org/audio/ph34r_my_podcast.mp3""/>
       <category term=""buzz"" scheme=""http://example.org/extra-dogs/"" label=""dogs"" />
       <id>tag:example.org,2003:3.2397</id>
       <updated>2005-07-31T12:29:29Z</updated>
       <published>2003-12-13T08:29:29-04:00</published>
       <author>
         <name>Mark Pilgrim</name>
         <uri>http://example.org/</uri>
         <email>f8dy@example.com</email>
       </author>
       <contributor>
         <name>Sam Ruby</name>
       </contributor>
       <contributor>
         <name>Joe Gregorio</name>
       </contributor>
       <content type=""xhtml"" xml:lang=""en""
        xml:base=""http://diveintomark.org/"">
         <div xmlns=""http://www.w3.org/1999/xhtml"">
           <p><i>[Update: The Atom draft is finished.]</i></p>
         </div>
       </content>
       <source>
         <id>tag:example.org,2003:2</id>
         <title type=""text"">Source feed</title>
         <updated>2005-06-29T12:29:29Z</updated>
       </source>
       <summary>
            Atom is cool
       </summary>
     </entry>";

            var entry = GetEntry(rawXml, _atomReader);

            Assert.Equal("tag:example.org,2003:3.2397", entry.Id);
            Assert.Equal("Atom draft-07 snapshot", entry.Title.Value);
            Assert.Equal(DateTime.Parse("2005-07-31T12:29:29Z"), entry.Updated);
            Assert.Equal(DateTime.Parse("2003-12-13T08:29:29-04:00"), entry.Published);

            Assert.Equal(1, entry.Categories.Count());
            Assert.Equal("buzz", entry.Categories.ToList()[0].Term);
            Assert.Equal("http://example.org/extra-dogs/", entry.Categories.ToList()[0].Scheme);
            Assert.Equal("dogs", entry.Categories.ToList()[0].Label);

            Assert.Equal(1, entry.Authors.Count());
            Assert.Equal("Mark Pilgrim", entry.Authors.ToList()[0].Name);
            Assert.Equal(new Uri("http://example.org/", UriKind.Absolute), entry.Authors.ToList()[0].Uri);
            Assert.Equal("f8dy@example.com", entry.Authors.ToList()[0].Email);

            Assert.Equal(2, entry.Contributors.Count());
            Assert.Equal("Sam Ruby", entry.Contributors.ToList()[0].Name);
            Assert.Equal("Joe Gregorio", entry.Contributors.ToList()[1].Name);

            Assert.Equal(2, entry.Links.Count());

            Assert.Equal(new Uri("http://example.org/2005/04/02/atom"), entry.Links.ToList()[0].HRef);
            Assert.Equal("alternate", entry.Links.ToList()[0].Rel);
            Assert.Equal("text/html", entry.Links.ToList()[0].Type);

            Assert.Equal(new Uri("http://example.org/audio/ph34r_my_podcast.mp3"), entry.Links.ToList()[1].HRef);
            Assert.Equal("enclosure", entry.Links.ToList()[1].Rel);
            Assert.Equal("audio/mpeg", entry.Links.ToList()[1].Type);

            Assert.Equal("xhtml", entry.Content.Type);

            Assert.Equal("tag:example.org,2003:2", entry.Source.Id);
            Assert.Equal("Source feed", entry.Source.Title.Value);
            Assert.Equal(AtomTextConstructType.Text, entry.Source.Title.Type);
            Assert.Equal(DateTime.Parse("2005-06-29T12:29:29Z"), entry.Source.Updated);
            Assert.Equal("Atom is cool", entry.Summary.Value.Trim());
        }

        private static AtomFeed GetFeed(String rawXml, AtomReader atomReader)
        {
            AtomEntryBase feed;

            using (var stringReader = new StringReader(rawXml))
            using (var reader = XmlReader.Create(stringReader,
                                                 new XmlReaderSettings {IgnoreWhitespace = true}))
                feed = atomReader.Read(reader);

            return feed as AtomFeed;
        }

        private static AtomEntry GetEntry(String rawXml, AtomReader atomReader)
        {
            AtomEntryBase entry;

            using (var stringReader = new StringReader(rawXml))
            using (var reader = XmlReader.Create(stringReader,
                                                 new XmlReaderSettings { IgnoreWhitespace = true }))
                entry = atomReader.Read(reader);

            return entry as AtomEntry;
        }
    }
}