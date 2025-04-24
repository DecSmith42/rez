namespace Atom;

[BuildDefinition]
[GenerateEntryPoint]
internal partial class Build : DefaultBuildDefinition,
    IGithubWorkflows,
    IGitVersion,
    IPackRez,
    IPackRezConfiguration,
    ITestRez,
    IPushToNuget,
    IPushToRelease
{
    public override IReadOnlyList<IWorkflowOption> DefaultWorkflowOptions =>
    [
        UseGitVersionForBuildId.Enabled, new SetupDotnetStep("9.0.x"),
    ];

    public override IReadOnlyList<WorkflowDefinition> Workflows =>
    [
        new("Validate")
        {
            Triggers = [GitPullRequestTrigger.IntoMain, ManualTrigger.Empty],
            StepDefinitions =
            [
                Commands.SetupBuildInfo,
                Commands.PackRez.WithSuppressedArtifactPublishing,
                Commands.PackRezConfiguration.WithSuppressedArtifactPublishing,
                Commands.TestRez,
            ],
            WorkflowTypes = [Github.WorkflowType],
        },
        new("Build")
        {
            Triggers = [GitPushTrigger.ToMain, GithubReleaseTrigger.OnReleased, ManualTrigger.Empty],
            StepDefinitions =
            [
                Commands.SetupBuildInfo,
                Commands.PackRez,
                Commands.PackRezConfiguration,
                Commands.TestRez,
                Commands.PushToNuget.WithAddedOptions(WorkflowSecretInjection.Create(Params.NugetApiKey)),
                Commands.PushToRelease.WithGithubTokenInjection(),
            ],
            WorkflowTypes = [Github.WorkflowType],
        },
        Github.DependabotDefaultWorkflow(),
    ];
}
