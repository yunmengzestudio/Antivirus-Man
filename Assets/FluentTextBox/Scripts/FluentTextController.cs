using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FluentTextController : MonoBehaviour
{
    public FluentTextPanel TargetPanel;
    public string CurrentText { get { return TargetPanel.CurrentText; } }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitPanel(Transform parent, Vector3 offset)
    {
        transform.position = parent.transform.position + offset;
        transform.SetParent(parent);
    }

    public void ShowPanel()
    {
        TargetPanel.Show();
    }

    public void HidePanel()
    {
        TargetPanel.Hide();
    }

    public void ChangeWord(string words)
    {
        TargetPanel.ChangeWord(words);
    }

}
