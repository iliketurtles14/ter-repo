using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using System;
using ImageMagick;

public class GetGivenData : MonoBehaviour
{
    public DataSender senderScript;

    private List<Texture2D> groundTextureList = new List<Texture2D>();
    public List<Sprite> groundList = new List<Sprite>();
    private List<Texture2D> tileTextureList = new List<Texture2D>();
    public List<Sprite> tileList = new List<Sprite>();
    public List<AudioClip> musicList = new List<AudioClip>();
    private string groundPath = "D:\\SteamLibrary\\steamapps\\common\\The Escapists\\Data\\images";
    private string musicPath = "D:\\SteamLibrary\\steamapps\\common\\The Escapists\\Music";
    private string tilePath = "D:\\SteamLibrary\\steamapps\\common\\The Escapists\\Data\\images\\custom";

    public List<Sprite> perksList = new List<Sprite>();
    public List<Sprite> stalagList = new List<Sprite>();
    public List<Sprite> shanktonList = new List<Sprite>();
    public List<Sprite> jungleList = new List<Sprite>();
    public List<Sprite> sanpanchoList = new List<Sprite>();
    public List<Sprite> hmpList = new List<Sprite>();

    public void Start()
    {
        LoadGroundTextures();
        LoadTileTextures();
        groundList = ConvertTexture2DListToSpriteList(groundTextureList);
        tileList = ConvertTexture2DListToSpriteList(tileTextureList);
        StartCoroutine(LoadMusicClips());

        // Separate the tiles into smaller sprites
        SeparateTilesIntoLists();

        senderScript.SetKnownLists(groundList, perksList, stalagList, shanktonList, jungleList, sanpanchoList, hmpList);
        senderScript.SetMusicList(musicList);
    }

    private void LoadGroundTextures()
    {
        string[] validFiles = new string[]
        {
            "ground_alca.gif", "ground_BC.gif", "ground_campepsilon.gif", "ground_CCL.gif",
            "ground_DTAF.gif", "ground_EA.gif", "ground_escapeteam.gif", "ground_fortbamford.gif",
            "ground_irongate.gif", "ground_jungle.gif", "ground_pcpen.gif", "ground_perks.gif",
            "ground_sanpancho.gif", "ground_shanktonstatepen.gif", "ground_SS.gif", "ground_stalagflucht.gif",
            "ground_TOL.gif", "ground_tutorial.gif", "soil.gif"
        };

        foreach (string file in Directory.GetFiles(groundPath))
        {
            if (System.Array.Exists(validFiles, element => file.EndsWith(element)))
            {
                try
                {
                    byte[] fileData = File.ReadAllBytes(file);
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

    private void LoadTileTextures()
    {
        string[] validFiles = new string[]
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
                    byte[] fileData = File.ReadAllBytes(filePath);
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

    private IEnumerator LoadMusicClips()
    {
        foreach (string file in Directory.GetFiles(musicPath))
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + file, AudioType.UNKNOWN))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    if (clip != null)
                    {
                        musicList.Add(clip);
                    }
                    else
                    {
                        Debug.LogError($"Failed to load audio clip from {file}");
                    }
                }
                else
                {
                    Debug.LogError($"Failed to load audio clip from {file}: {www.error}");
                }
            }
        }
    }

    private List<Sprite> ConvertTexture2DListToSpriteList(List<Texture2D> textures)
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach (Texture2D texture in textures)
        {
            Sprite sprite = Texture2DToSprite(texture);
            sprites.Add(sprite);
        }
        return sprites;
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        // Set the filter mode to Point (no filter) for pixel-perfect rendering
        texture.filterMode = FilterMode.Point;

        // Create the sprite with the appropriate pixels per unit
        float pixelsPerUnit = 100f; // Adjust this value based on your texture resolution and desired size
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
    }

    private void SeparateTilesIntoLists()
    {
        List<List<Sprite>> tileLists = new List<List<Sprite>> { perksList, stalagList, shanktonList, jungleList, sanpanchoList, hmpList };

        for (int i = 0; i < tileList.Count; i++)
        {
            Sprite tileSprite = tileList[i];
            Texture2D tileTexture = tileSprite.texture;

            for (int y = 0; y < tileTexture.height; y += 16)
            {
                for (int x = 0; x < tileTexture.width; x += 16)
                {
                    Rect rect = new Rect(x, y, 16, 16);
                    Sprite subSprite = Sprite.Create(tileTexture, rect, new Vector2(0.5f, 0.5f), 100f);
                    tileLists[i].Add(subSprite);
                }
            }
        }
    }
}