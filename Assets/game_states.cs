using UnityEngine;

public class game_states : MonoBehaviour
{
    public static bool prepPhase;
    public static bool duelPhase;
    const float PREP_TIME = 30;
    float prepTimer;
    void Start()
    {
        prepPhase = true;
        prepTimer = 45f;
    }

    // Update is called once per frame
    void Update()
    {
        if (prepPhase)
        {
            prepTimer -= Time.deltaTime;
            if (prepTimer < 0 )
            {
                prepPhase = false;
                duelPhase = true;
                prepTimer = PREP_TIME;
            }
        }
        
    }
}
