using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Asgn
{
    /// <summary>
    /// Parent class of both HuffmanTreeNodeLeaf and HuffmanTreeNodeComposite. 
    /// Cannot be instantiated. 
    /// </summary>
    /// 
    abstract class HuffmanTreeNode
    {
        /// <summary>
        /// The frequency of the node (or the sum of child node frequencies).
        /// </summary>
        public double freq { get; set; }

        /// <summary>
        /// The node's parent. 
        /// null if it is the root.
        /// </summary>
        public HuffmanTreeNode parent { get; set; }

        /// <summary>
        /// The node's 'bit'. 
        /// 0 if it is the left node, 1 if it is the right.
        /// </summary>
        public bool bit { get; set; }

        /// <summary>
        /// A recursive function for decoding a BitArray. 
        /// If the node is a composite it pops off the top bit and passes to it's child, returning it's value. 
        /// Otherwise if it is a leaf it returns it's symbol.
        /// </summary>
        /// <param name="bits">The array you are decoding.</param>
        /// <returns>The symbol of the array.</returns>
        public abstract char decode(DAABitArray bits);

        /// <summary>
        /// A recursive function for encoding a BitArray given a node (which is retrieved from a dictionary of characters).
        /// </summary>
        /// <param name="bits">The array you're appending your bit to</param>
        /// <returns>The array with the bits of all parents nodes and this node appended</returns>
        public abstract DAABitArray encode(DAABitArray bits);

        /// <summary>
        /// Factory for building a HuffmanTree from a frequency list. 
        /// Also builds up a Dictionary of the nodes for encoding.
        /// </summary>
        /// <param name="freqlist">List of frequencies of characters.</param>
        /// <param name="htDict">Empty dictionary to be filled.</param>
        /// <returns>A Composite HuffmanTreeNode which is the root of the tree. Also passes a dictionary of the nodes by reference.</returns>
        public static HuffmanTreeNodeComposite HuffmanTreeFactory(List<Frequency> freqlist, Dictionary<char, HuffmanTreeNodeLeaf> htDict)
        {
            foreach (Frequency element in freqlist)
            {
                HuffmanTreeNodeLeaf newLeaf = HuffmanTreeFactory(element.symbol, element.frequency);
                htDict.Add(newLeaf.symbol, newLeaf);
            }
            HuffmanTreeNode[] nodeArray = htDict.Values.ToArray();
            //swapped for more dynamic function which uses the array
            /*Heap heap = new Heap();
            for (int ii = 0; ii < nodeArray.Length; ii++)
            {
                if (nodeArray[ii] != null)
                    heap.insert(nodeArray[ii]);
            }*/
            Heap heap = new Heap(nodeArray);
            HuffmanTreeNodeComposite root = null;
            HuffmanTreeNode left;
            HuffmanTreeNode right;
            while (heap.size > 1)
            {
                left = heap.extractMin();
                right = heap.extractMin();
                root = HuffmanTreeFactory(left, right);
                if (heap.size > 0)
                    heap.insert(root);
            }
            root.parent = null;
            return root;
        }

        /// <summary>
        /// Factory for building a HuffmanTreeNodeLeaf given a symbol and it's frequency.
        /// </summary>
        /// <param name="sym">Symbol the leaf represents.</param>
        /// <param name="frequency">Frequency of the symbol in the plaintext.</param>
        /// <returns>A HuffmanTreeLeafNode with the fields passed to it.</returns>
        public static HuffmanTreeNodeLeaf HuffmanTreeFactory(char sym, double frequency)
        {
            return new HuffmanTreeNodeLeaf(sym, frequency);
        }

        /// <summary>
        /// Factory for building a HuffmanTreeNodeComposite given a both of it's children.
        /// </summary>
        /// <param name="left">Right child of node.</param>
        /// <param name="right">Left child of node.</param>
        /// <returns>A HuffmanTreeNodeComposite with the fields passed to it as it's children and the frequency as the sum of them both.</returns>
        public static HuffmanTreeNodeComposite HuffmanTreeFactory(HuffmanTreeNode left, HuffmanTreeNode right)
        {
            return new HuffmanTreeNodeComposite(left, right);
        }
    }

    /// <summary>
    /// Class representing a leaf node on the HuffmanTree which has a symbol and no children. 
    /// Inherits from HuffmanTreeNode.
    /// </summary>
    class HuffmanTreeNodeLeaf : HuffmanTreeNode
    {
        /// <summary>
        /// The symbol this node represnets.
        /// </summary>
        public char symbol { get; set; }

        /// <summary>
        /// Constructor for building a leaf from a symbol and it's frequency.
        /// </summary>
        /// <param name="sym">Symbol the leaf represents.</param>
        /// <param name="frequency">Frequency of the symbol in the plaintext.</param>
        /// <returns>A HuffmanTreeNodeLeaf with the fields passed to it.</returns>
        public HuffmanTreeNodeLeaf(char sym, double frequency)
        {
            symbol = sym;
            freq = frequency;
        }

        /// <summary>
        /// A function for decoding a BitArray. 
        /// When this function is called the symbol of the leaf is return and the recursion unwinds
        /// </summary>
        /// <param name="bits">The array you are decoding.</param>
        /// <returns>The symbol of this leafnode.</returns>
        public override char decode(DAABitArray bits)
        {
            return symbol;
        }

        /// <summary>
        /// A function for encoding a BitArray given a node (which is retrieved from a dictionary of characters)
        /// </summary>
        /// <param name="bits">The array you're appending your bit to</param>
        /// <returns>The array with the bits of all parents nodes and this node appended</returns>
        public override DAABitArray encode(DAABitArray bits)
        {
            return parent.encode(bits).Append(bit);
        }
    }

    /// <summary>
    /// A Class representing a composite node of two HuffmanTreeNodes which have 
    /// two children and a frequency which is the sum of their children's frequencies. 
    /// Inherits from HuffmanTreeNode.
    /// </summary>
    class HuffmanTreeNodeComposite : HuffmanTreeNode
    {
        /// <summary>
        /// The left child of the node.
        /// </summary>
        public HuffmanTreeNode left { get; set; }

        /// <summary>
        /// The right child of the node.
        /// </summary>
        public HuffmanTreeNode right { get; set; }

        /// <summary>
        /// A recursive function for decoding a BitArray. 
        /// It pops off the top bit and passes to it's child, returning it's value.
        /// </summary>
        /// <param name="bits">The array you are decoding.</param>
        /// <returns>The symbol of the array.</returns>
        public override char decode(DAABitArray bits)
        {
            HuffmanTreeNode node;
            char returnsymbol;
            if (bits.NumBits == 0)
                returnsymbol = (char)(0);
            else if (bits.pop())
            {
                returnsymbol = left.decode(bits);
            }
            else
            {
                returnsymbol = right.decode(bits);
            }
            return returnsymbol;
        }

        /// <summary>
        /// A function for encoding a BitArray given a node (which is retrieved from a dictionary of characters). 
        /// Returns the array vertibam if this node is the root.
        /// </summary>
        /// <param name="bits">The array you're appending your bit to</param>
        /// <returns>The array with the bits of all parents nodes and this node appended</returns>
        public override DAABitArray encode(DAABitArray bits)
        {
            DAABitArray returnbits;
            if (parent == null)
                returnbits = bits;
            else 
                returnbits = parent.encode(bits).Append(bit);
            return returnbits;
        }

        /// <summary>
        /// Constructor for building a Composite node from two nodes. 
        /// Also makes the new node their parent.
        /// </summary>
        /// <param name="left">The left node.</param>
        /// <param name="right">The right node.</param>
        /// <returns>A HuffmanTreeNodeNode with two children and a frequency that is the sum of the children.</returns>f
        public HuffmanTreeNodeComposite(HuffmanTreeNode left, HuffmanTreeNode right)
        {
            freq = left.freq + right.freq;
            this.left = left;
            this.right = right;
            left.parent = this;
            right.parent = this;
            left.bit = true;
            right.bit = false;
        }
    }
}