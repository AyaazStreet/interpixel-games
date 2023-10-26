using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerGainSequence : MonoBehaviour
{
    PlayerController pc;

    public bool started = false;

    private readonly float dur = 3f;

    [Range(1, 3)]
    public int power;

    void Start()
    {
        pc = GameManager.Instance.pc;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !started)
        {
            switch (power)
            {
                case 1:
                {
                    if (pc.A1_Unlocked)
                    {
                        StartCoroutine(Sequence());
                        SoundManager.PlaySound(SoundManager.Sound.snd_slowTime);
                        started = true;
                    }
                }
                break;
                case 2:
                {
                    if (pc.A2_Unlocked)
                    {
                        StartCoroutine(Sequence());
                        SoundManager.PlaySound(SoundManager.Sound.snd_accelTime);
                        started = true;
                    }
                }
                break;
                case 3:
                {
                    if (pc.A3_Unlocked)
                    {
                        StartCoroutine(Sequence());
                        SoundManager.PlaySound(SoundManager.Sound.snd_stopTime);
                        started = true;
                    }
                }
                break;
            }
        }
    }

    private IEnumerator Sequence()
    {
        //pc.hasControl = false;
        pc.anim.SetFloat("AnimMoveMagnitude", 0f);

        switch (power)
        {
            case 1:
            {
                //TimeScaleManager.globalTimescale = TimeScaleManager.A1_EFFECT;
                pc.activeAbility = 1;
                pc.abilityActiveTimer = 2;
                yield return new WaitForSeconds(dur);
            }
            break;
            case 2:
            {
                //TimeScaleManager.globalTimescale = TimeScaleManager.A2_EFFECT;
                pc.activeAbility = 2;
                pc.abilityActiveTimer = 2;
                yield return new WaitForSeconds(dur);
            }
            break; 
            case 3:
            {
                //TimeScaleManager.globalTimescale = TimeScaleManager.A3_EFFECT;
                pc.activeAbility = 3;
                pc.abilityActiveTimer = 2;
                yield return new WaitForSeconds(dur);
            }
            break;
        }

        //pc.activeAbility = 0;
        //pc.hasControl = true;
        
        yield break;
    }
}
