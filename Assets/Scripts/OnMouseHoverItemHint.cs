using UnityEngine;
using UnityEngine.EventSystems;

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
        gameObject.transform.GetChild(5).gameObject.SetActive(true); // Activate the hint UI element when the mouse hovers over the item
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse exited: " + gameObject.name); // for testing
        gameObject.transform.GetChild(5).gameObject.SetActive(false); // Deactivate the hint UI element when the mouse exits the item
    }

}
