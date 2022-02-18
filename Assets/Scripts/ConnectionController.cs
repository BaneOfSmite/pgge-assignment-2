using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PGGE.MultiPlayer {
    public class ConnectionController : MonoBehaviourPunCallbacks {
        public GameObject[] multiplayerUI;
        public GameObject roomListObj, roomListDisplay;
        private Dictionary<string, RoomInfo> avaliableRoomList;
        private Dictionary<string, GameObject> roomListObjects;
        [SerializeField]
        InputField InputName;
        [SerializeField]
        Button buttonJoinRoom, joinRandomRoom, hostRoom, backButton;

        [SerializeField]
        int version;

        private bool mIsConnecting = false;

        [SerializeField]
        byte maxPlayersPerRoom = 5;

        void Awake() {
            PhotonNetwork.AutomaticallySyncScene = true;
            avaliableRoomList = new Dictionary<string, RoomInfo>();
            roomListObjects = new Dictionary<string, GameObject>();

            buttonJoinRoom.onClick.AddListener(
                delegate {
                    OnClick_JoinButton();
                });
            joinRandomRoom.onClick.AddListener(
                delegate {
                    JoinRandomRoom();
                });
            hostRoom.onClick.AddListener(
                delegate {
                    HostRoom();
                });
            backButton.onClick.AddListener(
                delegate {
                    backToMenu();
                });
        }

        public void OnClick_JoinButton() {
            PhotonNetwork.NickName = InputName.text;
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
            DisplayRoomList();
        }
        public override void OnJoinRandomFailed(short returnCode, string message) {
            Debug.Log("OnJoinRandomFailed() was called by PUN. " +
                "No random room available" +
                ", so we create one by Calling: " +
                "PhotonNetwork.CreateRoom");
            //Debug.Log("No room. You cannot play!");

            // Failed to join a random room.
            // This may happen if no room exists or 
            // they are all full. In either case, we create a new room.
            PhotonNetwork.CreateRoom(null,
                new RoomOptions {
                    MaxPlayers = maxPlayersPerRoom
                });
        }
        public override void OnConnectedToMaster() {
            print("connected");
            if (!PhotonNetwork.InLobby) {
                PhotonNetwork.JoinLobby();
            }
        }
        public override void OnDisconnected(DisconnectCause cause) {
            Debug.Log("OnDisconnected() was called by PUN with reason " + cause);
            mIsConnecting = false;
        }
        public override void OnJoinedRoom() {
            Debug.Log("OnJoinedRoom() called by PUN. Client is in a room.");
            if (PhotonNetwork.IsMasterClient) {
                Debug.Log("We load the default room for multiplayer");
                PhotonNetwork.LoadLevel("MultiPlayerScene1");
            }
        }
        public void DisplayRoomList() {
            multiplayerUI[0].SetActive(false);
            multiplayerUI[1].SetActive(true);
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList) {
            ClearRoomList();
            UpdateRoomListDictionary(roomList);
            displayAvaliableRoom();
        }

        private void ClearRoomList() { //Delete all the room's UI
            foreach (GameObject entry in roomListObjects.Values) {
                Destroy(entry.gameObject);
            }
            roomListObjects.Clear();
        }

        private void UpdateRoomListDictionary(List<RoomInfo> roomList) {
            foreach (RoomInfo info in roomList) {

                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList) { // Remove room from dictionary list if it not avaliable
                    if (avaliableRoomList.ContainsKey(info.Name)) {
                        avaliableRoomList.Remove(info.Name);
                    }
                    continue;
                }
                if (avaliableRoomList.ContainsKey(info.Name)) {  // Update cached room info
                    avaliableRoomList[info.Name] = info;
                } else { // Add new room info to cache
                    avaliableRoomList.Add(info.Name, info);
                }
            }
        }
        private void displayAvaliableRoom() { //Add in the UI for the avaliable rooms
            foreach (RoomInfo info in avaliableRoomList.Values) {
                GameObject entry = Instantiate(roomListObj);

                entry.transform.SetParent(roomListDisplay.transform, false); //Set parent to the scroll view
                entry.transform.GetChild(0).GetComponent<Text>().text = info.Name; //Set room infomations
                entry.transform.GetChild(1).GetComponent<Text>().text = info.PlayerCount + "/" + info.MaxPlayers;
                entry.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { //Add a function to the button
                    if (PhotonNetwork.InLobby) {
                        PhotonNetwork.LeaveLobby();
                    }
                    PhotonNetwork.JoinRoom(info.Name);
                });
                roomListObjects.Add(info.Name, entry);
            }
        }
        public void JoinRandomRoom() {
            if (PhotonNetwork.IsConnected) {
                // Attempt joining a random Room. 
                // If it fails, we'll get notified in 
                // OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            } else {
                // Connect to Photon Online Server.
                mIsConnecting = PhotonNetwork.ConnectUsingSettings();
            }
        }
        public void HostRoom() {
            string roomName = PhotonNetwork.NickName + "'s Room";
            RoomOptions options = new RoomOptions { MaxPlayers = maxPlayersPerRoom, PlayerTtl = 10000 };
            PhotonNetwork.CreateRoom(roomName, options, null);
        }
        public void backToMenu() {
            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.Disconnect();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        }
    }
}