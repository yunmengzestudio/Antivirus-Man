using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class BroadcastAudio : MonoBehaviour
{
    private AudioSource broadcastAudio;
    [Inject]
    public LevelManager MLevelManager { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        broadcastAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        App.Container.Inject(this);
    }

    public void PlayAudio(AudioClip clip)
    {
        broadcastAudio.clip = clip;
        broadcastAudio.Play();
        MLevelManager.GetComponent<HumanFactory>().enabled = true;
    }

}
