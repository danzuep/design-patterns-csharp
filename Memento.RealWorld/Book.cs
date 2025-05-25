namespace Memento.Generic.BookProgram;

public class Book
{
    public string Title { get; }
    public int Chapter { get; set; }
    public int Verse { get; set; }
    public int Page { get; set; }

    public Book(string title)
    {
        Title = title;
        Chapter = 1;
        Verse = 1;
        Page = 1;
    }

    public override string ToString() =>
        $"'{Title}', page {Page}";
}

public class Program
{
    public static void Test()
    {
        var book = new Book("Genesis");

        var originator = new Originator<Book>(book);
        var caretaker = new Caretaker<Book>();
        var manager = new StateManager<Book>(originator, caretaker);

        manager.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(manager.State))
                Console.WriteLine($"PropertyChanged: State is now {manager.State}");
        };

        manager.OnStateChanged += (s, pos) =>
        {
            Console.WriteLine($"OnStateChanged event: {pos}");
        };

        // Initial state
        Console.WriteLine($"Starting reading {originator.State}");

        // Turn pages and save state
        originator.State.Page = 15;
        caretaker.Save(originator.Snapshot());
        Console.WriteLine($"Turned to {originator.State}");

        originator.State.Page = 30;
        caretaker.Save(originator.Snapshot());
        Console.WriteLine($"Turned to {originator.State}");

        originator.State.Page = 45;
        Console.WriteLine($"Turned to {originator.State} (not saved)");

        // Undo last saved state (should go back to page 30)
        if (caretaker.TryUndo(out var memento))
        {
            originator.Restore(memento);
            Console.WriteLine($"Restored to {originator.State}");
        }

        // Undo again (should go back to page 15)
        if (caretaker.TryUndo(out memento))
        {
            originator.Restore(memento);
            Console.WriteLine($"Restored to {originator.State}");
        }

        book.Verse = 2;
        manager.Update(book);
        book.Verse = 3;
        manager.Update(book);

        manager.Undo();
        manager.Undo();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}