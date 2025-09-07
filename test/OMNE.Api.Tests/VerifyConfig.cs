using System.Runtime.CompilerServices;

namespace OMNE.Api;

public static class VerifyConfig
{
    [Fact]
    public static Task Run() => VerifyChecks.Run();

    [ModuleInitializer]
    internal static void Initialize()
    {
        VerifierSettings.InitializePlugins();
        VerifierSettings.AutoVerify(includeBuildServer: false);
        VerifierSettings.UseSplitModeForUniqueDirectory();
    }
}
