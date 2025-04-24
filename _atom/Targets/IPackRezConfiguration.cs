namespace Atom.Targets;

[TargetDefinition]
internal partial interface IPackRezConfiguration : IDotnetPackHelper
{
    const string RezConfigurationProjectName = "DecSm.Rez.Configuration";

    Target PackRezConfiguration =>
        d => d
            .WithDescription("Builds the DecSm.Rez.Configuration project into a NuGet package")
            .ProducesArtifact(RezConfigurationProjectName)
            .Executes(async () => await DotnetPackProject(new(RezConfigurationProjectName)));
}
