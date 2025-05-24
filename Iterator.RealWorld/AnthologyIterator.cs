using System.Collections;

namespace RefactoringGuru.DesignPatterns.Iterator.RealWorld
{
    // EN: Concrete Iterators implement various traversal algorithms.
    public sealed class AnthologyIterator : IEnumerator<Chapter>
    {
        private IEnumerator<Chapter> _chapterEnumerator;

        private IEnumerator<Book> _bookEnumerator;

        private readonly IEnumerable<Book> _books;

        public AnthologyIterator(IEnumerable<Book> books)
        {
            _books = books ?? throw new ArgumentNullException(nameof(books));
            _bookEnumerator = _books.GetEnumerator();
            _chapterEnumerator = _bookEnumerator.Current.Chapters.GetEnumerator();
        }

        object IEnumerator.Current => Current;

        public Chapter Current => _chapterEnumerator.Current;

        public bool MoveNext()
        {
            if (_bookEnumerator.Current == null)
            {
                return false;
            }

            _chapterEnumerator.MoveNext();
            if (_chapterEnumerator.Current != null)
            {
                return true;
            }

            _bookEnumerator.MoveNext();
            _chapterEnumerator = _bookEnumerator.Current.Chapters.GetEnumerator();
            return _chapterEnumerator.Current != null;
        }

        public void Reset()
        {
            _bookEnumerator.Dispose();
            _bookEnumerator = _books.GetEnumerator();
            _chapterEnumerator.Dispose();
            _chapterEnumerator = _bookEnumerator.Current.Chapters.GetEnumerator();
        }

        public void Dispose()
        {
            _chapterEnumerator.Dispose();
            _bookEnumerator.Dispose();
        }
    }
}
