using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public Color EnabledColor;
    public Color DisabledColor;
    public GameObject Tower;

    private EventManager eventManager;
    private new SpriteRenderer renderer;

    private void Awake()
    {
        eventManager = GameControllerS.I.EventManager;
        GameControllerS.I.Path.TriggerEnter += Path_TriggerEnter;
        GameControllerS.I.Path.TriggerExit += Path_TriggerExit;
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Path_TriggerExit(object sender, TriggerEventArgs args)
    {
        if (args.Collision.gameObject == gameObject)
            EnablePlacement();
    }

    private void Path_TriggerEnter(object sender, TriggerEventArgs args)
    {
        if(args.Collision.gameObject == gameObject)
            DisablePlacement();
    }

    void Update()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        transform.position = newPos;
    }

    void EnablePlacement()
    {
        eventManager.PointerDown += EventManager_PointerDown;
        renderer.color = EnabledColor;
    }

    void DisablePlacement()
    {
        eventManager.PointerDown -= EventManager_PointerDown;
        renderer.color = DisabledColor;
    }

    private void EventManager_PointerDown(object sender, PointerEventArgs args)
    {
        PlaceTower();
    }

    void PlaceTower()
    {
        UnregisterEvents();
        var t = Instantiate(Tower, transform.position, transform.rotation, GameControllerS.I.TowersParent.transform);
        Destroy(this.gameObject);
    }

    void UnregisterEvents()
    {
        eventManager.PointerDown -= EventManager_PointerDown;
        GameControllerS.I.Path.TriggerEnter -= Path_TriggerEnter;
        GameControllerS.I.Path.TriggerExit -= Path_TriggerExit;
    }
}
