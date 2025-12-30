using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MEDeskController : MonoBehaviour
{
    public Transform uic;
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
    private bool hasSelected;
    public int selectedSlot;
    private bool hasSetText;
    public Transform currentDesk;
    public Transform canvases;
    public PanelSelect panelSelectScript;
    private void OnDisable()
    {
        DeselectSlots();
        SetDeskList();
        ActivateButtonsForSpecialObjects();
    }
    private void OnEnable()
    {
        SetIDs();
    }
    private void Update()
    {
        if (uic.Find("DeskPanel").gameObject.activeInHierarchy)
        {
            foreach(Transform slot in uic.Find("DeskPanel").Find("DeskItemGrid")) //change item images
            {
                int id = slot.GetComponent<MEDeskItemIDContainer>().id;
                
                if(id > -1 && id < 274)
                {
                    slot.GetComponent<Image>().sprite = DataSender.instance.ItemImages[itemSpriteDict[id]];
                }
                else
                {
                    slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
                }
            }

            //set item id and text
            if(hasSelected && selectedSlot != -1)
            {
                int i = 0;
                Transform currentSlot = null;
                foreach(Transform slot in uic.Find("DeskPanel").Find("DeskItemGrid"))
                {
                    if(i == selectedSlot)
                    {
                        currentSlot = slot;
                        break;
                    }
                    i++;
                }

                if (!hasSetText)
                {
                    hasSetText = true;
                    uic.Find("DeskPanel").Find("IDInput").GetComponent<TMP_InputField>().text = currentSlot.GetComponent<MEDeskItemIDContainer>().id.ToString();
                }

                int typedID = -1;
                try
                {
                    typedID = Convert.ToInt32(uic.Find("DeskPanel").Find("IDInput").GetComponent<TMP_InputField>().text);
                }
                catch { }
                currentSlot.GetComponent<MEDeskItemIDContainer>().id = typedID;
            }

            if (!hasSetText)
            {
                uic.Find("DeskPanel").Find("IDInput").GetComponent<TMP_InputField>().text = "";
            }

            //exit panel
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uic.Find("Black").gameObject.SetActive(false);
                uic.Find("DeskPanel").gameObject.SetActive(false);
                DeselectSlots();
                SetDeskList();
                ActivateButtonsForSpecialObjects();
            }
        }
    }
    public void SelectSlot(int slotNum)
    {
        DeselectSlots();
        hasSelected = true;
        selectedSlot = slotNum;
        int i = 0;
        foreach(Transform slot in uic.Find("DeskPanel").Find("DeskButtonGrid"))
        {
            if(i == slotNum)
            {
                slot.GetComponent<Button>().enabled = false;
                slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Map Editor Resources/UI/DeskSlotOutlinePressed");
            }
            i++;
        }
    }
    private void DeselectSlots()
    {
        hasSelected = false;
        hasSetText = false;
        selectedSlot = -1;

        foreach(Transform slot in uic.Find("DeskPanel").Find("DeskButtonGrid"))
        {
            slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
            slot.GetComponent<Button>().enabled = true;
        }
    }
    public void SetIDs()
    {
        try
        {
            List<int> ids = currentDesk.GetComponent<MEDeskListContainer>().ids;
            int i = 0;
            foreach (Transform slot in uic.Find("DeskPanel").Find("DeskItemGrid"))
            {
                slot.GetComponent<MEDeskItemIDContainer>().id = ids[i];
                i++;
            }
        }
        catch
        {
            Debug.Log("Something maybe went wrong here idk"); //it throws an error on the first frame every time and i cant stop it lol
        }
    }
    private void SetDeskList()
    {
        List<int> ids = new List<int>();
        foreach(Transform slot in uic.Find("DeskPanel").Find("DeskItemGrid"))
        {
            ids.Add(slot.GetComponent<MEDeskItemIDContainer>().id);
        }
        currentDesk.GetComponent<MEDeskListContainer>().ids = ids;
    }
    private void ActivateButtonsForSpecialObjects()
    {
        uic.Find("FileButton").GetComponent<Button>().enabled = true;
        uic.Find("FileButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("TilesButton").GetComponent<Button>().enabled = true;
        uic.Find("TilesButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("ObjectsButton").GetComponent<Button>().enabled = true;
        uic.Find("ObjectsButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("PropertiesButton").GetComponent<Button>().enabled = true;
        uic.Find("PropertiesButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("GroundButton").GetComponent<Button>().enabled = true;
        uic.Find("GroundButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("UndergroundButton").GetComponent<Button>().enabled = true;
        uic.Find("UndergroundButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("VentsButton").GetComponent<Button>().enabled = true;
        uic.Find("VentsButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("RoofButton").GetComponent<Button>().enabled = true;
        uic.Find("RoofButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("ZoneObjectsButton").GetComponent<Button>().enabled = true;
        uic.Find("ZoneObjectsButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("AdvancedButton").GetComponent<Button>().enabled = true;
        uic.Find("AdvancedButton").GetComponent<EventTrigger>().enabled = true;
        try
        {
            uic.Find(panelSelectScript.currentPanel).gameObject.SetActive(true);
        }
        catch { }
        GetComponent<ObjectsPanelController>().canBeShown = true;
        canvases.gameObject.SetActive(true);
    }
}
