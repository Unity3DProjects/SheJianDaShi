using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddIconBtn : MonoBehaviour
{
    public Button PlayGame;
    // Start is called before the first frame update
    void Start()
    {
        
        PlayGame.onClick.AddListener(StartGame);

    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        ADManager.Instance.ShowBox();
        ADManager.Instance.ShowBlackAd();
    }

  
}
