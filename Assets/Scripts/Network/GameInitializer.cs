using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    void Start()
    {
        Debug.Log("Jogador: " + PhotonNetwork.NickName);
        PhotonNetwork.Instantiate(player.name, this.transform.position, Quaternion.identity, 0);
    }
}
