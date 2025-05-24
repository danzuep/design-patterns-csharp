namespace DesignPatterns.Structural.Composite.GenericComponent
{
    /// <summary>
    /// The base Component class declares the common operations for both simple and complex objects.
    /// </summary>
    public interface IComponent
    {
        string? Name { get; }
        int Value { get; }
    }

    /// <summary>
    /// The Child or Leaf component class represents a dangling object with no children.
    /// The Child or Leaf component is the object that does the actual work.
    /// </summary>
    public class GenericComponent : IComponent
    {
        public virtual string? Name { get; set; }
        public virtual int Value { get; set; }
    }

    /// <summary>
    /// The Parent or Composite class represents an object with children.
    /// </summary>
    public sealed class CompositeInstance : GenericComponent
    {
        // All the child components of the composite object.
        private readonly IList<IComponent> children = new List<IComponent>();

        public void Add(IComponent component) =>
            children.Add(component);

        public void AddComponent(string? name, int value) =>
            children.Add(new GenericComponent { Name = name, Value = value });

        public void Remove(IComponent component) =>
            children.Remove(component);

        public override int Value =>
            children.Sum(x => x.Value);

        public override string ToString() =>
            $"The {Name} total is {Value}.";
    }

    public static class GenericComposite
    {
        public static void Test()
        {
            // Create a tree structure
            var rng = new Random();
            var root = new CompositeInstance { Name = "Root" };
            root.AddComponent("Leaf A", rng.Next(10));
            root.AddComponent("Leaf B", rng.Next(10));
            var comp = new CompositeInstance { Name = "Composite X" };
            comp.AddComponent("Leaf XA", rng.Next(10));
            comp.AddComponent("Leaf XB", rng.Next(10));
            root.Add(comp);
            root.AddComponent("Leaf C", rng.Next(10));
            // Display result
            Console.WriteLine(root);
            Console.WriteLine(comp);
            // Wait for user
            Console.ReadKey();
        }
    }
}
