// I FUCKING HATE THE MOUSE CURSOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefocusFixer : MonoBehaviour
{
    GameObject lastSelected;

    void Start()
    {
        lastSelected = new GameObject();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null) EventSystem.current.SetSelectedGameObject(lastSelected);
        else lastSelected = EventSystem.current.currentSelectedGameObject;

    }
}
