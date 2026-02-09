using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPCSave : MonoBehaviour
{
    public string playerName;
    public int playerCharacter;
    public List<string> npcNames = new List<string>();
    public List<int> npcCharacters = new List<int>();
    public static NPCSave instance {  get; private set; }

    public void SetPlayer(string pName, int pCharacter)
    {
        playerName = pName;
        playerCharacter = pCharacter;
    }
    public void SetNPC(List<string> names, List<int> characters)
    {
        npcNames = names;
        npcCharacters = characters;
    }
    private void Awake()
    {
        if(instance == null)
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
