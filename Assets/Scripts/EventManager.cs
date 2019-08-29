using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for calling global events
/// </summary>
public class EventManager : MonoBehaviour
{
    #region Game area events
    /// <summary>
    /// Called when user presses mouse button with mouse on game area; event can be blocked by an element in front (collider)
    /// </summary>
    public event PointerEventHandler PointerDown;
    /// <summary>
    /// Called when user raises mouse button with mouse on game area; event can be blocked by an element in front (collider)
    /// </summary>
    public event PointerEventHandler PointerUp;

    private void RaisePointerDown()
    {
        if (PointerDown != null)
            PointerDown(this, new PointerEventArgs(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    private void RaisePointerUp()
    {
        if (PointerUp != null)
            PointerUp(this, new PointerEventArgs(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }
    #endregion

    #region Global events
    /// <summary>
    /// Called when user presses mouse button anywhere
    /// </summary>
    public event PointerEventHandler GlobalPointerDown;
    /// <summary>
    /// Called when user raises mouse button anywhere
    /// </summary>
    public event PointerEventHandler GlobalPointerUp;

    private void RaiseGlobalPointerDown()
    {
        if (GlobalPointerDown != null)
            GlobalPointerDown(this, new PointerEventArgs(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    private void RaiseGlobalPointerUp()
    {
        if (GlobalPointerUp != null)
            GlobalPointerUp(this, new PointerEventArgs(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }
    #endregion

    #region RightPanel events
    /// <summary>
    /// Called when user presses mouse button over the right panel (shop)
    /// </summary>
    public event PointerEventHandler RightPanelPointerDown;

    public void RaiseRightPanelPointerDown()
    {
        if (RightPanelPointerDown != null)
            RightPanelPointerDown(this, new PointerEventArgs(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }
    #endregion

    private void OnMouseDown()
    {
        RaisePointerDown();
    }

    private void OnMouseUp()
    {
        RaisePointerUp();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            RaiseGlobalPointerDown();
        if (Input.GetMouseButtonUp(0))
            RaiseGlobalPointerUp();
    }
}
