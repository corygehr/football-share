using System;

namespace FootballShare.Web.Models
{
    [Serializable]
    public class UserMessageViewModel
    {
        /// <summary>
        /// CSS class
        /// </summary>
        public string CssClassName { get; set; }
        /// <summary>
        /// Message text
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Messsage title
        /// </summary>
        public string Title { get; set; }
    }
}
