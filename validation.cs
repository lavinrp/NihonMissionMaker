using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihon_Mission_Maker
{
    public static class Validation
    {
        public static bool FileNameValidation(string fileName)
        {
            if (fileName.Contains("<") || fileName.Contains(">") || fileName.Contains(":") || fileName.Contains("\"") || fileName.Contains("/")
                || fileName.Contains("\\") || fileName.Contains("|") || fileName.Contains("?") || fileName.Contains("*"))
            {
                return false;
            }
            return true;
        }
    }
}
