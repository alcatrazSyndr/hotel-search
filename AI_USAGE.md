# Use of AI Tools

AI assistance (Claude) was used throughout the development of this project. This document describes specifically how and where it was used, in the interest of transparency.

## Where AI was used

**Applying Clean Architecture / Domain-Driven Design patterns.** My prior professional experience is primarily in Unity/C#, with limited hands-on experience with ASP.NET Core, layered architecture, and Domain-Driven Design concepts (entities vs. value objects, dependency inversion, the responsibilities of each architectural layer). I used AI assistance to apply them throughout the solution's structure.

**Designing the search ranking algorithm.** The approach of using min-max normalization to make price and distance comparable on the same scale, then combining them into a single weighted score, was designed with AI assistance after discussing the problem (combining two differently-scaled measurements fairly) and working through the reasoning for why naive approaches (e.g. directly adding raw price and distance) don't work correctly.

**Designing the search parser Regex patterns.** AI made it simple for me to implement the text matching from the users prompt for extracting relevant data needed for getting good results on the search endpoint.

**Learning and implementing global exception handling middleware.** During manual testing, I discovered that invalid input (e.g. out-of-range coordinates) was surfacing as an unhandled `500 Internal Server Error` rather than a clean, informative response. I used AI assistance to learn about ASP.NET Core middleware and implement a exception handling middleware to address this.

**Code review and debugging.** Throughout development, my code was reviewed periodically for bugs and correctness. This surfaced several real issues I had introduced, including an operator precedence bug in a normalization formula, an off-by-one error in a manual pagination calculation, and a bug caused by not realizing that tuples in C# are value types — mutating a local copy of a tuple retrieved from a list does not update the list itself. I found AI to be incredibly useful here as it ensured a smooth developement resulting in less time spent debugging and fixing.

**General C#/.NET syntax and library guidance** — for example, explanation of certain LINQ methods (`.Where()`, `.Select()`, `.Skip()`, `.Take()`, `.Min()`, `.Max()`) and the discussion on when and when not to use them, `Nullable<T>` semantics and `ConcurrentDictionary` thread-safety.

**Generating reference data.** The list of known city names and coordinates used for location matching was generated with AI assistance to save time, rather than manually looking up coordinates for each city.

**Learning to write unit tests.** I had never written unit tests before this project. I used AI assistance to learn the Arrange-Act-Assert pattern and xUnit basics (`[Fact]`, `Assert.Equal`, `Assert.InRange`, `Record.Exception`), and wrote a small set of tests covering `GeoLocation`'s distance calculation and coordinate validation.

**Documentation.** The README and the AI usage documentation were drafted with AI assistance based on the actual implementation and decisions made throughout the project, then reviewed and edited by me.