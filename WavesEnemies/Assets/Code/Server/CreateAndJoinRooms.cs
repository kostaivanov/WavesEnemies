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

    [SerializeField] private GameObject playerPrefab;
    Vector2 randomPosition;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvasManager = canvas.GetComponent<CanvasManager>();
        randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

    }

    public void CreateRoom()
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
        GameObject myPlayer = (GameObject)PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity, 0);

    }
}
