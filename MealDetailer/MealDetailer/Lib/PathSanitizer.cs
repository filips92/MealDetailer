using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MealDetailer.Lib
{
    public static class PathSanitizer
    {
        public static string MakeValidFileName(string name)
        {
            Array.ForEach(Path.GetInvalidFileNameChars(),
                c => name = name.Replace(c.ToString(), String.Empty));

            return name;
        }
    }
}