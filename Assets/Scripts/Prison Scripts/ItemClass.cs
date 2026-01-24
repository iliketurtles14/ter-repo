using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string displayName;
    public int deskTier;
    public bool inInmateInv;
    public int cost;
    public bool inItemFetchFavors;
    public bool inStolenItemFavors;
    public bool inmateWeapon;
    public bool isContraband;
    public bool causeSolitary;
    public int durability;
    public int chippingPower;
    public int cuttingPower;
    public int ventBreakingPower;
    public int diggingPower;
    public int health;
    public int energy;
    public int defense;
    public int strength;
    public int opinion;
    public int cameraBlock;
    public Sprite sprite;
    public int currentDurability;
    public int id;
    public GameObject prefab;

    public ItemData(string displayName, int deskTier, bool inInmateInv, int cost, bool inItemFetchFavors, bool inStolenItemFavors, bool inmateWeapon, bool isContraband, bool causeSolitary, int durability, int chippingPower, int cuttingPower, int ventBreakingPower, int diggingPower, int health, int energy, int defense, int strength, int opinion, int cameraBlock, Sprite sprite, int currentDurability, int id, GameObject prefab)
    {
        this.displayName = displayName;
        this.deskTier = deskTier;
        this.inInmateInv = inInmateInv;
        this.cost = cost;
        this.inItemFetchFavors = inItemFetchFavors;
        this.inStolenItemFavors = inStolenItemFavors;
        this.inmateWeapon = inmateWeapon;
        this.isContraband = isContraband;
        this.causeSolitary = causeSolitary;
        this.durability = durability;
        this.chippingPower = chippingPower;
        this.cuttingPower = cuttingPower;
        this.ventBreakingPower = ventBreakingPower;
        this.diggingPower = diggingPower;
        this.health = health;
        this.energy = energy;
        this.defense = defense;
        this.strength = strength;
        this.opinion = opinion;
        this.cameraBlock = cameraBlock;
        this.sprite = sprite;
        this.currentDurability = currentDurability;
        this.id = id;
        this.prefab = prefab;
    }
}
