
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Start_game : MonoBehaviour {

    //main start + tut buttons and the level select container game object
    public Button startButton;
    public Button tutorialButton;
    public GameObject levelSelectContainer; 
    
    //levl buttons
    public Button level1Start;
    public Button level2Start;
    public Button level3Start;

    private bool levelSelectOpen;

    // Start is called before the first frame update
    void Start() {

        //fetch all the buttons and get level select object
        levelSelectContainer = GameObject.Find("Level_Select");
        startButton = GameObject.Find("start_game").GetComponent<Button>();
        level1Start = GameObject.Find("Level_1_Start").GetComponent<Button>();
        level2Start = GameObject.Find("Level_2_Start").GetComponent<Button>();
        tutorialButton = GameObject.Find("Tutorial").GetComponent<Button>();

        //add listeners for the buttons
        startButton.onClick.AddListener(openLevelSelect);
        level1Start.onClick.AddListener(loadLevelOne);
        level2Start.onClick.AddListener(loadlevelTwo);
        tutorialButton.onClick.AddListener(loadTutorial);

        //starts with level select closed
        levelSelectOpen = false;
        levelSelectContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if(levelSelectOpen) { levelSelectContainer.SetActive(true); }
    }

    private void openLevelSelect() {
        levelSelectOpen = true;
    }

    //methods for loading levels
    private void loadTutorial() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Tutorial");
    }

    private void loadLevelOne() {        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level_1");
    }

    private void loadlevelTwo() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level_2");
    }
}