using HtmlAgilityPack;

namespace FootballShare.Automation.Parsers
{
    /// <summary>
    /// Helper class for parsing HTML pages
    /// </summary>
    public abstract class HtmlParser
    {
        /// <summary>
        /// Web Parser client
        /// </summary>
        protected HtmlWeb WebClient { get; set; }

        /// <summary>
        /// Creates a new <see cref="HtmlParser"/> instance
        /// </summary>
        public HtmlParser()
        {
            this.WebClient = new HtmlWeb();
        }
    }
}
