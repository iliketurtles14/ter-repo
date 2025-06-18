using System.Collections.Generic;
using UnityEngine;

public class MouseCollisionOnMap : MonoBehaviour
{
    private HashSet<string> disabledTags = new HashSet<string>();
    private HashSet<Collider2D> hitColliders = new HashSet<Collider2D>();
    private HashSet<GameObject> collidedObjects = new HashSet<GameObject>();
    private List<string> priorityOrder = new List<string>();

    public bool isTouchingButton;
    public GameObject touchedButton;

    private void Update()
    {
        ClearCollisions();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        hitColliders = System.Linq.Enumerable.ToHashSet(Physics2D.OverlapPointAll(mousePosition));
        foreach(var obj in hitColliders)
        {
            collidedObjects.Add(obj.gameObject);
        }

        priorityOrder = new List<string>()
        {
            "Button"
        };

        GameObject highestPriorityObject = null;
        int highestPriorityIndex = int.MaxValue;

        foreach(GameObject obj in collidedObjects)
        {
            GameObject touchedObject = obj;

            if (disabledTags.Contains(touchedObject.tag))
            {
                continue;
            }

            int priorityIndex = priorityOrder.IndexOf(touchedObject.tag);
            if(priorityIndex != -1 && priorityIndex < highestPriorityIndex)
            {
                highestPriorityIndex = priorityIndex;
                highestPriorityObject = touchedObject;
            }
        }

        if(highestPriorityObject != null)
        {
            switch (priorityOrder[highestPriorityIndex])
            {
                case "Button":
                    isTouchingButton = true;
                    touchedButton = highestPriorityObject;
                    break;
            }
        }
    }

    public void DisableTag(string tag)
    {
        disabledTags.Add(tag);
    }
    public void EnableTag(string tag)
    {
        disabledTags.Remove(tag);
    }
    public void DisableAllTags()
    {
        foreach(string tag in priorityOrder)
        {
            disabledTags.Add(tag);
        }
    }
    public void EnableAllTags()
    {
        foreach(string tag in disabledTags)
        {
            disabledTags.Remove(tag);
        }
    }
    private void ClearCollisions()
    {
        hitColliders.Clear();
        collidedObjects.Clear();

        isTouchingButton = false;
        touchedButton = null;
    }
}
