
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Start_game : MonoBehaviour {

    public Button startButton;
    public GameObject levelSelectContainer; 
    
    public Button level1Start;
    public Button level2Start;
    public Button level3Start;

    private bool levelSelectOpen;

    // Start is called before the first frame update
    void Start() {

        levelSelectContainer = GameObject.Find("Level_Select");
        startButton = GameObject.Find("start_game").GetComponent<Button>();
        level1Start = GameObject.Find("Level_1_Start").GetComponent<Button>();
        level2Start = GameObject.Find("Level_2_Start").GetComponent<Button>();

        startButton.onClick.AddListener(openLevelSelect);
        level1Start.onClick.AddListener(loadLevelOne);
        level2Start.onClick.AddListener(loadlevelTwo);

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

    private void loadLevelOne() {
        Debug.Log("Level 1 loading");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level_1");
    }

    private void loadlevelTwo() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level_2");
    }
}