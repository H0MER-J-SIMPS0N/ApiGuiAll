using System;
using System.Text;
using System.Linq;

namespace ApiGuiAll.Models
{
    public class FormatJson
    {
        public static string Process(string json)
        {
            string INDENT_STRING = "    ";
            int indentation = 0;
            int quoteCount = 0;
            var x = json.Split('\n');
            StringBuilder temp = new StringBuilder();
            foreach (string z in x)
            {
                temp.Append(z.Trim());
            }
            //.Replace('\n', ' ').Replace('\r', ' ');
            var result = from ch in temp.ToString()
                         let quotes = ch == '"' ? quoteCount++ : quoteCount
                         let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(INDENT_STRING, indentation)) : null
                         let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(INDENT_STRING, ++indentation)) : ch.ToString()
                         let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + string.Concat(Enumerable.Repeat(INDENT_STRING, --indentation)) + ch : ch.ToString()
                         select lineBreak == null
                                     ? openChar.Length > 1
                                         ? openChar
                                         : closeChar
                                     : lineBreak;
            return string.Concat(result);
        }
    }
}