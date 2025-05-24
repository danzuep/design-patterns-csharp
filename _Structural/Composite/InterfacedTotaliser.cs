// Inspired by https://dotnettutorials.net/lesson/composite-design-pattern/
namespace DesignPatterns.Structural.Composite.Totaliser
{
    /// <summary>
    /// The base Component class declares the common operations for both simple and complex objects.
    /// </summary>
    public interface ITotalComponent
    {
        int Total { get; }
    }

    public interface ITotalComposite : ITotalComponent
    {
        string Name { get; }
        void Add(ITotalComponent component);
        void Remove(ITotalComponent component);
    }

    /// <summary>
    /// The Child or Leaf component class represents a dangling object with no children.
    /// The Child or Leaf component is the object that does the actual work.
    /// </summary>
    public sealed class Child : ITotalComponent
    {
        int value;

        public Child(int value)
        {
            this.value = value;
        }

        public int Total => value;
    }

    /// <summary>
    /// The Parent or Composite class represents an object with children.
    /// </summary>
    public sealed class Parent : ITotalComposite
    {
        public string Name { get; set; }

        // All the child components of the composite object.
        IList<ITotalComponent> children = new List<ITotalComponent>();

        public Parent(string name)
        {
            this.Name = name;
        }

        public void Add(ITotalComponent component) =>
            children.Add(component);

        public void Remove(ITotalComponent component) =>
            children.Remove(component);

        public int Total =>
            children.Sum(x => x.Total);

        public override string ToString() =>
            $"The {Name} total is {Total}.";

        public static void Test()
        {
            // Create a tree structure
            var rng = new Random();
            Parent root = new Parent("Root");
            root.Add(new Child(rng.Next(10)));
            root.Add(new Child(rng.Next(10)));
            Parent comp = new Parent("Composite");
            comp.Add(new Child(rng.Next(10)));
            comp.Add(new Child(rng.Next(10)));
            root.Add(comp);
            root.Add(new Child(rng.Next(10)));
            // Add and remove a Child
            Child child = new Child(rng.Next(10));
            root.Add(child);
            root.Remove(child);
            // Display result
            Console.WriteLine(root);
            Console.WriteLine(comp);
            // Wait for user
            Console.ReadKey();
        }
    }
}
