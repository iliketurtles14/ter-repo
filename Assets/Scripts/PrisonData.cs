using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;
[Serializable]
[CreateAssetMenu(menuName = "Prison Data")]
public class PrisonData : ScriptableObject
{
    public int inmateAmount;
    public int guardAmount;
    public int npcLevel; //1-3
    public int prisonStyle; //1-4 (4 being camp and 1-3 being min, med, and max)
    public int grounds; //1 - inside; 2 - inside outside; 3 - outside
    public string prisonName;
    public string wardenNote;
    public int specialNoteAmount;
    public List<string> specialNotes = new List<string>();
    public string startingJob;
    public bool metalshop;
    public bool mailman;
    public bool kitchen;
    public bool gardening;
    public bool janitor;
    public bool woodshop;
    public bool library;
    public bool tailor;
    public bool deliveries;
    public bool laundry;
    public bool hasHints;
    public string hint1;
    public string hint2;
    public string hint3;
    public int version = 1;
}
