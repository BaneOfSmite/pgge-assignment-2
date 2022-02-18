using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Lobby : MonoBehaviour {
    [SerializeField]
    InputField InputName;
    [SerializeField]
    Button ButtonJoinRoom;
    [SerializeField]
    Text TextConnectionStatus;

    void Start() {
        //ButtonJoinRoom.onClick.AddListener(
        //  delegate
        //  {
        //    OnClick_JoinRoom();
        //  });
    }

    public void OnClick_JoinRoom() {
        TextConnectionStatus.gameObject.SetActive(true);

        // now we will make a connection to the server using
        // PUN callbacks.
        Debug.Log("Hello " + PhotonNetwork.NickName);
    }
}
