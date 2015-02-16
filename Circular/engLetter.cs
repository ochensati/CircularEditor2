using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circular
{
    [Serializable]
    public class engLetter
    {
        private string _engSyllable = "";
        public bool isVowel { get; private set; }
        public bool isBlank { get; set; }
        public bool isPunct { get; set; }
        public bool isConnector { get; set; }
        public int NumberRepeats { get; private set; }
        public string Consonant { get; private set; }
        public string Vowel { get; private set; }

        public engLetter() { }

        public engLetter(bool connector)
        {
            if (connector)
                isConnector = connector;
            else
                isBlank = true;
        }
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string FindLetter(string word)
        {
            int i = 0;
            bool found = false;
            for (i = 0; i < word.Length; i++)
            {
                if ("aeiou".Contains(word[i]))
                {
                    found = true;
                    break;
                }
            }

            if (i == 0)
            {
                isVowel = true;
                Vowel = word[0].ToString();
                if (word.Length > i + 1)
                {
                    if (word[i + 1] == 'y')
                    {
                        Vowel = Vowel + "y";
                        return word.Substring(i + 2);
                    }
                }

                return word.Substring(1);
            }

            if (!found)
                i = 1;

            isVowel = false;

            Consonant = word[0].ToString();
            if (word.Length > 1 && word[1] == word[0])
            {
                NumberRepeats = 1;
                //for (int j = 1; j < word.Length; j++)
                //{
                //    if (word[j] == word[0])
                //    {
                //        NumberRepeats++;
                //    }
                //    else
                //    {
                //        i = j + 1;
                //        break;
                //    }
                //}
            }
            else
                i = 1;

            if (i < word.Length && "aeiou".Contains(word[i]))
            {
                Vowel = word[i].ToString();

                if (word.Length > i + 1)
                {
                    //if (word[i + 1] == 'y')
                    //{
                    //    Vowel = Vowel + "y";
                    //    return word.Substring(i + 2);
                    //}
                }
                i = i + 1;
            }

            return word.Substring(i);
        }

        public void SetSyllable(string syllable)
        {
            _engSyllable = syllable;
            if (syllable.Trim() == "")
            {
                isBlank = true;
                Consonant = "";
                Vowel = "";
            }

            if ("aeiou".Contains(syllable[0]) && syllable.Length == 1)
            {
                isVowel = true;
                Vowel = syllable;
            }
            else
            {

                if ("aeiou".Contains(syllable[0]) && "aeiou".Contains(syllable[1]))
                {
                    isVowel = true;
                    Vowel = syllable[0].ToString();
                }
                else
                {

                    Consonant = "";
                    Vowel = "";
                    foreach (var s in syllable.ToCharArray())
                    {
                        if ("aeiou".Contains(s))
                        {
                            Vowel += s;
                        }
                        else
                            Consonant += s;
                    }

                    if (Consonant == "'")
                    {
                        isPunct = true;
                    }
                }
            }
        }

        public override string ToString()
        {
            return _engSyllable;
        }
    }
}
