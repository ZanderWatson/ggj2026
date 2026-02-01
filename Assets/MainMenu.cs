using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject options;
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        game_states.gameTimer = 0;
        game_states.maskCollectingTimer = game_states.MASK_COLLECTING_TIME;
        game_states.maskCollectingPhase = true;
        game_states.prepPhase = true;
        game_states.round = 1;
        game_states.gameEnded = false;
    }
    public void ToggleOptionsScreen()
    {
        options.SetActive(!options.activeSelf);
    }
}
