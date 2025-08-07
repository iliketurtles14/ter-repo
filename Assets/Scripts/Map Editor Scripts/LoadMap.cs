using SFB;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class LoadMap : MonoBehaviour
{
    public Transform uic;
    
    public AudioClip music = null;
    public Sprite tiles = null;
    public Sprite ground = null;
    public Sprite icon = null;
    public string[] data = null;

    public Sprite checkedBox;
    public Sprite uncheckedBox;
    public void StartLoad()
    {
        ExtensionFilter[] extensions = new ExtensionFilter[]
        {
            new ExtensionFilter("Map Files", "zmap")
        };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a map file.", "", extensions, false);
        if(paths.Length < 0) //if nothign was selected
        {
            return;
        }

        //unzip files
        string extractPath = Path.Combine(Application.streamingAssetsPath, "Prisons", "CustomPrisons") + Path.DirectorySeparatorChar;

        ZipFile.ExtractToDirectory(paths[0], extractPath);

        //load Data.ini to a string[]
        data = File.ReadAllLines(Path.Combine(extractPath, "Data.ini"));

        //load other files to memory
        if(File.Exists(Path.Combine(extractPath, "Tiles.png")))
        {
            tiles = ConvertPNGToSprite(Path.Combine(extractPath, "Tiles.png"));
            File.Delete(Path.Combine(extractPath, "Tiles.png"));
        }
        if(File.Exists(Path.Combine(extractPath, "Ground.png")))
        {
            ground = ConvertPNGToSprite(Path.Combine(extractPath, "Ground.png"));
            File.Delete(Path.Combine(extractPath, "Ground.png"));
        }
        if(File.Exists(Path.Combine(extractPath, "Icon.png")))
        {
            icon = ConvertPNGToSprite(Path.Combine(extractPath, "Icon.png"));
            File.Delete(Path.Combine(extractPath, "Icon.png"));
        }
        if (File.Exists(Path.Combine(extractPath, "Music.mp3")))
        {
            ConvertMP3ToAudioClip("file://" + Path.Combine(extractPath, "Music.mp3"));
            File.Delete(Path.Combine(extractPath, "Music.mp3"));
        }

        LoadProperties();
        LoadTiles();
    }
    private Sprite ConvertPNGToSprite(string path)
    {
        byte[] pngBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(pngBytes);

        texture.filterMode = FilterMode.Point;

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100.0f);
    }
    private IEnumerator ConvertMP3ToAudioClip(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading audio: " + www.error);
            }
            else
            {
                music = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }
    private void LoadProperties()
    {
        Transform properties = uic.Find("PropertiesPanel");

        properties.Find("NameInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "MapName", data);
        properties.Find("GuardsNum").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Guards", data);
        properties.Find("InmatesNum").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Inmates", data);
        properties.Find("NPCLevelNum").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "NPCLevel", data);
        properties.Find("TilesetResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Tileset", data);
        properties.Find("GroundResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Ground", data);
        properties.Find("MusicResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Music", data);
        properties.Find("IconResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Icon", data);
        properties.Find("GroundsResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Grounds", data);
        properties.Find("SizeResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Size", data);
        uic.Find("NotePanel").Find("NoteInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = Regex.Unescape(GetINIVar("Properties", "Note", data));
        uic.Find("NotePanel").Find("WardenInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Warden", data);
        Transform routineInputGrid1 = uic.Find("RoutinePanel").Find("RoutineInputGrid1");
        for(int i = 0; i <= 11; i++)
        {
            string time = i.ToString();
            if(time.Length == 1)
            {
                time = "0" + time;
            }

            routineInputGrid1.Find(time + ":00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", time, data);
        }
        Transform routineInputGrid2 = uic.Find("RoutinePanel").Find("RoutineInputGrid2");
        for(int i = 12; i <= 23; i++)
        {
            string time = i.ToString();

            routineInputGrid2.Find(time + ":00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = GetINIVar("Routine", time, data);
        }
        Transform hintPanel = uic.Find("HintPanel");
        hintPanel.Find("Hint1Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = Regex.Unescape(GetINIVar("Properties", "Hint1", data));
        hintPanel.Find("Hint2Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = Regex.Unescape(GetINIVar("Properties", "Hint2", data));
        hintPanel.Find("Hint3Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = Regex.Unescape(GetINIVar("Properties", "Hint3", data));
        Transform jobPanel = uic.Find("JobPanel");
        jobPanel.Find("StartingJobInput").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = GetINIVar("Jobs", "StartingJob", data);
        Transform checkBoxGrid1 = jobPanel.Find("CheckBoxGrid1");
        Transform checkBoxGrid2 = jobPanel.Find("CheckBoxGrid2");
        foreach(Transform box in checkBoxGrid1)
        {
            if(GetINIVar("Jobs", box.name.Replace("CheckBox", ""), data) == "True")
            {
                box.GetComponent<Image>().sprite = checkedBox;
            }
            else if(GetINIVar("Jobs", box.name.Replace("CheckBox", ""), data) == "False")
            {
                box.GetComponent<Image>().sprite = uncheckedBox;
            }
        }
        foreach(Transform box in checkBoxGrid2)
        {
            if (GetINIVar("Jobs", box.name.Replace("CheckBox", ""), data) == "True")
            {
                box.GetComponent<Image>().sprite = checkedBox;
            }
            else if (GetINIVar("Jobs", box.name.Replace("CheckBox", ""), data) == "False")
            {
                box.GetComponent<Image>().sprite = uncheckedBox;
            }
        }
        Transform extrasPanel = uic.Find("ExtrasPanel");
        if (GetINIVar("Properties", "Snowing", data) == "True")
        {
            extrasPanel.Find("SnowingCheckbox").GetComponent<Image>().sprite = checkedBox;
        }
        else
        {
            extrasPanel.Find("SnowingCheckbox").GetComponent<Image>().sprite = uncheckedBox;
        }
        if (GetINIVar("Properties", "POWOutfits", data) == "True")
        {
            extrasPanel.Find("POWCheckbox").GetComponent<Image>().sprite = checkedBox;
        }
        else
        {
            extrasPanel.Find("POWCheckbox").GetComponent<Image>().sprite = uncheckedBox;
        }
        if (GetINIVar("Properties", "StunRods", data) == "True")
        {
            extrasPanel.Find("StunRodCheckbox").GetComponent<Image>().sprite = checkedBox;
        }
        else
        {
            extrasPanel.Find("StunRodCheckbox").GetComponent<Image>().sprite = uncheckedBox;
        }
    }
    private void LoadTiles()
    {

    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;
        
        for(int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for(int j = i; j < file.Length - i; j++)
                {
                    if (file[j].Contains(varName))
                    {
                        line = file[j];
                    }
                }
            }
        }

        if(line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');

        return parts[1];
    }
}
