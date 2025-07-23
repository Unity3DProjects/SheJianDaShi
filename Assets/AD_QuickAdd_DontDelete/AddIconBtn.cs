using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddIconBtn : MonoBehaviour
{
    public Button PrivacyBtn;
    public Button AddIcon;
    public Button PlayGame;
    // Start is called before the first frame update
    void Start()
    {
        AddIcon.onClick.AddListener(() =>
        {
            ADManager.Instance.CreateShortcutButton();
        });
       
        PrivacyBtn.onClick.AddListener(() =>
        {
            //ADManager.Instance.Panel.ShowPrivacy();
        });
        
        PlayGame.onClick.AddListener(StartGame);

    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        ADManager.Instance.ShowBox();
        ADManager.Instance.ShowBlackAd();
    }

  
}
