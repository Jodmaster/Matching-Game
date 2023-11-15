using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Colour_bomb_spawner : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler {

    public Colour_Bomb colorBombPrefab;
    private Game_manager manager;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Game_manager>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInitializePotentialDrag(PointerEventData eventData) {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPos = new Vector3(mousePos.x, mousePos.y, -0.15f);

        Quaternion rot = new Quaternion();
        rot.eulerAngles = Vector3.up;

        Colour_Bomb colorBomb = Instantiate(colorBombPrefab, spawnPos, rot);
        eventData.pointerDrag = colorBomb.transform.gameObject;
    }

    public void OnDrag(PointerEventData eventData) {}
}
