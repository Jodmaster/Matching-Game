using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Concretion_spawner : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler {
    public Concretion concretionPrefab;
    private Game_manager manager;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Game_manager>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update() {
        if(!(manager.concreteUsed < manager.concreteLimit) || manager.isPaused) { button.interactable = false; } else { button.interactable = true; }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData) {
        if(manager.concreteUsed < manager.concreteLimit && !manager.isPaused) {
            //on press of the button spawns a bomb prefab 
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 spawnPos = new Vector3(mousePos.x, mousePos.y, -0.15f);

            Quaternion rot = new Quaternion();
            rot.eulerAngles = Vector3.up;

            Concretion conc = Instantiate(concretionPrefab, spawnPos, rot);
            eventData.pointerDrag = conc.transform.gameObject;
        }
    }

    public void OnDrag(PointerEventData eventData) { }

}
