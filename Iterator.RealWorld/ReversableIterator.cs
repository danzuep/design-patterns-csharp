using System.Collections;

namespace Iterator.RealWorld
{
    // EN: Concrete Iterators implement various traversal algorithms.
    public sealed class ReversableIterator<T> : IEnumerator
    {
        private int _position = 0;

        private bool _reverse = false;

        private readonly List<T> _collection;

        public ReversableIterator(IEnumerable<T> collection)
        {
            _collection = collection.ToList();
        }

        object IEnumerator.Current => _collection[_position] ??
            throw new InvalidOperationException("Current item is null.");

        private bool Move(int count = 1)
        {
            int updatedPosition = _position + count;

            if (updatedPosition >= 0 && updatedPosition < _collection.Count)
            {
                _position = updatedPosition;
                return true;
            }

            return false;
        }

        public bool MoveNext()
        {
            return Move(_reverse ? -1 : 1);
        }

        public bool MovePrevious()
        {
            return Move(_reverse ? 1 : -1);
        }

        public void Reverse()
        {
            _reverse = !_reverse;
            Reset();
        }

        public void Reset()
        {
            _position = _reverse ? _collection.Count - 1 : 0;
        }
    }
}
