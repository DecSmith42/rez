namespace Atom;

[PublicAPI]
internal interface ITargets : IDotnetPackHelper, IDotnetTestHelper, INugetHelper, IGithubReleaseHelper, ISetupBuildInfo
{
    const string RezProjectName = "DecSm.Rez";
    const string RezConfigurationProjectName = "DecSm.Rez.Configuration";
    const string RezTestProjectName = "DecSm.Rez.UnitTests";

    [ParamDefinition("nuget-push-feed", "The Nuget feed to push to.", "https://api.nuget.org/v3/index.json")]
    string NugetFeed => GetParam(() => NugetFeed, "https://api.nuget.org/v3/index.json");

    [SecretDefinition("nuget-push-api-key", "The API key to use to push to Nuget.")]
    string? NugetApiKey => GetParam(() => NugetApiKey);

    Target PackRez =>
        d => d
            .DescribedAs("Builds the DecSm.Rez project into a NuGet package")
            .ProducesArtifact(RezProjectName)
            .Executes(async () => await DotnetPackProject(new(RezProjectName)));

    Target PackRezConfiguration =>
        d => d
            .DescribedAs("Builds the DecSm.Rez.Configuration project into a NuGet package")
            .ProducesArtifact(RezConfigurationProjectName)
            .Executes(async () => await DotnetPackProject(new(RezConfigurationProjectName)));

    Target TestRez =>
        d => d
            .DescribedAs("Runs the DecSm.Rez.UnitTests tests")
            .ProducesArtifact(RezTestProjectName)
            .Executes(async () =>
            {
                var exitCode = 0;

                exitCode += await RunDotnetUnitTests(new(RezTestProjectName));

                if (exitCode != 0)
                    throw new StepFailedException("One or more unit tests failed");
            });

    Target PushToNuget =>
        d => d
            .DescribedAs("Pushes the Atom projects to Nuget")
            .RequiresParam(nameof(NugetFeed))
            .RequiresParam(nameof(NugetApiKey))
            .ConsumesArtifact(nameof(PackRez), RezProjectName)
            .ConsumesArtifact(nameof(PackRezConfiguration), RezConfigurationProjectName)
            .Executes(async () =>
            {
                await PushProject(RezProjectName, NugetFeed, NugetApiKey!);
                await PushProject(RezConfigurationProjectName, NugetFeed, NugetApiKey!);
            });

    Target PushToRelease =>
        d => d
            .DescribedAs("Pushes the package to the release feed.")
            .RequiresParam(nameof(GithubToken))
            .ConsumesVariable(nameof(SetupBuildInfo), nameof(BuildVersion))
            .ConsumesArtifact(nameof(PackRez), RezProjectName)
            .ConsumesArtifact(nameof(PackRezConfiguration), RezConfigurationProjectName)
            .Executes(async () =>
            {
                if (BuildVersion.IsPreRelease)
                {
                    Logger.LogInformation("Skipping release push for pre-release version");

                    return;
                }

                var releaseTag = $"v{BuildVersion}";
                await UploadArtifactToRelease(RezProjectName, releaseTag);
                await UploadArtifactToRelease(RezConfigurationProjectName, releaseTag);
            });
}
