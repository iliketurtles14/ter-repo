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

    public static GetGivenData instance { get; private set; }

    public async void Start()
    {
        //load from ini file
        string configPath = Path.Combine(Application.streamingAssetsPath, "CTFAK", "config.ini");
        IniFile iniFile = new IniFile(configPath);
        mainPath = iniFile.Read("GameFolderPath", "Settings");
        groundPath = mainPath + "/Data/images";
        tilePath = mainPath + "/Data/images";

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
        "ground_alca.gif", "ground_BC.gif", "ground_campepsilon.gif", "ground_CCL.gif",
        "ground_DTAF.gif", "ground_EA.gif", "ground_escapeteam.gif", "ground_fortbamford.gif",
        "ground_irongate.gif", "ground_jungle.gif", "ground_pcpen.gif", "ground_perks.gif",
        "ground_sanpancho.gif", "ground_shanktonstatepen.gif", "ground_SS.gif", "ground_stalagflucht.gif",
        "ground_TOL.gif", "ground_tutorial.gif", "soil.gif"
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
    }

    private async Task LoadTileTextures()
    {
        HashSet<string> validFiles = new HashSet<string>
    {
        "tiles_alca.gif", "tiles_BC.gif", "tiles_campepsilon.gif", "tiles_CCL.gif",
        "tiles_DTAF.gif", "tiles_EA.gif", "tiles_escapeteam.gif", "tiles_fortbamford.gif",
        "tiles_irongate.gif", "tiles_jungle.gif", "tiles_pcpen.gif", "tiles_perks.gif",
        "tiles_sanpancho.gif", "tiles_shanktonstatepen.gif", "tiles_SS.gif", "tiles_stalagflucht.gif",
        "tiles_TOL.gif", "tiles_tutorial.gif"
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

    private void ReplaceWhiteWithTransparency(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r > 0.9f && pixels[i].g > 0.9f && pixels[i].b > 0.9f)
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