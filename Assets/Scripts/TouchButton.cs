using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event EventHandler PointerDownEvent, PointerUpEvent;


    public void OnPointerDown(PointerEventData eventData) {
        if (PointerDownEvent != null)
            PointerDownEvent(this, EventArgs.Empty);
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (PointerUpEvent != null)
            PointerUpEvent(this, EventArgs.Empty);
    }
}
