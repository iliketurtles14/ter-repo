using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class TextureSaver : MonoBehaviour
{
    public List<Texture2D> textureList = new List<Texture2D>(); // Your list of textures
    private string saveFolderPath = "C:\\Users\\creep\\OneDrive\\Desktop\\directory\\Prison Objects"; // Change to your preferred directory

    public void Start()
    {
        textureList = GetComponent<MemoryMappedFileReader>().PrisonObjectImages;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveTextures();
        }
    }
    

    public void SaveTextures()
    {
        // Ensure directory exists
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        // Save each texture as a PNG
        for (int i = 0; i < textureList.Count; i++)
        {
            byte[] bytes = textureList[i].EncodeToPNG(); // Convert Texture2D to PNG byte array
            string filePath = Path.Combine(saveFolderPath, $"Texture_{i}.png"); // Unique file name
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Saved texture: " + filePath);
        }
    }
}
