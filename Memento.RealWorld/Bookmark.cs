namespace Memento.RealWorld.BookReader;

using System;
using System.Collections.Generic;

public record Book(string Title);

public class BookReader
{
    public Book Book { get; }
    public int Page { get; set; }

    public BookReader(Book book)
    {
        Book = book;
        Page = 1;
    }
}

public record Bookmark(string BookTitle, int Page);

public class BookmarkManager
{
    private readonly Dictionary<string, Bookmark> _bookmarks = new();

    public void SaveBookmark(Bookmark bookmark)
    {
        _bookmarks[bookmark.BookTitle] = bookmark;
    }

    public Bookmark? GetBookmark(string bookTitle)
    {
        _bookmarks.TryGetValue(bookTitle, out var bookmark);
        return bookmark;
    }
}

public class Originator
{
    private readonly BookReader _reader;

    public Originator(BookReader reader)
    {
        _reader = reader;
    }

    public void TurnToPage(int page)
    {
        _reader.Page = page;
        Console.WriteLine($"Turned to page {_reader.Page} in '{_reader.Book.Title}'.");
    }

    public void DisplayPage()
    {
        Console.WriteLine($"Currently at page {_reader.Page} in '{_reader.Book.Title}'.");
    }

    public Bookmark CreateBookmark()
    {
        Console.WriteLine($"Bookmark created at page {_reader.Page} in '{_reader.Book.Title}'.");
        return new Bookmark(_reader.Book.Title, _reader.Page);
    }

    public void RestoreBookmark(Bookmark bookmark)
    {
        if (bookmark.BookTitle != _reader.Book.Title)
        {
            Console.WriteLine("Bookmark does not belong to this book.");
            return;
        }

        _reader.Page = bookmark.Page;
        Console.WriteLine($"Restored to page {_reader.Page} in '{_reader.Book.Title}'.");
    }
}

public class Program
{
    public static void Test()
    {
        var book1 = new Book("Design Patterns in C#");
        var book2 = new Book("The Art of Computer Programming");

        var reader1 = new BookReader(book1);
        var reader2 = new BookReader(book2);

        var originator1 = new Originator(reader1);
        var originator2 = new Originator(reader2);

        var caretaker = new BookmarkManager();

        originator1.DisplayPage();
        originator1.TurnToPage(40);
        caretaker.SaveBookmark(originator1.CreateBookmark());

        originator2.DisplayPage();
        originator2.TurnToPage(123);
        caretaker.SaveBookmark(originator2.CreateBookmark());

        originator1.TurnToPage(65);

        var bookmark1 = caretaker.GetBookmark(book1.Title);
        if (bookmark1 is not null)
            originator1.RestoreBookmark(bookmark1);

        originator1.DisplayPage();

        var bookmark2 = caretaker.GetBookmark(book2.Title);
        if (bookmark2 is not null)
            originator2.RestoreBookmark(bookmark2);

        originator2.DisplayPage();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}