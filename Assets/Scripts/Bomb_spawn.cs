using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bomb_spawn : MonoBehaviour, IPointerDownHandler {

    public Bomb bombPrefab;
    
    public void OnPointerDown(PointerEventData eventData) {
        
        Quaternion rot = new Quaternion();
        rot.eulerAngles = Vector3.up;
        
        Bomb bomb = Instantiate(bombPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), rot);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
