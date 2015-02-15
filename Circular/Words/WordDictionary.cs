using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Circular.Words
{
    public static class WordDictionary
    {
        public static  List<string> PreferedDictionary = new List<string>();

        public static void Initialize()
        {
            string[] files = Directory.GetFiles( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Circular");
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]).Replace("_Prefered_", "");

                PreferedDictionary.Add(files[i].ToLower());
            }
        }
    }
}
