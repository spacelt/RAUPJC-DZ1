using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericList
{
    class GenericListEnumerator<X> : IEnumerator<X>
    {
        private GenericList<X> _genericList;
        private int _index;

        public GenericListEnumerator(GenericList<X> genericList)
        {
            _genericList = genericList;
            _index = 0;
        }

        public X Current
        {
            get
            {
                return _genericList.GetElement(_index++);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            // not in use
        }

        public bool MoveNext()
        {
            return _index == _genericList.Count ? false : true;
        }

        public void Reset()
        {
            _index = 0;
        }
    }
}
