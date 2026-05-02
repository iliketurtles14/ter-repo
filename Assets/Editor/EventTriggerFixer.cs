using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class EventTriggerFixer
{
    [MenuItem("Tools/Convert PointerClick to PointerDown")]
    public static void ConvertTriggers()
    {
        EventTrigger[] triggers = Object.FindObjectsOfType<EventTrigger>(true);

        int count = 0;

        foreach (var trigger in triggers)
        {
            bool modified = false;

            foreach (var entry in trigger.triggers)
            {
                if (entry.eventID == EventTriggerType.PointerClick)
                {
                    entry.eventID = EventTriggerType.PointerDown;
                    modified = true;
                }
            }

            if (modified)
            {
                EditorUtility.SetDirty(trigger);
                count++;
            }
        }

        Debug.Log($"Converted {count} EventTrigger components.");
    }
}