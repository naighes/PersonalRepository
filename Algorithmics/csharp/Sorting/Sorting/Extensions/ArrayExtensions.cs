using System;
using Xunit;

namespace Sorting.Extensions
{
    public static class ArrayExtensions
    {
        public static void Swap<T>(this T[] array, Int32 a, Int32 b) where T : IComparable<T>
        {
            var tmp = array[a];
            array[a] = array[b];
            array[b] = tmp;
        }

        public static void Heapify<T>(this T[] array, Int32 index, Int32 size) where T : IComparable<T>
        {
            var left = index * 2 + 1;
            var right = left + 1;
            var largest = index;

            // Check whether left node is greater than its parent.
            if (left < size && array[left].CompareTo(array[index]) > 0)
                largest = left;

            // Check whether right node is greater than current largest value.
            if (right < size && array[right].CompareTo(array[largest]) > 0)
                largest = right;

            // Do I need to swap nodes?
            if (largest == index)
                return;

            array.Swap(index, largest);

            // If nodes are swapped out, than heapify the sub-tree.
            Heapify(array, largest, size);
        }

        public static void ToHeap<T>(this T[] array, Int32 size) where T : IComparable<T>
        {
            if (array == null)
                throw new ArgumentNullException("array");

            for (var i = array.Length / 2 - 1; i >= 0; i--)
                array.Heapify(i, size);
        }
    }

    public class ArrayExtensionsTests
    {
        [Fact]
        public void HeapTest()
        {
            var array = new[] { 16, 4, 10, 14, 7, 9, 3, 2, 8, 1 };
            array.ToHeap(array.Length);
            Assert.Equal(16, array[0]);
            Assert.Equal(14, array[1]);
            Assert.Equal(10, array[2]);
            Assert.Equal(8, array[3]);
            Assert.Equal(7, array[4]);
            Assert.Equal(9, array[5]);
            Assert.Equal(3, array[6]);
            Assert.Equal(2, array[7]);
            Assert.Equal(4, array[8]);
            Assert.Equal(1, array[9]);
        }
    }
}