using System;
using System.Collections.Generic;

namespace ApiGuiAll.Models
{
    public class PostRequestsSetting
    {
        public string What {get; set;}
        public List<string> Zone {get; set;}
        public WithWhat WithWhat {get; set;}
    }

    public class WithWhat
    {
        public string WhatToEnterOne {get; set;}
        public string WhatToEnterTwo {get; set;}        
        public string Endpoint {get; set;}
        public string PostData {get; set;}
        public string ArgumentOne {get; set;}
        public string ArgumentTwo {get; set;}
        public Dictionary<string, string> Headers {get; set;}
    }    
}