using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
/*  
    Copyright (C) 2014 Jason Giancono (jasongiancono@gmail.com)

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
*/
namespace Asgn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        /// <summary>
        /// Event handler for when the frequency button is clicked. 
        /// Finds the frequency of all symbols (only of length one) in the text and creates the table.
        /// </summary>
        /// <param name="sender">Event Stuff (Don't ask me, I didn't put it there?</param>
        /// <param name="e">State information</param>
        private void btnFreq_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string plain = txtPlain.Text;
                if (plain != string.Empty)
                {
                    Dictionary<char, Frequency> freq = new Dictionary<char, Frequency>();
                    foreach (char character in plain)
                    {
                        if (!freq.ContainsKey(character))
                        {
                            double f = 0;
                            foreach (char c in plain)
                            {
                                if (c == character)
                                    f++;
                            }
                            freq.Add(character, new Frequency(character, f));
                        }
                    }
                    List<Frequency> frequencies = freq.Values.ToList();
                    string outString = frequencies[0].ToString();
                    for (int ii = 1; ii < frequencies.Count; ii++)
                    {
                        outString = outString + "\n" + frequencies[ii].ToString();
                    }
                    txtFreqTbl.Text = outString;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Event handler for when the Decompress button is clicked. 
        /// Uses the frequency table to build a HuffmanTree then uses that tree to decode the data. 
        /// The frequency table must be the same as the one to encode it or bad things happen.
        /// </summary>
        /// <param name="sender">Event Stuff (Don't ask me, I didn't put it there?</param>
        /// <param name="e">State information</param>
        private void btnDecompress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] freqStringList = txtFreqTbl.Text.Split('\n');
                List<Frequency> frequencies = new List<Frequency>();
                foreach (string s in freqStringList)
                {
                    frequencies.Add(new Frequency(s));
                }
                HuffmanTreeNodeComposite root = HuffmanTreeNode.HuffmanTreeFactory(frequencies, new Dictionary<char, HuffmanTreeNodeLeaf>());
                Dictionary<char, DAABitArray> dict = new Dictionary<char, DAABitArray>();
                for (int ii = 0; ii < 64; ii++)
                {
                    long x = (long)ii;
                    DAABitArray bits = new DAABitArray();
                    bits.Append(x, 6);
                    dict.Add(DAABitArray.encoding[ii], bits);
                }
                DAABitArray encodedBits = new DAABitArray();
                foreach (char character in txtCompressed.Text)
                {
                    encodedBits.Append(dict[character]);
                }
                string decoded = "";
                while (encodedBits.NumBits > 0)
                {
                    decoded = decoded + root.decode(encodedBits).ToString();
                }
                txtPlain.Text = decoded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// Event handler for when the Compress button is clicked. 
        /// Uses the frequency table to build a HuffmanTree and a Dictionary of nodes
        /// then converts the symbols to DAABitArrays using the Dictionary to find the node.
        /// Also appends 0 bits to make the entire bit sequence divisable by 6.
        /// The frequency table must be the same as the one to encode it or bad things happen.
        /// </summary>
        /// <param name="sender">Event Stuff (Don't ask me, I didn't put it there?</param>
        /// <param name="e">State information</param>
        private void btnCompress_Click(object sender, RoutedEventArgs e)
        {
            //try
            {
                string[] freqStringList = txtFreqTbl.Text.Split('\n');
                List<Frequency> frequencies = new List<Frequency>();
                foreach (string s in freqStringList)
                {
                    frequencies.Add(new Frequency(s));
                }
                Dictionary<char, HuffmanTreeNodeLeaf> dict = new Dictionary<char, HuffmanTreeNodeLeaf>();
                HuffmanTreeNodeComposite root = HuffmanTreeNode.HuffmanTreeFactory(frequencies, dict);
                DAABitArray bits = new DAABitArray();
                string converted = "";
                foreach (char character in txtPlain.Text)
                {
                    DAABitArray append = new DAABitArray();
                    bits.Append(dict[character].encode(append));
                }
                while (bits.NumBits % 6 != 0)
                {
                    bits.Append(false);
                }
                int index = 0;
                while (index <= bits.NumBits - 6)
                {
                    converted = converted + bits.SixToChar(index);
                    index = index + 6;
                }
                txtCompressed.Text = converted;
            }
            //catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }

        }
    }
}
