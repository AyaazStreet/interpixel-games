using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public Button b;
    public Toggle t;
    public Slider s;

    private void Awake()
    {
        b = GetComponent<Button>();
        t = GetComponent<Toggle>();
        s = GetComponent<Slider>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (b) b.Select();
        if (t) t.Select();
        if (s) s.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_hover);
    }
}
