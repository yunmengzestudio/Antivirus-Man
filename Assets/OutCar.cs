using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OutCar : MonoBehaviour
{
    public int Health = 5;
    public float StartMoveTime = 30;
    public float MoveTime = 10;
    public Transform StartPoint;
    public Transform TargetPoint;

    private int currentHealth = 5;
    private bool isFly = false;
    private Tweener currentTweener;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = Health;
        currentTweener = transform.DOMove(TargetPoint.position, StartMoveTime).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeDamage()
    {
        if (!currentTweener.IsComplete())
        {
            currentTweener.Complete();
        }
        Debug.Log($"take damage");
        currentHealth--;
        if (currentHealth <= 0)
        {
            StartCoroutine(StartFly());
        }
        else
        {
            transform.DOShakePosition(0.5f);
            transform.DOShakeRotation(0.5f, 20);
        }

    }


    IEnumerator StartFly()
    {
        isFly = true;
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-30, 20, 0), ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(new Vector3(100, 0, 0), ForceMode.Impulse);
        yield return new WaitForSeconds(5);
        Init();
        isFly = false;
    }

    private void Init()
    {
        currentHealth = Health;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = StartPoint.position;
        transform.rotation = StartPoint.rotation;
        currentTweener = transform.DOMove(TargetPoint.position, MoveTime).SetEase(Ease.Linear);
    }



}
