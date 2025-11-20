using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class LoadPrison : MonoBehaviour
{
    private AudioClip music;
    private Sprite tileset;
    private Sprite ground;
    private Sprite icon;

    private List<Sprite> ItemSprites = new List<Sprite>();
    private List<Sprite> NPCSprites = new List<Sprite>();
    private List<Sprite> PrisonObjectSprites = new List<Sprite>();
    private List<Sprite> UISprites = new List<Sprite>();

    private ApplyPrisonData dataScript;

    private Dictionary<string, int> tilesetDict = new Dictionary<string, int>()
    {
        { "tutorial", 0 }, { "perks", 1 }, { "stalagflucht", 2 }, { "shanktonstatepen", 3 },
        { "jungle", 4 }, { "sanpancho", 5 }, { "irongate", 6 }, { "CCL", 7 },
        { "BC", 8 }, { "TOL", 9 }, { "pcpen", 10 }, { "SS", 11 },
        { "DTAF", 12 }, { "escapeteam", 13 }, { "alca", 14 }, { "EA", 15 },
        { "campepsilon", 16 }, { "fortbamford", 17 }
    };

    private Dictionary<string, string> prisonDict = new Dictionary<string, string>() //this is for converting the result text to the real prison names
    {
        { "perks", "perks" }, { "stalag", "stalagflucht" }, { "shankton", "shanktonstatepen" },
        { "jungle", "jungle" }, { "sanpancho", "sanpancho" }, { "irongate", "irongate" },
        { "JC", "CCL" }, { "BC", "BC" }, { "london", "TOL" }, { "PCP", "pcpen" }, { "SS", "SS" },
        { "DTAF", "DTAF" }, { "ET", "escapeteam" }, { "alca", "alca" }, { "fhurst", "EA" },
        { "epsilon", "campepsilon" }, { "bamford", "fortbamford" }, { "tutorial", "tutorial" }
    };
    private Dictionary<int, string> layerDict = new Dictionary<int, string>()
    {
        { 0, "Underground" }, { 1, "Ground" }, { 2, "Vents" }, { 3, "Roof" }
    };

    //these dictionaries compare the tile ID to what tile it should be based on the prison
    private Dictionary<int, string> perksDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstNoShad" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstacle" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "obstNoShad" }, { 59, "elec" },
        { 60, "obstacle" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstNoShad" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "inFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "inFloor" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "inFloor" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "obstNoShad" },
        { 92, "ne" }, { 93, "outFloor" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "obstNoShad" }, { 99, "obstNoShad" }
    };
    private Dictionary<int, string> stalagDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstNoShad" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstacle" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "inFloor" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "inFloor" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "inFloor" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "obstNoShad" }, { 59, "elec" },
        { 60, "obstacle" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstNoShad" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "outFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "outFloor" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstacle" }, { 79, "outFloor" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "outFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "outFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "outFloor" },
        { 92, "ne" }, { 93, "obstNoShad" }, { 94, "obstNoShad" }, { 95, "outFloor" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "outFloor" }, { 99, "outFloor" }
    };
    private Dictionary<int, string> shanktonDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstNoShad" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstacle" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstNoShad" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "outFloor" }, { 58, "inFloor" }, { 59, "elec" },
        { 60, "obstacle" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstacle" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "inFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "inFloor" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "inFloor" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "obstNoShad" },
        { 92, "ne" }, { 93, "obstNoShad" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "obstNoShad" }, { 99, "obstNoShad" }
    };
    private Dictionary<int, string> jungleDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstNoShad" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "nw" }, { 43, "obstacle" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "ne" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "se" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstacle" }, { 54, "sw" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "outFloor" }, { 59, "elec" },
        { 60, "chip" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "chip" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "outFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "outFloor" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "outFloor" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "outFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstacle" }, { 87, "outFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstacle" }, { 91, "outFloor" },
        { 92, "ne" }, { 93, "obstNoShad" }, { 94, "obstNoShad" }, { 95, "outFloor" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "outFloor" }, { 99, "outFloor" }
    };
    private Dictionary<int, string> panchoDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstNoShad" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstNoShad" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstacle" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstacle" }, { 58, "obstNoShad" }, { 59, "elec" },
        { 60, "obstacle" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstNoShad" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "obstNoShad" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "obstNoShad" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstacle" }, { 79, "obstNoShad" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstacle" }, { 83, "obstNoShad" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "obstNoShad" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "obstNoShad" },
        { 92, "ne" }, { 93, "obstNoShad" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "obstNoShad" }, { 99, "obstNoShad" }
    };
    private Dictionary<int, string> hmpDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstNoShad" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstNoShad" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstacle" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstacle" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "inFloor" }, { 59, "elec" },
        { 60, "obstacle" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstNoShad" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "inFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "inFloor" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "inFloor" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "obstNoShad" },
        { 92, "ne" }, { 93, "outFloor" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "obstNoShad" }, { 99, "outFloor" }
    };
    private Dictionary<int, string> tutorialDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstacle" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "obstNoShad" }, { 59, "elec" },
        { 60, "obstNoShad" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstNoShad" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "obstNoShad" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "obstNoShad" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "obstNoShad" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "obstNoShad" },
        { 92, "ne" }, { 93, "outFloor" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "obstNoShad" }, { 99, "obstNoShad" }
    };
    private Dictionary<int, string> jingleDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstacle" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstacle" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "chip" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "chip" }, { 43, "chip" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "outFloor" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "outFloor" }, { 58, "outFloor" }, { 59, "elec" },
        { 60, "obstNoShad" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "cut" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "inFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "obstNoShad" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "obstNoShad" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "obstNoShad" },
        { 92, "ne" }, { 93, "box" }, { 94, "box" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "outFloor" }, { 98, "inFloor" }, { 99, "obstNoShad" }
    };
    private Dictionary<int, string> bcDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "inFloor" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstacle" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "nw" }, { 43, "obstacle" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "ne" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "se" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstacle" }, { 54, "sw" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "outFloor" }, { 59, "elec" },
        { 60, "chip" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "chip" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "obstNoShad" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "obstNoShad" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "obstNoShad" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "obstNoShad" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "obstNoShad" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "inFloor" },
        { 92, "ne" }, { 93, "obstNoShad" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "outFloor" }, { 98, "obstNoShad" }, { 99, "obstNoShad" }
    };
    private Dictionary<int, string> tolDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "outFloor" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "outFloor" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstNoShad" }, { 17, "box" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstNoShad" }, { 22, "obstNoShad" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "outFloor" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstacle" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstacle" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstacle" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "outFloor" }, { 54, "obstacle" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "outFloor" }, { 58, "outFloor" }, { 59, "elec" },
        { 60, "obstacle" }, { 61, "obstacle" }, { 62, "obstNoShad" }, { 63, "elec" },
        { 64, "outFloor" }, { 65, "obstacle" }, { 66, "sw" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "obstacle" }, { 71, "inFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "se" }, { 75, "outFloor" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "outFloor" }, { 79, "inFloor" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "outFloor" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "outFloor" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "outFloor" }, { 91, "obstNoShad" },
        { 92, "ne" }, { 93, "outFloor" }, { 94, "obstacle" }, { 95, "outFloor" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "outFloor" }, { 99, "outFloor" }
    };
    private Dictionary<int, string> pcpDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstNoShad" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "inFloor" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "inFloor" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "inFloor" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "inFloor" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "inFloor" }, { 43, "inFloor" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "outFloor" }, { 58, "outFloor" }, { 59, "elec" },
        { 60, "obstacle" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstacle" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "inFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "inFloor" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "inFloor" }, { 79, "inFloor" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "inFloor" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "outFloor" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "inFloor" }, { 90, "outFloor" }, { 91, "outFloor" },
        { 92, "ne" }, { 93, "inFloor" }, { 94, "outFloor" }, { 95, "outFloor" },
        { 96, "nw" }, { 97, "outFloor" }, { 98, "outFloor" }, { 99, "outFloor" }
    };
    private Dictionary<int, string> ssDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstNoShad" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstacle" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "chip" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "chip" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "inFloor" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "inFloor" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "inFloor" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstacle" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstacle" }, { 58, "obstNoShad" }, { 59, "elec" },
        { 60, "chip" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "se" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "obstNoShad" },
        { 72, "outFloor" }, { 73, "obstacle" }, { 74, "outFloor" }, { 75, "obstNoShad" },
        { 76, "cut" }, { 77, "inFloor" }, { 78, "obstNoShad" }, { 79, "obstNoShad" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "sw" }, { 83, "obstNoShad" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "inFloor" }, { 87, "obstNoShad" },
        { 88, "sw" }, { 89, "obstacle" }, { 90, "obstacle" }, { 91, "obstacle" },
        { 92, "ne" }, { 93, "obstacle" }, { 94, "nw" }, { 95, "ne" },
        { 96, "nw" }, { 97, "inFloor" }, { 98, "sw" }, { 99, "se" }
    };
    private Dictionary<int, string> dtafDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "sw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "nw" }, { 14, "obstacle" }, { 15, "ne" },
        { 16, "inFloor" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "obstacle" },
        { 20, "chip" }, { 21, "sw" }, { 22, "obstacle" }, { 23, "se" },
        { 24, "chip" }, { 25, "obstNoShad" }, { 26, "obstNoShad" }, { 27, "obstNoShad" },
        { 28, "obstacle" }, { 29, "obstacle" }, { 30, "ne" }, { 31, "se" },
        { 32, "nw" }, { 33, "obstacle" }, { 34, "ne" }, { 35, "ne" },
        { 36, "obstNoShad" }, { 37, "obstacle" }, { 38, "chip" }, { 39, "nw" },
        { 40, "sw" }, { 41, "obstacle" }, { 42, "se" }, { 43, "nw" },
        { 44, "nw" }, { 45, "obstacle" }, { 46, "ne" }, { 47, "ne" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstacle" }, { 51, "se" },
        { 52, "sw" }, { 53, "obstacle" }, { 54, "se" }, { 55, "sw" },
        { 56, "obstNoShad" }, { 57, "obstNoShad" }, { 58, "obstNoShad" }, { 59, "inFloor" },
        { 60, "obstNoShad" }, { 61, "obstacle" }, { 62, "obstNoShad" }, { 63, "inFloor" },
        { 64, "obstNoShad" }, { 65, "obstNoShad" }, { 66, "obstNoShad" }, { 67, "obstacle" },
        { 68, "obstacle" }, { 69, "obstacle" }, { 70, "inFloor" }, { 71, "obstacle" },
        { 72, "obstacle" }, { 73, "obstacle" }, { 74, "inFloor" }, { 75, "obstacle" },
        { 76, "inFloor" }, { 77, "inFloor" }, { 78, "inFloor" }, { 79, "obstacle" },
        { 80, "obstNoShad" }, { 81, "obstNoShad" }, { 82, "obstacle" }, { 83, "obstacle" },
        { 84, "obstacle" }, { 85, "inFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "obstNoShad" }, { 89, "obstacle" }, { 90, "inFloor" }, { 91, "obstNoShad" },
        { 92, "se" }, { 93, "obstacle" }, { 94, "obstacle" }, { 95, "obstacle" },
        { 96, "sw" }, { 97, "sw" }, { 98, "obstacle" }, { 99, "se" }
    };
    private Dictionary<int, string> etDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "outFloor" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "outFloor" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "outFloor" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "outFloor" },
        { 16, "obstacle" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "outFloor" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstacle" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "sw" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "nw" }, { 43, "obstacle" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "ne" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "se" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstacle" }, { 54, "sw" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "outFloor" }, { 58, "outFloor" }, { 59, "elec" },
        { 60, "chip" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "chip" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "outFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "outFloor" },
        { 76, "obstNoShad" }, { 77, "highFloor" }, { 78, "inFloor" }, { 79, "outFloor" },
        { 80, "obstNoShad" }, { 81, "outFloor" }, { 82, "inFloor" }, { 83, "outFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstacle" }, { 87, "obstacle" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstacle" }, { 91, "outFloor" },
        { 92, "ne" }, { 93, "obstNoShad" }, { 94, "outFloor" }, { 95, "outFloor" },
        { 96, "nw" }, { 97, "outFloor" }, { 98, "outFloor" }, { 99, "outFloor" }
    };
    private Dictionary<int, string> alcaDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstacle" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "box" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "inFloor" }, { 59, "elec" },
        { 60, "obstNoShad" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstNoShad" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "outFloor" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "outFloor" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "outFloor" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "inFloor" },
        { 92, "ne" }, { 93, "outFloor" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "outFloor" }, { 99, "obstNoShad" }
    };
    private Dictionary<int, string> fhurstDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstacle" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstNoShad" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstNoShad" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "inFloor" }, { 59, "elec" },
        { 60, "obstacle" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstNoShad" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "obstNoShad" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "obstNoShad" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "obstNoShad" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "obstNoShad" },
        { 92, "ne" }, { 93, "outFloor" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "obstNoShad" }, { 98, "obstNoShad" }, { 99, "obstNoShad" }
    };
    private Dictionary<int, string> epsilonDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstacle" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "inFloor" }, { 59, "elec" },
        { 60, "obstNoShad" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstNoShad" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "obstNoShad" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "obstNoShad" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "obstNoShad" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "obstNoShad" },
        { 92, "ne" }, { 93, "outFloor" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "nw" }, { 97, "outFloor" }, { 98, "obstNoShad" }, { 99, "obstNoShad" }
    };
    private Dictionary<int, string> bamfordDict = new Dictionary<int, string>()
    {
        { 0, "inFloor" }, { 1, "inFloor" }, { 2, "inFloor" }, { 3, "nw" },
        { 4, "inFloor" }, { 5, "box" }, { 6, "obstacle" }, { 7, "obstacle" },
        { 8, "inFloor" }, { 9, "obstacle" }, { 10, "obstacle" }, { 11, "obstacle" },
        { 12, "obstNoShad" }, { 13, "obstacle" }, { 14, "obstacle" }, { 15, "sw" },
        { 16, "obstacle" }, { 17, "obstacle" }, { 18, "obstacle" }, { 19, "ne" },
        { 20, "chip" }, { 21, "obstacle" }, { 22, "bar" }, { 23, "obstNoShad" },
        { 24, "chip" }, { 25, "horiLedge" }, { 26, "obstacle" }, { 27, "obstacle" },
        { 28, "nw" }, { 29, "vertLedge" }, { 30, "obstacle" }, { 31, "se" },
        { 32, "ne" }, { 33, "obstacle" }, { 34, "obstNoShad" }, { 35, "obstacle" },
        { 36, "sw" }, { 37, "obstacle" }, { 38, "obstNoShad" }, { 39, "obstacle" },
        { 40, "se" }, { 41, "obstacle" }, { 42, "obstNoShad" }, { 43, "obstNoShad" },
        { 44, "obstacle" }, { 45, "obstacle" }, { 46, "obstNoShad" }, { 47, "obstacle" },
        { 48, "obstacle" }, { 49, "obstacle" }, { 50, "obstNoShad" }, { 51, "obstacle" },
        { 52, "obstacle" }, { 53, "obstNoShad" }, { 54, "obstNoShad" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstNoShad" }, { 58, "obstNoShad" }, { 59, "elec" },
        { 60, "obstNoShad" }, { 61, "obstacle" }, { 62, "outFloor" }, { 63, "elec" },
        { 64, "obstNoShad" }, { 65, "obstacle" }, { 66, "outFloor" }, { 67, "obstacle" },
        { 68, "inFloor" }, { 69, "lowFloor" }, { 70, "outFloor" }, { 71, "obstNoShad" },
        { 72, "outFloor" }, { 73, "medFloor" }, { 74, "outFloor" }, { 75, "obstNoShad" },
        { 76, "cut" }, { 77, "highFloor" }, { 78, "obstNoShad" }, { 79, "obstNoShad" },
        { 80, "cut" }, { 81, "outFloor" }, { 82, "obstNoShad" }, { 83, "inFloor" },
        { 84, "se" }, { 85, "outFloor" }, { 86, "obstNoShad" }, { 87, "inFloor" },
        { 88, "sw" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "inFloor" },
        { 92, "ne" }, { 93, "outFloor" }, { 94, "obstNoShad" }, { 95, "inFloor" },
        { 96, "nw" }, { 97, "outFloor" }, { 98, "inFloor" }, { 99, "inFloor" }
    };
    private Dictionary<int, string> customDict = new Dictionary<int, string>()
    {
        { 0, "chip" }, { 1, "chip" }, { 2, "chip" }, { 3, "chip" },
        { 4, "chip" }, { 5, "chip" }, { 6, "chip" }, { 7, "chip" },
        { 8, "cut" }, { 9, "cut" }, { 10, "cut" }, { 11, "cut" },
        { 12, "cut" }, { 13, "cut" }, { 14, "cut" }, { 15, "cut" },
        { 16, "bar" }, { 17, "bar" }, { 18, "elec" }, { 19, "elec" },
        { 20, "nw" }, { 21, "nw" }, { 22, "nw" }, { 23, "nw" },
        { 24, "ne" }, { 25, "ne" }, { 26, "ne" }, { 27, "ne" },
        { 28, "sw" }, { 29, "sw" }, { 30, "sw" }, { 31, "sw" },
        { 32, "se" }, { 33, "se" }, { 34, "se" }, { 35, "se" },
        { 36, "outFloor" }, { 37, "outFloor" }, { 38, "outFloor" }, { 39, "outFloor" },
        { 40, "outFloor" }, { 41, "outFloor" }, { 42, "outFloor" }, { 43, "outFloor" },
        { 44, "inFloor" }, { 45, "inFloor" }, { 46, "inFloor" }, { 47, "inFloor" },
        { 48, "inFloor" }, { 49, "inFloor" }, { 50, "inFloor" }, { 51, "inFloor" },
        { 52, "box" }, { 53, "box" }, { 54, "obstacle" }, { 55, "obstacle" },
        { 56, "obstacle" }, { 57, "obstacle" }, { 58, "obstacle" }, { 59, "obstacle" },
        { 60, "obstacle" }, { 61, "obstacle" }, { 62, "obstacle" }, { 63, "obstacle" },
        { 64, "obstacle" }, { 65, "obstacle" }, { 66, "obstacle" }, { 67, "obstacle" },
        { 68, "obstacle" }, { 69, "obstacle" }, { 70, "obstacle" }, { 71, "obstacle" },
        { 72, "horiLedge" }, { 73, "vertLedge" }, { 74, "highFloor" }, { 75, "medFloor" },
        { 76, "lowFloor" }, { 77, "obstNoShad" }, { 78, "obstNoShad" }, { 79, "obstNoShad" },
        { 80, "obstNoShad" }, { 81, "obstNoShad" }, { 82, "obstNoShad" }, { 83, "obstNoShad" },
        { 84, "obstNoShad" }, { 85, "obstNoShad" }, { 86, "obstNoShad" }, { 87, "obstNoShad" },
        { 88, "obstNoShad" }, { 89, "obstNoShad" }, { 90, "obstNoShad" }, { 91, "obstNoShad" },
        { 92, "obstNoShad" }, { 93, "obstNoShad" }, { 94, "obstNoShad" }, { 95, "obstNoShad" },
        { 96, "obstNoShad" }, { 97, "obstNoShad" }, { 98, "obstNoShad" }, { 99, "obstNoShad" }
    };

    private GetGivenData givenDataScript;
    private DataSender dataSenderScript;

    private Transform tiles;


    [SerializeField]
    public Map currentMap;
    private void Start()
    {
        givenDataScript = GetGivenData.instance;
        dataSenderScript = DataSender.instance;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        StartCoroutine(LoadWait());
    }
    private IEnumerator LoadWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        StartLoad();
    }
    public void StartLoad()
    {
        currentMap = MakeMapObject(dataSenderScript.currentMapPath);

        foreach(char c in currentMap.tilesetStr)
        {
            Debug.Log(c);
        }

        Dictionary<int, string> currentTileDict;
        currentTileDict = customDict;
        switch (currentMap.tilesetStr)
        {
            case "perks":
                currentTileDict = perksDict;
                break;
            case "stalag":
                currentTileDict = stalagDict;
                break;
            case "shankton":
                currentTileDict = shanktonDict;
                break;
            case "jungle":
                currentTileDict = jungleDict;
                break;
            case "sanpancho":
                currentTileDict = panchoDict;
                break;
            case "irongate":
                currentTileDict = hmpDict;
                break;
            case "JC":
                currentTileDict = jingleDict;
                break;
            case "BC":
                currentTileDict = bcDict;
                break;
            case "london":
                currentTileDict = tolDict;
                break;
            case "PCP":
                currentTileDict = pcpDict;
                break;
            case "SS":
                currentTileDict = ssDict;
                break;
            case "DTAF":
                currentTileDict = dtafDict;
                break;
            case "ET":
                currentTileDict = etDict;
                break;
            case "alca":
                currentTileDict = alcaDict;
                break;
            case "fhurst":
                currentTileDict = fhurstDict;
                break;
            case "epsilon":
                currentTileDict = epsilonDict;
                break;
            case "bamford":
                currentTileDict = bamfordDict;
                break;
            case "tutorial":
                currentTileDict = tutorialDict;
                break;
            case "Custom":
                currentTileDict = customDict;
                break;
        }

        SetGround();
        SetTiles(currentTileDict);
        SetObjects();
        //set the zones when you need to
    }
    private void SetGround()
    {
        Texture2D groundTex;
        string groundChoice = currentMap.groundStr;
        Debug.Log("Setting " + groundChoice);

        if (groundChoice != "Custom")
        {
            if (groundChoice == "black")
            {
                groundTex = givenDataScript.groundTextureList[19];
            }
            else
            {
                int prisonIndex = tilesetDict[prisonDict[groundChoice]];
                Debug.Log("prisonIndex = " + prisonIndex);
                groundTex = givenDataScript.groundTextureList[prisonIndex];
            }
        }
        else
        {
            groundTex = ground.texture;
        }

        groundTex.filterMode = FilterMode.Point;

        bool isTiled = false;
        if (groundChoice != "BC" && groundChoice != "JC" && groundChoice != "DTAF" && groundChoice != "Custom")
        {
            groundTex = textureCornerGet(groundTex, 16, 16);
            isTiled = true;
        }

        Transform groundTransform = tiles.Find("GroundPlane");

        Sprite groundSprite = Sprite.Create(groundTex, new Rect(0, 0, groundTex.width, groundTex.height), new Vector2(.5f, .5f), 100.0f);

        groundTransform.GetComponent<SpriteRenderer>().sprite = groundSprite;

        if (isTiled)
        {
            groundTransform.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            groundTransform.GetComponent<SpriteRenderer>().size = new Vector2(currentMap.sizeX * .16f, currentMap.sizeY * .16f);
        }
        else
        {
            groundTransform.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
            groundTransform.GetComponent<SpriteRenderer>().size = new Vector2(groundTex.width * .01f, groundTex.height * .01f);
        }

        //set underground plane
        tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sprite = TextureToSprite(givenDataScript.groundTextureList[18]);
        tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().size = new Vector2(currentMap.sizeX * .16f, currentMap.sizeY * .16f);

        //set ground sizes
        int x = currentMap.sizeX;
        int y = currentMap.sizeY;

        if (isTiled)
        {
            groundTransform.position = new Vector2((x - 1) * .8f, (y - 1) * .8f);
        }
        else
        {
            float sizeX = groundTransform.GetComponent<SpriteRenderer>().size.x;
            float sizeY = groundTransform.GetComponent<SpriteRenderer>().size.y;

            groundTransform.position = new Vector2(-.8f, -.8f);
            groundTransform.position += new Vector3(sizeX / 2f, sizeY / 2f, 0);
        }
        tiles.Find("UndergroundPlane").position = new Vector2((x - 1) * .8f, (y - 1) * .8f);
    }
    private void SetTiles(Dictionary<int, string> tileDict)
    {
        int tilesetID = tilesetDict[currentMap.tilesetStr];
        List<Sprite> tileList = SliceAndDice(currentMap.tileset);
        foreach (int[] tileVars in currentMap.tilesList)
        {
            if (tileVars[0] == 100) //if its an empty tile
            {
                Vector3 emptyTilePos = new Vector3((tileVars[1] * 1.6f) - 1.6f, (tileVars[2] * 1.6f) - 1.6f, 0);
                GameObject emptyTile = new GameObject("Empty");
                emptyTile.transform.position = emptyTilePos;
                emptyTile.AddComponent<TileCollectionData>().tileData = new TileData();
                emptyTile.GetComponent<TileCollectionData>().tileData.tileType = "outFloor";
                emptyTile.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
                emptyTile.GetComponent<TileCollectionData>().tileData.holeStability = -1;
                emptyTile.AddComponent<NavMeshModifier>();
                emptyTile.AddComponent<BoxCollider2D>().isTrigger = true;
                emptyTile.GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 1.6f);
                emptyTile.tag = "Digable";
                emptyTile.layer = 10;
                emptyTile.transform.parent = tiles.Find("Ground");
                continue;
            }
            
            string tileType = tileDict[tileVars[0]];
            Debug.Log("Tile ID: " + tileVars[0] + ", TileType: " + tileType);
            string tileLayer = layerDict[tileVars[3]];

            GameObject tile;
            if(tileType == "highFloor" || tileType == "medFloor" || tileType == "lowFloor" ||
                tileType == "outFloor" || tileType == "inFloor")
            {
                tile = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Tiles/Floor"), tiles.Find(tileLayer));
            }
            else
            {
                tile = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Tiles/Wall"), tiles.Find(tileLayer));
            }

            Vector3 tilePos = new Vector3((tileVars[1] * 1.6f) - 1.6f, (tileVars[2] * 1.6f) - 1.6f, 0);
            tile.transform.position = tilePos;
            tile.name = tileVars[0].ToString();
            tile.AddComponent<TileCollectionData>();
            tile.GetComponent<TileCollectionData>().tileData = new TileData();
            tile.GetComponent<TileCollectionData>().tileData.tileType = tileType;
            tile.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
            tile.GetComponent<TileCollectionData>().tileData.holeStability = -1;
            tile.GetComponent<SpriteRenderer>().sprite = tileList[tileVars[0]];

            switch (tileLayer)
            {
                case "Underground":
                    tile.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    tile.layer = 11;
                    break;
                case "Ground":
                    tile.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    tile.layer = 10;
                    break;
                case "Vents":
                    tile.GetComponent<SpriteRenderer>().sortingOrder = 9;
                    tile.layer = 12;
                    break;
                case "Roof":
                    tile.GetComponent<SpriteRenderer>().sortingOrder = 13;
                    tile.layer = 13;
                    break;
            }
        }

        //set tile tags
        foreach (Transform tile in tiles.Find("Ground"))
        {
            switch (tile.GetComponent<TileCollectionData>().tileData.tileType)
            {
                case "inFloor":
                case "outFloor":
                case "lowFloor":
                case "medFloor":
                case "highFloor":
                    tile.tag = "Digable";
                    break;
                case "chip":
                    tile.tag = "Wall";
                    break;
                case "cut":
                    tile.tag = "Fence";
                    break;
                case "elec":
                    tile.tag = "ElectricFence";
                    break;
                case "bar":
                    tile.tag = "Bars";
                    break;
                case "obstacle":
                case "obstNoShad":
                case "nw":
                case "ne":
                case "sw":
                case "se":
                case "box":
                    tile.tag = "Obstacle";
                    break;
                case "horiLedge":
                case "vertLedge":
                    tile.tag = "RoofLedge";
                    break;
            }
        }
        foreach (Transform tile in tiles.Find("Underground"))
        {

            switch (tile.GetComponent<TileCollectionData>().tileData.tileType)
            {
                case "inFloor":
                case "outFloor":
                case "lowFloor":
                case "medFloor":
                case "highFloor":
                    tile.tag = "Digable";
                    break;
                case "chip":
                    tile.tag = "Wall";
                    break;
                case "cut":
                    tile.tag = "Fence";
                    break;
                case "elec":
                    tile.tag = "ElectricFence";
                    break;
                case "bar":
                    tile.tag = "Bars";
                    break;
                case "obstacle":
                case "obstNoShad":
                case "nw":
                case "ne":
                case "sw":
                case "se":
                case "box":
                    tile.tag = "Obstacle";
                    break;
                case "horiLedge":
                case "vertLedge":
                    tile.tag = "RoofLedge";
                    break;
            }
        }
        foreach (Transform tile in tiles.Find("Vents"))
        {

            switch (tile.GetComponent<TileCollectionData>().tileData.tileType)
            {
                case "inFloor":
                case "outFloor":
                case "lowFloor":
                case "medFloor":
                case "highFloor":
                    tile.tag = "Digable";
                    break;
                case "chip":
                    tile.tag = "Wall";
                    break;
                case "cut":
                    tile.tag = "Fence";
                    break;
                case "elec":
                    tile.tag = "ElectricFence";
                    break;
                case "bar":
                    tile.tag = "Bars";
                    break;
                case "obstacle":
                case "obstNoShad":
                case "nw":
                case "ne":
                case "sw":
                case "se":
                case "box":
                    tile.tag = "Obstacle";
                    break;
                case "horiLedge":
                case "vertLedge":
                    tile.tag = "RoofLedge";
                    break;
            }
        }
        foreach (Transform tile in tiles.Find("Roof"))
        {
            switch (tile.GetComponent<TileCollectionData>().tileData.tileType)
            {
                case "inFloor":
                case "outFloor":
                case "lowFloor":
                case "medFloor":
                case "highFloor":
                    tile.tag = "Digable";
                    break;
                case "chip":
                    tile.tag = "Wall";
                    break;
                case "cut":
                    tile.tag = "Fence";
                    break;
                case "elec":
                    tile.tag = "ElectricFence";
                    break;
                case "bar":
                    tile.tag = "Bars";
                    break;
                case "obstacle":
                case "obstNoShad":
                case "nw":
                case "ne":
                case "sw":
                case "se":
                case "box":
                    tile.tag = "Obstacle";
                    break;
                case "horiLedge":
                case "vertLedge":
                    tile.tag = "RoofLedge";
                    break;
            }
        }
    }
    private void SetObjects()
    {
        Debug.Log("objVars count = " + currentMap.objVars.Count);
        Debug.Log("objNames count = " + currentMap.objNames.Count);

        for (int i = 0; i < currentMap.objNames.Count; i++)
        {
            float[] objVars = currentMap.objVars[i];
            string objName = currentMap.objNames[i];

            bool objIsAvailable = false;
            GameObject objPrefab = null;
            foreach (GameObject obj in Resources.LoadAll<GameObject>("PrisonPrefabs/Objects"))
            {
                if (obj.name == objName)
                {
                    objIsAvailable = true;
                    objPrefab = obj;
                }
            }
            if (objName.StartsWith("Ladder"))//special cases can go here :)
            {
                objIsAvailable = true;

                if (objName == "LadderUp")
                {
                    switch (objVars[2])
                    {
                        case 1:
                            objPrefab = Resources.Load<GameObject>("PrisonPrefabs/Objects/LadderUp (Ground)");
                            objName = "LadderUp (Ground)";
                            break;
                        case 2:
                            objPrefab = Resources.Load<GameObject>("PrisonPrefabs/Objects/LadderUp (Vent)");
                            objName = "LadderUp (Vent)";
                            break;
                        default:
                            objIsAvailable = false;
                            break;
                    }
                }
                else if (objName == "LadderDown")
                {
                    switch (objVars[2])
                    {
                        case 2:
                            objPrefab = Resources.Load<GameObject>("PrisonPrefabs/Objects/LadderDown (Vent)");
                            objName = "LadderDown (Vent)";
                            break;
                        case 3:
                            objPrefab = Resources.Load<GameObject>("PrisonPrefabs/Objects/LadderDown (Roof)");
                            objName = "LadderDown (Roof)";
                            break;
                        default:
                            objIsAvailable = false;
                            break;
                    }
                }
            }

            if (objIsAvailable)
            {
                Vector3 objPos = new Vector3((objVars[0] * 1.6f) - 1.6f, (objVars[1] * 1.6f) - 1.6f, 0);
                GameObject objInst = Instantiate(objPrefab);
                string objLayer = layerDict[Convert.ToInt32(objVars[2])];
                objInst.transform.position = objPos;
                if (objVars[2] == 2) //this is here because "Vents" and "VentObjects" are named differently (the 's' isnt on the VentObjects one, which the layerDict uses)
                {
                    objInst.transform.parent = tiles.Find("VentObjects");
                }
                else
                {
                    objInst.transform.parent = tiles.Find(objLayer + "Objects");
                }
                objInst.name = objName;

                switch (objLayer)
                {
                    case "Underground":
                        objInst.GetComponent<SpriteRenderer>().sortingOrder = -1;
                        objInst.layer = 11;
                        break;
                    case "Ground":
                        objInst.GetComponent<SpriteRenderer>().sortingOrder = 3;
                        objInst.layer = 10;
                        break;
                    case "Vents":
                        objInst.GetComponent<SpriteRenderer>().sortingOrder = 10;
                        objInst.layer = 12;
                        break;
                    case "Roof":
                        objInst.GetComponent<SpriteRenderer>().sortingOrder = 14;
                        objInst.layer = 13;
                        break;
                }

                dataScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
                ItemSprites = dataScript.ItemSprites;
                NPCSprites = dataScript.NPCSprites;
                PrisonObjectSprites = dataScript.PrisonObjectSprites;
                UISprites = dataScript.UISprites;

                switch (objInst.name)
                {
                    case "NPCDesk":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[51];
                        break;
                    case "PlayerDesk":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[143];
                        break;
                    case "Benchpress":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[52];
                        break;
                    case "Treadmill":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[54];
                        break;
                    case "RunningMat":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[212];
                        break;
                    case "PushupMat":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[213];
                        break;
                    case "SpeedBag":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[259];
                        objInst.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[258];
                        objInst.transform.Find("Bag").GetComponent<SpriteRenderer>().size = new Vector2(1, 1.2f);
                        break;
                    case "PunchingMat":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[236];
                        objInst.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[234];
                        objInst.transform.Find("Bag").GetComponent<SpriteRenderer>().size = new Vector2(.8f, 1.7f);
                        break;
                    case "JumpropeMat":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[238];
                        break;
                    case "PullupBar":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[250];
                        break;
                    case "ComputerTable":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[188];
                        break;
                    case "PlayerBedVertical":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[264];
                        break;
                    case "MedicBed":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[56];
                        break;
                    case "Lounger":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[206];
                        break;
                    case "Seat":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[78];
                        break;
                    case "EmptyVentCover":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[138];
                        break;
                    case "Vent":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[137];
                        objInst.GetComponent<TileCollectionData>().tileData = new TileData();
                        objInst.GetComponent<TileCollectionData>().tileData.tileType = null;
                        objInst.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
                        objInst.GetComponent<TileCollectionData>().tileData.holeStability = -1;
                        break;
                    case "SheetRope":
                        objInst.GetComponent<SpriteRenderer>().sprite = UISprites[162];
                        break;
                    case "Rope":
                        objInst.GetComponent<SpriteRenderer>().sprite = UISprites[163];
                        break;
                    case "Grapple":
                        objInst.GetComponent<SpriteRenderer>().sprite = UISprites[163];
                        break;
                    case "100%HoleDown":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[132];
                        break;
                    case "100%HoleUp":
                        objInst.GetComponent<UnityEngine.Rendering.Universal.Light2D>().lightCookieSprite = UISprites[38];
                        break;
                    case "Brace":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[96];
                        break;
                    case "Rock":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[41];
                        break;
                    case "LadderDown (Vent)":
                    case "LadderDown (Roof)":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[140];
                        break;
                    case "LadderUp (Ground)":
                    case "LadderUp (Vent)":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[147];
                        break;
                    case "SlatsHorizontal":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[139];
                        objInst.GetComponent<TileCollectionData>().tileData = new TileData();
                        objInst.GetComponent<TileCollectionData>().tileData.tileType = null;
                        objInst.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
                        objInst.GetComponent<TileCollectionData>().tileData.holeStability = -1;
                        break;
                    case "SlatsVertical":
                        objInst.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[40];
                        objInst.GetComponent<TileCollectionData>().tileData = new TileData();
                        objInst.GetComponent<TileCollectionData>().tileData.tileType = null;
                        objInst.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
                        objInst.GetComponent<TileCollectionData>().tileData.holeStability = -1;
                        break;

                }
            }
        }
    }
    public Map MakeMapObject(string path)
    {
        //unzip files
        string extractPath = Path.Combine(Application.streamingAssetsPath, "Prisons", "CustomPrisons") + Path.DirectorySeparatorChar;

        ZipFile.ExtractToDirectory(path, extractPath);

        //load Data.ini to a string[]
        string[] data = File.ReadAllLines(Path.Combine(extractPath, "Data.ini"));
        File.Delete(Path.Combine(extractPath, "Data.ini"));

        //load other files to memory
        if (File.Exists(Path.Combine(extractPath, "Tiles.png")))
        {
            tileset = ConvertPNGToSprite(Path.Combine(extractPath, "Tiles.png"));
            File.Delete(Path.Combine(extractPath, "Tiles.png"));
        }
        if (File.Exists(Path.Combine(extractPath, "Ground.png")))
        {
            ground = ConvertPNGToSprite(Path.Combine(extractPath, "Ground.png"));
            File.Delete(Path.Combine(extractPath, "Ground.png"));
        }
        if (File.Exists(Path.Combine(extractPath, "Icon.png")))
        {
            icon = ConvertPNGToSprite(Path.Combine(extractPath, "Icon.png"));
            File.Delete(Path.Combine(extractPath, "Icon.png"));
        }
        if (File.Exists(Path.Combine(extractPath, "Music.mp3")))
        {
            StartCoroutine(ConvertMP3ToAudioClip("file://" + Path.Combine(extractPath, "Music.mp3")));
            File.Delete(Path.Combine(extractPath, "Music.mp3"));
        }

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = data[i].Trim();
            data[i] = data[i].Replace("\n", "");
            data[i] = data[i].Replace("\r", "");
            data[i] = data[i].Replace("\u200B", "");
        }

        string mapName = GetINIVar("Properties", "MapName", data);
        string note = Regex.Unescape(GetINIVar("Properties", "Note", data));
        string warden = GetINIVar("Properties", "Warden", data);
        int guardCount = Convert.ToInt32(GetINIVar("Properties", "Guards", data));
        int inmateCount = Convert.ToInt32(GetINIVar("Properties", "Inmates", data));
        string tilesetStr = GetINIVar("Properties", "Tileset", data);
        string groundStr = GetINIVar("Properties", "Ground", data);
        string iconStr = GetINIVar("Properties", "Icon", data);
        string musicStr = GetINIVar("Properties", "Music", data);
        if (tilesetStr != "Custom")
        {
            tileset = TextureToSprite(givenDataScript.tileTextureList[tilesetDict[prisonDict[tilesetStr]]]);
        }
        if (groundStr != "Custom")
        {
            if (groundStr == "black")
            {
                ground = TextureToSprite(givenDataScript.groundTextureList[19]);
            }
            else
            {
                ground = TextureToSprite(givenDataScript.groundTextureList[tilesetDict[prisonDict[groundStr]]]);
            }
        }
        //set music later. this still needs to be added to getgivendata (i think)
        if (iconStr == "None")
        {
            icon = Resources.Load<Sprite>("Map Editor Resources/UI Stuff/noicon");
        }
        int npcLevel = Convert.ToInt32(GetINIVar("Properties", "NPCLevel", data));
        string grounds = GetINIVar("Properties", "Grounds", data);
        string sizeStr = GetINIVar("Properties", "Size", data);
        string[] sizeParts = sizeStr.Split('x');
        int sizeX = Convert.ToInt32(sizeParts[0]);
        int sizeY = Convert.ToInt32(sizeParts[1]);
        string hint1 = Regex.Unescape(GetINIVar("Properties", "Hint1", data));
        string hint2 = Regex.Unescape(GetINIVar("Properties", "Hint2", data));
        string hint3 = Regex.Unescape(GetINIVar("Properties", "Hint3", data));
        string snowingStr = GetINIVar("Properties", "Snowing", data);
        string powStr = GetINIVar("Properties", "POWOutifts", data);
        string stunRodsStr = GetINIVar("Properties", "StunRods", data);
        bool snowing = false;
        bool powOutfits = false;
        bool stunRods = false;
        if (snowingStr == "True")
        {
            snowing = true;
        }
        if (powStr == "True")
        {
            powOutfits = true;
        }
        if (stunRodsStr == "True")
        {
            stunRods = true;
        }
        List<string> routineSet = GetINISet("Routine", data);
        Dictionary<int, string> routineDict = new Dictionary<int, string>();
        foreach (string str in routineSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split('=');
                int time = Convert.ToInt32(strParts[0]);
                string period = strParts[1];
                routineDict.Add(time, period);
            }
        }
        string startingJob = GetINIVar("Jobs", "StartingJob", data);
        bool janitor = false;
        bool gardening = false;
        bool laundry = false;
        bool kitchen = false;
        bool tailor = false;
        bool woodshop = false;
        bool metalshop = false;
        bool deliveries = false;
        bool mailman = false;
        bool library = false;
        if (GetINIVar("Properties", "Janitor", data) == "True")
        {
            janitor = true;
        }
        if (GetINIVar("Properties", "Gardening", data) == "True")
        {
            gardening = true;
        }
        if (GetINIVar("Properties", "Laundry", data) == "True")
        {
            laundry = true;
        }
        if (GetINIVar("Properties", "Kitchen", data) == "True")
        {
            kitchen = true;
        }
        if (GetINIVar("Properties", "Tailor", data) == "True")
        {
            tailor = true;
        }
        if (GetINIVar("Properties", "Woodshop", data) == "True")
        {
            woodshop = true;
        }
        if (GetINIVar("Properties", "Metalshop", data) == "True")
        {
            metalshop = true;
        }
        if (GetINIVar("Properties", "Deliveries", data) == "True")
        {
            deliveries = true;
        }
        if (GetINIVar("Properties", "Mailman", data) == "True")
        {
            mailman = true;
        }
        if (GetINIVar("Properties", "Library", data) == "True")
        {
            library = true;
        }
        List<int[]> tilesList = new List<int[]>();
        List<string> groundTiles = GetINISet("GroundTiles", data);
        List<string> undergroundTiles = GetINISet("UndergroundTiles", data);
        List<string> ventTiles = GetINISet("VentTiles", data);
        List<string> roofTiles = GetINISet("RoofTiles", data);
        foreach (string str in groundTiles)
        {
            string[] strParts = str.Split('=');
            string[] tileVars = strParts[1].Split(',');
            int tileID = Convert.ToInt32(strParts[0].Split('e')[1]);
            int posX = Convert.ToInt32(tileVars[0]);
            int posY = Convert.ToInt32(tileVars[1]);
            int layer = 1;
            int[] tileArr = new int[4]
            {
                tileID, posX, posY, layer
            };
            tilesList.Add(tileArr);
        }
        foreach (string str in undergroundTiles)
        {
            string[] strParts = str.Split('=');
            string[] tileVars = strParts[1].Split(',');
            int tileID = Convert.ToInt32(strParts[0].Split('e')[1]);
            int posX = Convert.ToInt32(tileVars[0]);
            int posY = Convert.ToInt32(tileVars[1]);
            int layer = 0;
            int[] tileArr = new int[4]
            {
                tileID, posX, posY, layer
            };
            tilesList.Add(tileArr);
        }
        foreach (string str in ventTiles)
        {
            string[] strParts = str.Split('=');
            string[] tileVars = strParts[1].Split(',');
            int tileID = Convert.ToInt32(strParts[0].Split('e')[1]);
            int posX = Convert.ToInt32(tileVars[0]);
            int posY = Convert.ToInt32(tileVars[1]);
            int layer = 2;
            int[] tileArr = new int[4]
            {
                tileID, posX, posY, layer
            };
            tilesList.Add(tileArr);
        }
        foreach (string str in roofTiles)
        {
            string[] strParts = str.Split('=');
            string[] tileVars = strParts[1].Split(',');
            int tileID = Convert.ToInt32(strParts[0].Split('e')[1]);
            int posX = Convert.ToInt32(tileVars[0]);
            int posY = Convert.ToInt32(tileVars[1]);
            int layer = 3;
            int[] tileArr = new int[4]
            {
                tileID, posX, posY, layer
            };
            tilesList.Add(tileArr);
        }
        List<string> groundObjSet = GetINISet("GroundObjects", data);
        List<string> undergroundObjSet = GetINISet("UndergroundObjects", data);
        List<string> ventObjSet = GetINISet("VentObjects", data);
        List<string> roofObjSet = GetINISet("RoofObjects", data);
        List<string> objNames = new List<string>();
        List<float[]> objVars = new List<float[]>();
        foreach (string str in groundObjSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split("=");
                objNames.Add(strParts[0]);
                string[] varParts = strParts[1].Split(',');
                float[] vars = new float[3]
                {
                    Convert.ToSingle(varParts[0]),
                    Convert.ToSingle(varParts[1]),
                    1
                };

                objVars.Add(vars);
            }
        }
        foreach (string str in undergroundObjSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split("=");
                objNames.Add(strParts[0]);
                string[] varParts = strParts[1].Split(',');
                float[] vars = new float[3]
                {
                    Convert.ToSingle(varParts[0]),
                    Convert.ToSingle(varParts[1]),
                    0
                };

                objVars.Add(vars);
            }
        }
        foreach (string str in ventObjSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split("=");
                objNames.Add(strParts[0]);
                string[] varParts = strParts[1].Split(',');
                float[] vars = new float[3]
                {
                    Convert.ToSingle(varParts[0]),
                    Convert.ToSingle(varParts[1]),
                    2
                };

                objVars.Add(vars);
            }
        }
        foreach (string str in roofObjSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split("=");
                objNames.Add(strParts[0]);
                string[] varParts = strParts[1].Split(',');
                float[] vars = new float[3]
                {
                    Convert.ToSingle(varParts[0]),
                    Convert.ToSingle(varParts[1]),
                    3
                };

                objVars.Add(vars);
            }
        }
        List<string> zoneNames = new List<string>();
        List<float[]> zoneVars = new List<float[]>();
        List<string> zoneSet = GetINISet("Zones", data);
        foreach(string str in zoneSet)
        {
            string[] strParts = str.Split('=');
            zoneNames.Add(strParts[0]);
            string[] varParts = strParts[1].Split(';');
            string[] posParts = varParts[0].Split(',');
            string[] aSizeParts = varParts[1].Split("x");
            float[] vars = new float[4]
            {
                Convert.ToSingle(posParts[0]), Convert.ToSingle(posParts[1]),
                Convert.ToSingle(aSizeParts[0]), Convert.ToSingle(aSizeParts[1])
            };

            zoneVars.Add(vars);
        }

        Map map = new Map(mapName, note, warden, guardCount, inmateCount, tilesetStr, groundStr, musicStr, tileset, ground, music, icon, npcLevel, grounds, sizeX, sizeY, hint1, hint2, hint3, snowing, powOutfits, stunRods, routineDict, startingJob, janitor, gardening, laundry, kitchen, tailor, woodshop, metalshop, deliveries, mailman, library, tilesList, objNames, objVars, zoneNames, zoneVars);
        return map;
    }
    private Sprite ConvertPNGToSprite(string path)
    {
        byte[] pngBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(pngBytes);

        texture.filterMode = FilterMode.Point;

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100.0f);
    }
    private IEnumerator ConvertMP3ToAudioClip(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading audio: " + www.error);
            }
            else
            {
                music = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }
    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f), // pivot in the center
            100f                     // pixels per unit
        );
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
    private Texture2D textureCornerGet(Texture2D source, int sizeX, int sizeY)
    {
        Color[] pixels = source.GetPixels(0, 0, sizeX, sizeY);

        Texture2D cornerTex = new Texture2D(sizeX, sizeY, source.format, false);
        cornerTex.SetPixels(pixels);
        cornerTex.Apply();
        cornerTex.filterMode = FilterMode.Point;

        return cornerTex;
    }
    private List<Sprite> SliceAndDice(Sprite tileset)
    {
        Texture2D tilesetTex = tileset.texture;
        List<Sprite> tileList = new List<Sprite>();

        int tileWidth = 16;
        int tileHeight = 16;

        // Start from the top row and move down
        for (int y = tilesetTex.height - tileHeight; y >= 0; y -= tileHeight)
        {
            for (int x = 0; x < tilesetTex.width; x += tileWidth)
            {
                Rect rect = new Rect(x, y, tileWidth, tileHeight);
                Sprite subSprite = Sprite.Create(tilesetTex, rect, new Vector2(.5f, .5f), 100f);
                tileList.Add(subSprite);
            }
        }

        return tileList;
    }
}
