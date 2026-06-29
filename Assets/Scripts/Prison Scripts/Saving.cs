using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class Saving : MonoBehaviour
{
	private Map currentMap;
	private Transform player;
	private PlayerData playerData;
	private Inventory inventoryScript;
	private PlayerIDInv idInvScript;
	private Transform mc;
	private MissionAsk missionAskScript;
	private Lockdown lockdownScript;
	private PayphoneMenu payphoneScript;
	private Solitary solitaryScript;
	private Routine routineScript;
	private Transform ic;
	private Sittables sittablesScript;
	private Transform aStar;
	private Shops shopsScript;
	public bool wardenWent;
	private Transform tiles;
    private List<string> objLayers = new List<string>
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
	private List<string> tileLayers = new List<string>
	{
		"Underground", "Ground", "Vents", "Roof"
	};
    private List<Transform> desks = new List<Transform>();
	private List<Transform> jobBoxes = new List<Transform>();
	private List<Transform> toilets = new List<Transform>();
	public float jobQuotaBarSize = 0;
    private void Start()
	{
		player = RootObjectCache.GetRoot("Player").transform;
		inventoryScript = GetComponent<Inventory>();
		mc = RootObjectCache.GetRoot("MenuCanvas").transform;
		ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
		idInvScript = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>();
		missionAskScript = GetComponent<MissionAsk>();
		lockdownScript = GetComponent<Lockdown>();
		payphoneScript = mc.Find("PayphoneMenuPanel").GetComponent<PayphoneMenu>();
		solitaryScript = GetComponent<Solitary>();
		routineScript = ic.Find("Time").GetComponent<Routine>();
		sittablesScript = GetComponent<Sittables>();
		aStar = RootObjectCache.GetRoot("A*").transform;
		shopsScript = GetComponent<Shops>();
		tiles = RootObjectCache.GetRoot("Tiles").transform;
		StartCoroutine(StartWait());
	}
	private IEnumerator StartWait()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		currentMap = GetComponent<LoadPrison>().currentMap;
        for (int i = 0; i < 4; i++)
        {
            foreach (Transform obj in tiles.Find(objLayers[i]))
            {
				if (obj.CompareTag("Toilet"))
				{
					toilets.Add(obj);
				}
				switch (obj.name)
                {
                    case "CutleryTable":
                    case "ToiletDown":
					case "ToiletLeft":
					case "ToiletRight":
                    case "DTAFSpecialDesk":
                    case "ETSpecialDesk":
                    case "ETNPCDesk":
                    case "ETPlayerDesk":
                    case "MedicDesk":
                    case "NPCDesk":
                    case "PlayerDesk":
                    case "ChristmasDesk":
                    case "JanitorDesk":
                    case "YardWorkBox":
                         desks.Add(obj);
                        break;
					case "Oven":
					case "Washer":
						jobBoxes.Add(obj);
						break;
                }
            }
        }
    }
    public void Save()
	{
		playerData = player.GetComponent<PlayerCollectionData>().playerData;
		
		string save = "";
		save += "[Map]\n";
		save += "FileName=" + currentMap.fileName + "\n";
		save += "Name=" + currentMap.mapName + "\n";
		save += "Time=" + routineScript.min + "," + routineScript.sec + "," + routineScript.day;
		save += "WardenHasGone=" + wardenWent + "\n";
		save += "\n";
		save += "[Player]\n";
		save += "Strength=" + playerData.strength + "\n";
		save += "Speed=" + playerData.speed + "\n";
		save += "Intellect=" + playerData.intellect + "\n";
		save += "Health=" + playerData.health + "\n";
		save += "Energy=" + playerData.energy + "\n";
		save += "Money=" + playerData.money + "\n";
		//save += "Recruits="
		save += "Name=" + playerData.displayName.Replace("\n", "") + "\n";
		save += "Character=" + NPCSave.instance.playerCharacter + "\n";
		save += "JobQuotaBarSize=" + jobQuotaBarSize + "\n";
		save += "Inventory=";
		foreach(InventoryItem item in inventoryScript.inventory)
		{
			if(item.itemData == null)
			{
				save += "null;";
				continue;
			}
			
			save += item.itemData.id.ToString() + ",";
			save += item.itemData.currentDurability.ToString() + ",";
			save += item.itemData.forFavor.ToString() + ",";
			save += item.itemData.inmateGiveName.ToString() + ";";
		}
		for(int i = 0; i < 2; i++)
		{
            if (idInvScript.idInv[i].itemData == null)
            {
                save += "null;";
                continue;
            }

            save += idInvScript.idInv[i].itemData.id.ToString() + ",";
			save += idInvScript.idInv[i].itemData.currentDurability.ToString() + ",";
			save += idInvScript.idInv[i].itemData.forFavor.ToString() + ",";
			save += idInvScript.idInv[i].itemData.inmateGiveName.ToString() + ";";
		}
		save += "\n";
		save += "Position=(" + player.position.x + "," + player.position.y + ")\n";
		save += "Layer=" + LayerMask.LayerToName(player.gameObject.layer) + "\n";
		save += "Job=" + playerData.job.Replace("\n", "") + "\n";
		save += "Missions=";
		foreach(Mission mission in missionAskScript.savedMissions) // in the order of the MissionData class
		{
			save += mission.type + ",";
			save += mission.item + ",";
			save += mission.giver.Replace("\n", "") + ",";
			save += mission.target.Replace("\n", "") + ",";
			save += mission.period + ",";
			save += mission.pay + ";";
		}
		save += "\n";
		save += "Lockdown=" + lockdownScript.lockdownIsActive + "," + lockdownScript.lockdownTime + "\n";
		save += "Solitary=" + solitaryScript.inSolitary + "\n";
		save += "Hints=" + payphoneScript.hint1Bought + "," + payphoneScript.hint2Bought + "," + payphoneScript.hint3Bought + "\n";
		save += "OnSittable=" + sittablesScript.onSittable + "\n";
		save += "HasFood=" + playerData.hasFood + "\n";
		save += "\n";
		foreach(Transform npc in aStar)
		{
			if(npc.name.Contains("Inmate") || npc.name.Contains("Guard"))
			{
				NPCData npcData = npc.GetComponent<NPCCollectionData>().npcData;

				save += "[" + npc.name + "]\n";
				save += "Strength=" + npcData.strength + "\n";
				save += "Speed=" + npcData.speed + "\n";
				save += "Intellect=" + npcData.intellect + "\n";
				save += "Opinion=" + npcData.opinion + "\n";
				save += "Name=" + npcData.displayName.Replace("\n", "") + "\n";
				save += "Character=" + npcData.charNum + "\n";
				save += "Inventory=";
                foreach (NPCInvItem item in npcData.inventory)
                {
                    if (item.itemData == null)
                    {
                        save += "null;";
                        continue;
                    }

                    save += item.itemData.id.ToString() + ",";
                    save += item.itemData.currentDurability.ToString() + ",";
                    save += item.itemData.forFavor.ToString() + ",";
                    save += item.itemData.inmateGiveName.ToString() + ";";
                }
				save += "\n";
				save += "Position=(" + npc.position.x + "," + npc.position.y + ")\n";
				save += "HasFood=" + npcData.hasFood + "\n";
				save += "Sleeping=" + npcData.isSleeping + "\n";
				save += "Dead=" + npcData.isDead + "\n";
				save += "Tied=" + npcData.isTied + "\n";
				save += "Job=" + npcData.job + "\n";
				save += "Mission=";
				if (npcData.hasFavor)
				{
                    Mission mission = npcData.mission;
                    save += mission.type + ",";
                    save += mission.item + ",";
                    save += mission.giver.Replace("\n", "") + ",";
                    save += mission.target.Replace("\n", "") + ",";
					save += Regex.Escape(mission.message);
                    save += mission.period + ",";
                    save += mission.pay + "\n";
                }
				else
				{
					save += "null\n";
				}
				save += "Shop=";
				bool hasShop1 = false;
				bool hasShop2 = false;
				if(npc.name == shopsScript.shop1NPC.name)
				{
					hasShop1 = true;
				}
				else if(npc.name == shopsScript.shop2NPC.name)
				{
					hasShop2 = true;
				}
				if(hasShop1 || hasShop2)
				{
					List<NPCInvItem> shop = null;
					if (hasShop1)
					{
						shop = shopsScript.shop1;
					}
					else if (hasShop2)
					{
						shop = shopsScript.shop2;
					}
					foreach(NPCInvItem item in shop)
					{
						if(item.itemData == null)
						{
							save += "null,";
							continue;
						}

						save += item.itemData.id + ",";
					}
					save += "\n";
				}
				else
				{
					save += "null\n";
				}
            }
			save += "\n";
        }
		save += "[Desks]\n"; //this is done in a way so that the positions of these objects dont need to be saved
		int index = 0;
		foreach(Transform desk in desks)
		{
			save += "Desk" + index + "=";
			index++;
			foreach(DeskItem item in desk.GetComponent<DeskData>().deskInv)
			{
                if (item.itemData == null)
                {
                    save += "null;";
                    continue;
                }

                save += item.itemData.id.ToString() + ",";
                save += item.itemData.currentDurability.ToString() + ",";
                save += item.itemData.forFavor.ToString() + ",";
                save += item.itemData.inmateGiveName.ToString() + ";";
            }
			save += "\n";
        }
		save += "[DeskPositions]\n";
		index = 0;
		foreach(Transform desk in desks)
		{
			save += "Desk" + index + "=(" + desk.position.x + "," + desk.position.y + ")\n";
			index++;
		}
		save += "[JobBoxes]\n";
		index = 0;
		foreach(Transform jobBox in jobBoxes)
		{
			save += "JobBox" + index + "=" + jobBox.GetComponent<ItemTransformerData>().heldID + "\n";
			index++;
		}
		save += "\n";
		//cameras
		save += "[Items]\n";
		index = 0;
		for(int i = 0; i < 4; i++)
		{
			foreach(Transform item in tiles.Find(objLayers[i]))
			{
				if (!item.CompareTag("Item"))
				{
					continue;
				}
				save += index + "=";
				save += LayerMask.LayerToName(item.gameObject.layer) + ",";
				save += item.position.x + ",";
				save += item.position.y + ",";
                save += item.GetComponent<ItemCollectionData>().itemData.id.ToString() + ",";
                save += item.GetComponent<ItemCollectionData>().itemData.currentDurability.ToString() + ",";
                save += item.GetComponent<ItemCollectionData>().itemData.forFavor.ToString() + ",";
                save += item.GetComponent<ItemCollectionData>().itemData.inmateGiveName.ToString() + "\n";
				index++;
            }
        }
		save += "\n";
		save += "[Tiles]\n";
		index = 0;
		for(int i = 0; i < 4; i++)
		{
			foreach(Transform tile in tiles.Find(tileLayers[i])) //doesnt iinclude floor tiles (cuz underground)
			{
				if(tile.GetComponent<TileCollectionData>() != null)
				{
                    if (tile.GetComponent<TileCollectionData>().tileData.currentDurability == 100 || tile.CompareTag("Digable"))
                    {
						index++;
						continue;
                    }
                }
                save += index + "=";
				index++;
				save += tile.GetComponent<TileCollectionData>().tileData.currentDurability + "\n";
			}
		}
		//prisoner stashes
		save += "\n";
		save += "[Underground]\n";
		foreach(Transform obj in tiles.Find("UndergroundObjects"))
		{
			if(!obj.name.Contains("Brace") && !obj.name.Contains("Rock") && !obj.name.Contains("Hole") &&
				!obj.name.Contains("Dirt"))
			{
				continue;
			}
			save += obj.name + "=";
			save += obj.position.x + ",";
			save += obj.position.y + ",";
			if(obj.GetComponent<TileCollectionData>() != null)
			{
				save += obj.GetComponent<TileCollectionData>().tileData.currentDurability + ",";
                if (obj.name.Contains("DirtEmpty"))
                {
					save += obj.GetComponent<TileCollectionData>().tileData.holeStability + ",";
				}
            }
			save += "\n";
		}
		save += "\n";
		save += "[Vents]\n";
		foreach(Transform obj in tiles.Find("VentObjects"))
		{
			if(obj.name != "Vent" && obj.name != "EmptyVentCover" && obj.name != "FakeVent" && !obj.name.Contains("Slats"))
			{
				continue;
			}
            save += obj.name + "=";
            save += obj.position.x + ",";
            save += obj.position.y + ",";
            if (obj.name == "Vent" || obj.name.Contains("Slats"))
            {
                save += obj.GetComponent<TileCollectionData>().tileData.currentDurability + ",";
            }
            if (obj.name == "FakeVent")
            {
                save += obj.GetComponent<PatchUpHandler>().durability + ",";
            }
			save += "\n";
        }
		save += "\n";
		save += "[PatchUps]\n";
		for(int i = 0; i < 4; i++)
		{
			foreach(Transform obj in tiles.Find(objLayers[i]))
			{
				if(obj.name != "Poster" && obj.name != "FakeWallBlock")
				{
					continue;
				}
				save += obj.name + "=";
				save += obj.position.x + ",";
				save += obj.position.y + ",";
				save += LayerMask.LayerToName(obj.gameObject.layer) + ",";
				if(obj.name == "FakeWallBlock")
				{
					save += obj.GetComponent<PatchUpHandler>().durability + ",";
				}
				save += "\n";
			}
		}
		save += "\n";
		save += "[Mines]\n";
		for(int i = 0; i < 4; i++)
		{
			foreach(Transform obj in tiles.Find(objLayers[i]))
			{
				if(obj.name != "Mine")
				{
					continue;
				}
				save += "Mine=";
				save += obj.position.x + ",";
				save += obj.position.y + ",";
				save += LayerMask.LayerToName(obj.gameObject.layer) + ",";
				save += "\n";
			}
		}
		save += "\n";
		save += "[Stepladders]\n";
		foreach(Transform obj in tiles.Find("GroundObjects"))
		{
			if (obj.name.Contains("Stepladder"))
			{
				save += "Stepladder=";
				save += obj.position.x + ",";
				save += obj.position.y + ",";
				save += "\n";
			}
		}
		save += "\n";
		save += "[EscapeObjects]\n";
		index = 0;
		for(int i = 0; i < 4; i++)
		{
			foreach(Transform obj in tiles.Find(objLayers[i]))
			{
				if (!obj.CompareTag("EscapeObject"))
				{
					index++;
					continue;
				}
				save += index + "=";
				save += obj.GetComponent<EscapeObjectHandler>().objectivesCleared + "\n";
			}
		}
		save += "\n";
		save += "[BedStuff]\n";//sheets, dummies, if a bed has sheets or pillow
		for(int i = 0; i < 4; i++)
		{
			foreach(Transform obj in tiles.Find(objLayers[i]))
			{
				if(obj.name.Contains("Dummy") || obj.name == "Sheet")
				{
					save += obj.name + "=";
					save += obj.position.x + ",";
					save += obj.position.y + "\n";
				}
				//if bed has sheets and or pillow
			}
		}
		//escape score stuff
		save += "\n";
		save += "[ToiletStuff]\n";
		index = 0;
		foreach(Transform toilet in toilets)
		{
			ToiletInv a = toilet.GetComponent<ToiletInv>();
			save += index + "=" + a.isClogged + "," + a.flushTimer + "\n";
			index++;
		}
    }
	public void Load()
	{
		//save if hole under patch up or something lol
		//save connected tiles on patchups and stuff
	}
}
