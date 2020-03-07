using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class ApiCredentials
    {
        public string ApiKey { get; set; }

        public string BaseUri { get; set; }

        public string PartUri { get; set; }
    }
}
