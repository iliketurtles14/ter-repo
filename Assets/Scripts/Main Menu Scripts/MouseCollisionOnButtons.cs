using System.Collections.Generic;
using UnityEngine;

public class MouseCollisionOnButtons : MonoBehaviour
{
    private HashSet<GameObject> buttons = new HashSet<GameObject>();
    private HashSet<GameObject> inmates = new HashSet<GameObject>();
    private HashSet<GameObject> guards = new HashSet<GameObject>();
    private HashSet<GameObject> smallMenuPanels = new HashSet<GameObject>();

    [SerializeField] private bool _isTouchingButton;
    [SerializeField] private GameObject _touchedButton;
    [SerializeField] private bool _isTouchingInmate;
    [SerializeField] private GameObject _touchedInmate;
    [SerializeField] private bool _isTouchingGuard;
    [SerializeField] private GameObject _touchedGuard;
    [SerializeField] private bool _isTouchingSmallMenuPanel;
    [SerializeField] private GameObject _touchedSmallMenuPanel;

    // Public properties for other scripts
    public bool isTouchingButton => _isTouchingButton;
    public GameObject touchedButton => _touchedButton;
    public bool isTouchingInmate => _isTouchingInmate;
    public GameObject touchedInmate => _touchedInmate;
    public bool isTouchingGuard => _isTouchingGuard;
    public GameObject touchedGuard => _touchedGuard;
    public bool isTouchingSmallMenuPanel => _isTouchingSmallMenuPanel;
    public GameObject touchedSmallMenuPanel => _touchedSmallMenuPanel;

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);

        ClearCollisions();

        foreach (var collider in hitColliders)
        {
            GameObject touchedObject = collider.gameObject;
            AddCollision(touchedObject);
        }

        // Update serialized fields for debugging
        UpdateInspectorFields();
    }

    private void AddCollision(GameObject obj)
    {
        switch (obj.tag)
        {
            case "Button":
                buttons.Add(obj);
                break;
            case "Inmate":
                inmates.Add(obj);
                break;
            case "Guard":
                guards.Add(obj);
                break;
            case "SmallMenuPanel":
                smallMenuPanels.Add(obj);
                break;
        }
    }

    private void ClearCollisions()
    {
        buttons.Clear();
        inmates.Clear();
        guards.Clear();
        smallMenuPanels.Clear();
    }

    private void UpdateInspectorFields()
    {
        _isTouchingButton = buttons.Count > 0;
        _touchedButton = GetFirst(buttons);

        _isTouchingInmate = inmates.Count > 0;
        _touchedInmate = GetFirst(inmates);

        _isTouchingGuard = guards.Count > 0;
        _touchedGuard = GetFirst(guards);

        _isTouchingSmallMenuPanel = smallMenuPanels.Count > 0;
        _touchedSmallMenuPanel = GetFirst(smallMenuPanels);
    }

    private GameObject GetFirst(HashSet<GameObject> set)
    {
        foreach (var obj in set)
        {
            return obj;
        }
        return null;
    }
}
