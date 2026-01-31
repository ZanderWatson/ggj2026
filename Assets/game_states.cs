using UnityEngine;
using UnityEngine.SceneManagement;

public class game_states : MonoBehaviour
{
    GameObject character;
    public static bool prepPhase;
    public static bool duelPhase;
    const float PREP_TIME = 30;
    static float prepTimer;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        character = GameObject.Find("Player");
        prepPhase = true;
        prepTimer = 10f;
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
                switchPhases();
                prepTimer = PREP_TIME;
            }
        }
        
    }
    public void switchPhases()
    {
        
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            character.transform.position = new Vector3(-5, 0, 0);
            SceneManager.LoadScene("Battle");
        } else if (SceneManager.GetActiveScene().name.Equals("Battle"))
        {   
            character.transform.position = Vector3.zero;
            SceneManager.LoadScene("Main");
        }
    }
}
