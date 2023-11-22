using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_end : MonoBehaviour
{

    Button resetButton;
    Button quitButton;

    GameObject resetObject;
    GameObject quitObject;
    GameObject win_Text;
    GameObject Lose_Text;
    GameObject menu;

    public bool isOpen;
    public bool reset;
    public bool shouldquit;

    // Start is called before the first frame update
    void Start()
    {
        menu = gameObject.transform.GetChild(0).gameObject;
        
        resetObject = GameObject.Find("Restart_Level_End");
        quitObject = GameObject.Find("Quit_Level_End");

        resetButton = resetObject.GetComponent<Button>();
        quitButton = quitObject.GetComponent<Button>();

        menu.SetActive(false);
        isOpen = false;

        resetButton.onClick.AddListener(resetGame);
        quitButton.onClick.AddListener(quitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showEnd() {
        isOpen = true;
        menu.SetActive(true);
    }

    private void resetGame() {
        reset = true;
    }


    private void quitGame() {
        shouldquit = true;
    }
}
