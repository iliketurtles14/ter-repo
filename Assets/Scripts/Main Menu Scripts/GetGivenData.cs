using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using System;
using ImageMagick;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class GetGivenData : MonoBehaviour
{
    public DataSender senderScript;
    public LoadingPanel loadScript;
    public TileDecrypt tileDecryptScript;

    public List<Texture2D> groundTextureList = new List<Texture2D>();
    public List<Texture2D> tileTextureList = new List<Texture2D>();
    public List<AudioClip> musicList = new List<AudioClip>();
    private string groundPath;
    //private string musicPath = "D:\\SteamLibrary\\steamapps\\common\\The Escapists\\Music\\alca.ogg";
    private string tilePath;
    private string mainPath;
    private string blackGroundPath;

    public static GetGivenData instance { get; private set; }

    public async void Start()
    {
        //load from ini file
        string configPath = Path.Combine(Application.streamingAssetsPath, "CTFAK", "config.ini");
        IniFile iniFile = new IniFile(configPath);
        mainPath = iniFile.Read("GameFolderPath", "Settings");
        groundPath = mainPath + "/Data/images";
        tilePath = mainPath + "/Data/images";
        blackGroundPath = mainPath + "/Data/images/custom/ground_cus_black.gif";

        await LoadGroundTextures();
        await LoadTileTextures();

        //senderScript.SetMusicList(musicList);

    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F1))
    //    {
    //        StartCoroutine(PlayAudioClipFromDisk(musicPath));
    //    }
    //}

    private async Task LoadGroundTextures()
    {
        HashSet<string> validFiles = new HashSet<string>
    {
        "ground_tutorial.gif", "ground_perks.gif", "ground_stalagflucht.gif", "ground_shanktonstatepen.gif",
        "ground_jungle.gif", "ground_sanpancho.gif", "ground_irongate.gif", "ground_CCL.gif",
        "ground_BC.gif", "ground_TOL.gif", "ground_pcpen.gif", "ground_SS.gif",
        "ground_DTAF.gif", "ground_escapeteam.gif", "ground_alca.gif", "ground_EA.gif",
        "ground_campepsilon.gif", "ground_fortbamford.gif", "soil.gif"
    };

        foreach (string file in Directory.GetFiles(groundPath))
        {
            string fileName = Path.GetFileName(file);
            if (validFiles.Contains(fileName))
            {
                try
                {
                    byte[] fileData = await File.ReadAllBytesAsync(file);
                    Texture2D texture = LoadGifAsTexture2D(fileData);
                    if (texture != null)
                    {
                        groundTextureList.Add(texture);
                        loadScript.LogLoad($"Successfully loaded texture from {file}");
                    }
                    else
                    {
                        loadScript.LogLoad($"Failed to load texture from {file}");
                    }
                }
                catch (Exception ex)
                {
                    loadScript.LogLoad($"Exception occurred while loading texture from {file}: {ex.Message}");
                }
            }
            else
            {
                loadScript.LogLoad($"File {file} is not a valid ground texture.");
            }
        }

        byte[] blackGroundData = await File.ReadAllBytesAsync(blackGroundPath);
        Texture2D blackTexture = LoadGifAsTexture2D(blackGroundData);
        groundTextureList.Add(blackTexture);
    }

    private async Task LoadTileTextures()
    {
        HashSet<string> validFiles = new HashSet<string>
        {
            "tiles_alca.gif", "tiles_BC.gif", "tiles_campepsilon.gif", "tiles_CCL.gif",
            "tiles_DTAF.gif", "tiles_EA.gif", "tiles_escapeteam.gif", "tiles_fortbamford.gif",
            "tiles_pcpen.gif", "tiles_SS.gif", "tiles_TOL.gif", "tiles_tutorial.gif"
        };

        foreach (string validFile in validFiles)
        {
            //decrypt
            tileDecryptScript.DecryptTileset(Path.Combine(tilePath, validFile));
            loadScript.LogLoad($"Successfully decrypted {validFile}");

            string encryptedPath = Path.Combine(tilePath, validFile);
            string[] parts = encryptedPath.Split('.');
            string part1 = parts[0] + "_decr.";
            string part2 = parts[1];
            string decryptedPath = part1 + part2;

            string filePath = decryptedPath;
            if (File.Exists(filePath))
            {
                try
                {
                    byte[] fileData = await File.ReadAllBytesAsync(filePath);
                    Texture2D texture = LoadGifAsTexture2D(fileData);
                    if (texture != null)
                    {
                        ReplaceColorWithTransparency(texture, Color.white, 0.0001f);
                        if(validFile == "tiles_DTAF.gif")
                        {
                            ReplaceColorWithTransparency(texture, new Color(48f / 255f, 1f, 0f));
                        }
                        tileTextureList.Add(texture);
                        loadScript.LogLoad($"Successfully loaded texture from {filePath}");

                        File.Delete(filePath);
                    }
                    else
                    {
                        loadScript.LogLoad($"Failed to load texture from {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    loadScript.LogLoad($"Exception occurred while loading texture from {filePath}: {ex.Message}");
                }
            }
            else
            {
                loadScript.LogLoad($"File {filePath} does not exist.");
            }
        }

        //load the custom versions of main maps
        HashSet<string> customFiles = new HashSet<string>
        {
            "tiles_cus_irongate.gif", "tiles_cus_jungle.gif", "tiles_cus_perks.gif", "tiles_cus_sanpancho.gif",
            "tiles_cus_shanktonstatepen.gif", "tiles_cus_stalagflucht.gif"
        };
        foreach(string customFile in customFiles)
        {
            string filePath = Path.Combine(tilePath, "custom", customFile);
            if (File.Exists(filePath))
            {
                try
                {
                    byte[] fileData = await File.ReadAllBytesAsync(filePath);
                    Texture2D texture = LoadGifAsTexture2D(fileData);
                    if (texture != null)
                    {
                        ReplaceColorWithTransparency(texture, Color.white, 0.0001f);
                        tileTextureList.Add(texture);
                        loadScript.LogLoad($"Successfully loaded texture from {filePath}");
                    }
                    else
                    {
                        loadScript.LogLoad($"Failed to load texture from {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    loadScript.LogLoad($"Exception occurred while loading texture from {filePath}: {ex.Message}");
                }
            }
            else
            {
                loadScript.LogLoad($"File {filePath} does not exist.");
            }

        }

        //reorder the texture list to fit with the list laid out in discord
        List<Texture2D> tempList = tileTextureList;
        tileTextureList[0] = tempList[11];
        tileTextureList[1] = tempList[14];
        tileTextureList[2] = tempList[17];
        tileTextureList[3] = tempList[16];
        tileTextureList[4] = tempList[13];
        tileTextureList[5] = tempList[15];
        tileTextureList[6] = tempList[12];
        tileTextureList[7] = tempList[3];
        tileTextureList[8] = tempList[1];
        tileTextureList[9] = tempList[10];
        tileTextureList[10] = tempList[8];
        tileTextureList[11] = tempList[9];
        tileTextureList[12] = tempList[4];
        tileTextureList[13] = tempList[6];
        tileTextureList[14] = tempList[0];
        tileTextureList[15] = tempList[5];
        tileTextureList[16] = tempList[2];
        tileTextureList[17] = tempList[7];
    }
    private Texture2D LoadGifAsTexture2D(byte[] fileData)
    {
        using (MagickImageCollection collection = new MagickImageCollection(fileData))
        {
            // Get the first frame of the GIF
            IMagickImage<ushort> firstFrame = collection[0];

            // Convert the first frame to PNG format
            firstFrame.Format = MagickFormat.Png;

            // Get the byte array of the PNG image
            byte[] pngData = firstFrame.ToByteArray();

            // Load the PNG data into a Texture2D
            Texture2D texture = new Texture2D(2, 2);

            texture.filterMode = FilterMode.Point;
            if (texture.LoadImage(pngData))
            {
                // Replace white pixels with transparency
                //ReplaceWhiteWithTransparency(texture);
                return texture;
            }
            else
            {
                return null;
            }
        }
    }

    private void ReplaceColorWithTransparency(Texture2D texture, Color color, float tolerance = 0.01f)
    {
        Color[] pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            if (Mathf.Abs(pixels[i].r - color.r) < tolerance &&
                Mathf.Abs(pixels[i].g - color.g) < tolerance &&
                Mathf.Abs(pixels[i].b - color.b) < tolerance)
            {
                pixels[i] = new Color(pixels[i].r, pixels[i].g, pixels[i].b, 0);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
    }
    private IEnumerator PlayAudioClipFromDisk(string filePath)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.clip = clip;
                    audioSource.Play();
                    Debug.Log($"Playing audio clip from {filePath}");
                }
                else
                {
                    Debug.LogError($"Failed to load audio clip from {filePath}");
                }
            }
            else
            {
                Debug.LogError($"Failed to load audio clip from {filePath}: {www.error}");
            }
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}