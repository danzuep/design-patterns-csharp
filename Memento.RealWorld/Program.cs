namespace Memento.RealWorld;

// Memento, stores a snapshot of the position
public class ReadingProgressMemento
{
    public string Book { get; }
    public int Chapter { get; }
    public int Verse { get; }

    public ReadingProgressMemento(string book, int chapter, int verse)
    {
        Book = book;
        Chapter = chapter;
        Verse = verse;
    }
}

// Originator, encapsulates the current position
public class ReadingProgressOriginator
{
    public string Book { get; private set; }
    public int Chapter { get; private set; }
    public int Verse { get; private set; }

    public ReadingProgressOriginator(string book, int chapter, int verse)
    {
        Book = book;
        Chapter = chapter;
        Verse = verse;
    }

    public void UpdateProgress(string book, int chapter, int verse)
    {
        Book = book;
        Chapter = chapter;
        Verse = verse;
    }

    // Create a memento with current state
    public ReadingProgressMemento Save()
    {
        return new ReadingProgressMemento(Book, Chapter, Verse);
    }

    // Restore state from memento
    public void Restore(ReadingProgressMemento memento)
    {
        Book = memento.Book;
        Chapter = memento.Chapter;
        Verse = memento.Verse;
    }

    public override string ToString()
    {
        return $"{Book} {Chapter}:{Verse}";
    }
}

// Manages the history stack of mementos
public class ReadingProgressCaretaker
{
    private readonly Stack<ReadingProgressMemento> _history = new Stack<ReadingProgressMemento>();

    public void Save(ReadingProgressOriginator progress)
    {
        _history.Push(progress.Save());
    }

    public bool CanUndo => _history.Count > 0;

    public ReadingProgressMemento Undo()
    {
        if (CanUndo)
            return _history.Pop();
        return null;
    }
}

public class BibleReader
{
    private ReadingProgressOriginator _currentProgress;
    private ReadingProgressCaretaker _caretaker = new ReadingProgressCaretaker();

    public BibleReader(string startBook, int startChapter, int startVerse)
    {
        _currentProgress = new ReadingProgressOriginator(startBook, startChapter, startVerse);
    }

    public void ReadNextVerse(string book, int chapter, int verse)
    {
        // Save current progress before changing
        _caretaker.Save(_currentProgress);

        // Update reading position
        _currentProgress.UpdateProgress(book, chapter, verse);

        Console.WriteLine($"Moved to {_currentProgress}");
    }

    public void GoBack()
    {
        if (_caretaker.CanUndo)
        {
            var previousState = _caretaker.Undo();
            _currentProgress.Restore(previousState);
            Console.WriteLine($"Went back to {_currentProgress}");
        }
        else
        {
            Console.WriteLine("No previous reading progress to go back to.");
        }
    }

    public void ShowCurrentProgress()
    {
        Console.WriteLine($"Current position: {_currentProgress}");
    }
}

public class Program
{
    public static void Main()
    {
        var reader = new BibleReader("Genesis", 1, 1);

        reader.ShowCurrentProgress();  // Genesis 1:1

        reader.ReadNextVerse("Genesis", 1, 2);
        reader.ReadNextVerse("Genesis", 1, 3);
        reader.ReadNextVerse("Genesis", 1, 4);

        reader.GoBack();   // Goes back to Genesis 1:3
        reader.GoBack();   // Goes back to Genesis 1:2
        reader.GoBack();   // Goes back to Genesis 1:1
        reader.GoBack();   // No previous progress

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}