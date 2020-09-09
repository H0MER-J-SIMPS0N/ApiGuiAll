using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGuiAll.Models
{
    class ProcedureRequestResponse
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
            public Requisition requisition { get; set; }
            public Code code { get; set; }
            public Subject subject { get; set; }
            public string occurrenceDateTime { get; set; }
            public Supportinginfo[] supportingInfo { get; set; }
            public Specimen[] specimen { get; set; }
        }

        public class Meta
        {
            public string versionId { get; set; }
            public string lastUpdated { get; set; }
            public Security[] security { get; set; }
        }

        public class Security
        {
            public string system { get; set; }
            public string code { get; set; }
        }

        public class Requisition
        {
            public string system { get; set; }
            public string value { get; set; }
        }

        public class Code
        {
            public Coding[] coding { get; set; }
        }

        public class Coding
        {
            public string system { get; set; }
            public string code { get; set; }
            public string display { get; set; }
        }

        public class Subject
        {
            public string reference { get; set; }
        }

        public class Identifier
        {
            public string system { get; set; }
            public string value { get; set; }
        }

        public class Supportinginfo
        {
            public string reference { get; set; }
        }

        public class Specimen
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
