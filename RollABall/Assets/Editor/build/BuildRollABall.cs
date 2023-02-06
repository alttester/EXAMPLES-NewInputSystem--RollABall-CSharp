using System;
using Altom.AltTester;
using Altom.AltTesterEditor;
using UnityEditor;
using UnityEngine;

class BuildRollABall
{
	public static void WindowsBuildForRollABallFromCommandLine(){
		WindowsBuildFromCommandLine(true,13000);
	}

	static void WindowsBuildFromCommandLine(bool withAltunity, int proxyPort = 13000)
  {
    SetPlayerSettings(false);

    Debug.Log("Starting Windows build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
    BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
    buildPlayerOptions.scenes = new string[]
    {
            "Assets/Scenes/MiniGame.unity"
    };
    if (withAltunity)
    {
      buildPlayerOptions.locationPathName = "RollABallWindowsTest/RollABall.exe";

    }
    else
    {
      buildPlayerOptions.locationPathName = "RollABallWindows/RollABall.exe";

    }
    buildPlayerOptions.target = BuildTarget.StandaloneWindows;
    buildPlayerOptions.targetGroup = BuildTargetGroup.Standalone;
    if (withAltunity)
    {
      buildPlayerOptions.options = BuildOptions.Development| BuildOptions.IncludeTestAssemblies;
    }
    BuildGame(buildPlayerOptions, withAltunity, proxyPort: proxyPort);

  }

  private static void SetPlayerSettings(bool customBuild)
  {
    PlayerSettings.companyName = "Altom";
    PlayerSettings.productName = "RollABall";
    PlayerSettings.bundleVersion = "1.0";
    PlayerSettings.resizableWindow = true;
    PlayerSettings.defaultScreenHeight = 900;
    PlayerSettings.defaultScreenWidth = 1200;
    PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
    PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);
    PlayerSettings.runInBackground = true;
  }

  static void BuildGame(BuildPlayerOptions buildPlayerOptions, bool withAltUnity, string proxyHost = null, int proxyPort = 13000)
  {
    try
    {
      if (withAltUnity)
      {
        AddAltUnity(buildPlayerOptions.targetGroup, buildPlayerOptions.scenes[0], proxyHost, proxyPort);
      }
      var results = BuildPipeline.BuildPlayer(buildPlayerOptions);

      if (withAltUnity)
      {
        RemoveAltUnity(buildPlayerOptions.targetGroup);
      }

      if (results.summary.totalErrors == 0 || results.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
      {
        Debug.Log("Build succeeded!");

      }
      else
      {
        Debug.LogError("Build failed! " + results.steps + "\n Result: " + results.summary.result + "\n Stripping info: " + results.strippingInfo);
        // EditorApplication.Exit(1);
      }

      Debug.Log("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
      // EditorApplication.Exit(0);
    }
    catch (Exception exception)
    {

      Debug.LogException(exception);
      // EditorApplication.Exit(1);
    }
  }

  static void AddAltUnity(BuildTargetGroup buildTargetGroup, string firstSceneName, string proxyHost = null, int proxyPort = 13000)
  {
    AltBuilder.PreviousScenePath = firstSceneName;
    var instrumentationSettings = new AltInstrumentationSettings();
    instrumentationSettings.ProxyPort = proxyPort;
    if (!string.IsNullOrEmpty(proxyHost)) instrumentationSettings.ProxyHost = proxyHost;
    AltBuilder.AddAltTesterInScriptingDefineSymbolsGroup(buildTargetGroup);
    AltBuilder.InsertAltInScene(firstSceneName, instrumentationSettings);
    Debug.Log("Instrumenting with proxyHost: " + proxyHost + ", proxyPort: " + proxyPort);

  }
  
  static void RemoveAltUnity(BuildTargetGroup buildTargetGroup)
  {
    AltBuilder.RemoveAltTesterFromScriptingDefineSymbols(buildTargetGroup);
  }
}