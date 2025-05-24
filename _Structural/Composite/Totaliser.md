### Composite Design Pattern (Structural)

#### Diagram

```plantuml
frame "Component Design Pattern" {
  skinparam linetype ortho
  interface "ITotalComponent" as IComponent {
    + int Total { get; }
  }
  interface "ITotalComposite" as IComposite {
    + string Name { get; }
    .. Methods ..
    + void Add(ITotalComponent component);
    + void Remove(ITotalComponent component);
  }
  Client .r.> IComponent
  IComposite --|> IComponent
  IComponent <..o IComposite : Children
}
```

```plantuml
title Composite Design Pattern Implementation
skinparam componentStyle rectangle
skinparam linetype polyline
component Client
component IComposite
component IComponent
component Parent
component Child
Client .r.> IComponent
IComposite -l-|> IComponent
IComposite <|-- Parent
IComponent <|-- Child
Parent o-l-> Child : Children
```

```plantuml
title Sequence Diagram
participant Client as c
participant "Composite" as s
participant Component as b
group Component
c -> b : Total
c <- b : Value
end
group Composite
c -> s : Total
  s -> b : Total
  s <- b
  s -> b : Total
  s <- b
c <- s : Sum
end
```

```plantuml
title Component Types
enum "ComponentType" {
  Node,
  Root
}
```

<!--
To display PlantUML previews in Visual Studio Code:  
 1. Extensions (Ctrl+Shift+X) -> Install "PlantUML".
 2. Click the gear icon -> Extension Settings.
 3. "plantuml.server": "https://kroki.example.com/plantuml".
 4. Ctrl+Shift+V to view.
-->