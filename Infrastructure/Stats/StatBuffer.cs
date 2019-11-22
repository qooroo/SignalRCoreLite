using System.Collections.Generic;

namespace Infrastructure.Stats
{
    public class StatBuffer<T>
    {
        private readonly T[] _buffer;
        private int _nextFree;

        public StatBuffer(int length)
        {
            _buffer = new T[length];
            _nextFree = 0;
        }

        public void Add(T o)
        {
            if (_nextFree >= _buffer.Length)
                return;

            _buffer[_nextFree] = o;
            _nextFree++;
        }

        public List<T> GetBatch()
        {
            var retList = new List<T>();

            for (var i = 0; i < _nextFree; i++)
            {
                retList.Add(_buffer[i]);
            }

            _nextFree = 0;

            return retList;
        }
    }
}
