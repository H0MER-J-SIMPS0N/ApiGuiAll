using System;
using System.Collections.Generic;

namespace ApiGuiAll.Models
{
    public class GetRequestsSetting
    {
        public string What {get; set;}
        public List<string> Zone {get; set;}
        public List<ByWhat> ByWhat {get; set;}
    }

    public class ByWhat
    {
        public string Name {get; set;}
        public string WhatToEnter {get; set;}
        public string Endpoint {get; set;}
        public string Argument {get; set;}
        public Dictionary<string, string> Headers {get; set;}
    }
}
