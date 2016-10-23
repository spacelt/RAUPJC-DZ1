using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericList
{
    public class GenericList<T> : IGenericList<T>
    {
        private T[] _internalStorage;
        private int _count;

        public GenericList () : this(2)
        {}

        public GenericList(int initialSize)
        {
            _internalStorage = new T[initialSize];
            _count = 0;
        }
        public int Count
        {
            get
            {
                return _count;
            }
        }

        public void Add(T item)
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
            _internalStorage = new T[2];
            _count = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) < 0 ? false : true;
        }

        public T GetElement(int index)
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

        public int IndexOf(T item)
        {
            int index = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_internalStorage[i].Equals(item))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public bool Remove(T item)
        {
            return RemoveAt(IndexOf(item));
        }

        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= _internalStorage.Length)
            {
                return false;
            }
            if (_internalStorage[index] == null)
            {
                return false;
            }
            ShiftLeft(index);
            _count--;
            return true;
        }

        private void Resize(int newLength)
        {
            if (newLength == 0)
            {
                newLength = 2;
            }
            T[] temp = new T[newLength];
            for (int i = 0; i < _internalStorage.Length; i++)
            {
                temp[i] = _internalStorage[i];
            }
            _internalStorage = temp;
        }

        private void ShiftLeft(int toIndex)
        {
            T[] temp = new T[_internalStorage.Length];
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

       
        public IEnumerator<T> GetEnumerator()
        {
            return new GenericListEnumerator<T>(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
