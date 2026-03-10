using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JobMenu : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private ApplyPrisonData applyScript;
    private Transform player;
    private bool menuIsOpen;
    private Map currentMap;
    private Transform aStar;
    private Transform mc;
    private Schedule scheduleScript;
    private PauseController pc;
    private Dictionary<string, int> jobIntDict = new Dictionary<string, int>() //compares a job to its required intellect
    {
        { "Janitor", 10 }, { "Gardening", 20 }, { "Tailor", 30 }, { "Laundry", 30 }, { "Library", 40 },
        { "Kitchen", 40 }, { "Mailman", 50 }, { "Woodshop", 50 }, { "Deliveries", 60 }, { "Metalshop", 70 }
    };
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        aStar = RootObjectCache.GetRoot("A*").transform;
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        StartCoroutine(StartWait());
        CloseJobBoard();
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
        CreateJobButtons();
    }
    private void Update()
    {
        if (menuIsOpen && !mcs.isTouchingIDPanel && Input.GetMouseButtonDown(0))
        {
            CloseJobBoard();
        }

        if (mcs.isTouchingJobBoard && Input.GetMouseButtonDown(0))
        {
            float distance = Vector2.Distance(player.position, mcs.touchedJobBoard.transform.position);
            if(distance <= 2.4f)
            {
                OpenJobBoard();
            }
        }
    }
    private void OpenJobBoard()
    {
        transform.Find("JobScrollRect").gameObject.SetActive(true);
        transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = "JOB BOARD";
        transform.Find("TitleText").gameObject.SetActive(true);
        transform.Find("TipText").gameObject.SetActive(true);
        transform.GetComponent<BoxCollider2D>().enabled = true;
        transform.GetComponent<Image>().enabled = true;
        mc.Find("Black").GetComponent<Image>().enabled = true;
        pc.Pause(true);
        menuIsOpen = true;
    }
    private void CloseJobBoard()
    {
        transform.Find("JobScrollRect").gameObject.SetActive(false);
        transform.Find("TitleText").gameObject.SetActive(false);
        transform.Find("TipText").gameObject.SetActive(false);
        transform.Find("DescriptionText").gameObject.SetActive(false);
        transform.Find("BackButton").gameObject.SetActive(false);
        transform.Find("ResignButton").gameObject.SetActive(false);
        transform.Find("ApplyButton").gameObject.SetActive(false);
        transform.GetComponent<BoxCollider2D>().enabled = false;
        transform.GetComponent<Image>().enabled = false;
        mc.Find("Black").GetComponent<Image>().enabled = false;
        pc.Unpause();
        menuIsOpen = false;
    }
    public void OpenJobDescription(string job)
    {
        string desc = GetINIVar("Job_Descrip", job, currentMap.speech);
        desc = desc.Replace('#', '\n');

        bool canApply = true;
        bool canResign = false;
        if(player.GetComponent<PlayerCollectionData>().playerData.job == job)
        {
            canResign = true;
        }
        if (!canResign)
        {
            foreach(Transform npc in aStar)
            {
                if(npc.name.StartsWith("Inmate") && npc.GetComponent<NPCCollectionData>().npcData.job == job)
                {
                    canApply = false;
                    break;
                }
            }
        }

        if (canApply)
        {
            transform.Find("ApplyButton").gameObject.SetActive(true);
        }
        else if (canResign)
        {
            transform.Find("ResignButton").gameObject.SetActive(true);
        }

        transform.Find("BackButton").gameObject.SetActive(true);
        transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = job.ToUpper() + " POSITION";
        transform.Find("TipText").gameObject.SetActive(false);
        transform.Find("JobScrollRect").gameObject.SetActive(false);
        transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = desc;
        transform.Find("DescriptionText").gameObject.SetActive(true);
    }
    public void Apply(string job)
    {
        int requiredInt = jobIntDict[job];
        bool canApply = true;
        if(player.GetComponent<PlayerCollectionData>().playerData.intellect < requiredInt)
        {
            canApply = false;
        }
        if(player.GetComponent<PlayerCollectionData>().playerData.heat >= 50)
        {
            canApply = false;
        }
        if(scheduleScript.periodCode == "W")
        {
            canApply = false;
        }
        if (!canApply)
        {
            Debug.LogError("MAKE SURE TO MAKE IT OPEN THE WARDEN NOTE THING ASDFASDFASDFASDF");
        }

        if (canApply)
        {
            string jobToOpen = player.GetComponent<PlayerCollectionData>().playerData.job;
            player.GetComponent<PlayerCollectionData>().playerData.job = job;

            Transform content = transform.Find("JobScrollRect").Find("Viewport").Find("Content");
            foreach(Transform button in content)
            {
                if(button.name == jobToOpen)
                {
                    button.Find("JobText").GetComponent<TextMeshProUGUI>().text = button.name + ": Vacant";
                    button.Find("JobText").GetComponent<TextMeshProUGUI>().color = new Color(0, 128f / 255f, 0);
                }
                else if(button.name == job)
                {
                    button.Find("JobText").GetComponent<TextMeshProUGUI>().text = button.name + ": " + player.GetComponent<PlayerCollectionData>().playerData.displayName;
                    button.Find("JobText").GetComponent<TextMeshProUGUI>().color = new Color(0, 113f / 255f, 170f / 255f);
                }
            }
        }

        GoToAllJobView();
    }
    public void Resign(string job)
    {
        bool canResign = true;
        
        if(scheduleScript.periodCode == "W")
        {
            canResign = false;
        }
        if (!canResign)
        {
            Debug.LogError("PLEASE MAKE SURE TO REMEMEBR TO MAKE IT OPNE A WARDEN NOTE HERERERERERERERER");
        }

        if (canResign)
        {
            player.GetComponent<PlayerCollectionData>().playerData.job = "";

            Transform content = transform.Find("JobScrollRect").Find("Viewport").Find("Content");
            foreach (Transform button in content)
            {
                if (button.name == job)
                {
                    button.Find("JobText").GetComponent<TextMeshProUGUI>().text = button.name + ": Vacant";
                    button.Find("JobText").GetComponent<TextMeshProUGUI>().color = new Color(0, 128f / 255f, 0);
                    break;
                }
            }
        }

        GoToAllJobView();
    }
    public void GoToAllJobView()
    {
        transform.Find("JobScrollRect").gameObject.SetActive(true);
        transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = "JOB BOARD";
        transform.Find("TipText").gameObject.SetActive(true);
        transform.Find("DescriptionText").gameObject.SetActive(false);
        transform.Find("BackButton").gameObject.SetActive(false);
        transform.Find("ResignButton").gameObject.SetActive(false);
        transform.Find("ApplyButton").gameObject.SetActive(false);
    }
    private void CreateJobButtons()
    {
        List<string> availableJobs = new List<string>();
        if (currentMap.janitor)
        {
            availableJobs.Add("Janitor");
        }
        if (currentMap.gardening)
        {
            availableJobs.Add("Gardening");
        }
        if (currentMap.library)
        {
            availableJobs.Add("Library");
        }
        if (currentMap.mailman)
        {
            availableJobs.Add("Mailman");
        }
        if (currentMap.deliveries)
        {
            availableJobs.Add("Deliveries");
        }
        if (currentMap.metalshop)
        {
            availableJobs.Add("Metalshop");
        }
        if (currentMap.woodshop)
        {
            availableJobs.Add("Woodshop");
        }
        if (currentMap.tailor)
        {
            availableJobs.Add("Tailor");
        }
        if (currentMap.laundry)
        {
            availableJobs.Add("Laundry");
        }
        if (currentMap.kitchen)
        {
            availableJobs.Add("Kitchen");
        }

        Transform content = transform.Find("JobScrollRect").Find("Viewport").Find("Content");
        for(int i = 0; i < availableJobs.Count; i++)
        {
            GameObject button = Instantiate(content.Find("PlaceholderButton").gameObject);
            button.name = availableJobs[i];
            button.transform.parent = content;
            button.GetComponent<RectTransform>().localScale = Vector3.one;
            button.SetActive(true);
        }
        foreach(Transform button in content)
        {
            //: Vacancy

            bool npcHasJob = false;
            bool playerHasJob = false;
            string npcName = "";

            if(player.GetComponent<PlayerCollectionData>().playerData.job == button.name)
            {
                playerHasJob = true;
            }

            if (!playerHasJob)
            {
                foreach (Transform npc in aStar)
                {
                    if (npc.name.StartsWith("Inmate") && npc.GetComponent<NPCCollectionData>().npcData.job == button.name)
                    {
                        npcHasJob = true;
                        npcName = npc.GetComponent<NPCCollectionData>().npcData.displayName;
                        break;
                    }
                }
            }

            if (npcHasJob) //128, 0, 0
            {
                button.Find("JobText").GetComponent<TextMeshProUGUI>().text = button.name + ": " + npcName;
                button.Find("JobText").GetComponent<TextMeshProUGUI>().color = new Color(128f / 255f, 0, 0);
            }
            else if (playerHasJob) //0, 113, 170
            {
                button.Find("JobText").GetComponent<TextMeshProUGUI>().text = button.name + ": " + player.GetComponent<PlayerCollectionData>().playerData.displayName;
                button.Find("JobText").GetComponent<TextMeshProUGUI>().color = new Color(0, 113f / 255f, 170f / 255f);
            }
            else //0, 128, 0
            {
                button.Find("JobText").GetComponent<TextMeshProUGUI>().text = button.name + ": Vacant";
                button.Find("JobText").GetComponent<TextMeshProUGUI>().color = new Color(0, 128f / 255f, 0);
            }

            button.GetComponent<Image>().sprite = applyScript.UISprites[268];
            SpriteState spriteState = button.GetComponent<Button>().spriteState;
            spriteState.highlightedSprite = applyScript.UISprites[269];
            button.GetComponent<Button>().spriteState = spriteState;
        }
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
}
