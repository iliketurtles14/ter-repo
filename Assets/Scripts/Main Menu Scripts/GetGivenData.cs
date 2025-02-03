using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetGivenData : MonoBehaviour
{
    public List<Texture2D> groundList = new List<Texture2D>();
    public List<AudioClip> musicList = new List<AudioClip>();
    private string groundPath = "D:\\SteamLibrary\\steamapps\\common\\The Escapists\\Data\\images".Trim('"');
    private string musicPath = "D:\\SteamLibrary\\steamapps\\common\\The Escapists\\Music".Trim('"');

    [System.Obsolete]
    public void Start()
    {
        foreach(string file in Directory.GetFiles(groundPath))
        {
            if (file == groundPath + "\\ground_alca.gif" || 
                file == groundPath + "\\ground_BC.gif" ||
                file == groundPath + "\\ground_campepsilon.gif" ||
                file == groundPath + "\\ground_CCL.gif" ||
                file == groundPath + "\\ground_DTAF.gif" || 
                file == groundPath + "\\ground_EA.gif" ||
                file == groundPath + "\\ground_escapeteam.gif" ||
                file == groundPath + "\\ground_fortbamford.gif" ||
                file == groundPath + "\\ground_irongate.gif" ||
                file == groundPath + "\\ground_jungle.gif" ||
                file == groundPath + "\\ground_pcpen.gif" ||
                file == groundPath + "\\ground_perks.gif" ||
                file == groundPath + "\\ground_sanpancho.gif" ||
                file == groundPath + "\\ground_shanktonstatepen.gif" ||
                file == groundPath + "\\ground_SS.gif" ||
                file == groundPath + "\\ground_stalagflucht.gif" ||
                file == groundPath + "\\ground_TOL.gif" ||
                file == groundPath + "\\ground_tutorial.gif" ||
                file == groundPath + "\\soil.gif")
            {
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(File.ReadAllBytes(file));
                groundList.Add(texture);
            }
        }
        foreach(string file in Directory.GetFiles(musicPath))
        {
            WWW www = new WWW("file:///" + file);
            AudioClip clip = www.GetAudioClip();
            musicList.Add(clip);
        }
    }
}
