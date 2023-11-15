using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class OnHoverMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnHover;
    public UnityEvent OnHoverExit;
    // Start is called before the first frame update

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit.Invoke();
    }
}
