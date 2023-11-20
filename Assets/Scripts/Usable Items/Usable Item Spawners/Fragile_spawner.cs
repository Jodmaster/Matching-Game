using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Fragile_spawner : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler {

    public Fragile fragilePrefab;
    private Game_manager manager;
    private Button button;

    // Start is called before the first frame update
    void Start(){
        manager = FindObjectOfType<Game_manager>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!(manager.fragileUsed < manager.fragileLimit) || manager.isPaused) { button.interactable = false; } else { button.interactable = true; }
    }

    public void OnDrag(PointerEventData eventData) {}

    public void OnInitializePotentialDrag(PointerEventData eventData) {
        if(manager.fragileUsed < manager.fragileLimit && !manager.isPaused) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 spawnPos = new Vector3(mousePos.x, mousePos.y, -0.15f);

            Quaternion rot = new Quaternion();
            rot.eulerAngles = Vector3.up;

            Fragile fragile = Instantiate(fragilePrefab, spawnPos, rot);
            eventData.pointerDrag = fragile.transform.gameObject;
        }
    }
}
