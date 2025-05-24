using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace RefactoringGuru.DesignPatterns.Iterator.RealWorld
{
    public sealed record Chapter(string BookName, int ChapterNumber);

    public sealed record Book(string Name, IEnumerable<Chapter> Chapters);

    public static class Iterator
    {
        public static IEnumerable<Chapter> IterateChapters(this IEnumerable<Book> books)
        {
            ArgumentNullException.ThrowIfNull(nameof(books));

            foreach (var book in books)
            {
                if (book?.Chapters == null) continue;

                foreach (var chapter in book.Chapters)
                {
                    yield return chapter;
                }
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var collection = GetSampleAnthology();

            var logger = CreateDebugConsoleLogger<Program>();
            logger.LogInformation("Traverse with iterator:");

            foreach (var element in collection.IterateChapters())
            {
                logger.LogInformation(JsonSerializer.Serialize(element));
            }
        }

        private static ILogger<T> CreateDebugConsoleLogger<T>()
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug().AddConsole());
            var logger = loggerFactory.CreateLogger<T>();
            return logger;
        }

        private static IReadOnlyList<Book> GetSampleAnthology(int bookCount = 2, int chapterCount = 3)
        {
            var books = new List<Book>();
            foreach (var bookId in Enumerable.Range(1, bookCount))
            {
                var bookName = $"Book #{bookId}";
                var chapters = Enumerable.Range(1, chapterCount)
                    .Select(chapterNo => new Chapter(bookName, chapterNo)).ToList();
                var book = new Book(bookName, chapters);
                books.Add(book);
            }
            return books;
        }
    }
}
