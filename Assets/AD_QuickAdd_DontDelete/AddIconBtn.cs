using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddIconBtn : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    public Button PlayGame;
    // Start is called before the first frame update
    void Start()
    {
        
        if (PlayGame == null)
        {
            PlayGame = GetComponent<Button>();
        }

        if (PlayGame != null)
        {
            PlayGame.onClick.AddListener(StartGame);
        }

    }

    public void StartGame()
    {
        // Time.timeScale is global and can remain zero after a pause or ad.
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(gameSceneName);
    }

  
}
