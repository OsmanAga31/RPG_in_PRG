using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class OnMouseHoverItemHint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void Start()
    {
        // Ensure the hint is initially inactive
        gameObject.transform.GetChild(5).gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse entered: " + gameObject.name); // for testing
        gameObject.transform.GetChild(5).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse exited: " + gameObject.name); // for testing
        gameObject.transform.GetChild(5).gameObject.SetActive(false);
    }

}
