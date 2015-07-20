using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
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
    /// Real men write their own unit tests when they don't want to learn dirty unfree propriatry IDE methods.
    /// </summary>
    /// 
    class heaptest
    {
        public static int start()
        {
            Debug.Print("TESTING HEAP");

            Random rand = new Random();
            List<HuffmanTreeNode> huff = new List<HuffmanTreeNode>();
            for (int ii = 0; ii < 100; ii++)
                huff.Add(HuffmanTreeNode.HuffmanTreeFactory('x', rand.Next(256)));

            Heap h = new Heap(huff.ToArray());
            for (int ii = 0; ii < 40; ii++)
                h.extractMin();
            Debug.Print(h.size.ToString());

            for (int ii = 0; ii < 40; ii++)
                h.insert(HuffmanTreeNode.HuffmanTreeFactory('x', rand.Next(256)));
            Debug.Print(h.size.ToString());

            double one = h.extractMin().freq;
            double two;
            while (h.size > 0)
            {
                two = h.extractMin().freq;
                if (one > two)
                {
                    Debug.Print("FAILED " + one.ToString() + " " + two.ToString());
                }
                one = two;
            }
            Debug.Print("PASSED");
            return 0;
        }
    }
}
