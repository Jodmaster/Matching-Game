
using UnityEngine;
using static GridItem_interface;

public class Sand : MonoBehaviour, GridItem_interface
{
    gridItemType GridItem_interface.itemType => gridItemType.Sand;

    Cell currentParent;
    Game_manager manager;

    // Start is called before the first frame update
    void Start()
    {
        currentParent = transform.parent.gameObject.GetComponent<Cell>();
        manager = FindObjectOfType<Game_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
