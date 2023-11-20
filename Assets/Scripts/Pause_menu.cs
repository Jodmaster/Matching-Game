using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause_menu : MonoBehaviour
{

    Button button;
    GameObject menu;

    bool isOpen;

    // Start is called before the first frame update
    void Start()
    {       
        
        menu = gameObject.transform.GetChild(0).gameObject;
        button = GetComponent<Button>();

        menu.SetActive(false);
        isOpen = false;

        button.onClick.AddListener(showMenu);
    }

    // Update is called once per frame
    void Update() {}  
    
    private void showMenu() {
        isOpen = true;

        menu.SetActive(true);
    }
}
