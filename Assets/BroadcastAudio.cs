using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class BroadcastAudio : MonoBehaviour
{
    private AudioSource audio;
    [Inject]
    public LevelManager MLevelManager { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        App.Container.Inject(this);
    }

    public void PlayAudio(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
        MLevelManager.GetComponent<HumanFactory>().enabled = true;
    }

}
