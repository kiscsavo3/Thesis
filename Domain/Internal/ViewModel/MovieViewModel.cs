using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Internal.ViewModel
{
    public class MovieViewModel
    {
        public string ImdbId { get; set; }
        public string TmdbId { get; set; }
        public string Title { get; set; }
        public string ImageUri { get; set; }
        public int RatingCount { get; set; }
        public float RatingAverage { get; set; }
    }
}
