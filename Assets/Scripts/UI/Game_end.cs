
using UnityEngine;
using UnityEngine.UI;

public class Game_end : MonoBehaviour
{

    Button resetButton;
    Button quitButton;
    Button nextButton;

    GameObject resetObject;
    GameObject quitObject;
    GameObject winObject;
    GameObject loseObject;
    GameObject nextObject;

    GameObject menu;

    public bool isOpen;
    public bool reset;
    public bool shouldquit;
    public bool nextLevel;

    // Start is called before the first frame update
    void Start()
    {
        menu = gameObject.transform.GetChild(0).gameObject;
        
        resetObject = GameObject.Find("Restart_Level_End");
        quitObject = GameObject.Find("Quit_Level_End");
        winObject = GameObject.Find("win_Text");
        loseObject = GameObject.Find("Lose_Text");
        nextObject = GameObject.Find("Next_Level");
  
        resetButton = resetObject.GetComponent<Button>();
        quitButton = quitObject.GetComponent<Button>();
        nextButton = nextObject.GetComponent<Button>();

        menu.SetActive(false);
        winObject.SetActive(false);
        loseObject.SetActive(false);

        isOpen = false;

        resetButton.onClick.AddListener(resetGame);
        quitButton.onClick.AddListener(quitGame);
        nextButton.onClick.AddListener(toNextLevel);
    }

    public void showEnd(bool wonGame) {
        isOpen = true;
        menu.SetActive(true);

        if(wonGame == true) {
            winObject.SetActive(true);
            loseObject.SetActive(false); 
        } else {
            winObject.SetActive(false);
            loseObject.SetActive(true);
        }
    }

    private void resetGame() {
        reset = true;
    }

    private void quitGame() {
        shouldquit = true;
    }

    private void toNextLevel() {        
        nextLevel = true;
    }
}
