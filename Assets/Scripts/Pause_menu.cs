using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause_menu : MonoBehaviour
{

    Button button;
    Button resumeButton;
    Button resetButton;
    Button quitButton;
    
    GameObject resumeObject;
    GameObject resetObject;
    GameObject menu;
    GameObject quitObject;

    public bool isOpen;
    public bool reset;
    public bool shouldQuit;

    // Start is called before the first frame update
    void Start()
    {               
        menu = gameObject.transform.GetChild(0).gameObject;
        button = GetComponent<Button>();
        
        resumeObject = GameObject.Find("Resume_Level");
        resetObject = GameObject.Find("Reset_Level");
        quitObject = GameObject.Find("Quit_Level");

        resumeButton = resumeObject.GetComponent<Button>();
        resetButton = resetObject.GetComponent<Button>();
        quitButton = quitObject.GetComponent<Button>();

        menu.SetActive(false);
        isOpen = false;

        button.onClick.AddListener(showMenu);
        resumeButton.onClick.AddListener(resumeGame);
        resetButton.onClick.AddListener(resetGame);
        quitButton.onClick.AddListener(quitGame);
    }

    // Update is called once per frame
    void Update() {}  
    
    private void showMenu() {
        isOpen = true;
        menu.SetActive(true);
    }

    private void resumeGame() {
        isOpen = false;
        menu.SetActive(false);
    }

    private void resetGame() {
        reset = true;
    }

    private void quitGame() {
        shouldQuit = true;
    }
}
