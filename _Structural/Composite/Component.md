### Composite Design Pattern (Structural)

#### Diagram

```plantuml
@startuml
allow_mixing
title Composite Design Pattern Implementation
skinparam componentStyle rectangle
frame "Component Design Pattern" {
  interface "Component" as IComponent {
    + string Name { get; }
    .. Methods ..
    + void Add(Component component);
    + void Remove(Component component);
    + void Display(int depth);
  }
  component Composite {
  }
  component Leaf {
  }
  Client .r.> IComponent
  IComponent <|-- Composite
  IComponent <|-- Leaf
  Leaf <-l-o Composite : Children
}
@enduml
```

```plantuml
title State Diagram
state Active : Name
[*] --> Active : Add
Active --> [*] : Remove
```

<!--
To display PlantUML previews in Visual Studio Code:  
 1. Extensions (Ctrl+Shift+X) -> Install "PlantUML".
 2. Click the gear icon -> Extension Settings.
 3. "plantuml.server": "https://kroki.example.com/plantuml".
 4. Ctrl+Shift+V to view.
-->