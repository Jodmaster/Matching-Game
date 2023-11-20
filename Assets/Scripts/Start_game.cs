
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Start_game : MonoBehaviour
{

    public Button start_button;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = start_button.GetComponent<Button>();
        
        start_button.onClick.AddListener(loadLevelOne);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void loadLevelOne() {
        Debug.Log("Level 1 loading");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("test_scene");
    }
}
