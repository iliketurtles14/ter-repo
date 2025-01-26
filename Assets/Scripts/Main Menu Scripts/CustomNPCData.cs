using UnityEngine;
using System;
using System.Collections.Generic;
[Serializable]
[CreateAssetMenu(menuName = "Custom NPC Data")]
public class CustomNPCData : ScriptableObject
{
    public string displayName;
    public string npcType;
}
