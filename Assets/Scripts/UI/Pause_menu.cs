using UnityEngine;
using UnityEngine.UI;

public class Pause_menu : MonoBehaviour
{

    Button pausebutton;
    Button resumeButton;
    Button resetButton;
    Button quitButton;
    
    GameObject resumeObject;
    GameObject resetObject;
    GameObject menu;
    GameObject quitObject;

    //bools for game_manager
    public bool isOpen;
    public bool reset;
    public bool shouldQuit;

    // Start is called before the first frame update
    void Start()
    {               
        //get references to all the buttons required by the pause menu 
        menu = gameObject.transform.GetChild(0).gameObject;               
        resumeObject = GameObject.Find("Resume_Level");
        resetObject = GameObject.Find("Reset_Level");
        quitObject = GameObject.Find("Quit_Level");

        resumeButton = resumeObject.GetComponent<Button>();
        resetButton = resetObject.GetComponent<Button>();
        quitButton = quitObject.GetComponent<Button>();
        pausebutton = GetComponent<Button>();

        menu.SetActive(false);
        isOpen = false;

        //set up the on click listeners 
        pausebutton.onClick.AddListener(showMenu);
        resumeButton.onClick.AddListener(resumeGame);
        resetButton.onClick.AddListener(resetGame);
        quitButton.onClick.AddListener(quitGame);
    }

    public void disableButton() {
        pausebutton.enabled = false;
    }
    //methods setting bool values for button conditions, these bools are then used by the game_manager to execute actions
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
