using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System;

public class Speech_Contoller : MonoBehaviour
{
    private RectTransform[] trans;
    public TMP_Text text;

    public int width;
    public int height;

    private Dictionary<string, Action> methods = new Dictionary<string, Action>() {
        {"intro", () => intro() }
    };

    // Start is called before the first frame update
    void Start() {
        trans = GetComponentsInChildren<RectTransform>();
        text = GetComponentInChildren<TMP_Text>();        
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
       
    }

    private static void intro() {
        
    }
}
