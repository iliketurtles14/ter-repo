using System;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Net;
using UnityEngine.Audio;
public class MemoryMappedFileReader : MonoBehaviour
{
    public string mapName = "ClickteamMemoryMap";

    // Public lists to store images and sounds
    public List<Texture2D> ImageList = new List<Texture2D>();
    public List<byte[]> ByteSoundList = new List<byte[]>();
    public List<AudioClip> SoundList = new List<AudioClip>();



    public void ReadDataFromMemory()
    {
        using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(mapName))
        {
            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
            {
                byte[] buffer = new byte[1024 * 1024 * 10]; // 10 MB buffer
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("Received Data: " + jsonData);

                ParseData(jsonData);
            }
        }
    }

    private void ParseData(string jsonData)
    {
        Dictionary<string, List<string>> parsedData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData);
        // Clear previous data
        ImageList.Clear();
        ByteSoundList.Clear();
        SoundList.Clear();

        // Process images
        foreach (string base64Image in parsedData["ImageList"])
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            ImageList.Add(texture);
            Debug.Log("Loaded Image: " + texture.width + "x" + texture.height);
        }

        // Process sounds
        foreach (string base64Sound in parsedData["SoundList"])
        {
            byte[] soundBytes = Convert.FromBase64String(base64Sound);
            ByteSoundList.Add(soundBytes);
            Debug.Log("Loaded Sound (bytes): " + soundBytes.Length);
        }

        ConvertSounds();
    }
    public void ConvertSounds()
    {
        StartCoroutine(ConvertByteArrayToAudioClips(ByteSoundList));
    }

    private IEnumerator ConvertByteArrayToAudioClips(List<byte[]> rawSoundData)
    {
        foreach (byte[] soundBytes in rawSoundData)
        {
            AudioClip clip = ConvertByteToAudioClip(soundBytes);
            if (clip != null)
            {
                SoundList.Add(clip);
                Debug.Log("Loaded AudioClip: " + clip.length + " seconds.");
            }
            else
            {
                Debug.LogError("Failed to convert byte array to AudioClip.");
            }
            yield return null; // Allow Unity to process the next frame
        }
    }

    private AudioClip ConvertByteToAudioClip(byte[] soundBytes)
    {
        // Assume it's a WAV file; adjust logic if using raw PCM
        if (!IsWavFile(soundBytes))
        {
            Debug.LogError("Invalid WAV file format.");
            return null;
        }

        // Decode WAV to PCM Data
        float[] audioData;
        int sampleRate;
        int channels;
        if (!WavUtility.ConvertWavToPCM(soundBytes, out audioData, out sampleRate, out channels))
        {
            Debug.LogError("Failed to decode WAV file.");
            return null;
        }

        // Create AudioClip
        AudioClip clip = AudioClip.Create("AudioClip", audioData.Length, channels, sampleRate, false);
        clip.SetData(audioData, 0);
        return clip;
    }

    private bool IsWavFile(byte[] data)
    {
        return data.Length > 12 &&
               data[0] == 'R' && data[1] == 'I' && data[2] == 'F' && data[3] == 'F' &&
               data[8] == 'W' && data[9] == 'A' && data[10] == 'V' && data[11] == 'E';
    }
    private Dictionary<string, List<string>> DeserializeData(string jsonData)
    {
        Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

        // Parse JSON-like format manually (consider using Newtonsoft.Json for better handling)
        string imagesData = jsonData.Split(new[] { "\"ImageList\":[" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "],\"SoundList\":[" }, StringSplitOptions.None)[0];
        string soundsData = jsonData.Split(new[] { "\"SoundList\":[" }, StringSplitOptions.None)[1].TrimEnd(']');

        data["ImageList"] = new List<string>(imagesData.Split(new[] { "\",\"" }, StringSplitOptions.None));
        data["SoundList"] = new List<string>(soundsData.Split(new[] { "\",\"" }, StringSplitOptions.None));

        // Remove extraneous quotes
        data["ImageList"] = data["ImageList"].ConvertAll(i => i.Trim('"'));
        data["SoundList"] = data["SoundList"].ConvertAll(s => s.Trim('"'));

        return data;
    }
}
