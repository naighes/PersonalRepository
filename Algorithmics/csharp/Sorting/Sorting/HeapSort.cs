using System;
using Sorting.Extensions;
using Xunit;

namespace Sorting
{
    public class HeapSortTests
    {
        [Fact]
        public void SortingTest()
        {
            var array = new[] { 16, 4, 10, 14, 7, 9, 3, 2, 8, 1 };
            new HeapSort().Sort(array);
            Assert.Equal(array[0], 1);
            Assert.Equal(array[1], 2);
            Assert.Equal(array[2], 3);
            Assert.Equal(array[3], 4);
            Assert.Equal(array[4], 7);
            Assert.Equal(array[5], 8);
            Assert.Equal(array[6], 9);
            Assert.Equal(array[7], 10);
            Assert.Equal(array[8], 14);
            Assert.Equal(array[9], 16);
        }
    }

    public class HeapSort
    {
        public void Sort<T>(T[] array)
            where T : IComparable<T>
        {
            var cursor = array.Length;
            array.ToHeap(cursor);

            while (cursor-- > 0)
            {
                array.Swap(0, cursor);
                array.ToHeap(cursor);
            }
        }
    }
}