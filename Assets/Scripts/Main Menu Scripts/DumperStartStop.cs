using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System;

public class DumperStartStop : MonoBehaviour
{
    private Process dumper;
    public MemoryMappedFileReader mmfrScript; // Assign in Inspector

    private void OnEnable()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        StartDumper();
        UnityEngine.Debug.Log("Start Wait");
        yield return new WaitForSecondsRealtime(5); // Wait for the process to initialize
        UnityEngine.Debug.Log("Start Read");
        mmfrScript.ReadDataFromMemory();
        yield return new WaitForSecondsRealtime(5);
        StopDumper();
    }

    private void StartDumper()
    {
        try
        {
            UnityEngine.Debug.Log("Attempting to start CTFAK.Cli...");

            dumper = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Users\\creep\\OneDrive\\Desktop\\CTFAK\\CTFAK.Cli.exe",
                    WorkingDirectory = "C:\\Users\\creep\\OneDrive\\Desktop\\CTFAK",
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = false
                }
            };

            dumper.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log("Output: " + args.Data);
            dumper.ErrorDataReceived += (sender, args) => UnityEngine.Debug.LogError("Error: " + args.Data);

            dumper.Start();
            dumper.BeginOutputReadLine();
            dumper.BeginErrorReadLine();

            UnityEngine.Debug.Log("CTFAK.Cli started successfully!");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Failed to start CTFAK.Cli: " + ex.Message);
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
            UnityEngine.Debug.Log("CTFAK.Cli process stopped.");
        }
    }

}
