namespace DesignPatterns.Structural.Composite.GenericComponent
{
    public abstract class NamedComponent
    {
        public NamedComponent(string? name)
        {
            Name = name;
        }

        public string? Name { get; set; }
    }

    /// <summary>
    /// The Child or Leaf component class represents a dangling object with no children.
    /// The Child or Leaf component is the object that does the actual work.
    /// </summary>
    public sealed class Component : NamedComponent, IComponent
    {
        public int Value { get; set; }

        public Component(string? name, int value)
            : base(name)
        {
            Value = value;
        }
    }

    /// <summary>
    /// The Parent or Composite class represents an object with children.
    /// </summary>
    public class Composite : NamedComponent, IComponent
    {
        public Composite(string? name, IList<IComponent>? components = null)
            : base(name)
        {
            if (components != null)
            {
                foreach (var component in components)
                {
                    if (component != null)
                    {
                        components.Add(component);
                    }
                }
            }
        }

        private Composite(string? name, IComponent? component)
            : this(name, component != null ? new IComponent[] { component } : null)
        {
        }

        // All the child components of the composite object.
        public List<IComponent> Components { get; private set; } = new();

        public void Add(IComponent component) =>
            Components.Add(component);

        public void AddComponent(string? name, int value) =>
            Components.Add(new Component(name, value));

        public void AddComposite(string? name, IList<IComponent>? components = null) =>
            Components.Add(new Composite(name, components));

        public void AddComposite(string? name, IComponent? component = null) =>
            Components.Add(new Composite(name, component));

        public void Remove(IComponent component) =>
            Components.Remove(component);

        public int Value =>
            Components.Sum(x => x.Value);

        public override string ToString() =>
            $"The {Name} total is {Value}.";
    }
}
