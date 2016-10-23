using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegerList
{
    public class IntegerList : IIntegerList
    {
        private int[] _internalStorage;
        private int _count;

        public IntegerList() : this(2)
        {
        }

        public IntegerList(int initialSize)
        {
            if (initialSize < 0)
            {
                throw new ArgumentException("Initial size has to be greater than zero!");
            }
            _internalStorage = new int[initialSize];
            _count = 0;
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public void Add(int item)
        {
            if (_count == _internalStorage.Length)
            {
                Resize(2 * _internalStorage.Length);
            }
            _internalStorage[_count] = item;
            _count++;
        }

        public void Clear()
        {
            _internalStorage = new int[_internalStorage.Length];
            _count = 0;
        }

        public bool Contains(int item)
        {
            return IndexOf(item) < 0 ? false : true;
        }

        public int GetElement(int index)
        {
            if (index < 0 || index >= _internalStorage.Length)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return _internalStorage[index];
            }
        }

        public int IndexOf(int item)
        {
            int index = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_internalStorage[i] == item)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public bool Remove(int item)
        {
            return RemoveAt(IndexOf(item));
        }

        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= _internalStorage.Length)
            {
                return false;
            }
            if (_internalStorage[index] == 0)
            {
                return false;
            }
            ShiftLeft(index);
            _count--;
            return true;
        }

        private void ShiftLeft(int toIndex)
        {
            int[] temp = new int[_internalStorage.Length];
            for (int i = 0; i < toIndex; i++)
            {
                temp[i] = _internalStorage[i];
            }
            int end = _internalStorage.Length - 1;
            for (int i = toIndex; i < end; i++)
            {
                temp[i] = _internalStorage[i + 1];
            }
            _internalStorage = temp;
        }

        private void Resize(int newLength)
        {
            if (newLength == 0)
            {
                newLength = 2;
            }
            int[] temp = new int[newLength];
            for (int i = 0; i < _internalStorage.Length; i++)
            {
                temp[i] = _internalStorage[i];
            }
            _internalStorage = temp;
        }

        public void ListExample(IIntegerList listOfIntegers)
        {
            listOfIntegers.Add(1); // [1]
            listOfIntegers.Add(2); // [1 ,2]
            listOfIntegers.Add(3); // [1 ,2 ,3]
            listOfIntegers.Add(4); // [1 ,2 ,3 ,4]
            listOfIntegers.Add(5); // [1 ,2 ,3 ,4 ,5]
            listOfIntegers.RemoveAt(0); // [2 ,3 ,4 ,5]
            listOfIntegers.Remove(5); // [2 ,3 ,4]
            Console.WriteLine ( listOfIntegers.Count ) ; // 3
            Console.WriteLine ( listOfIntegers.Remove (100) ) ; // false
            Console.WriteLine ( listOfIntegers.RemoveAt (5) ) ; // false
            listOfIntegers.Clear () ; // []
            Console.WriteLine ( listOfIntegers.Count ) ; // 0
            Console.ReadLine();
        }
    }
}
