using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSelect : MonoBehaviour, IPointerEnterHandler
{
    public Button b;

    private void Awake()
    {
        b = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        b.Select();
    }
}
