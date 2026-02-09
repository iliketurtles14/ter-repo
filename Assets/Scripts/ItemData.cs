using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string displayName;
    public int id;
    public int currentDurability;
    public bool isContraband;
    public bool causeSolitary;
    public int durability;
    public int chippingPower;
    public int cuttingPower;
    public int ventBreakingPower;
    public int diggingPower;
    public int health;
    public int energy;
    public int defence;
    public int strength;
    public int opinion;
    public int cameraBlock;
    public Sprite icon;
    public GameObject prefab;
}
