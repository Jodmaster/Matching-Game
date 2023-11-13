using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bomb_spawn : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler {

    public Bomb bombPrefab;
    
    public void OnInitializePotentialDrag(PointerEventData eventData) {

        //on press of the button spawns a bomb prefab 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPos = new Vector3(mousePos.x, mousePos.y, -0.15f);

        Quaternion rot = new Quaternion();
        rot.eulerAngles = Vector3.up;

        Bomb bomb = Instantiate(bombPrefab, spawnPos, rot);
        eventData.pointerDrag = bomb.transform.gameObject;
    
    }

    public void OnDrag(PointerEventData eventData) {       
    }

}
