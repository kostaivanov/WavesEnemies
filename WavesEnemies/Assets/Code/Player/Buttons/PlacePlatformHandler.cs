using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlacePlatformHandler : MonoBehaviour, IPointerDownHandler
{
    internal bool putPlatformClicked;
    public void OnPointerDown(PointerEventData eventData)
    {
        putPlatformClicked = true;
    }
}
