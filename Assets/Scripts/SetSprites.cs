using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.UI;

public class SetSprites : MonoBehaviour
{
    public GameObject CenterPerksTiles;
    public Canvas InventoryCanvas;
    public GameObject Ground;
    private string aName;


    private Dictionary<string, int> perksTilesetDict = new Dictionary<string, int>() //this was a bitch to code holy shit
    {
        { "Bars", 78 }, { "Bottom Wall Left", 91 }, { "Bottom Wall Middle", 63 }, { "Bottom Wall Right", 71 },
        { "Box", 93 }, { "Bush", 79 }, { "Concrete", 35 }, { "Electric Horizontal", 43 },
        { "Electric Vertical", 39 }, { "Fence Horizontal", 20 }, { "Fence NE Corner", 4 }, { "Fence NW Corner", 0 },
        { "Fence SE Corner", 12 }, { "Fence SW Corner", 8 }, { "Fence Vertical", 16 }, { "Garage", 74 },
        { "Hard Wall Horizontal", 70 }, { "Hard Wall Vertical", 66 }, { "Mask Left", 51 }, { "Mask Middle", 55 },
        { "Mask Right", 47 }, { "Obstacle", 84 }, { "Roofing E End", 94 }, { "Roofing Horizontal", 73 },
        { "Roofing N End", 86 }, { "Roofing NE Corner", 61 }, { "Roofing NW Corner", 57 }, { "Roofing S End", 82 },
        { "Roofing SE Corner", 53 }, { "Roofing SW Corner", 49 }, { "Roofing Vertical", 69 }, { "Roofing W End", 90 },
        { "Top Wall Horizontal", 67 }, { "Top Wall NE Corner", 83 }, { "Top Wall NW Corner", 99 }, { "Top Wall SE Corner", 75 },
        { "Top Wall SW Corner", 91 }, { "Top Wall Vertical", 95 }, { "Vent E", 85 }, { "Vent N", 89 },
        { "Vent NE Corner", 33 }, { "Vent NW Corner", 37 }, { "Vent S", 77 }, { "Vent W", 81 },
        { "Wall E T-Shape", 40 }, { "Wall Horizontal", 76 }, { "Wall N T-Shape", 44 }, { "Wall NE Corner", 64 },
        { "Wall NW Corner", 68 }, { "Wall Plus Shape", 36 }, { "Wall S T-Shape", 52 }, { "Wall SE Corner", 56 },
        { "Wall SW Corner", 60 }, { "Wall Vertical", 72 }, { "Wall W T-Shape", 48 }, { "Water", 6 },
        { "Water E", 45 }, { "Water N", 9 }, { "Water NE Corner", 22 }, { "Water NW Corner", 18 },
        { "Water S", 2 }, { "Water SE Corner", 10 }, { "Water SW Corner", 14 }, { "Water W", 41 }, { "Window", 65 },
        { "Floor 1", 96 }, { "Floor 2", 97 }, { "Floor 3", 98 }, { "Floor 4", 92 },
        { "Floor 5", 88 }, { "Floor 6", 84 }, { "Floor 7", 24 }, { "Floor 7 NE", 38 },
        { "Floor 7 NW", 34 }, { "Floor 7 SE", 26 }, { "Floor 7 SW", 30 }, { "Floor 8", 31 },
        { "Floor 9", 27 }, { "Floor 10", 23 }, { "Floor 11 Horizontal", 17 }, { "Floor 11 Vertical", 13 },
        { "Floor 12 Horizontal", 15 }, { "Floor 12 Vertical", 19 }, { "Floor 13", 5 }, { "Floor 14", 28 },
        { "GrassPlaceholder", 1 }, { "Roof Floor High", 21 }, { "Roof Floor Low", 29 }, { "Roof Floor Medium", 25 }
    };
    private Dictionary<int, int> itemSpriteDict = new Dictionary<int, int>() //this was even more of a bitch to code
    {
        { 0, 6 }, { 1, 7 }, { 2, 64 }, { 3, 4 }, { 4, 113 }, { 5, 68 }, { 6, 67 }, { 7, 66 },
        { 8, 69 }, { 9, 114 }, { 10, 5 }, { 11, 5 }, { 12, 5 }, { 13, 31 }, { 14, 31 }, {15, 31 },
        { 16, 103 }, { 17, 103 }, { 18, 103 }, { 19, 173 }, { 20, 19 }, { 21, 177 }, { 22, 32 },
        { 23, 176 }, { 24, 46 }, { 25, 20 }, { 26, 98 }, { 27, 97 }, { 28, 96 }, { 29, 9 }, {30, 133 },
        { 31, 132 }, { 32, 131 }, { 33, 112 }, { 34, 130 }, { 35, 128 }, { 36, 129 }, { 37, 54 },
        { 38, 53 }, { 39, 235 }, { 40, 235 }, { 41, 235 }, { 42, 235 }, { 43, 235 }, { 44, 255 },
        { 45, 212 }, { 46, 212 }, { 47, 212 }, { 48, 212 }, { 49, 225 }, { 50, 207 }, { 51, 207 },
        { 52, 207 }, { 53, 207 }, { 54, 208 }, { 55, 259 }, { 56, 22 }, { 57, 24 }, { 58, 109 },
        { 59, 90 }, { 60, 26 }, { 61, 123 }, { 62, 92 }, { 63, 27 }, { 64, 99 }, { 65, 77 }, { 66, 86 },
        { 67, 174 }, { 68, 86 }, { 69, 79 }, { 70, 91 }, { 71, 172 }, { 72, 15 }, { 73, 36 }, { 74, 56 },
        { 75, 111 }, { 76, 51 }, { 77, 40 }, { 78, 0 }, { 79, 11 }, { 80, 100 }, { 81, 44 }, { 82, 12 },
        { 83, 110 }, { 84, 48 }, { 85, 106 }, { 86, 72 }, { 87, 87 }, { 88, 16 }, { 89, 147 }, { 90, 85 },
        { 91, 187 }, { 92, 175 }, { 93, 44 }, { 94, 122 }, { 95, 170 }, { 96, 104 }, { 97, 102 }, { 98, 82 },
        { 99, 89 }, { 100, 13 }, { 101, 93 }, { 102, 94 }, { 103, 30 }, { 104, 28 }, { 105, 34 }, { 106, 220 },
        { 107, 127 }, { 108, 3 }, { 109, 168 }, { 110, 116 }, { 111, 65 }, { 112, 167 }, { 113, 121 },
        { 114, 115 }, { 115, 39 }, { 116, 124 }, { 117, 125 }, { 118, 126 }, { 119, 75 }, { 120, 83 },
        { 121, 188 }, { 122, 62 }, { 123, 21 }, { 124, 81 }, { 125, 18 }, { 126, 76 }, { 127, 14 }, { 128, 37 },
        { 129, 183 }, { 130, 59 }, { 131, 63 }, { 132, 120 }, { 133, 73 }, { 134, 38 }, { 135, 57 }, { 136, 23 },
        { 137, 50 }, { 138, 169 }, { 139, 1 }, { 140, 55 }, { 141, 105 }, { 142, 78 }, { 143, 29 }, { 144, 42 },
        { 145, 41 }, { 146, 35 }, { 147, 47 }, { 148, 119 }, { 149, 58 }, { 150, 44 }, { 151, 49 }, { 152, 118 },
        { 153, 43 }, { 154, 101 }, { 155, 2 }, { 156, 80 }, { 157, 44 }, { 158, 256 }, { 159, 184 },
        { 160, 236 }, { 161, 240 }, { 162, 189 }, { 163, 153 }, { 164, 261 }, { 165, 192 }, { 166, 216 },
        { 167, 262 }, { 168, 193 }, { 169, 196 }, { 170, 238 }, { 171, 218 }, { 172, 194 }, { 173, 222 },
        { 174, 45 }, { 175, 266 }, { 176, 239 }, { 177, 268 }, { 178, 117 }, { 179, 267 }, { 180, 274 },
        { 181, 217 }, { 182, 60 }, { 183, 260 }, { 184, 265 }, { 185, 275 }, { 186, 195 }, { 187, 197 },
        { 188, 181 }, { 189, 198 }, { 190, 199 }, { 191, 200 }, { 192, 211 }, { 193, 201 }, { 194, 229 },
        { 195, 202 }, { 196, 191 }, { 197, 251 }, { 198, 269 }, { 199, 264 }, { 200, 252 }, { 201, 243 },
        { 202, 180 }, { 203, 273 }, { 204, 276 }, { 205, 270 }, { 206, 215 }, { 207, 182 }, { 208, 209 },
        { 209, 204 }, { 210, 277 }, { 211, 245 }, { 212, 210 }, { 213, 233 }, { 214, 246 }, { 215, 247 },
        { 216, 248 }, { 217, 254 }, { 218, 249 }, { 219, 263 }, { 220, 250 }, { 221, 137 }, { 222, 146 },
        { 223, 136 }, { 224, 149 }, { 225, 152 }, { 226, 138 }, { 227, 151 }, { 228, 150 }, { 229, 145 },
        { 230, 135 }, { 231, 148 }, { 232, 134 }, { 233, 219 }, { 234, 228 }, { 235, 221 }, { 236, 224 },
        { 237, 232 }, { 238, 223 }, { 239, 230 }, { 240, 234 }, { 241, 227 }, { 242, 231 }, { 243, 206 },
        { 244, 203 }, { 245, 205 }, { 246, 178 }, { 247, 179 }, { 248, 139 }, { 249, 158 }, { 250, 140 },
        { 251, 143 }, { 252, 142 }, { 253, 159 }, { 254, 144 }, { 255, 166 }, { 256, 141 }, { 257, 157 },
        { 258, 237 }, { 259, 241 }, { 260, 242 }, { 261, 244 }, { 262, 164 }, { 263, 163 }, { 264, 160 },
        { 265, 161 }, { 266, 162 }, { 267, 156 }, { 268, 154 }, { 269, 153 }, { 270, 155 }, { 271, 272 },
        { 272, 271 }, { 273, 214 }, { 274, 165 }
    };
    private void Start()
    {
        SetTiles();
        SetItems();
        SetGround();
    }
    private void SetTiles()
    {
        foreach(Transform child in CenterPerksTiles.transform.Find("Ground"))
        {
            if(child.name.IndexOf(" (") != -1)
            {
                int index = child.name.IndexOf(" (");
                aName = child.name.Remove(index, child.name.Length - index);
            }
            else
            {
                aName = child.name;
            }
            child.GetComponent<SpriteRenderer>().sprite = DataSender.instance.GetComponent<DataSender>().TileList[perksTilesetDict[aName]];
        }
        CenterPerksTiles.transform.Find("Vents").gameObject.SetActive(true);
        foreach(Transform child in CenterPerksTiles.transform.Find("Vents"))
        {
            if (child.name.IndexOf(" (") != -1)
            {
                int index = child.name.IndexOf(" (");
                aName = child.name.Remove(index, child.name.Length - index);
            }
            else
            {
                aName = child.name;
            }
            child.GetComponent<SpriteRenderer>().sprite = DataSender.instance.GetComponent<DataSender>().TileList[perksTilesetDict[aName]];
        }
        CenterPerksTiles.transform.Find("Vents").gameObject.SetActive(false);
    }
    private void SetItems()
    {
        foreach(ItemData data in Resources.LoadAll("Item Scriptable Objects"))
        {
            data.icon = DataSender.instance.GetComponent<DataSender>().ItemImages[itemSpriteDict[data.id]];
        }
    }
    private void SetGround()
    {
        Ground.GetComponent<SpriteRenderer>().sprite = DataSender.instance.GetComponent<DataSender>().GroundSprite;
    }
}
