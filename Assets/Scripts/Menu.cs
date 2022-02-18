using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    // We are going to add the button click 
    // from script instead of doing it from 
    // the Unity Editor.
    [SerializeField]
    Button ButtonSinglePlayer;
    [SerializeField]
    Button ButtonMultiPlayer;

    private void Start() {
        ButtonSinglePlayer.onClick.AddListener(
          delegate () {
              OnClick_SinglePlayer();
          });

        ButtonMultiPlayer.onClick.AddListener(
          delegate () {
              OnClick_MultiPlayer();
          });
    }

    public void OnClick_SinglePlayer() {
        Debug.Log("Loading Single Player");
        SceneManager.LoadScene("SinglePlayer");
    }
    public void OnClick_MultiPlayer() {
        Debug.Log("Loading Multiplayer");
        SceneManager.LoadScene("MultiPlayerLauncher");
    }
}
