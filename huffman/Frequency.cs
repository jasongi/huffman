using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Asgn
{
    /// <summary>
    /// Class to parse and store frequencies from the frequency table
    /// </summary>
    /// 

    class Frequency
    {
        /// <summary>
        /// The symbol this frequency represents.
        /// </summary>
        public char symbol { get; set; }

        /// <summary>
        /// The number of times the symbol appears in the text.
        /// </summary>
        public double frequency { get; set; }

        /// <summary>
        /// Construct a Frequency object from a string in the form of char:frequency. 
        /// Special cases for the word newline and return as these are delimiters in the frequency box.
        /// </summary>
        /// <param name="freq">A string representing one row in the frequency table.</param>
        /// <returns>Frequency Object.</returns>
        public Frequency(string freq)
        {
            string sPattern = "^.*:[0-9]+([\\.][0-9]+)?$";
            if (System.Text.RegularExpressions.Regex.IsMatch(freq, sPattern, System.Text.RegularExpressions.RegexOptions.None))
            {
                string[] temp = freq.Split(':');
                if (temp[0].Equals("newline"))
                    symbol = '\n';
                else if (temp[0].Equals("return"))
                    symbol = '\r';
                else if (freq[0] == ':')
                    symbol = ':';
                else symbol = temp[0][0];
                string num;
                if (symbol == ':')
                    num = temp[2];
                else
                    num = temp[1];
                double f;
                if (!Double.TryParse(num, out f))
                {
                    throw new System.ArgumentException("Frequency was not a number");
                }
                frequency = f;
            }
        }

        /// <summary>
        /// Construct a Frequency object from a char and frequency value
        /// </summary>
        /// <param name="sym">The symbol represented in the table.</param>
        /// <param name="freq">The number of times it appears.</param>
        /// <returns>Frequency Object.</returns>
        public Frequency(char sym, double freq)
        {
            frequency = freq;
            symbol = sym;
        }

        /// <summary>
        /// Convert the object to a string for output to the frequency table box.
        /// Makes special accomadations for \r and \n characters
        /// </summary>
        /// <returns>Frequency represented as a string in the form char:frequency.</returns>
        public override string ToString()
        {
            string returnstr;
            if (symbol == '\n')
                returnstr = "newline" + ":" + frequency.ToString();
            else if (symbol == '\r')
                returnstr = "return" + ":" + frequency.ToString();
            else
                returnstr = symbol.ToString() + ":" + frequency.ToString();
            return returnstr;
        }
    }
}
