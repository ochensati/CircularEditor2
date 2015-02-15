using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circular
{
    [Serializable]
    public class engWord
    {
        public int nSyllables { get { return Syllables.Count; } }
        public List<engLetter> Syllables { get; private set; }
        private string _OriginalWord;

        public void Initialize(string word, Circular.aCircleObject.ScriptStyles scriptStyle)
        {

            word = word.ToLower().Replace(",", "").Replace(".", "").Replace(":", "").Replace("-", " ");
            _OriginalWord = word;
            word = word.Replace("ch", "_.ch_").Replace("qu", "q")
                   .Replace("ng", "_.ng_").Replace("th", "_.th_").Replace("sh", "_.sh_");

            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft || scriptStyle == Circular.aCircleObject.ScriptStyles.Small)
            {
                word = word.Replace("st", "_st_");

                List<string> combo = new List<string>();
                if (scriptStyle != Circular.aCircleObject.ScriptStyles.Small)
                {
                    if (word.EndsWith("ment"))
                        word = word.Substring(0, word.Length - 4) + "_.1_";

                    if (word.EndsWith("sion"))
                        word = word.Substring(0, word.Length - 4) + "_.2_";

                    if (word.EndsWith("tion"))
                        word = word.Substring(0, word.Length - 4) + "_.2_";

                    if (word.EndsWith("ion"))
                        word = word.Substring(0, word.Length - 4) + "_.2_";

                    if (word.EndsWith("er"))
                        word = word.Substring(0, word.Length - 2) + "_.3_";

                    if (word.EndsWith("ize"))
                        word = word.Substring(0, word.Length - 3) + "_.4_";


                    for (int i = 0; i < word.Length - 1; i++)
                    {
                        if ("aeiou".Contains(word[i]))
                        {
                            if ("aeiou".Contains(word[i + 1]))
                            {
                                if ((i + 2) < word.Length && "aeiou".Contains(word[i + 2]))
                                {
                                    combo.Add(word[i].ToString() + word[i + 1].ToString() + word[i + 2].ToString());
                                }
                                else
                                    combo.Add(word[i].ToString() + word[i + 1].ToString());
                            }
                        }
                    }

                }


                foreach (var s in combo)
                {
                    word = word.Replace(s, "_." + s + "_");
                }


                word = word.Replace("_.1_", "_.ment_").Replace("_.2_", "_.tion_").Replace("_.3_", "_.er_").Replace("_.4_", "_..ize_");
            }





            string[] parts = word.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);

            string newword = "";
            foreach (var p in parts)
            {

                if (p.StartsWith(".") == false)
                {
                    string p2 = p;

                    if (scriptStyle == Circular.aCircleObject.ScriptStyles.Small)
                    {
                        p2 = p2.Replace("a", "_a_");
                        p2 = p2.Replace("e", "_e_");
                        p2 = p2.Replace("i", "_i_");
                        p2 = p2.Replace("o", "_o_");
                        p2 = p2.Replace("u", "_u_");
                    }
                    else
                    {
                        p2 = p2.Replace("a", "a_");
                        p2 = p2.Replace("e", "e_");
                        p2 = p2.Replace("i", "i_");
                        p2 = p2.Replace("o", "o_");
                        p2 = p2.Replace("u", "u_");
                    }

                    for (int i = 0; i < p2.Length - 1; i++)
                    {
                        if ("aeiou_".Contains(p2[i]) == false)
                        {
                            if ("aeiou_".Contains(p2[i + 1]) == false)
                            {
                                p2 = p2.Insert(i + 1, "_");
                            }
                        }
                    }
                    newword += (p2 + "_");
                }
                else
                {
                    newword += (p.Substring(1, p.Length - 1) + "_");
                }

            }



            Syllables = new List<engLetter>();


            parts = newword.Split('_');
            foreach (var part in parts)
            {
                if (part.Trim() != "")
                {
                    engLetter g = new engLetter();
                    g.SetSyllable(part);
                    Syllables.Add(g);
                }
            }

           
        }

        public engWord()
        {

        }

        public engWord(engLetter[] syllables)
        {
            Syllables = new List<engLetter>(syllables);
            _OriginalWord = "";
        }


        public engWord(string word, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            Initialize(word, scriptStyle);
        }

        public void AddBlank()
        {
            var e = new engLetter();
            e.isBlank = true;
            Syllables.Add(e);
        }

        public void AddConnector()
        {
            var e = new engLetter();
            e.isConnector = true;
            Syllables.Add(e);

        }
        public override string ToString()
        {
            return _OriginalWord;
        }
    }
}
