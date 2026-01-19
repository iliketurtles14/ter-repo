using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System;

public class DumperStartStop : MonoBehaviour
{
    private Process dumper;
    public LoadingPanel loadScript;
    public MemoryMappedFileReader mmfrScript; // Assign in Inspector
    public CheckForDependencies dependenciesScript;
    public PrisonSelect prisonSelectScript;
    private bool hasLoaded; //this is for the initial load at the start of the game. if you are coming back to the main menu, this is why this is here.

    private void OnEnable()
    {
        if (!hasLoaded)
        {
            StartCoroutine(LoadAll());
        }
    }

    public IEnumerator LoadAll() //this is where it all starts...
    {
        while (true)
        {
            if (dependenciesScript.hasChecked)
            {
                break;
            }
            yield return null;
        }
        
        StartDumper();
        loadScript.LogLoad("Starting Dumper");
        StartCoroutine(mmfrScript.ReadDataFromMemory());

        while (true)
        {
            if (mmfrScript.canStopDumper)
            {
                break;
            }
            yield return null;
        }

        StopDumper();
        loadScript.LogLoad("Stopping Dumper");
        loadScript.LogLoad("Loading Prisons");
        prisonSelectScript.ReloadPrisons(false);
        hasLoaded = true;
    }

    private void StartDumper()
    {
        try
        {

            dumper = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Application.streamingAssetsPath + "/CTFAK/CTFAK.Cli.exe",
                    WorkingDirectory = Application.streamingAssetsPath + "/CTFAK",
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = true
                }
            };

            dumper.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log("Output: " + args.Data);
            dumper.ErrorDataReceived += (sender, args) => UnityEngine.Debug.LogError("Error: " + args.Data);

            dumper.Start();
            dumper.BeginOutputReadLine();
            dumper.BeginErrorReadLine();

        }
        catch (Exception ex)
        {
            loadScript.LogLoad("Failed to start CTFAK.Cli: " + ex.Message);
        }
    }
    private void StopDumper()
    {
        if (dumper != null && !dumper.HasExited)
        {
            dumper.Kill();
            dumper.WaitForExit();
            dumper.Dispose();
            dumper = null;
        }
    }

}
