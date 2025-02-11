using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using System;
using ImageMagick;
using System.Threading.Tasks;

public class GetGivenData : MonoBehaviour
{
    public DataSender senderScript;

    public List<Texture2D> groundTextureList = new List<Texture2D>();
    public List<Texture2D> tileTextureList = new List<Texture2D>();
    public List<AudioClip> musicList = new List<AudioClip>();
    private string groundPath = "D:\\SteamLibrary\\steamapps\\common\\The Escapists\\Data\\images";
    private string musicPath = "D:\\SteamLibrary\\steamapps\\common\\The Escapists\\Music\\alca.ogg";
    private string tilePath = "D:\\SteamLibrary\\steamapps\\common\\The Escapists\\Data\\images\\custom";

    public async void Start()
    {
        await LoadGroundTextures();
        await LoadTileTextures();

        senderScript.SetMusicList(musicList);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartCoroutine(PlayAudioClipFromDisk(musicPath));
        }
    }

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
                        Debug.Log($"Successfully loaded texture from {file}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to load texture from {file}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception occurred while loading texture from {file}: {ex.Message}");
                }
            }
            else
            {
                Debug.Log($"File {file} is not a valid ground texture.");
            }
        }
    }

    private async Task LoadTileTextures()
    {
        HashSet<string> validFiles = new HashSet<string>
    {
        "tiles_cus_perks.gif", "tiles_cus_stalagflucht.gif", "tiles_cus_shanktonstatepen.gif",
        "tiles_cus_jungle.gif", "tiles_cus_sanpancho.gif", "tiles_cus_irongate.gif"
    };

        foreach (string validFile in validFiles)
        {
            string filePath = Path.Combine(tilePath, validFile);
            if (File.Exists(filePath))
            {
                try
                {
                    byte[] fileData = await File.ReadAllBytesAsync(filePath);
                    Texture2D texture = LoadGifAsTexture2D(fileData);
                    if (texture != null)
                    {
                        tileTextureList.Add(texture);
                        Debug.Log($"Successfully loaded texture from {filePath}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to load texture from {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception occurred while loading texture from {filePath}: {ex.Message}");
                }
            }
            else
            {
                Debug.Log($"File {filePath} does not exist.");
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
            if (texture.LoadImage(pngData))
            {
                // Replace white pixels with transparency
                ReplaceWhiteWithTransparency(texture);
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
}