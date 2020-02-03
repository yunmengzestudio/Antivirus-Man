using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CloseTip : MonoBehaviour
{
    public KeyCode TriggerKey;
    public GameObject TipPrefab;
    public Transform TipPoint;
    public string TipWords;
    public bool OneTime = false;
    public UnityEvent TriggeredEvent;

    private GameObject shownTip;
    private Coroutine destroyTextCoroutine;

    private bool InTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InTrigger = true;
            if (shownTip == null)
            {
                shownTip = Instantiate(TipPrefab);
                shownTip.GetComponent<FluentTextController>().InitPanel(TipPoint, Vector3.zero);
            }
            else
            {
                StopCoroutine(destroyTextCoroutine);
            }
            shownTip.GetComponent<FluentTextController>().ChangeWord(TipWords);
            shownTip.GetComponent<FluentTextController>().ShowPanel();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InTrigger = false;
            if(shownTip != null)
            {
                destroyTextCoroutine = StartCoroutine(DestroyTipText(shownTip, 1));
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(TriggerKey))
        {
            if (shownTip != null && InTrigger)
            {
                destroyTextCoroutine = StartCoroutine(DestroyTipText(shownTip, 1,OneTime));
                TriggeredEvent.Invoke();
            }
        }
    }

    IEnumerator DestroyTipText(GameObject obj, float time,bool destroyOnTime=false)
    {
        obj.GetComponent<FluentTextController>().HidePanel();
        yield return new WaitForSeconds(time);
        if (destroyOnTime)
        {
            Destroy(gameObject);
            yield break;
        }
        Destroy(obj);
    }
}
