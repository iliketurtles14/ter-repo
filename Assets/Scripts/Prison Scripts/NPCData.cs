using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;
[System.Serializable]
[CreateAssetMenu(menuName = "NPC Data")]

public class NPCData : ScriptableObject
{
    public int health;
    public int strength;
    public int speed;
    public int intellect;
    public int opinion;
    public List<NPCInvItem> inventory; //indexes 6 and 7 are the weapon and outfit respectively
    public string displayName; // not including the "Officer" in guards
    public bool isGuard;
    public int charNum;
    public string job;
    public int order;
    public bool hasShop;
    public bool hasFavor;
    public bool isAggro;
    public GameObject aggroTarget;
    public bool isDead;
    public bool hasFood;
    public GameObject bed;
    public GameObject desk;
    public Mission mission;
}
