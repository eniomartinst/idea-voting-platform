using System.Runtime.CompilerServices;

// Exposes internal members of the Presentation layer to the test project.
// This is used to enable unit testing of internal components without exposing them publicly.
[assembly: InternalsVisibleTo("CisApi.Test.Presentation.RestApi")]