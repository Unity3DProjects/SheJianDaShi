using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPrivacy_Btn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //ADManager.Instance.Panel.ShowPrivacy();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
