
using UnityEngine;
using static GridItem_interface;

public class Jewel : MonoBehaviour, GridItem_interface
{
    gridItemType GridItem_interface.itemType => gridItemType.Jewel;
    
    public Color jewelColor;
    public SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = jewelColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
