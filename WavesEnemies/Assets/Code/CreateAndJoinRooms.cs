using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    private GameObject canvas;
    private CanvasManager canvasManager;
    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvasManager = canvas.GetComponent<CanvasManager>();
    }

    public void CreaRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        canvasManager.SwitchCanvas(CanvasType.Game);
    }
}
