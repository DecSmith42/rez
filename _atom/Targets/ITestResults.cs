namespace Atom.Targets;

[TargetDefinition]
internal partial interface ITestRez : IDotnetTestHelper
{
    const string RezTestProjectName = "DecSm.Rez.UnitTests";

    Target TestRez =>
        d => d
            .WithDescription("Runs the DecSm.Rez.UnitTests tests")
            .ProducesArtifact(RezTestProjectName)
            .Executes(async () =>
            {
                var exitCode = 0;

                exitCode += await RunDotnetUnitTests(new(RezTestProjectName));

                if (exitCode != 0)
                    throw new StepFailedException("One or more unit tests failed");
            });
}
