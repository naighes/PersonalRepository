using System;
using Xunit;

namespace Sorting
{
    public class InsertionSortTests
    {
        [Fact]
        public void SortingTest()
        {
            var array = new[] {4, 3, 2, 6};
            new InsertionSort().Sort(array);
            Assert.Equal(2, array[0]);
            Assert.Equal(3, array[1]);
            Assert.Equal(4, array[2]);
            Assert.Equal(6, array[3]);
        }
    }

    public class InsertionSort
    {
        public void Sort<T>(T[] array) where T : IComparable<T>
        {
            if (array == null)
                throw new ArgumentNullException("array");

            for (var j = 1; j < array.Length; j++)
            {
                var key = array[j];
                var i = j - 1;

                while (i >= 0 && array[i].CompareTo(key) > 0)
                {
                    array[i + 1] = array[i];
                    i--;
                }

                array[i + 1] = key;
            }
        }

        /*
              key : 3
              i   j            
            | 4 | 3 | 2 | 6 |
         
              key : 2
                  i   j
            | 3 | 4 | 2 | 6 |
         
              i       j
            | 3 | 4 | 4 | 6 |

            | 2 | 3 | 4 | 6 |
        */
    }
}