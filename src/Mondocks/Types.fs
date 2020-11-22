namespace Mondocks.Types

type Collation =
    { Locale: string
      CaseLevel: Option<bool>
      CaseFirst: Option<bool>
      Strength: Option<bool>
      NumericOrdering: Option<bool>
      Alternate: Option<bool>
      MaxVariable: Option<bool>
      Backwards: Option<bool> }
