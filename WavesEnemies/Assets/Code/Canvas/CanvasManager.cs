using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CanvasManager : MonoBehaviour
{
    private List<CanvasController> canvasControllerList;
    private CanvasController lastActiveCanvas;


    private void Awake()
    {
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchCanvas(CanvasType.Game);
    }

    public void SwitchCanvas(CanvasType canvasType)
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == canvasType);

        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas = desiredCanvas;
        }

        else
        {
            Debug.LogWarning("The main menu canvas was not found!");
        }
    }
}
