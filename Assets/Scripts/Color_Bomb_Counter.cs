using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Color_Bomb_Counter : MonoBehaviour
{

    private Game_manager manager;
    private TMP_Text counter;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Game_manager>();
        counter = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string text = (manager.colorBombsLimit - manager.colorBombsUsed).ToString();
        counter.SetText(text);
    }
}
