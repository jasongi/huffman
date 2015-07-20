using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Asgn
{
    /// <summary>
    /// Class to sort array of HuffmanTreeNodes into a heap and then do heap things.
    /// Used as a priority queue when building the HuffmanTree.
    /// </summary>
    /// 
    class Heap
    {
        /// <summary>
        /// The number of nodes in the heap.
        /// </summary>
        public int size { get; private set; }

        /// <summary>
        /// The array the heap is stored in.
        /// </summary>
        public HuffmanTreeNode[] heapArray { get; private set; }

        /// <summary>
        /// Equivilant of Build_Max_Heap. Takes an unsorted array and turns into heap
        /// </summary>
        /// <param name="array">Array to heapify.</param>
        /// <returns>Heapified Array</returns>
        public Heap(HuffmanTreeNode[] array)
        {
            heapArray = new HuffmanTreeNode[array.Length + 1];
            Array.Copy(array, heapArray, array.Length);
            size = array.Length;
            for (int ii = size - 1; ii >= 0; ii--)
                this.minHeapify(ii);
        }

        /// <summary>
        /// Restores the heap property assuming the subtrees below are also heapified.
        /// </summary>
        /// <param name="index">index of the node of the sub-branch to heapify.</param>
        public void minHeapify(int index)
        {

            int left = leftChild(index);
            int right = rightChild(index);
            int smallest = index;
            if ((left < this.size) && (heapArray[left].freq < heapArray[smallest].freq))
            {
                smallest = left;
            }
            if ((right < this.size) && (heapArray[right].freq < heapArray[smallest].freq))
            {
                smallest = right;
            }
            if (smallest != index)
            {
                swap(index, smallest);
                this.minHeapify(smallest);
            }
        }

        /// <summary>
        /// Swaps the data of two nodes
        /// </summary>
        /// <param name="a">First node to swap.</param>
        /// <param name="b">Second node to swap.</param>
        public void swap(int a, int b)
        {
            HuffmanTreeNode tmp = heapArray[a];
            heapArray[a] = heapArray[b];
            heapArray[b] = tmp;
        }

        /// <summary>
        /// Insert a node into the heap preserving heap status
        /// </summary>
        /// <param name="item">Data to insert.</param>
        public void insert(HuffmanTreeNode item)
        {
            size++;
            int index = size - 1;
            heapArray[index] = item;
            while (index > 0 && heapArray[parent(index)].freq > item.freq)
            {
                heapArray[index] = heapArray[parent(index)];
                index = parent(index);
            }
            heapArray[index] = item;
        }

        /// <summary>
        /// Check to see if heap is empty
        /// </summary>
        /// <returns>True if tree is empty, otherwise False</returns>
        public bool isEmpty()
        {
            return size == 0;
        }

        /// <summary>
        /// Extract item with smallest priority and the heapify the heap.
        /// </summary>
        /// <returns>Root node of the heap</returns>
        public HuffmanTreeNode extractMin()
        {
            HuffmanTreeNode max = null;
            if (!isEmpty())
            {
                max = heapArray[0];
                heapArray[0] = heapArray[size - 1];
                size--;
                this.minHeapify(0);
            }
            return max;

        }

        /// <summary>
        /// Calculate Parent index.
        /// </summary>
        /// <returns>Parent index.</returns>
        public int parent(int index)
        {
            return (index - 1) / 2;
        }

        /// <summary>
        /// Calculate left child index.
        /// </summary>
        /// <returns>Left child index.</returns>
        public int leftChild(int index)
        {
            return 2 * index + 1;
        }

        /// <summary>
        /// Calculate right child index.
        /// </summary>
        /// <returns>Right child index.</returns>
        public int rightChild(int index)
        {
            return 2 * index + 2;
        }

    }
}
