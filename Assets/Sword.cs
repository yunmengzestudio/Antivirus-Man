using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PullOut()
    {
        GameObject.FindObjectOfType<PlayerAttack>().HasWeapon = true;
        Destroy(gameObject);
    }

}
