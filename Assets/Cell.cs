using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public GridItem containedItem;
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

    public void setContainedItem(GridItem item)
    {
        //Position to spawn item in cell just for off setting the z value to put it infront of the cell
        Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y, -0.1f);
        
        //spawns the container item at the origin of the cell and then makes it its child
        containedItem = Instantiate(item, spawnPos, this.transform.rotation, this.transform);
    }
    
    public void setSelected(bool isSelected){this.isSelected = isSelected;}
}
