using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipIconSelection : MonoBehaviour
{
    [Range(1, 3)]
    public  int slot;
    public Sprite bg0;
    public Sprite bg1;
    public GameObject icon;

    private EquipManager em;
    private Image sr;

    [SerializeField] private int active;
    [SerializeField] private bool full = false;

    void Start()
    {
        em = GameManager.Instance.player.GetComponent<EquipManager>();
        sr = GetComponent<Image>();
        active = em.selected;
        if(slot == 1)
        {
            sr.sprite = bg0;
        }
    }

    void Update()
    {
        if (em.selected != active)
        {
            active = em.selected;

            if (slot == active)
            {
                sr.sprite = bg0;
                if (!icon.activeSelf)
                {
                    icon.SetActive(true);
                }
            }
            else
            {
                sr.sprite = bg1;
            }
        }

        if (icon.activeSelf) 
        {
            if (icon.transform.localScale.x < 1f && !full)
            {
                float inc = 3.25f * Time.deltaTime;
                icon.transform.localScale += new Vector3(inc, inc, inc);
            }
            else
            {
                full = true;
            }

            if (full)
            {
                if (icon.transform.localScale.x > 0.75f)
                {
                    float inc = 2.25f * Time.deltaTime;
                    icon.transform.localScale -= new Vector3(inc, inc, inc);
                }
                else if (icon.transform.localScale.x != 0.75f)
                {
                    icon.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }
            }
        }
    }
}
