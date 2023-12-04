using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_manager : MonoBehaviour
{
    private Pause_menu pauseMen;
    private Game_manager Game_manger;
    private Tutorial_manager tut_manager;

    // Start is called before the first frame update
    void Start()
    {
        pauseMen = FindObjectOfType<Pause_menu>();
        Game_manger = FindObjectOfType<Game_manager>();
        tut_manager = FindObjectOfType<Tutorial_manager>();

        Game_manger.isPaused = true;
        Game_manger.isTutorial = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(pauseMen.shouldQuit) { quitGame(); }   
    }

    private void quitGame() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Title_Screen");
    }
}
