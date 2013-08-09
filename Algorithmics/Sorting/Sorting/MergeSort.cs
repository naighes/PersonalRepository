using System;
using Xunit;

namespace Sorting
{
    public class MergeSortTests
    {
        [Fact]
        public void SortingTest()
        {
            var array = new[] { 4, 3, 2, 6, 1, 5 };
            new MergeSort().Sort(array);
            Assert.Equal(1, array[0]);
            Assert.Equal(2, array[1]);
            Assert.Equal(3, array[2]);
            Assert.Equal(4, array[3]);
            Assert.Equal(5, array[4]);
            Assert.Equal(6, array[5]);
        }
    }

    public class MergeSort
    {
        public void Sort<T>(T[] array)
            where T : IComparable<T>
        {
            Sort(array, 0, array.Length - 1);
        }

        private void Sort<T>(T[] array, Int32 left, Int32 right)
            where T : IComparable<T>
        {
            if (left == right)
                return;

            var center = (left + right) / 2;
            Sort(array, left, center);
            Sort(array, center + 1, right);
            Merge(array, left, center + 1, right);
        }

        public void Merge<T>(T[] array, Int32 left, Int32 center, Int32 right)
            where T : IComparable<T>
        {
            var tmp = new T[right - left + 1];
            var leftCursor = left;
            var rightCursor = center;
            var currentTmpIndex = 0;

            while (leftCursor <= center - 1 && rightCursor <= right)
                if (array[leftCursor].CompareTo(array[rightCursor]) <= 0)
                    tmp[currentTmpIndex++] = array[leftCursor++];
                else
                    tmp[currentTmpIndex++] = array[rightCursor++];

            while (leftCursor <= center - 1)
                tmp[currentTmpIndex++] = array[leftCursor++];

            while (rightCursor <= right)
                tmp[currentTmpIndex++] = array[rightCursor++];

            foreach (var t in tmp)
                array[left++] = t;
        }
    }
}