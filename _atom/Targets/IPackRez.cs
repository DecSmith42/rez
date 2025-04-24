namespace Atom.Targets;

[TargetDefinition]
internal partial interface IPackRez : IDotnetPackHelper
{
    const string RezProjectName = "DecSm.Rez";

    Target PackRez =>
        d => d
            .WithDescription("Builds the DecSm.Rez project into a NuGet package")
            .ProducesArtifact(RezProjectName)
            .Executes(async () => await DotnetPackProject(new(RezProjectName)));
}
