using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Correct capitalization

public class Spawn : MonoBehaviour
{
    public GameObject Prefabs;
    Vector3 MoveDir2 = Vector3.zero;

    void Start()
    {
        // Ensure the prefab is properly instantiated over the network
        PhotonNetwork.Instantiate(Prefabs.name, MoveDir2, Quaternion.identity);
    }
}
