using System;
using System.IO;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using NLog;


namespace ApiGuiAll.Models
{
    public static class RequestsSettings
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static ObservableCollection<T> Get<T>(string file)
        {
            ObservableCollection<T> result = null;
            string requestsSetting = string.Empty;
            try
            {
                requestsSetting = File.ReadAllText(file);
            }
            catch (Exception ex) { logger.Error($"Не удалось прочитать файл запросов {file}!\r\n" + ex.ToString()); }
            if (!string.IsNullOrEmpty(requestsSetting))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<ObservableCollection<T>>(requestsSetting);
                }
                catch (Exception ex) { logger.Error($"Не удалось десериализовать данные из файла запросов {file}!\r\n" + ex.ToString()); }
            }
            return result;
        }
    }
}