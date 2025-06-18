using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class Item
{

    public string name;
    public int id;
    public bool isContraband;
    public bool causeSolitary;

    public Item(string aName, int aId, bool aIsContraband, bool aCauseSolitary)
    {
        name = aName;
        id = aId;
        isContraband = aIsContraband;
        causeSolitary = aCauseSolitary;
    }

    //keys
    public Item CellKey = new Item("Cell Key", 0, true, true);
    public Dictionary<string, int> CellKeyDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item StaffKey = new Item("Staff Key", 1, true, true);
    public Dictionary<string, int> StaffKeyDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item UtilityKey = new Item("Utility Key", 2, true, true);
    public Dictionary<string, int> UtilityKeyDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item EnteranceKey = new Item("Enterance Key", 3, true, true);
    public Dictionary<string, int> EnteranceKeyDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item WorkKey = new Item("Work Key", 4, true, true);
    public Dictionary<string, int> WorkKeyDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item PlasticCellKey = new Item("Plastic Cell Key", 5, true, false);
    public Dictionary<string, int> PlasticCellKeyDict = new Dictionary<string, int>()
    {
        {"Durability", 20},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PlasticStaffKey = new Item("Plastic Staff Key", 6, true, false);
    public Dictionary<string, int> PlasticStaffKeyDict = new Dictionary<string, int>()
    {
        {"Durability", 20},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PlasticUtilityKey = new Item("Plastic Utility Key", 7, true, false);
    public Dictionary<string, int> PlasticUtilityKeyDict = new Dictionary<string, int>()
    {
        {"Durability", 20},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PlasticEnteranceKey = new Item("Plastic Enterance Key", 8, true, false);
    public Dictionary<string, int> PlasticEnteranceKeyDict = new Dictionary<string, int>()
    {
        {"Durability", 20},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PlasticWorkKey = new Item("Plastic Work Key", 9, true, false);
    public Dictionary<string, int> PlasticWorkKeyDict = new Dictionary<string, int>()
    {
        {"Durability", 20},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item FlimsyShovel = new Item("Flimsy Shovel", 10, true, false); //tools
    public Dictionary<string, int> FlimsyShovelDict = new Dictionary<string, int>()
    {
        {"Durability", 8},
        {"Chipping Power", 8},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", 12},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item LightweightShovel = new Item("Lightweight Shovel", 11, true, false);
    public Dictionary<string, int> LightweightShovelDict = new Dictionary<string, int>()
    {
        {"Durability", 5},
        {"Chipping Power", 8},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", 16},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item SturdyShovel = new Item("Sturdy Shovel", 12, true, false);
    public Dictionary<string, int> SturdyShovelDict = new Dictionary<string, int>()
    {
        {"Durability", 3},
        {"Chipping Power", 8},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", 20},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item FlimsyPickaxe = new Item("Flimsy Pickaxe", 13, true, false);
    public Dictionary<string, int> FlimsyPickaxeDict = new Dictionary<string, int>()
    {
        {"Durability", 8},
        {"Chipping Power", 12},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", 8},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item LightweightPickaxe = new Item("Lightweight Pickaxe", 14, true, false);
    public Dictionary<string, int> LightweightPickaxeDict = new Dictionary<string, int>()
    {
        {"Durability", 5},
        {"Chipping Power", 16},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", 12},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item SturdyPickaxe = new Item("Sturdy Pickaxe", 15, true, false);
    public Dictionary<string, int> SturdyPickaxeDict = new Dictionary<string, int>()
    {
        {"Durability", 3},
        {"Chipping Power", 20},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", 16},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item FlimsyCutters = new Item("Flimsy Cutters", 16, true, false);
    public Dictionary<string, int> FlimsyCuttersDict = new Dictionary<string, int>()
    {
        {"Durability", 8},
        {"Chipping Power", -1},
        {"Cutting Power", 12},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item LightweightCutters = new Item("Lightweight Cutters", 17, true, false);
    public Dictionary<string, int> LightweightCuttersDict = new Dictionary<string, int>()
    {
        {"Durability", 5},
        {"Chipping Power", -1},
        {"Cutting Power", 16},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item SturdyCutters = new Item("Sturdy Cutters", 18, true, false);
    public Dictionary<string, int> SturdyCuttersDict = new Dictionary<string, int>()
    {
        {"Durability", 3},
        {"Chipping Power", -1},
        {"Cutting Power", 20},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item Multitool = new Item("Multitool", 19, true, false);
    public Dictionary<string, int> MultitoolDict = new Dictionary<string, int>()
    {
        {"Durability", 1},
        {"Chipping Power", 20},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", 20},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item Screwdriver = new Item("Screwdriver", 20, true, false);
    public Dictionary<string, int> ScrewdriverDict = new Dictionary<string, int>()
    {
        {"Durability", 10},
        {"Chipping Power", 8},
        {"Cutting Power", -1},
        {"Vent Breaking Power", 12},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item PoweredScrewdriver = new Item("Powered Screwdriver", 21, true, false);
    public Dictionary<string, int> PoweredScrewdriverDict = new Dictionary<string, int>()
    {
        {"Durability", 5},
        {"Chipping Power", 12},
        {"Cutting Power", -1},
        {"Vent Breaking Power", 20},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Trowel = new Item("Trowel", 22, true, false);
    public Dictionary<string, int> TrowelDict = new Dictionary<string, int>()
    {
        {"Durability", 5},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", 12},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item CuttingFloss = new Item("Cutting Floss", 23, true, false);
    public Dictionary<string, int> CuttingFlossDict = new Dictionary<string, int>()
    {
        {"Durability", 8},
        {"Chipping Power", -1},
        {"Cutting Power", 12},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item File = new Item("File", 24, true, false);
    public Dictionary<string, int> FileDict = new Dictionary<string, int>()
    {
        {"Durability", 10},
        {"Chipping Power", -1},
        {"Cutting Power", 8},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Crowbar = new Item("Crowbar", 25, true, false);
    public Dictionary<string, int> CrowbarDict = new Dictionary<string, int>()
    {
        {"Durability", 5},
        {"Chipping Power", 8},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 3},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item PlasticFork = new Item("Plastic Fork", 26, false, false);
    public Dictionary<string, int> PlasticForkDict = new Dictionary<string, int>()
    {
        {"Durability", 15},
        {"Chipping Power", 4},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PlasticKnife = new Item("Plastic Knife", 27, false, false);
    public Dictionary<string, int> PlasticKnifeDict = new Dictionary<string, int>()
    {
        {"Durability", 15},
        {"Chipping Power", -1},
        {"Cutting Power", 4},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PlasticSpoon = new Item("Plastic Spoon", 28, false, false);
    public Dictionary<string, int> PlasticSpoonDict = new Dictionary<string, int>()
    {
        {"Durability", 15},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", 4},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item InmateOutfit = new Item("Inmate Outfit", 29, false, false); //Outfits except for infirmary overalls lol
    public Dictionary<string, int> InmateOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 0},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CushionedInmateOutfit = new Item("Cushioned Inmate Outfit", 30, true, false);
    public Dictionary<string, int> CushionedInmateOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 2},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PaddedInmateOutfit = new Item("Padded Inmate Outfit", 31, true, false);
    public Dictionary<string, int> PaddedInmateOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 3},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PlatedInmateOutfit = new Item("Plated Inmate Outfit", 32, true, false);
    public Dictionary<string, int> PlatedInmateOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 4},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item POWOutfit = new Item("POW Outfit", 33, false, false);
    public Dictionary<string, int> POWOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 0},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CushionedPOWOutfit = new Item("Cushioned POW Outfit", 34, true, false);
    public Dictionary<string, int> CushionedPOWOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 2},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PaddedPOWOutfit = new Item("Padded POW Outfit", 35, true, false);
    public Dictionary<string, int> PaddedPOWOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 3},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PlatedPOWOutfit = new Item("Plated POW Outfit", 36, true, false);
    public Dictionary<string, int> PlatedPOWOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 4},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item DirtyInmateOutfit = new Item("Dirty Inmate Outfit", 37, false, false);
    public Dictionary<string, int> DirtyInmateOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item DirtyGuardOutfit = new Item("Dirty Guard Outfit", 38, true, false);
    public Dictionary<string, int> DirtyGuardOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item GuardOutfit = new Item("Guard Outfit", 39, true, false);
    public Dictionary<string, int> GuardOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item ElfOutfit = new Item("Elf Outfit", 40, false, false);
    public Dictionary<string, int> ElfOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 0},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CushionedElfOutfit = new Item("Cushioned Elf Outfit", 41, true, false);
    public Dictionary<string, int> CushionedElfOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 2},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PaddedElfOutfit = new Item("Padded Elf Outfit", 42, true, false);
    public Dictionary<string, int> PaddedElfOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 3},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PlatedElftOutfit = new Item("Plated Elf Outfit", 43, true, false);
    public Dictionary<string, int> PlatedElfOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 4},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item GuardElfOutfit = new Item("Guard Elf Outfit", 44, true, false);
    public Dictionary<string, int> GuardElfOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item TuxOutfit = new Item("Tux Outfit", 45, false, false);
    public Dictionary<string, int> TuxOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 0},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CushionedTuxOutfit = new Item("Cushioned Tux Outfit", 46, true, false);
    public Dictionary<string, int> CushionedTuxOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 2},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PaddedTuxOutfit = new Item("Padded Tux Outfit", 47, true, false);
    public Dictionary<string, int> PaddedTuxOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 3},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PlatedTuxOutfit = new Item("Plated Tux Outfit", 48, true, false);
    public Dictionary<string, int> PlatedTuxOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 4},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item HenchmanOutfit = new Item("Henchman Outfit", 49, true, false);
    public Dictionary<string, int> HenchmanOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PrisonerOutfit = new Item("Prisoner Outfit", 50, false, false);
    public Dictionary<string, int> PrisonerOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 0},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CushionedPrisonerOutfit = new Item("Cushioned Prisoner Outfit", 51, true, false);
    public Dictionary<string, int> CushionedPrisonerOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 2},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PaddedPrisonerOutfit = new Item("Padded Prisoner Outfit", 52, true, false);
    public Dictionary<string, int> PaddedPrisonerOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 3},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PlatedPrisonerOutfit = new Item("Plated Prisoner Outfit", 53, true, false);
    public Dictionary<string, int> PlatedPrisonerOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 4},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item SoldierOutfit = new Item("Soldier Outfit", 54, true, false);
    public Dictionary<string, int> SoldierOutfitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item BaseballBat = new Item("Baseball Bat", 55, true, false); //Weapoins
    public Dictionary<string, int> BaseballBatDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 4},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item Baton = new Item("Baton", 56, true, false);
    public Dictionary<string, int> BatonDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 3},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Broom = new Item("Broom", 57, false, false);
    public Dictionary<string, int> BroomDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 2},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CombBlade = new Item("Comb Blade", 58, true, false);
    public Dictionary<string, int> CombBladeDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 2},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item CombShiv = new Item("Comb Shiv", 59, true, false);
    public Dictionary<string, int> CombShivDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 2},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Hammer = new Item("Hammer", 60, true, false);
    public Dictionary<string, int> HammerDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 3},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Hoe = new Item("Hoe", 61, false, false);
    public Dictionary<string, int> HoeDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 2},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item KnuckleDuster = new Item("Knuckle Duster", 62, true, false);
    public Dictionary<string, int> KnuckleDusterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 4},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item Mop = new Item("Mop", 63, false, false);
    public Dictionary<string, int> MopDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 2},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item Nunchuks = new Item("Nunchuks", 64, true, false);
    public Dictionary<string, int> NunchucksDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 5},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item Pillow = new Item("Pillow", 65, false, false);
    public Dictionary<string, int> PillowDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item SockMace = new Item("Sock Mace", 66, true, false);
    public Dictionary<string, int> SockMaceDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 3},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item SpikedBat = new Item("Spiked Bat", 67, true, false);
    public Dictionary<string, int> SpikedBatDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 4},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item SuperSockMace = new Item("Super Sock Mace", 68, true, false);
    public Dictionary<string, int> SuperSockMaceDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 3},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item ToothbrushShiv = new Item("Toothbrush Shiv", 69, true, false);
    public Dictionary<string, int> ToothbrushShivDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 2},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Whip = new Item("Whip", 70, true, false);
    public Dictionary<string, int> WhipDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 5},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item WoodenBat = new Item("Wooden Bat", 71, true, false);
    public Dictionary<string, int> WoodenBatDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 3},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item GlassShank = new Item("Glass Shank", 72, true, false);
    public Dictionary<string, int> GlassShankDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 4},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item BarOfChocolate = new Item("Bar of Chocolate", 73, false, false); //other random items (not prison-specific)
    public Dictionary<string, int> BarOfChcolateDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 15},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Battery = new Item("Battery", 74, false, false);
    public Dictionary<string, int> BatteryDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item BedDummy = new Item("Bed Dummy", 75, true, false);
    public Dictionary<string, int> BedDummyDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item BedSheet = new Item("Bed Sheet", 76, false, false);
    public Dictionary<string, int> BedSheetDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Book = new Item("Book", 77, false, false);
    public Dictionary<string, int> BookDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item BottleOfMedicine = new Item("Bottle of Medicine", 78, false, false);
    public Dictionary<string, int> BottleOfMedicineDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", 5},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item BottleOfSleepingPills = new Item("Bottle of Sleeping Pills", 79, true, false);
    public Dictionary<string, int> BottleOfSleepingPillsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item Candle = new Item("Candle", 80, false, false);
    public Dictionary<string, int> CandleDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item CellKeyMold = new Item("Cell Key Mold", 81, true, false);
    public Dictionary<string, int> CellKeyMoldDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Comb = new Item("Comb", 82, false, false);
    public Dictionary<string, int> CombDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item ContrabandPouch = new Item("Contraband Pouch", 83, true, false);
    public Dictionary<string, int> ContrabandPouchDict = new Dictionary<string, int>()
    {
        {"Durability", 25},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item CookedFood = new Item("Cooked Food", 84, false, false);
    public Dictionary<string, int> CookedFoodDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 10},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item CraftingNote = new Item("Crafting Note", 85, true, false);
    public Dictionary<string, int> CraftingNoteDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Cup = new Item("Cup", 86, false, false);
    public Dictionary<string, int> CupDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CupOfMoltenChocolate = new Item("Cup of Molten Chocolate", 87, true, false);
    public Dictionary<string, int> CupOfMoltenChocolateDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item DeadRat = new Item("Dead Rat", 88, false, false);
    public Dictionary<string, int> DeadRatDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", -10},
        {"Camera Block", -1}
    };
    public Item DentalFloss = new Item("Dental Floss", 89, false, false);
    public Dictionary<string, int> DentalFlossDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item DIYTattooKit = new Item("DIY Tattoo Kit", 90, false, false);
    public Dictionary<string, int> DIYTattooKitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item DodoDonut = new Item("DoDo Donut", 91, false, false);
    public Dictionary<string, int> DodoDonutDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 10},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item DurableContrabandPouch = new Item("Durable Contraband Pouch", 92, true, false);
    public Dictionary<string, int> DurableContrabandPouchDict = new Dictionary<string, int>()
    {
        {"Durability", 15},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item EntranceKeyMold = new Item("Entrance Key Mold", 93, true, false);
    public Dictionary<string, int> EntranceKeyMoldDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Fabric = new Item("Fabric", 94, false, false);
    public Dictionary<string, int> FabricDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item FakeFence = new Item("Fake Fence", 95, true, false);
    public Dictionary<string, int> FakeFendceDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item FakeVentCover = new Item("Fake Vent Cover", 96, true, false);
    public Dictionary<string, int> FakeVentCoverDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item FakeWallBlock = new Item("Fake Wall Block", 97, true, false);
    public Dictionary<string, int> FakeWallBlockDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Foil = new Item("Foil", 98, true, false);
    public Dictionary<string, int> FoilDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item GameSet = new Item("Game Set", 99, false, false);
    public Dictionary<string, int> GameSetDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item GlassShard = new Item("Glass Shard", 100, true, false);
    public Dictionary<string, int> GlassShardDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item GrappleHead = new Item("Grapple Head", 101, true, false);
    public Dictionary<string, int> GrappleHeadDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item GrapplingHook = new Item("Grappling Hook", 102, true, false);
    public Dictionary<string, int> GrapplingHookDict = new Dictionary<string, int>()
    {
        {"Durability", 10},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item InfirmaryOveralls = new Item("Infirmary Overalls", 103, true, false);
    public Dictionary<string, int> InfirmaryOverallsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", 1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item JarOfInk = new Item("Jar of Ink", 104, false, false);
    public Dictionary<string, int> JarOfInkDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item LengthOfRope = new Item("Length of Rope", 105, true, false);
    public Dictionary<string, int> LengthOfRopeDict = new Dictionary<string, int>()
    {
        {"Durability", 25},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };

    //106 doesnt exist. this is replaced by the empty bullets item left out of te1


    public Item Letter = new Item("Letter", 107, false, false);
    public Dictionary<string, int> LetterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Lighter = new Item("Lighter", 108, false, false);
    public Dictionary<string, int> LighterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item Magazine = new Item("Magazine", 109, false, false);
    public Dictionary<string, int> MagazineDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Medkit = new Item("Medkit", 110, false, false);
    public Dictionary<string, int> MedkitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", 15},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item MoltenPlastic = new Item("Molten Plastic", 111, true, false);
    public Dictionary<string, int> MoltenuPlasticDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Nails = new Item("Nails", 112, false, false);
    public Dictionary<string, int> NailsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item NeedleAndThread = new Item("Needle and Thread", 113, false, false);
    public Dictionary<string, int> NeedleAndThreadDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PackOfMints = new Item("Pack of Mints", 114, false, false);
    public Dictionary<string, int> PackOfMintsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 5},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item PackOfPlayingCards = new Item("Pack of Playing Cards", 115, false, false);
    public Dictionary<string, int> PackOfPlayingCardsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item BluePackage = new Item("Blue Package", 116, false, false);
    public Dictionary<string, int> BluePackageDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item RedPackage = new Item("Red Package", 117, false, false);
    public Dictionary<string, int> RedPackageDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item GreenPackage = new Item("Green Package", 118, false, false);
    public Dictionary<string, int> GreenPackageDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PaperClip = new Item("Paper Clip", 119, false, false);
    public Dictionary<string, int> PaperClipDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PaperMache = new Item("Paper Mache", 120, true, false);
    public Dictionary<string, int> PaperMacheDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PenariumBarrel = new Item("Penarium Barrel", 121, false, false);
    public Dictionary<string, int> PenariumBarrelDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 15},
        {"Camera Block", -1}
    };
    public Item Plunger = new Item("Plunger", 122, false, false);
    public Dictionary<string, int> PlungerDict = new Dictionary<string, int>() //CAN EAT!!!!
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -50},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Dirt = new Item("Dirt", 123, true, true); //random dirt lmao
    public Dictionary<string, int> DirtDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", -50},
        {"Camera Block", -1}
    };
    public Item Poster = new Item("Poster", 124, false, false);
    public Dictionary<string, int> PosterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item RadioReciever = new Item("Radio Reciever", 125, false, false);
    public Dictionary<string, int> RadioRecieverDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item RazorBlade = new Item("Razor Blade", 126, false, false);
    public Dictionary<string, int> RazorBladeDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item RollOfDuctTape = new Item("Roll of Duct Tape", 127, true, false);
    public Dictionary<string, int> RollOfDuctTapeDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", 60}
    };
    public Item RollOfToiletPaper = new Item("Roll of Toilet Paper", 128, false, false);
    public Dictionary<string, int> RollOfToiletPaperDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item ShavingCream = new Item("Shaving Cream", 129, false, false);
    public Dictionary<string, int> ShavingCreamDict = new Dictionary<string, int>()
    {
        {"Durability", 20},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", 30}
    };
    public Item SheetOfMetal = new Item("Sheet of Metal", 130, true, false);
    public Dictionary<string, int> SheetOfMetalDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item SheetRope = new Item("Sheet Rope", 131, true, false);
    public Dictionary<string, int> SheetRopeDict = new Dictionary<string, int>()
    {
        {"Durability", 50},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Shorts = new Item("Shorts", 132, false, false);
    public Dictionary<string, int> ShortsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item SmallSpeaker = new Item("Small Speaker", 133, true, false);
    public Dictionary<string, int> SmallSpeakerDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Soap = new Item("Soap", 134, false, false);
    public Dictionary<string, int> SoapDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Sock = new Item("Sock", 135, false, false);
    public Dictionary<string, int> SockDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Spatula = new Item("Spatula", 136, true, false);
    public Dictionary<string, int> SpatulaDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Stepladder = new Item("Stepladder", 137, true, false);
    public Dictionary<string, int> StepladderDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item StingerStrip = new Item("Stinger Strip", 138, true, false);
    public Dictionary<string, int> StingerStripDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item Timber = new Item("Timber", 139, true, false);
    public Dictionary<string, int> TimberDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item TimberBrace = new Item("Timber Brace", 140, true, false);
    public Dictionary<string, int> TimberBraceDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item ToolHandle = new Item("Tool Handle", 141, true, false);
    public Dictionary<string, int> ToolHandleDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Toothbrush = new Item("Toothbrush", 142, false, false);
    public Dictionary<string, int> ToothbrushDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item TubOfBleack = new Item("Tub of Bleach", 143, false, false);
    public Dictionary<string, int> TubOfBleachDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item TubOfTalcumPowder = new Item("Tub of Talcum Powder", 144, false, false);
    public Dictionary<string, int> TubOfTalcumPowderDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item TubeOfSuperGlue = new Item("Tube of Super Glue", 145, false, false);
    public Dictionary<string, int> TubeOfSuperGlueDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item TubeOfToothpaste = new Item("Tube of Toothpaste", 146, false, false);
    public Dictionary<string, int> TubeOfToothpasteDict = new Dictionary<string, int>()
    {
        {"Durability", 50},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", 30}
    };
    public Item UncookedFood = new Item("Uncooked Food", 147, false, false);
    public Dictionary<string, int> UncookedFoodDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Underpants = new Item("Underpants", 148, false, false);
    public Dictionary<string, int> UnderpantsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item UnvarnishedChair = new Item("Unvarnished Chair", 149, false, false);
    public Dictionary<string, int> UnvarnishedChairDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item UtilityKeyMold = new Item("Utility Key Mold", 150, true, false);
    public Dictionary<string, int> UtilityKeyMoldDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item VentCover = new Item("Vent Cover", 151, true, true);
    public Dictionary<string, int> VentCoverDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Vest = new Item("Vest", 152, false, false);
    public Dictionary<string, int> VestDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WadOfPutty = new Item("Wad of Putty", 153, true, false);
    public Dictionary<string, int> WadOfPuttyDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WallBlock = new Item("Wall Block", 154, true, true);
    public Dictionary<string, int> WallBlockDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", -50},
        {"Camera Block", -1}
    };
    public Item Watch = new Item("Watch", 155, false, false);
    public Dictionary<string, int> WatchDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Wire = new Item("Wire", 156, false, false);
    public Dictionary<string, int> WireDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WorkKeyMold = new Item("Work Key Mold", 157, true, false);
    public Dictionary<string, int> WorkKeyMoldDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WormsGame = new Item("Worms Game", 158, false, false);
    public Dictionary<string, int> WormsGameDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item ZiplineHook = new Item("Zipline Hook", 159, true, false);
    public Dictionary<string, int> ZiplineHookDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Bulbs = new Item("Bulbs", 160, true, false); // prison specific items
    public Dictionary<string, int> BulbsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CandyCaneLever = new Item("Candy Cane Lever", 161, true, false);
    public Dictionary<string, int> CandyCaneLeverDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Charcoal = new Item("Charcoal", 162, true, false);
    public Dictionary<string, int> CharcoalDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Chisel = new Item("Chisel", 163, true, false);
    public Dictionary<string, int> ChiselDict = new Dictionary<string, int>()
    {
        {"Durability", 25},
        {"Chipping Power", 4},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Cork = new Item("Cork", 164, false, false);
    public Dictionary<string, int> CorkDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CorrugatedIron = new Item("Corrugated Iron", 165, true, false);
    public Dictionary<string, int> CorrugatedIronDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item DirtyGlass = new Item("Dirty Glass", 166, true, false);
    public Dictionary<string, int> DirtyGlassDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item EmptyBottle = new Item("Empty Bottle", 167, false, false);
    public Dictionary<string, int> EmptyBottleDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item ExplosiveCompound = new Item("Explosive Compound", 168, true, false);
    public Dictionary<string, int> ExplosiveCompoundDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item ExplosiveMix = new Item("Explosive Mix", 169, true, false);
    public Dictionary<string, int> ExplosiveMixDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item FairyLights = new Item("Fairy Lights", 170, true, false);
    public Dictionary<string, int> FairyLightsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item FakeFingerprint = new Item("Fake Fingerprint", 171, true, false);
    public Dictionary<string, int> FakeFingerprintDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Fertililzer = new Item("Fertilizer", 172, true, false);
    public Dictionary<string, int> FertilizerDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Flamethrower = new Item("Flamethrower", 173, true, false);
    public Dictionary<string, int> FlamethrowerDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Flashlight = new Item("Flashlight", 174, false, false); //random flashlight lmao
    public Dictionary<string, int> FlashlightDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Fuel = new Item("Fuel", 175, true, false);
    public Dictionary<string, int> FuelDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Glitter = new Item("Glitter", 176, true, false);
    public Dictionary<string, int> GlitterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item HalogenBulb = new Item("Halogen Bulb", 177, true, false);
    public Dictionary<string, int> HalogenBulbDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Hat = new Item("Hat", 178, false, false); // random hat lolllll
    public Dictionary<string, int> HatDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item HighBeamFlashlight = new Item("High Beam Flashlight", 179, true, false);
    public Dictionary<string, int> HighBeamFlashlightDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item JuicyCookedCarrots = new Item("Juicy Cooked Carrots", 180, false, false);
    public Dictionary<string, int> JuicyCookedCarrotsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Keycard = new Item("Keycard", 181, true, false);
    public Dictionary<string, int> KeycardDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item LicensePlate = new Item("License Plate", 182, true, false); //random license plate lomaoamoma
    public Dictionary<string, int> LicensePlateDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Magnet = new Item("Magnet", 183, false, false);
    public Dictionary<string, int> MagnetDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MagnetizedNeedle = new Item("Magnetized Needle", 184, false, false);
    public Dictionary<string, int> MagnetizedNeedleDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MakeshiftCompass = new Item("Makeshift Compass", 185, true, false);
    public Dictionary<string, int> MakeshiftCompassDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MakeshiftExplosiveRound = new Item("Makeshift Explosive Round", 186, true, false);
    public Dictionary<string, int> MakeshiftExplosiveRoundDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MakeshiftFuse = new Item("Makeshift Fuse", 187, true, false);
    public Dictionary<string, int> MakeshiftFuseDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MakeshiftRaft = new Item("Makeshift Raft", 188, true, false);
    public Dictionary<string, int> MakeshiftRaftDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MakeshiftTankBarrel = new Item("Makeshift Tank Barrel", 189, true, false);
    public Dictionary<string, int> DMakeshiftTankBarrelict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MakeshiftTankFiringBase = new Item("Makeshift Tank Firing Base", 190, true, false);
    public Dictionary<string, int> MakeshiftTankFiringBaseDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MakeshiftTankTurret = new Item("Makeshift Tank Turret", 191, true, false);
    public Dictionary<string, int> MakeshiftTankTurretDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MemoirTapes = new Item("Memoir Tapes", 192, true, false);
    public Dictionary<string, int> MemoirTapesDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MetalCone = new Item("Metal Cone", 193, true, false);
    public Dictionary<string, int> MetalConeDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MetalRimmedHat = new Item("Metal Rimmed Hat", 194, true, false);
    public Dictionary<string, int> MetalRimmedHatDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 4},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item MetalTube = new Item("Metal Tube", 195, true, false);
    public Dictionary<string, int> MetalTubeDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MixingContainer = new Item("Mixing Container", 196, true, false);
    public Dictionary<string, int> MixingContainerDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item NaughtyLetter = new Item("Naughty Letter", 197, false, false);
    public Dictionary<string, int> NaughtyLetterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item NavigationLighting = new Item("Navigation Lighting", 198, true, false);
    public Dictionary<string, int> NavigationLightingDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Needle = new Item("Needle", 199, false, false);
    public Dictionary<string, int> NeedleDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item NiceLetter = new Item("Nice Letter", 200, false, false);
    public Dictionary<string, int> NiceLetterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Presents = new Item("Presents", 201, true, false);
    public Dictionary<string, int> PresentsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item RaftBase = new Item("Raft Base", 202, true, false);
    public Dictionary<string, int> RaftBaseDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item RawNastyCarrots = new Item("Raw Nasty Carrots", 203, false, false);
    public Dictionary<string, int> RawNastyCarrotsDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item RedHerring = new Item("Red Herring", 204, false, false);
    public Dictionary<string, int> RedHerringDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item ReindeerBowl = new Item("Reindeer Bowl", 205, false, false);
    public Dictionary<string, int> ReindeerBowlDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item StickyTape = new Item("Sticky Tape", 206, false, false);
    public Dictionary<string, int> StickyTapeDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item StunRod = new Item("Stun Rod", 207, true, false);
    public Dictionary<string, int> StunRodDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 100},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item TapePlayer = new Item("Tape Player", 208, true, false);
    public Dictionary<string, int> TapePlayerDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Taper = new Item("Taper", 209, true, false);
    public Dictionary<string, int> TaperDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Thruster = new Item("Thruster", 210, true, false);
    public Dictionary<string, int> ThrusterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Tinsel = new Item("Tinsel", 211, true, false);
    public Dictionary<string, int> TinselDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item VoiceRecording = new Item("Voice Recording", 212, true, false);
    public Dictionary<string, int> VoiceRecordingDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WeakFingerprint = new Item("Weak Fingerprint", 213, true, false);
    public Dictionary<string, int> WeakFingerprintDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WoodenBall = new Item("Wooden Ball", 214, false, false);
    public Dictionary<string, int> WoodenBallDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WoodenBlock = new Item("Wooden Block", 215, false, false);
    public Dictionary<string, int> WoodenBlockDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WoodenDoll = new Item("Wooden Doll", 216, false, false);
    public Dictionary<string, int> WoodenDollDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WoodenJoystick = new Item("Wooden Joystick", 217, false, false);
    public Dictionary<string, int> WoodenJoystickDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WoodenScooter = new Item("Wooden Scooter", 218, false, false);
    public Dictionary<string, int> WoodenScooterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WorkHat = new Item("Work Hat", 219, false, false);
    public Dictionary<string, int> WorkHatDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item WrappingPaper = new Item("Wrapping Paper", 220, true, false);
    public Dictionary<string, int> WrappingPaperDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Cookie = new Item("Cookie", 221, false, false);
    public Dictionary<string, int> CookieDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 15},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item DeluxeToiletRoll = new Item("Deluxe Toilet Roll", 222, false, false);
    public Dictionary<string, int> DeluxeToiletRollDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item DVD = new Item("DVD", 223, false, false);
    public Dictionary<string, int> DVDDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item HandCream = new Item("Hand Cream", 224, false, false);
    public Dictionary<string, int> HandCreamDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item HandFan = new Item("Hand Fan", 225, false, false);
    public Dictionary<string, int> HandFanDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Muffin = new Item("Muffin", 226, false, false);
    public Dictionary<string, int> MuffinDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 15},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PedicureKit = new Item("Pedicure Kit", 227, false, false);
    public Dictionary<string, int> PedicureKitDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Postcard = new Item("Postcard", 228, false, false);
    public Dictionary<string, int> PostcardDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item SilkHandkerchief = new Item("Silk Handkerchief", 229, false, false);
    public Dictionary<string, int> SilkHandkerchiefDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Sponge = new Item("Sponge", 230, false, false);
    public Dictionary<string, int> SpongeDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item TeddyBear = new Item("Teddy Bear", 231, false, false);
    public Dictionary<string, int> TeddyBearDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item TVRemote = new Item("TV Remote", 232, false, false);
    public Dictionary<string, int> TVRemovteDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item BombSurprise = new Item("Bomb Surprise", 233, true, false);
    public Dictionary<string, int> BombSurpriseDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item CuttingLaserWatch = new Item("Cutting Laser Watch", 234, true, false);
    public Dictionary<string, int> CuttingLaserWatchDict = new Dictionary<string, int>()
    {
        {"Durability", 100},
        {"Chipping Power", -1},
        {"Cutting Power", 100},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item FakeDecoratedEgg = new Item("Fake Decorated Egg", 235, false, false);
    public Dictionary<string, int> FakeDecoratedEggDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 15},
        {"Camera Block", -1}
    };
    public Item FakeShoe = new Item("Fake Shoe", 236, false, false);
    public Dictionary<string, int> FakeShoeDict = new Dictionary<string, int>()
    {
        {"Durability", 25},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item GarottingWireWatch = new Item("Garotting Wire Watch", 237, true, false);
    public Dictionary<string, int> GarottingWireWatchDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Hairspray = new Item("Hairspray", 238, true, false);
    public Dictionary<string, int> HairsprayDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PorkPieHat = new Item("Pork Pie Hat", 239, false, false);
    public Dictionary<string, int> PorkPieHatDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item SharpTeaTray = new Item("Sharp Tea Tray", 240, true, false);
    public Dictionary<string, int> SharpTeaTrayDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item ShoeKnife = new Item("Shoe Knife", 241, true, false);
    public Dictionary<string, int> ShowKnifeDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 3},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item StunPen = new Item("Stun Pen", 242, true, false);
    public Dictionary<string, int> StunPenDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 3},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Aftershave = new Item("Aftershave", 243, false, false);
    public Dictionary<string, int> AftershaveDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item SovereignRing = new Item("Sovereign Ring", 244, false, false);
    public Dictionary<string, int> SovereignRingDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item TrashBag = new Item("Trash Bag", 245, false, false);
    public Dictionary<string, int> TrashBagDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item BalsaWood = new Item("Balsa Wood", 246, true, false);
    public Dictionary<string, int> BalsaWoodDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Sail = new Item("Sail", 247, true, false);
    public Dictionary<string, int> SailDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Bananas = new Item("Bananas", 248, false, false);
    public Dictionary<string, int> BananasDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 10},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Coconut = new Item("Coconut", 249, false, false);
    public Dictionary<string, int> CoconutDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 10},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item ExoticFeather = new Item("Exotic Feather", 250, false, false);
    public Dictionary<string, int> ExoticFeatherDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item GreenHerb = new Item("Green Herb", 251, false, false);
    public Dictionary<string, int> GreenHerbDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", 5},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item IDPapers = new Item("ID Papers", 252, true, false);
    public Dictionary<string, int> IDPapersDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 3},
        {"Camera Block", -1}
    };
    public Item Mango = new Item("Mango", 253, false, false);
    public Dictionary<string, int> MangoDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 10},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 1},
        {"Camera Block", -1}
    };
    public Item RedHerb = new Item("Red Herb", 254, false, false);
    public Dictionary<string, int> RedHerbDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", 10},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item TribalDrum = new Item("Tribal Drum", 255, false, false);
    public Dictionary<string, int> TribalDrumDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item UnsignedIDPapers = new Item("Unsigned ID Papers", 256, true, false);
    public Dictionary<string, int> UnsignedIDPapersDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Vines = new Item("Vines", 257, false, false);
    public Dictionary<string, int> VinesDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Cracker = new Item("Cracker", 258, true, false);
    public Dictionary<string, int> CrackerDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item GiantLollipop = new Item("Giant Lollipop", 259, true, false);
    public Dictionary<string, int> GiantLollipopDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", 3},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item MincePie = new Item("Mince Pie", 260, false, false);
    public Dictionary<string, int> MincePieDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 5},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Stocking = new Item("Stocking", 261, false, false);
    public Dictionary<string, int> StockingDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Burrito = new Item("Burrito", 262, false, false);
    public Dictionary<string, int> BurritoDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", 10},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 2},
        {"Camera Block", -1}
    };
    public Item Poncho = new Item("Poncho", 263, false, false);
    public Dictionary<string, int> PonchoDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item RedChili = new Item("Red Chili", 264, false, false);
    public Dictionary<string, int> RedChiliDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -5},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item Sand = new Item("Sand", 265, true, true);
    public Dictionary<string, int> SandDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", -50},
        {"Camera Block", -1}
    };
    public Item Somberero = new Item("Somberero", 266, false, false);
    public Dictionary<string, int> SombereroDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item DogTag = new Item("Dog Tag", 267, false, false);
    public Dictionary<string, int> DogTagDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item FamiliyPhoto = new Item("Family Photo", 268, false, false);
    public Dictionary<string, int> FamilyPhotoDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item PocketWatch = new Item("Pocket Watch", 269, false, false);
    public Dictionary<string, int> PocketWatchDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item ServiceMedal = new Item("Service Medal", 270, false, false);
    public Dictionary<string, int> ServiceMedalDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item BowlOfWater = new Item("Bowl of Water", 271, false, false);
    public Dictionary<string, int> BowlOfWaterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item BottleOfWater = new Item("Bottle of Water", 272, false, false);
    public Dictionary<string, int> BottleOfWaterDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item BluePutty = new Item("Blue Putty", 273, false, false);
    public Dictionary<string, int> BluePuttyDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };
    public Item UncookedBurrito = new Item("Uncooked Burrito", 274, false, false);
    public Dictionary<string, int> UncookedBurritoDict = new Dictionary<string, int>()
    {
        {"Durability", -1},
        {"Chipping Power", -1},
        {"Cutting Power", -1},
        {"Vent Breaking Power", -1},
        {"Digging Power", -1},
        {"Health", -1},
        {"Energy", -1},
        {"Defence", -1},
        {"Strength", -1},
        {"Opinion", 0},
        {"Camera Block", -1}
    };

}
