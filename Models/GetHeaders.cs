using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;

namespace ApiGuiAll.Models
{

    public class Header
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public string HeaderName { get; set; }
        public List<HeaderItem> HeaderItems { get; set; }
        public Header(string headerName, string[] headerValues)
        {
            HeaderName = headerName;
            HeaderItems = new List<HeaderItem>();
            logger.Info(headerName);
            foreach (var headerValue in headerValues)
            {
                HeaderItems.Add(new HeaderItem() { HeaderValue = headerValue, IsChecked = false });
            }
        }
    }

    public class HeaderItem
    {
        public string HeaderValue { get; set; }
        public bool IsChecked { get; set; }
    }
    public class GetHeaders
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static List<Header> Get(out List<string> contType)
        {
            contType = new List<string>();
            List<Header> headers = new List<Header>();
            Dictionary<string, string[]> settings = null;
            try
            {
                settings = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText("Headers.json"));
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot read Headers.json: {ex.ToString()}");
            }
            if (!(settings is null))
            {
                var keys = settings.Keys;
                foreach (string key in keys)
                {
                    if (key == "Content-Type")
                    {
                        contType = new List<string>(settings[key]);
                    }
                    else
                    {
                        headers.Add(new Header(key, settings[key]));
                    }
                    //logger.Info($"{key}, {settings[key]}");
                }
            }
            if (!contType.Contains("application/json"))
            {
                contType.Add("application/json");
            }
            if (!contType.Contains("application/fhir+json"))
            {
                contType.Add("application/fhir+json");
            }
            return headers;
        }
    }


}