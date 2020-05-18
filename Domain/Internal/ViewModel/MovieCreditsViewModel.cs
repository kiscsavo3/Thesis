using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Internal.ViewModel
{
    public class MovieCreditsViewModel
    {
        public CastViewModel[] Cast { get; set; }
        public CrewViewModel[] Crew { get; set; }
    }
}
