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
    public Sprite p1;
    public Sprite p2;
    public Sprite p3;
    public GameObject icon;
    public TextMeshProUGUI count;

    private EquipManager em;
    private PlayerController pc;
    private Image sr;

    private Animator anim;

    [SerializeField] private int active;
    [SerializeField] private bool full = false;

    void Start()
    {
        em = GameManager.Instance.player.GetComponent<EquipManager>();
        pc = GameManager.Instance.player.GetComponent<PlayerController>();
        sr = GetComponent<Image>();
        active = em.selected;
        if(slot == 1)
        {
            sr.sprite = bg0;
        }

        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        active = em.selected;

        if (active == 2 && slot == 2)
        {
            switch (pc.throwController.infused)
            {
                case 1: icon.GetComponent<Image>().sprite = p1; break;
                case 2: icon.GetComponent<Image>().sprite = p2; break;
                case 3: icon.GetComponent<Image>().sprite = p3; break;
            }
        }

            

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

        switch (slot)
        {
            case 1:
                if (pc.T1_Unlocked)
                {
                    count.text = "x" + em.E1_count;
                    icon.SetActive(true);
                }
                else
                {
                    count.text = "";
                    icon.SetActive(false);
                }
                break;
            case 2:
                if (pc.T2_Unlocked)
                {
                    count.text = "x" + em.E2_count;
                    icon.SetActive(true);
                }
                else
                {
                    count.text = "";
                    icon.SetActive(false);
                }
                break;
            case 3:
                if (pc.T3_Unlocked)
                {
                    count.text = "x" + em.E3_count;
                    icon.SetActive(true);
                }
                else
                {
                    count.text = "";
                    icon.SetActive(false);
                }
                break;
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
