using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGuiAll.Models
{
    class SpecimenResponse
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
            public Type type { get; set; }
            public Subject subject { get; set; }
            public Collection collection { get; set; }
            public Container[] container { get; set; }
            public Extension[] extension { get; set; }
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

        public class Type
        {
            public Coding[] coding { get; set; }
        }

        public class Coding
        {
            public string system { get; set; }
            public string code { get; set; }
        }

        public class Subject
        {
            public string reference { get; set; }
        }

        public class Collection
        {
            public string collectedDateTime { get; set; }
            public Method method { get; set; }
        }

        public class Method
        {
            public Coding[] coding { get; set; }
        }

        public class Identifier
        {
            public string system { get; set; }
            public string value { get; set; }
        }

        public class Container
        {
            public Identifier[] identifier { get; set; }
            public Type type { get; set; }
            public Specimenquantity specimenQuantity { get; set; }
        }

        public class Specimenquantity
        {
            public float value { get; set; }
        }

        public class Extension
        {
            public string url { get; set; }
            public string valueString { get; set; }
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
