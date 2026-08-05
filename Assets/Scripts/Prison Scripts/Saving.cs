using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using Color = UnityEngine.Color;

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
	private Recruits recruitsScript;
	private GeneratorController genScript;
	private Escaping escapingScript;
	private CPCharlie charlieScript;
	private IGTimer igtScript;
	private UnlockDoors unlockDoorsScript;
	private Schedule scheduleScript;
	private List<Transform> slots = new List<Transform>();
	private ItemDataCreator creator;
	private Sprite clear;
    private int groundLayer;
    private int undergroundLayer;
    private int ventLayer;
    private int roofLayer;
    private int playerLayer;
    private int uiLayer;
    private int ventCoverLayer;
    private HoleClimb holeClimbScript;
    private Transform globalLight;
    private Transform undergroundLight;
	private NPCSleep npcSleepScript;
	private Death deathScript;
    private List<string> objLayers = new List<string>
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
	private List<string> tileLayers = new List<string>
	{
		"Underground", "Ground", "Vents", "Roof"
	};
    private List<string> bedNames = new List<string>
    {
        "BedHorizontal", "BedVertical", "PlayerBedHorizontal", "PlayerBedVertical"
    };
    private List<Transform> desks = new List<Transform>();
	private List<Transform> jobBoxes = new List<Transform>();
	private List<Transform> toilets = new List<Transform>();
	private List<Transform> cameras = new List<Transform>();
	public float jobQuotaBarSize = 0;
    private void Start()
	{
		player = RootObjectCache.GetRoot("Player").transform;
		inventoryScript = GetComponent<Inventory>();
		mc = RootObjectCache.GetRoot("MenuCanvas").transform;
		ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
		idInvScript = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>();
		missionAskScript = mc.Find("MissionPanel").GetComponent<MissionAsk>();
		lockdownScript = GetComponent<Lockdown>();
		payphoneScript = mc.Find("PayphoneMenuPanel").GetComponent<PayphoneMenu>();
		solitaryScript = GetComponent<Solitary>();
		routineScript = ic.Find("Time").GetComponent<Routine>();
		sittablesScript = GetComponent<Sittables>();
		aStar = RootObjectCache.GetRoot("A*").transform;
		shopsScript = GetComponent<Shops>();
		tiles = RootObjectCache.GetRoot("Tiles").transform;
		recruitsScript = GetComponent<Recruits>();
		genScript = GetComponent<GeneratorController>();
		escapingScript = GetComponent<Escaping>();
		charlieScript = GetComponent<CPCharlie>();
		igtScript = GetComponent<IGTimer>();
		unlockDoorsScript = GetComponent<UnlockDoors>();
		scheduleScript = ic.Find("Period").GetComponent<Schedule>();
		creator = GetComponent<ItemDataCreator>();
		clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
		foreach(Transform slot in ic.Find("GUIPanel"))
		{
			slots.Add(slot);
		}
        groundLayer = LayerMask.NameToLayer("Ground");
        undergroundLayer = LayerMask.NameToLayer("Underground");
        ventLayer = LayerMask.NameToLayer("Vents");
        roofLayer = LayerMask.NameToLayer("Roof");
        playerLayer = LayerMask.NameToLayer("Player");
        uiLayer = LayerMask.NameToLayer("UI");
        ventCoverLayer = LayerMask.NameToLayer("VentCovers");
        holeClimbScript = GetComponent<HoleClimb>();
        globalLight = RootObjectCache.GetRoot("GlobalLight").transform;
        undergroundLight = RootObjectCache.GetRoot("UndergroundLight").transform;
		npcSleepScript = GetComponent<NPCSleep>();
		deathScript = GetComponent<Death>();
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
					case "Camera":
						cameras.Add(obj);
						break;
                }
            }
        }
    }
    public void Save()
	{
		/*
		 *  if the player is on a desk or table, dont let the user save
			if the player has a desk, npc, or deer picked up, dont let the user save
			if in a fight, dont save
			if dead, dont save
			if the player is zipping, roping, or grappling, dont let the user save
			if the player is climbing into a seat, bed, desk, etc... dont let the user save
			if the player is exercising
		*/


		playerData = player.GetComponent<PlayerCollectionData>().playerData;
		
		string save = "";
		save += "[Map]\n";
		save += "FileName=" + currentMap.fileName + "\n";
		save += "Name=" + currentMap.mapName + "\n";
		save += "Time=" + routineScript.min + "," + routineScript.sec + "," + routineScript.day + "\n";
		save += "Period=" + scheduleScript.periodCode + "\n";
		save += "WardenHasGone=" + wardenWent + "\n";
		save += "\n";
		save += "[Player]\n";
		save += "Strength=" + playerData.strength + "\n";
		save += "Speed=" + playerData.speed + "\n";
		save += "Intellect=" + playerData.intellect + "\n";
		save += "Health=" + playerData.health + "\n";
		save += "Energy=" + playerData.energy + "\n";
		save += "Money=" + playerData.money + "\n";
		save += "Heat=" + playerData.heat + "\n";
		save += "Recruits=";
		foreach(Transform recruit in recruitsScript.recruits)
		{
			save += recruit.name.Replace("Inmate", "") + ",";
		}
		save += "\n";
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
			if (String.IsNullOrEmpty(item.itemData.inmateGiveName))
			{
				save += ";";
			}
			else
			{
                save += item.itemData.inmateGiveName + ";";
            }
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
			if (String.IsNullOrEmpty(idInvScript.idInv[i].itemData.inmateGiveName))
			{
				save += ";";
			}
			else
			{
                save += idInvScript.idInv[i].itemData.inmateGiveName + ";";
            }
        }
		save += "\n";
		save += "Position=" + player.position.x + "," + player.position.y + "\n";
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
		save += "SittablePos=";
		if(sittablesScript.sittable == null)
		{
			save += "null";
		}
		else
		{
			save += sittablesScript.sittable.transform.position.x + "," + sittablesScript.sittable.transform.position.y;
		}
		save += "\n";
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
                    if (String.IsNullOrEmpty(item.itemData.inmateGiveName))
                    {
                        save += ";";
                    }
                    else
                    {
                        save += item.itemData.inmateGiveName + ";";
                    }
                }
				save += "\n";
				save += "Position=" + npc.position.x + "," + npc.position.y + "\n";
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
				save += "\n";
            }
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
                if (String.IsNullOrEmpty(item.itemData.inmateGiveName))
                {
                    save += ";";
                }
                else
                {
                    save += item.itemData.inmateGiveName + ";";
                }
            }
            save += "\n";
        }
        save += "\n";
        save += "[Toilets]\n";
		index = 0;
        foreach (Transform toilet in toilets)
        {
            save += "Toilet" + index + "=";
            index++;
            foreach (ItemData item in toilet.GetComponent<ToiletInv>().toiletInv)
            {
                if (item == null)
                {
                    save += "null;";
                    continue;
                }

                save += item.id.ToString() + ",";
                save += item.currentDurability.ToString() + ",";
                save += item.forFavor.ToString() + ",";
                if (String.IsNullOrEmpty(item.inmateGiveName))
                {
                    save += ";";
                }
                else
                {
                    save += item.inmateGiveName + ";";
                }
            }
            save += "\n";
        }
        save += "\n";
        save += "[DeskPositions]\n";
		index = 0;
		foreach(Transform desk in desks)
		{
			save += "Desk" + index + "=" + desk.position.x + "," + desk.position.y + "\n";
			index++;
		}
        save += "\n";
        save += "[JobBoxes]\n";
		index = 0;
		foreach(Transform jobBox in jobBoxes)
		{
			save += "JobBox" + index + "=" + jobBox.GetComponent<ItemTransformerData>().heldID + "\n";
			index++;
		}
        save += "\n";
        save += "[Cameras]\n";
		index = 0;
		foreach(Transform camera in cameras)
		{
			save += "Camera" + index + "=" + camera.GetComponent<CameraController>().camTime + "\n";
			index++;
		}
        save += "\n";
        save += "[Generators]\n";
		save += "GenIsOff=" + genScript.genIsOff.ToString() + "\n";
        save += "\n";
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
                if (String.IsNullOrEmpty(item.GetComponent<ItemCollectionData>().itemData.inmateGiveName))
                {
                    save += ";";
                }
                else
                {
                    save += item.GetComponent<ItemCollectionData>().itemData.inmateGiveName + ";";
                }
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
				save += obj.GetComponent<PatchUpHandler>().hasHoleUnder.ToString() + ",";
				save += obj.GetComponent<PatchUpHandler>().holeDurability + ",";
				if(obj.name == "FakeWallBlock")
				{
					save += obj.GetComponent<PatchUpHandler>().durability + ",";
				}
				save += "\n";
			}
		}
        save += "\n";
        save += "[Stashes]\n";
        for (int i = 0; i < 4; i++)
        {
            foreach (Transform stash in tiles.Find(objLayers[i]))
            {
                if (stash.name == "Stash")
                {
                    save += stash.position.x + "," + stash.position.y + "," + LayerMask.LayerToName(stash.gameObject.layer) + "\n";
                }
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
					save += obj.position.y + ",";
					save += LayerMask.LayerToName(obj.gameObject.layer) + "\n";
				}
				else if (bedNames.Contains(obj.name))//0 = nothing, 1 = pillow only, 2 = all
				{
					save += obj.name + "=";
					save += obj.position.x + ",";
					save += obj.position.y + ",";
					save += LayerMask.LayerToName(obj.gameObject.layer) + ";";
					SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
					DataSender ds = DataSender.instance;
					if (obj.name.Contains("BedVertical"))
					{
						if (sr.sprite == ds.PrisonObjectImages[262])
						{
							save += "0";
						}
						else if(sr.sprite == ds.PrisonObjectImages[261])
						{
							save += "1";
						}
						else if(sr.sprite == ds.PrisonObjectImages[264])
						{
							save += "2";
						}
					}
					else if (obj.name.Contains("BedHorizontal"))
					{
						if(sr.sprite == ds.PrisonObjectImages[267])
						{
							save += "0";
						}
						else if(sr.sprite == ds.PrisonObjectImages[266])
						{
							save += "1";
						}
						else if(sr.sprite == ds.PrisonObjectImages[265])
						{
							save += "2";
						}
					}
					save += "\n";
				}
			}
		}
		save += "\n";
		save += "[EscapeScore]\n";
		save += "TotalHeat=" + escapingScript.totalHeat + "\n";
		save += "HighestHeat=" + escapingScript.highestHeat + "\n";
		save += "Good=" + escapingScript.good + "\n";
		save += "Bad=" + escapingScript.bad + "\n";
		save += "EffNum=" + escapingScript.effNum + "\n";
		save += "\n";
		save += "[ToiletStuff]\n";
		index = 0;
		foreach(Transform toilet in toilets)
		{
			ToiletInv a = toilet.GetComponent<ToiletInv>();
			save += index + "=" + a.isClogged + "," + a.flushTimer + "\n";
			index++;
		}
		save += "\n";
		save += "[Charlie]\n";
		save += "DestroyedGate=" + charlieScript.destroyedGate.ToString() + "\n";
		save += "\n";
		save += "[UnlockedDoors]\n";
		foreach(string type in unlockDoorsScript.unlockedTypes)
		{
			save += type + ",";
		}
		save += "\n";
		save += "\n";
		save += "[FoodTables]\n";
		for(int i = 0; i < 4; i++)
		{
			foreach(Transform obj in tiles.Find(objLayers[i]))
			{
				if(obj.name == "FoodTable")
				{
					save += "FoodTable=";
					save += obj.position.x + "," + obj.position.y + "," + LayerMask.LayerToName(obj.gameObject.layer) + "," + obj.GetComponent<FoodTableCounter>().foodCount + "\n";
				}
			}
		}
		save += "\n";
		save += "[Timers]\n";
		save += "IGT=" + igtScript.igt + "\n";
		save += "PausedIGT=" + igtScript.pausedIGT + "\n";

		string savePath = Path.Combine(Application.streamingAssetsPath, "Saves", "Save" + DataSender.instance.currentSave + ".ini");
		if (File.Exists(savePath))
		{
			File.Delete(savePath);
		}
		File.WriteAllText(savePath, save);
    }
	public IEnumerator Load()
	{
		//make sure to load connected tiles on patch ups
		//put bad objects where needed (items and prolly other stuff)

		//set player name, character, npc names, characters at the title screen load (NPCSave.cs)
		//intersept the npc set stuff by checking if the save file exists
		for(int i = 0; i < 15; i++)
		{
			yield return new WaitForEndOfFrame();
		}

		PlayerCollectionData playerColData = player.GetComponent<PlayerCollectionData>();
		DataSender ds = DataSender.instance;
		string[] saveFile = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Saves", "Save" + ds.currentSave + ".ini"));

		string time = GetINIVar("Map", "Time", saveFile);
		string[] timeVars = time.Split(",");
		routineScript.min = Convert.ToInt32(timeVars[0]);
		routineScript.sec = Convert.ToInt32(timeVars[1]);
		routineScript.day = Convert.ToInt32(timeVars[2]);
		scheduleScript.periodCode = GetINIVar("Map", "Period", saveFile);
		foreach(Transform npc in aStar)
		{
			if(npc.name == "Warden")
			{
				npc.GetComponent<ExtraNPCAI>().alreadyWentToday = GetINIVar("Map", "WardenHasGone", saveFile) == "True";
				break;
			}
		}
        playerColData.playerData.strength = Convert.ToInt32(GetINIVar("Player", "Strength", saveFile));
        playerColData.playerData.speed = Convert.ToInt32(GetINIVar("Player", "Speed", saveFile));
        playerColData.playerData.intellect = Convert.ToInt32(GetINIVar("Player", "Intellect", saveFile));
        playerColData.playerData.health = Convert.ToInt32(GetINIVar("Player", "Health", saveFile));
        playerColData.playerData.energy = Convert.ToInt32(GetINIVar("Player", "Energy", saveFile));
        playerColData.playerData.money = Convert.ToInt32(GetINIVar("Player", "Money", saveFile));
        playerColData.playerData.heat = Convert.ToInt32(GetINIVar("Player", "Heat", saveFile));
		string[] recruitNames = GetINIVar("Player", "Recruits", saveFile).Split(",");
		foreach(string recruitName in recruitNames)
		{
			foreach(Transform npc in aStar)
			{
				if(npc.name == recruitName)
				{
					recruitsScript.Recruit(npc, true, true);
				}
			}
		}
		if (scheduleScript.periodCode == "W")
		{
            float maxWidth = 280;
			float size = Convert.ToSingle(GetINIVar("Player", "JobQuotaBarSize", saveFile));
            GameObject bar = ic.Find("QuotaPanel").Find("BarLine").gameObject;
            if (bar.GetComponent<RectTransform>().sizeDelta.x + size > maxWidth)
            {
                bar.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, 25);
            }
            else
            {
                bar.GetComponent<RectTransform>().sizeDelta += new Vector2(size, 0);
            }
            bar.transform.localPosition = new Vector2((bar.GetComponent<RectTransform>().sizeDelta.x / 2f) - 140, -25);
        }
		List<ItemData> datas = GetItems(GetINIVar("Player", "Inventory", saveFile));
		for(int i = 0; i < 6; i++)
		{
			inventoryScript.inventory[i].itemData = datas[i];
			if (datas[i] == null)
			{
				slots[i].GetComponent<Image>().sprite = clear;
			}
			else
			{
                slots[i].GetComponent<Image>().sprite = datas[i].sprite;
            }
        }
		for(int i = 0; i < 2; i++)
		{
			idInvScript.idInv[i].itemData = datas[i + 6];
		}
        float posX = Convert.ToSingle(GetINIVar("Player", "Position", saveFile).Split(",")[0]);
        float posY = Convert.ToSingle(GetINIVar("Player", "Position", saveFile).Split(",")[1]);
		player.position = new Vector2(posX, posY);
		SendToLayer(GetINIVar("Player", "Layer", saveFile));
		playerColData.playerData.job = GetINIVar("Player", "Job", saveFile);

		string missionStr = GetINIVar("Player", "Missions", saveFile);
		foreach(string mission in missionStr.Split(";"))
		{
			if (string.IsNullOrEmpty(mission))
			{
				continue;
			}
			string[] missionVars = mission.Split(",");
			int item = -1;
			if (!String.IsNullOrEmpty(missionVars[1]))
			{
				item = Convert.ToInt32(missionVars[1]);
			}
			Mission miss = new Mission(missionVars[0], item, missionVars[2], missionVars[3], "", missionVars[4], Convert.ToInt32(missionVars[5]));
			missionAskScript.savedMissions.Add(miss);
		}

		string lockdownStr = GetINIVar("Player", "Lockdown", saveFile);
		string[] lockdownVars = lockdownStr.Split(",");
		if (lockdownVars[0] == "True")
		{
			lockdownScript.StartLockdown();
			lockdownScript.lockdownTime = Convert.ToInt32(lockdownVars[1]);
		}
		solitaryScript.inSolitary = GetINIVar("Player", "Solitary", saveFile) == "True";

		string[] hintVars = GetINIVar("Player", "Hints", saveFile).Split(",");
		payphoneScript.hint1Bought = hintVars[0] == "True";
		payphoneScript.hint2Bought = hintVars[1] == "True";
		payphoneScript.hint3Bought = hintVars[2] == "True";

		string sittableBool = GetINIVar("Player", "OnSittable", saveFile);
		string sittablePos = GetINIVar("Player", "SittablePos", saveFile);
		Dictionary<string, string> layerDict = new Dictionary<string, string>
		{
			{ "Ground", "GroundObjects" }, { "Underground", "UndergroundObjects" }, { "Vents", "VentObjects" }, { "Roof", "RoofObjects" }
		};
		if(sittableBool == "True")
		{
			float x = Convert.ToSingle(sittablePos.Split(",")[0]);
			float y = Convert.ToSingle(sittablePos.Split(",")[1]);
			Vector3 vect = new Vector2(x, y);
			foreach (Transform obj in tiles.Find(layerDict[LayerMask.LayerToName(player.gameObject.layer)]))
			{
				if(obj.position.x == x && obj.position.y == y)
				{
					sittablesScript.sittable = obj.gameObject;
					break;
				}
			}
			StartCoroutine(sittablesScript.ClimbSittable());
		}
		playerColData.playerData.hasFood = GetINIVar("Player", "HasFood", saveFile) == "True";

		foreach(Transform npc in aStar)
		{
			if (npc.name.Contains("Inmate") || npc.name.Contains("Guard"))
			{
				NPCCollectionData npcColData = npc.GetComponent<NPCCollectionData>();
				npcColData.npcData.strength = Convert.ToInt32(GetINIVar(npc.name, "Strength", saveFile));
				npcColData.npcData.speed = Convert.ToInt32(GetINIVar(npc.name, "Speed", saveFile));
				npcColData.npcData.intellect = Convert.ToInt32(GetINIVar(npc.name, "Intellect", saveFile));
				npcColData.npcData.opinion = Convert.ToInt32(GetINIVar(npc.name, "Opinion", saveFile));
				npcColData.npcData.displayName = GetINIVar(npc.name, "Name", saveFile);
				npcColData.npcData.charNum = Convert.ToInt32(GetINIVar(npc.name, "Character", saveFile));
				List<ItemData> aDatas = GetItems(GetINIVar(npc.name, "Inventory", saveFile));
				for (int i = 0; i < 8; i++)
				{
					npcColData.npcData.inventory[i].itemData = aDatas[i];
				}

				string posStr = GetINIVar(npc.name, "Position", saveFile);
				float x = Convert.ToSingle(posStr.Split(","));
				float y = Convert.ToSingle(posStr.Split(","));
				Vector2 pos = new Vector2(x, y);
				npc.position = pos;

				npcColData.npcData.hasFood = GetINIVar(npc.name, "HasFood", saveFile) == "True";
				if (GetINIVar(npc.name, "Sleeping", saveFile) == "True")
				{
					foreach (Transform obj in tiles.Find("GroundObjects"))
					{
						if (obj.name.Contains("Bed"))
						{
							if (Vector2.Distance(npc.position, obj.position) <= .8f)
							{
								npcSleepScript.Sleep(npc.gameObject, obj.gameObject);
								break;
							}
						}
					}
				}

				npcColData.npcData.isTied = GetINIVar(npc.name, "Tied", saveFile) == "True";
				if (GetINIVar(npc.name, "Dead", saveFile) == "True")
				{
					deathScript.KillNPC(npc.gameObject);
				}

				npcColData.npcData.job = GetINIVar(npc.name, "Job", saveFile);
				if(GetINIVar(npc.name, "Mission", saveFile) != "null")
				{
					string[] missionVars = GetINIVar(npc.name, "Mission", saveFile).Split(",");
					int item = -1;
					if (!String.IsNullOrEmpty(missionVars[1]))
					{
						item = Convert.ToInt32(missionVars[1]);
					}
					Mission miss = new Mission(missionVars[0], item, missionVars[2], missionVars[3], missionVars[4], missionVars[5], Convert.ToInt32(missionVars[6]));
					npcColData.npcData.mission = miss;
				}

				if(GetINIVar(npc.name, "Shop", saveFile) != "null")
				{
					int shop = 1;
					if(shopsScript.shop1NPC != null)
					{
						shopsScript.shop1NPC = npc.gameObject;
						shop = 1;
					}
					else if(shopsScript.shop2NPC != null)
					{
						shopsScript.shop2NPC = npc.gameObject;
						shop = 2;
					}
					string shopRaw = GetINIVar(npc.name, "Shop", saveFile);
					string[] shopVars = shopRaw.Split(",");
					List<ItemData> bDatas = new List<ItemData>();
					for (int i = 0; i < 4; i++)
					{
						if (shopVars[i] == "null")
						{
							bDatas.Add(null);
						}
						else
						{
							bDatas.Add(creator.CreateItemData(Convert.ToInt32(shopVars[i])));
						}
					}
					for(int i = 0; i < 4; i++)
					{
						if(shop == 1)
						{
							shopsScript.shop1[i].itemData = bDatas[i];
						}
						else if(shop == 2)
						{
							shopsScript.shop2[i].itemData = bDatas[i];
						}
					}
				}
            }
        }


    }
    private void SendToLayer(string layer)
	{
        switch (layer)
        {
            case "Underground":
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, undergroundLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, undergroundLayer, false);
                player.GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                tiles.Find("UndergroundObjects").gameObject.SetActive(true);
                foreach (SpriteRenderer sr in holeClimbScript.brokenTileSRs)
                {
                    if (sr != null)
                    {
                        sr.sortingLayerName = "UndergroundVisible";
                    }
                }
                undergroundLight.gameObject.SetActive(true);
                globalLight.gameObject.SetActive(false);
                holeClimbScript.isUnderground = true;
                break;
            case "Ground":
                player.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                tiles.Find("RoofTiles").gameObject.SetActive(false);
                tiles.Find("RoofObjects").gameObject.SetActive(false);
                tiles.Find("VentTiles").gameObject.SetActive(false);
                tiles.Find("VentObjects").gameObject.SetActive(false);
                tiles.Find("RoofShadowPlane").gameObject.SetActive(false);
                tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundObjects").gameObject.SetActive(false);
                foreach (SpriteRenderer sr in holeClimbScript.brokenTileSRs)
                {
                    if (sr != null)
                    {
                        sr.sortingLayerName = "Underground";
                    }
                }
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
                undergroundLight.gameObject.SetActive(false);
                globalLight.gameObject.SetActive(true);
                holeClimbScript.isUnderground = false;
                break;
            case "Vents":
                player.GetComponent<SpriteRenderer>().sortingLayerName = "Vents";
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingLayerName = "Vents";
                tiles.Find("RoofTiles").gameObject.SetActive(false);
                tiles.Find("RoofObjects").gameObject.SetActive(false);
                tiles.Find("VentTiles").gameObject.SetActive(true);
                tiles.Find("VentObjects").gameObject.SetActive(true);
                tiles.Find("RoofShadowPlane").gameObject.SetActive(false);
                tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundObjects").gameObject.SetActive(false);
                foreach (SpriteRenderer sr in holeClimbScript.brokenTileSRs)
                {
                    if (sr != null)
                    {
                        sr.sortingLayerName = "Underground";
                    }
                }
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, false);
                Physics2D.IgnoreLayerCollision(uiLayer, ventCoverLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, ventLayer, false);
                VentEnable();
                undergroundLight.gameObject.SetActive(false);
                globalLight.gameObject.SetActive(true);
                holeClimbScript.isUnderground = false;
                break;
            case "Roof":
                player.GetComponent<SpriteRenderer>().sortingLayerName = "Roof";
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingLayerName = "Roof";
                tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                tiles.Find("RoofTiles").gameObject.SetActive(true);
                tiles.Find("RoofObjects").gameObject.SetActive(true);
                tiles.Find("VentTiles").gameObject.SetActive(false);
                tiles.Find("VentObjects").gameObject.SetActive(false);
                tiles.Find("RoofShadowPlane").gameObject.SetActive(true);
                tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundObjects").gameObject.SetActive(false);
                foreach (SpriteRenderer sr in holeClimbScript.brokenTileSRs)
                {
                    if (sr != null)
                    {
                        sr.sortingLayerName = "Underground";
                    }
                }
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, roofLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, roofLayer, false);
                undergroundLight.gameObject.SetActive(false);
                globalLight.gameObject.SetActive(true);
                holeClimbScript.isUnderground = false;
                break;
        }
    }
    private void DisableAllLayerCollisions()
    {
        Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, roofLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventCoverLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, roofLayer, true);
    }
    public void VentEnable()
    {
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = true;
        Color color = tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color;
        color.a = 235f / 256f;
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color = color;

        SpriteRenderer ventTilesSpriteRenderer = tiles.Find("VentTiles").GetComponent<SpriteRenderer>();
        SpriteRenderer[] ventObjectSpriteRenderers = tiles.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
        ventTilesSpriteRenderer.color = new Color(ventTilesSpriteRenderer.color.r, ventTilesSpriteRenderer.color.g, ventTilesSpriteRenderer.color.b, 1);
        foreach (SpriteRenderer sr in ventObjectSpriteRenderers)
        {
            Color aColor = sr.color;
            aColor.a = 1;
            sr.color = aColor;
        }
    }
    private List<ItemData> GetItems(string itemStr)
	{
		string[] items = itemStr.Split(";");
		List<ItemData> datas = new List<ItemData>();
		foreach(string item in items)
		{
			if(item == "null")
			{
				datas.Add(null);
				continue;
			}
			else if (string.IsNullOrEmpty(item))
			{
				continue;
			}

			string[] itemVars = item.Split(",");
			int id = Convert.ToInt32(itemVars[0]);
			int currentDurability = Convert.ToInt32(itemVars[1]);
			bool forFavor = itemVars[2] == "True";
			string inmateGiveName = itemVars[3];
			ItemData data = creator.CreateItemData(id);
			data.currentDurability = currentDurability;
			data.forFavor = forFavor;
			if (String.IsNullOrEmpty(inmateGiveName))
			{
				data.inmateGiveName = null;
			}
			else
			{
				data.inmateGiveName = inmateGiveName;
			}
			datas.Add(data);
		}
		return datas;
	}
    public List<string> GetINISet(string header, string[] file)
    {
        int startLine = -1;
        int endLine = file.Length;

        // Find the header line
        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains($"[{header}]"))
            {
                startLine = i + 1; // Start after the header
                break;
            }
        }

        if (startLine == -1)
            return new List<string>(); // Header not found

        // Find the next header or end of file
        for (int i = startLine; i < file.Length; i++)
        {
            if (file[i].StartsWith("[") && file[i].EndsWith("]"))
            {
                endLine = i;
                break;
            }
        }

        List<string> setList = new List<string>();
        for (int i = startLine; i < endLine; i++)
        {
            if (file[i].Contains('='))
            {
                setList.Add(file[i]);
            }
        }

        return setList;
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length; j++)
                {
                    if (file[j].Split('=')[0] == varName)
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }



        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');
        return parts[1];
    }
    public string GetINISetVar(string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Split('=')[0] == varName)
            {
                line = file[i];
                break;
            }
        }

        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');
        return parts[1];
    }
}
