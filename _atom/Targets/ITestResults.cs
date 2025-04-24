namespace Atom.Targets;

[TargetDefinition]
internal partial interface ITestRez : IDotnetTestHelper
{
    const string RezTestProjectName = "DecSm.Rez.UnitTests";

    Target TestRez =>
        d => d
            .WithDescription("Runs the DecSm.Rez.UnitTests tests")
            .ProducesArtifact(RezTestProjectName)
            .Executes(async () => await RunDotnetUnitTests(new(RezTestProjectName)));
}
