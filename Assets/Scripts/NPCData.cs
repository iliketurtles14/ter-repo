using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;
[Serializable]
[CreateAssetMenu(menuName = "NPC Data")]

public class NPCData : ScriptableObject
{
    public int health;
    public int strength;
    public int speed;
    public int intellect;
    public int opinion;
    public List<InventoryItem> inventory; //indexes 6 and 7 are the weapon and outfit respectively
    public string displayName;
    public string job;
    public int order;
    public bool hasShop;
    public bool hasFavor;
    public bool isAggro;
}
