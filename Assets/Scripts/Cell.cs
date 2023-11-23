using UnityEngine;
using static IGridItem;

public class Cell : MonoBehaviour
{
    public Game_manager manager;

    public IGridItem containedItem;
    public BoxCollider2D bxcollider;
    public SpriteRenderer rend;
    
    public bool isSelected;
    public int cellNumber;
    
    public int[] position;
  
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Game_manager>();

        bxcollider = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
        
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelected){     
           rend.material.color = Color.yellow;
        } else {
           rend.material.color = Color.white;
        }
    }

    public void setContainedItem(IGridItem item)
    {
        //Position to spawn item in cell just for off setting the z value to put it infront of the cell
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, -0.1f);

        //spawns the container item at the origin of the cell and then makes it its child
        checkWhichItemToSpawn(item);

        //checks what item type to cast to and spawn in the cell 
        void checkWhichItemToSpawn(IGridItem item)
        {
            if(item.itemType == gridItemType.Jewel){
                containedItem = Instantiate((Jewel)item, spawnPos, transform.rotation, transform);
            } else if (item.itemType == gridItemType.Blocker){
                containedItem = Instantiate((Blocker)item, spawnPos, transform.rotation, transform);
            } else if (item.itemType == gridItemType.Sand){
                containedItem = Instantiate((Sand)item, spawnPos, transform.rotation, transform);
            }                                                    
        }
    }

    public void setSelected(bool isSelected){this.isSelected = isSelected;}

    public void setCellNumber(int cellNum) {cellNumber = cellNum;}

    
}
