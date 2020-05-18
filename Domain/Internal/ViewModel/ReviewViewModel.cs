using System;

namespace Domain.Internal.ViewModel
{
    public class ReviewViewModel
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public int TmdbId { get; set; }
        public string EntityId { get; set; }
    }
}
