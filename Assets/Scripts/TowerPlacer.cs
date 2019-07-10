using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the preview of new tower being placed; controls where tower can be placed
/// </summary>
public class TowerPlacer : MonoBehaviour
{
    /// <summary>
    /// Color of the preview when placing is possible
    /// </summary>
    [SerializeField]
    private Color EnabledColor;
    /// <summary>
    /// Color of the preview when placing is not possible
    /// </summary>
    [SerializeField]
    private Color DisabledColor;
    /// <summary>
    /// Tower object to be created when user clicks and posistion is valid
    /// </summary>
    public Tower Tower;

    private EventManager eventManager;
    private new SpriteRenderer renderer;
    /// <summary>
    /// number of other objects (other towers, path) blocking the placement
    /// </summary>
    private int collisionCounter = 0;

    /// <summary>
    /// GameObject to display the range of Tower to player
    /// </summary>
    [SerializeField]
    private GameObject TowerRange;

    private void Awake()
    {
        eventManager = GameControllerS.I.EventManager;
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        TowerRange.transform.localScale = new Vector3(2 * Tower.GetRange(), 2 * Tower.GetRange(), 1);
    }

    private void Update()
    {
        // set the position to mouse position
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // other towers block placement (can't be placed over each other)
        // Note: path should also have "Tower" tag
        if (collider.gameObject.CompareTag("Tower"))
        {
            collisionCounter++;
            DisablePlacement();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Tower"))
        {
            collisionCounter--;
            if(collisionCounter == 0)
                EnablePlacement();
        }
    }

    /// <summary>
    /// Display that placement here is possible and register click event
    /// </summary>
    private void EnablePlacement()
    {
        eventManager.PointerDown += EventManager_PointerDown;
        renderer.color = EnabledColor;
    }

    /// <summary>
    /// Display that placement here is not possible
    /// </summary>
    private void DisablePlacement()
    {
        eventManager.PointerDown -= EventManager_PointerDown;
        renderer.color = DisabledColor;
    }

    private void EventManager_PointerDown(object sender, PointerEventArgs args)
    {
        PlaceTower();
    }

    /// <summary>
    /// Places the tower on game plan
    /// </summary>
    private void PlaceTower()
    {
        UnregisterEvents();
        var t = Instantiate(Tower, transform.position, transform.rotation, GameControllerS.I.TowersParent.transform);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Unsubscribe all events that this script can be subscribed to
    /// </summary>
    private void UnregisterEvents()
    {
        eventManager.PointerDown -= EventManager_PointerDown;
    }
}
