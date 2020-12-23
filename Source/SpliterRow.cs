using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartfonManager.Source
{
    public static class SpliterRow
    {
        // Разделить строку от символов \r\n
        public static List<string> SplitRow(string row)
        {
            var str = "";
            List<string> result = new List<string>();
            foreach (char ch in row)
            {
                if (ch != '\r' && ch != '\n') str += ch;
                else if (ch == '\n')
                {
                    result.Add(str);
                    str = "";
                }
            }
            // Добавить строку, которая не заканчивается на \r\n
            if (row != "")
            {
                if (row[row.Length - 1] != '\n') result.Add(str);
            }
            else result.Add("");
            return result;
        }
    }
}
