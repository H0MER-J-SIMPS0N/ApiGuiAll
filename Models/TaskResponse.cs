using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGuiAll.Models
{
    class TaskResponse
    {
        public class Rootobject
        {
            public string resourceType { get; set; }
            public string type { get; set; }
            public Link[] link { get; set; }
            public Entry[] entry { get; set; }
        }

        public class Link
        {
            public string relation { get; set; }
            public string url { get; set; }
        }

        public class Entry
        {
            public string fullUrl { get; set; }
            public Resource resource { get; set; }
            public Search search { get; set; }
            public Response response { get; set; }
        }

        public class Resource
        {
            public string resourceType { get; set; }
            public string id { get; set; }
            public Meta meta { get; set; }
            public Identifier[] identifier { get; set; }
            public Basedon[] basedOn { get; set; }
            public Partof[] partOf { get; set; }
            public string status { get; set; }
        }

        public class Meta
        {
            public string versionId { get; set; }
            public string lastUpdated { get; set; }
            public Security[] security { get; set; }
            public Tag[] tag { get; set; }
        }

        public class Security
        {
            public string system { get; set; }
            public string code { get; set; }
        }

        public class Tag
        {
            public string system { get; set; }
            public string code { get; set; }
        }

        public class Identifier
        {
            public string system { get; set; }
            public string value { get; set; }
        }

        public class Basedon
        {
            public string reference { get; set; }
        }

        public class Partof
        {
            public string reference { get; set; }
        }

        public class Search
        {
            public string mode { get; set; }
        }

        public class Response
        {
            public string location { get; set; }
            public string etag { get; set; }
            public string lastModified { get; set; }
        }
    }
}
