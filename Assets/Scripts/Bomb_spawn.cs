using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bomb_spawn : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler {

    public Bomb bombPrefab;
    private Game_manager manager;
    private Button button;

    void Start() {
        manager = FindObjectOfType<Game_manager>();
        button = GetComponent<Button>();
    }

    public void Update() {
        if(!(manager.bombsUsed < manager.bombLimit)) { button.interactable = false; }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData) {
        if(manager.bombsUsed < manager.bombLimit) {
            //on press of the button spawns a bomb prefab 
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 spawnPos = new Vector3(mousePos.x, mousePos.y, -0.15f);

            Quaternion rot = new Quaternion();
            rot.eulerAngles = Vector3.up;

            Bomb bomb = Instantiate(bombPrefab, spawnPos, rot);
            eventData.pointerDrag = bomb.transform.gameObject;
        } 
    }

    public void OnDrag(PointerEventData eventData) {}

}
