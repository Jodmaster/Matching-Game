using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GridItem_interface;

public class Cell : MonoBehaviour
{
    public GridItem_interface containedItem;
    public BoxCollider2D bxcollider;
    public SpriteRenderer rend;
    public bool isSelected;
  
    // Start is called before the first frame update
    void Start()
    {
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

    public void setContainedItem(GridItem_interface item)
    {
        //Position to spawn item in cell just for off setting the z value to put it infront of the cell
        Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y, -0.1f);


        //spawns the container item at the origin of the cell and then makes it its child
        checkWhichItemToSpawn(item);

        //checks what item type to cast to and spawn in the cell 
        void checkWhichItemToSpawn(GridItem_interface item)
        {
            if(item.itemType == gridItemType.Jewel){
                containedItem = Instantiate((Jewel)item, spawnPos, this.transform.rotation, this.transform);
            } else if (item.itemType == gridItemType.Blocker){
                containedItem = Instantiate((Blocker)item, spawnPos, this.transform.rotation, this.transform);
            } else if (item.itemType == gridItemType.Sand){
                //TODO implement sand class
            }                                                    
        }
    }
    
    public void setSelected(bool isSelected){this.isSelected = isSelected;}
}
