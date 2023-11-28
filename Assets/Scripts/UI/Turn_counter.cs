using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Turn_counter : MonoBehaviour
{
    public Game_manager manager;
    private int counter;
    private TMP_Text number;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Game_manager>();
        number = GetComponent<TMP_Text>();
        number.color = Color.white;      
    }

    // Update is called once per frame
    void Update()
    {
        counter = manager.turnCounter;
        number.SetText(counter.ToString());

        if(counter < 3) { number.color = Color.red; }
    }
}
