using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;

namespace ApiGuiAll.Models
{
    public class Setting
    {
        [JsonProperty("TokenAddress")]
        public string TokenAddress { get; private set; }

        [JsonProperty("Zone")]
        public string Zone { get; private set; }

        [JsonProperty("Data")]
        public Dictionary<string, string> Data { get; private set; }

        [JsonProperty("BaseUrl")]
        public string BaseUrl { get; private set; }

        public Setting(string tokenAddress, string zone, Dictionary<string, string> data, string baseUrl)
        {
            TokenAddress = tokenAddress;
            Zone = zone;
            Data = data;
            BaseUrl = baseUrl;
        }

        public override string ToString()
        {
            return Zone;
        }
    }

    public static class GetSettings
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static List<Setting> Get()
        {
            List<Setting> result = null;
            string settings = null;
            try
            {
                settings = File.ReadAllText("settings.json");
            }
            catch (Exception ex) { logger.Error("Не удалось прочитать файл настроек!\r\n" + ex.ToString()); }
            if (!string.IsNullOrEmpty(settings))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<List<Setting>>(settings);
                    foreach (Setting s in result)
                    {
                        logger.Info($"BaseUrl - {s.BaseUrl}; Zone - {s.Zone}; TokenAddress - {s.TokenAddress}");
                    }
                }
                catch (Exception ex) { logger.Error("Не удалось десериализовать данные из файла настроек!\r\n" + ex.ToString()); }
            }
            return result;
        }
    }
}