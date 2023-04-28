using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZ2V.Parser
{
    public abstract class ParserConfig
    {       
        public Uri BaseUri { get; set; }
        public string BaseUrl { get; set; }
        protected ParserConfig(Uri baseUri)
        {
            BaseUri = baseUri;
            BaseUrl = "https://www.ilcats.ru";
        }
    }
}
