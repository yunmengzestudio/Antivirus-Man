using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    public Transform BoomPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEffect(GameObject effect)
    {
        Instantiate(effect,BoomPoint.position,Quaternion.identity);
    }

}
