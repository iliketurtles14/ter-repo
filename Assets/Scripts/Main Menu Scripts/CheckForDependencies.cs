using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class CheckForDependencies : MonoBehaviour
{
    public bool hasChecked = false;
    private void Start()
    {
        CheckDependencies();
    }
    public void CheckDependencies()
    {
        //.net 6.0 stuff
        bool hasDesktop = false;
        bool hasRuntime = false;
        bool hasCore = false;

        try
        {
            var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "--list-runtimes";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(1000);

            hasDesktop = output.Contains("Microsoft.WindowsDesktop.App 6.");
            hasRuntime = output.Contains("Microsoft.NETCore.App 6.");
            hasCore = output.Contains("Microsoft.AspNetCore.App 6.");
        }
        catch { }

        //python 3
        bool hasPython = false;
        string whichPython = null;
        try
        {
            var process = new Process();
            process.StartInfo.FileName = "python";
            process.StartInfo.Arguments = "--version";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string output = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd(); //sometimes it writes to stderr

            process.WaitForExit(1000);

            var match = Regex.Match(output, @"Python\s+(\d+)\.(\d+)");
            if (match.Success)
            {
                int major = int.Parse(match.Groups[1].Value);
                int minor = int.Parse(match.Groups[2].Value);

                hasPython = major > 3 || (major == 3 && minor >= 4);

                if (hasPython)
                {
                    whichPython = "python";
                }
            }
        }
        catch { }

        if (!hasPython)
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = "python3";
                process.StartInfo.Arguments = "--version";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd(); //sometimes it writes to stderr

                process.WaitForExit(1000);

                var match = Regex.Match(output, @"Python\s+(\d+)\.(\d+)");
                if (match.Success)
                {
                    int major = int.Parse(match.Groups[1].Value);
                    int minor = int.Parse(match.Groups[2].Value);

                    hasPython = major > 3 || (major == 3 && minor >= 4);

                    if (hasPython)
                    {
                        whichPython = "python3";
                    }
                }
            }
            catch { }
        }

        //blowfish python package
        bool hasBlowfish = false;
        if (hasPython)
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = whichPython;
                process.StartInfo.Arguments = "-c \"import blowfish\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit(1000);

                hasBlowfish = process.ExitCode == 0;
            }
            catch { }
        }

        //escapists filepath
        bool hasEscapists = false;

        string pathToConfig = Path.Combine(Application.streamingAssetsPath, "CTFAK", "config.ini");
        string[] data = File.ReadAllLines(pathToConfig);
        string pathToEscapistsFolder = GetINIVar("Settings", "GameFolderPath", data).Trim('\"').Replace('/', '\\');
        string pathToEscapistsEXE = Path.Combine(pathToEscapistsFolder, "TheEscapists_eur.exe");
        if (File.Exists(pathToEscapistsEXE))
        {
            hasEscapists = true;
        }

        //escapists DLC
        bool hasAllDLC = false;

        string pathToAlca = Path.Combine(pathToEscapistsFolder, "Data", "Maps", "alca.map");
        string pathToDTAF = Path.Combine(pathToEscapistsFolder, "Data", "Maps", "DTAF.map");
        string pathToEA = Path.Combine(pathToEscapistsFolder, "Data", "Maps", "EA.map");
        string pathToEscapeTeam = Path.Combine(pathToEscapistsFolder, "Data", "Maps", "escapeteam.map");

        if(File.Exists(pathToEscapeTeam) && File.Exists(pathToAlca) && File.Exists(pathToDTAF) && File.Exists(pathToEA))
        {
            hasAllDLC = true;
        }

        //make the warning message
        string msg = "";

        string desktopMsg = "";
        string runtimeMsg = "";
        string coreMsg = "";
        string pythonMsg = "";
        string blowfishMsg = "";

        bool makeDependencyMsg = false;

        if (!hasDesktop)
        {
            desktopMsg = ".NET 6.0 Desktop Runtime";
            makeDependencyMsg = true;
        }
        if (!hasRuntime)
        {
            runtimeMsg = ".NET 6.0 Runtime";
            makeDependencyMsg = true;
        }
        if (!hasCore)
        {
            coreMsg = "ASP.NET Core Runtime 6.0";
            makeDependencyMsg = true;
        }
        if (!hasPython)
        {
            pythonMsg = "Python 3.4+";
            makeDependencyMsg = true;
        }
        if (!hasBlowfish)
        {
            blowfishMsg = "PyPI blowfish Package";
            makeDependencyMsg = true;
        }

        List<string> messages = new List<string>
        {
            desktopMsg, runtimeMsg, coreMsg, pythonMsg, blowfishMsg
        };

        if (makeDependencyMsg)
        {
            msg += "There are missing dependencies:\n";
            foreach(string message in messages)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    msg += "\t" + message + "\n";
                }
            }
            msg += "The game will not work properly without these.\n\n";
        }
        if (!hasEscapists)
        {
            msg += "The path for The Escapists is invalid.\n";
            msg += "The game will not work properly without this.\n\n";
        }
        if (!hasAllDLC && hasEscapists)
        {
            msg += "You do not have all DLC for The Escapists.\n";
            msg += "This will cause certain prisons and features of the Map Editor to break.";
        }
        if(!makeDependencyMsg && hasEscapists && hasAllDLC)
        {
            hasChecked = true;
        }
        else
        {
            GetComponent<Warnings>().CreateWarning(msg, transform.parent.Find("LoadingPanel"));
        }
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length; j++)
                {
                    if (file[j].Split('=')[0] == varName)
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }



        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');
        return parts[1];
    }

}
