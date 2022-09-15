using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlacePlatformHandler : MonoBehaviour, IPointerDownHandler
{
    internal bool putPlatformClicked = false;
    internal bool searchPathAgain = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        putPlatformClicked = true;
        searchPathAgain = true;
    }
}
