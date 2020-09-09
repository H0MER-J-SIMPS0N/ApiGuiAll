using System;
using System.IO;
using System.Linq;
using NLog;

namespace ApiGuiAll.Models
{
    public class ResponseFile
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static string Save(string resp, ref bool IsFilePath, ref string FilePath)
        {
            string answer = string.Empty;
            try
            {
                var files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Responses"), "response*.json").ToList();
                string lastFile = files.Count > 0 ? files.OrderByDescending(x => x).FirstOrDefault() : string.Empty;
                string fileNumber = string.IsNullOrEmpty(lastFile) ? string.Empty : Path.GetFileName(lastFile).Split('.').FirstOrDefault().Split("request").LastOrDefault();
                int number = 0;
                int.TryParse(fileNumber, out number);
                do
                {
                    number += 1;
                }
                while (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Responses", $"response{number}.json")));
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "Responses", $"response{number}.json"), FormatJson.Process(resp));
                IsFilePath = true;
                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Responses", $"response{number}.json");
                answer = $"Слишком много букв!!!\r\nВремя ответа: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}\r\nОтвет сохранен в файле \"" + $"{FilePath}\"";
            }
            catch (Exception ex) { logger.Error($"Не удалось сохранить ответ!\r\n{ex.ToString()}"); }
            return answer;
        }
    }
}