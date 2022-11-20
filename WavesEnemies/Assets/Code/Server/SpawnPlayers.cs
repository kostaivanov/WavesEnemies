using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    public override void OnJoinedRoom()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject myPlayer = (GameObject)PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity, 0);
    }
}
