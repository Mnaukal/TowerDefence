using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public event PointerDownEventHandler PointerDown;
    public event PointerUpEventHandler PointerUp;

    private void RaisePointerDown()
    {
        if (PointerDown != null)
            PointerDown(this, null);
    }

    private void RaisePointerUp()
    {
        if (PointerUp != null)
            PointerUp(this, null);
    }


    private void Awake()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            RaisePointerDown();
        if (Input.GetMouseButtonUp(0))
            RaisePointerUp();
    }

}
