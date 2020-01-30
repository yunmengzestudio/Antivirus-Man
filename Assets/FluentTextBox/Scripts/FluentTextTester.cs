using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluentTextTester : MonoBehaviour
{

    public FluentTextController TestCtrl;
    public Transform TargetParent;

    public string[] Words;
    private int wordIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TestCtrl.InitPanel(TargetParent, new Vector3(0, 1, 0));
            ShowNextWord();
            TestCtrl.ShowPanel();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TestCtrl.ShowPanel();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            TestCtrl.HidePanel();
        }
    }

    private void ShowNextWord()
    {
        TestCtrl.ChangeWord(Words[wordIndex]);

        wordIndex = (wordIndex + 1) % (Words.Length);
    }



}
