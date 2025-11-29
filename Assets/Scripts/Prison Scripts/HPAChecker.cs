using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HPAChecker : MonoBehaviour //HPA means High-Priority Action
{
    public bool isChipping;
    public bool isDigging;
    public bool isCutting;
    public bool isScrewing;
    public bool isRoping; //or grappling
    public bool hasPickedUp; //desks, deer, inmates
    public bool isSearching;
    public bool isReading; //or browsing
    public bool isExercising;
    public bool isSeated; //or in bed or etc...
    public bool isZipping;
    public bool isPunching;
    public bool isDead;

    public bool isBusy;

    private List<bool> actionList;

    private void Start()
    {
        actionList = new List<bool>()
        {
            isChipping,
            isDigging,
            isCutting,
            isScrewing,
            isRoping,
            hasPickedUp,
            isSearching,
            isReading,
            isExercising,
            isSeated,
            isZipping,
            isPunching,
            isDead
        };
    }
    private void Update()
    {
        foreach(bool action in actionList)
        {
            if (action)
            {
                isBusy = true;
                break;
            }
            else
            {
                isBusy = false;
            }
        }
    }
}
