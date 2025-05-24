// See https://www.dofactory.com/net/composite-design-pattern
namespace DesignPatterns.Structural.Composite.AbstractComponent
{
    /// <summary>
    /// The 'Component' abstract class
    /// </summary>
    public abstract class AbstractComponent
    {
        protected string name;

        public AbstractComponent(string name)
        {
            this.name = name;
        }

        public abstract void Add(AbstractComponent c);
        public abstract void Remove(AbstractComponent c);
        public abstract void Display(int depth);
    }

    /// <summary>
    /// The 'Leaf' class
    /// </summary>
    public sealed class Leaf : AbstractComponent
    {
        public Leaf(string name)
            : base(name)
        {
        }

        public override void Add(AbstractComponent c)
        {
            Console.WriteLine("Cannot add to a leaf");
        }
        public override void Remove(AbstractComponent c)
        {
            Console.WriteLine("Cannot remove from a leaf");
        }
        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + name);
        }
    }

    /// <summary>
    /// The 'Composite' class
    /// </summary>
    public sealed class ConcreteComposite : AbstractComponent
    {
        List<AbstractComponent> children = new List<AbstractComponent>();

        public ConcreteComposite(string name)
            : base(name)
        {
        }

        public override void Add(AbstractComponent component)
        {
            children.Add(component);
        }

        public override void Remove(AbstractComponent component)
        {
            children.Remove(component);
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + name);

            // Recursively display child nodes
            foreach (AbstractComponent component in children)
            {
                component.Display(depth + 2);
            }
        }

        public static void Test()
        {
            // Create a tree structure
            var root = new ConcreteComposite("root");
            root.Add(new Leaf("Leaf A"));
            root.Add(new Leaf("Leaf B"));
            var comp = new ConcreteComposite("Composite X");
            comp.Add(new Leaf("Leaf XA"));
            comp.Add(new Leaf("Leaf XB"));
            root.Add(comp);
            root.Add(new Leaf("Leaf C"));
            // Add and remove a leaf
            var leaf = new Leaf("Leaf D");
            root.Add(leaf);
            root.Remove(leaf);
            // Recursively display tree
            root.Display(1);
            // Wait for user
            Console.ReadKey();
        }
    }
}
