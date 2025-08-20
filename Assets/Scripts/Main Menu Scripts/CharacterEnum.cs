using System;
using UnityEngine;

public static class CharacterEnumClass
{
	public enum CharacterEnum
	{
        Rabbit,
        BaldEagle,
        Lifer,
        YoungBuck,
        OldTimer,
        BillyGoat,
        Froseph,
        Tango,
        Maru,
        Buddy,
        IceElf,
        BlackElf,
        YellowElf,
        PinkElf,
        OrangeElf,
        BrownElf,
        WhiteElf,
        Genie,
        GuardElf,
        Connelly,
        Elbrah,
        Chen,
        Piers,
        Mourn,
        Lazeeboi,
        Blonde,
        Walton,
        Prowler,
        Crane,
        Henchman,
        Clint,
        Cage,
        Sean,
        Andy,
        Soldier
    }
    public static string GetCharacterString(int characterNum)
    {
        return ((CharacterEnum)characterNum).ToString();
    }
    public static int GetCharacterInt(string characterName)
    {
        return (int)Enum.Parse(typeof(CharacterEnum), characterName);
    }
}
