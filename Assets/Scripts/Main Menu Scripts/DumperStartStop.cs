using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Net;
using UnityEngine.UI;

public class DumperStartStop : MonoBehaviour
{
    private Process dumper;
    public LoadingPanel loadScript;
    public MemoryMappedFileReader mmfrScript;
    public CheckForDependencies dependenciesScript;
    public PrisonSelect prisonSelectScript;
    private ApplyMainMenuData applyScript;
    public Transform mmc;
    public bool isGoingToMainMenu; //for quitting from a prison (this is set in Pause.cs when quitting a prison)

    private void Awake()
    {
        applyScript = GetComponent<ApplyMainMenuData>();
        
        StartCoroutine(LoadAll());
    }
    private void Update()
    {
        if (isGoingToMainMenu)
        {
            isGoingToMainMenu = false;
            StartCoroutine(ReloadMainMenu());
        }
    }

    private IEnumerator ReloadMainMenu() //for quitting from a prison
    {
        //get mmc (waiting until the main menu scene is actually loaded
        while (true)
        {
            yield return null;

            Scene mainMenuScene = SceneManager.GetSceneByName("Main Menu");
            foreach (var root in mainMenuScene.GetRootGameObjects())
            {
                if (root.name == "MainMenuCanvas")
                {
                    mmc = root.transform;
                    break;
                }
            }

            if (mmc == null)
            {
                continue;
            }
            else
            {
                break;
            }
        }

        prisonSelectScript = mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>();

        //disable opening sequences
        mmc.Find("LoadingPanel").gameObject.SetActive(false);
        mmc.Find("WarningPanel").gameObject.SetActive(false);
        mmc.Find("WarningBlack").gameObject.SetActive(false);
        mmc.Find("LogoPanel").gameObject.SetActive(false);

        //enable titlescreen buttons
        mmc.Find("TitlePanel").Find("PlayButton").GetComponent<Button>().enabled = true;
        mmc.Find("TitlePanel").Find("OptionsButton").GetComponent<Button>().enabled = true;
        mmc.Find("TitlePanel").Find("MapEditorButton").GetComponent<Button>().enabled = true;

        //do textures and prisons
        applyScript.LoadImages();
        prisonSelectScript.ReloadPrisons(false);
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
