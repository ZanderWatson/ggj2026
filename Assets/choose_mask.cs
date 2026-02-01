using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class choose_mask : MonoBehaviour
{
    GameObject character;
    public Image maskBG1;
    public Image maskBG2;
    public Image maskBG3;
    static float cycleTimer = -1;
    static bool choseMask = false;
    static bool shouldCycle = false;
    float timer1 = 0.2f;
    static bool one = false;
    static bool two = false;
    static bool three = false;
    float timer2 = 0.2f;
    float timer3 = 0.2f;
    // Player attack scripts
    BandanaMask bandana;
    RockMask rock;
    HospitalMask hospital;
    SkiMask ski;
    void Start()
    {
        character = GameObject.Find("Player");
        bandana = character.GetComponent<BandanaMask>();
        rock = character.GetComponent<RockMask>();
        hospital = character.GetComponent<HospitalMask>();
        ski = character.GetComponent<SkiMask>();
    }

    void Update()
    {
        if (shouldCycle)
        {
            cycleTimer -= Time.deltaTime;
            if (cycleTimer > 0)
            {
                if (one)
                {
                    if (timer1 > 0)
                    {
                        maskBG1.color = new Color(1, 1, 1, 0.5f);
                        timer1 -= Time.deltaTime;
                    }
                    else
                    {
                        maskBG1.color = new Color(1, 1, 1, 1);
                        two = true;
                        one = false;
                        timer1 = 0.2f;
                    }
                }
                if (two)
                {
                    if (timer2 > 0)
                    {
                        maskBG2.color = new Color(1, 1, 1, 0.5f);
                        timer2 -= Time.deltaTime;
                    }
                    else
                    {
                        maskBG2.color = new Color(1, 1, 1, 1);
                        three = true;
                        two = false;
                        timer2 = 0.2f;
                    }
                }
                if (three)
                {
                    if (timer3 > 0)
                    {
                        maskBG3.color = new Color(1, 1, 1, 0.5f);
                        timer3 -= Time.deltaTime;
                    }
                    else
                    {
                        maskBG3.color = new Color(1, 1, 1, 1);
                        one = true;
                        three = false;
                        timer3 = 0.2f;
                    }
                }
            }
            else if (!choseMask)
            {
                StartCoroutine(StartDuel());
                choseMask = true;
                shouldCycle = false;
                if (one)
                {
                    player.activeMask = player.maskInventory[0];
                }
                else if (two && player.maskInventory.Count > 1)
                {
                    player.activeMask = player.maskInventory[1];
                } else if (three && player.maskInventory.Count > 2)
                {
                    player.activeMask = player.maskInventory[2];
                }
            }
        }
        
        // Updating mask selection
        if (player.activeMask == 0)
        {
            bandana.enabled = false;
            rock.enabled = false;
            hospital.enabled = false;
            ski.enabled = false;
        } else if (player.activeMask == 1)
        {
            rock.enabled = true;
        }
        else if (player.activeMask == 2)
        {
            ski.enabled = true;
        }
        else if (player.activeMask == 3)
        {
            bandana.enabled = true;
        }
        else if (player.activeMask == 4)
        {
            hospital.enabled = true;
        }
    }

    IEnumerator StartDuel()
    {
        yield return new WaitForSeconds(2);
        game_states.SwitchPhases();
    }
    public static void chooseMaskForDuel()
    {
        cycleTimer = Random.value * 4 + 4;
        choseMask = false;
        shouldCycle = true;
        one = true;
    }
}
