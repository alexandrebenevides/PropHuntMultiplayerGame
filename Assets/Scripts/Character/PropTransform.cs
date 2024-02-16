using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PropTransform : MonoBehaviour
{
    private PhotonView photonView;
    private Camera playerCamera;
    private Text propTransformText, crosshair;
    private float rayDistance = 2f;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            playerCamera = GetComponentInChildren<Camera>();
            propTransformText = playerCamera.GetComponentInChildren<Canvas>().GetComponentsInChildren<Text>().ToList().Find(obj => obj.name.Contains("PropTransformText"));
            crosshair = playerCamera.GetComponentInChildren<Canvas>().GetComponentsInChildren<Text>().ToList().Find(obj => obj.name.Contains("Crosshair"));
            crosshair.enabled = true;
        }
        else
        {
            GetComponentInChildren<Camera>().GetComponentInChildren<Canvas>().GetComponentsInChildren<Text>().ToList().Find(obj => obj.name.Contains("PropTransformText")).enabled = false;
            GetComponentInChildren<Camera>().GetComponentInChildren<Canvas>().GetComponentsInChildren<Text>().ToList().Find(obj => obj.name.Contains("Crosshair")).enabled = false;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = playerCamera.ScreenPointToRay(screenCenter);
            RaycastHit hitInfo;

            propTransformText.enabled = false;
            if (Physics.Raycast(ray, out hitInfo, rayDistance))
            {
                if (hitInfo.collider.CompareTag("PropObject"))
                {
                    propTransformText.enabled = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GetComponent<MeshFilter>().mesh = hitInfo.collider.GetComponent<MeshFilter>().mesh;
                        GetComponent<MeshRenderer>().material = hitInfo.collider.GetComponent<MeshRenderer>().material;
                        photonView.RPC("RPC_ChangePlayerMesh", RpcTarget.OthersBuffered, hitInfo.collider.GetComponent<MeshFilter>().mesh.name, hitInfo.collider.GetComponent<MeshRenderer>().materials[0].name);
                    }
                }
            }

            //Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
        }
    }

    [PunRPC]
    void RPC_ChangePlayerMesh(string meshName, string materialName)
    {
        Mesh mesh = Resources.Load<Mesh>("RPGPP_LT/Models/" + meshName.TrimEnd(" Instance"));
        Material material = Resources.Load<Material>("RPGPP_LT/Materials/" + materialName.TrimEnd(" (Instance)"));

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;
    }
}
