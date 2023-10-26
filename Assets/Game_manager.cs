using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    public Cell[] cells;

    public GridItem item1;
    public GridItem item2;

    [SerializeField] public int numOfCols;
    [SerializeField] public int numOfRows;

    public Level_1_setup level1;
    public Initialize_Grid gridInit;

    // Start is called before the first frame update
    void Start()
    {
        //finds both the grid initialization script and level item placement script 
        gridInit = FindObjectOfType<Initialize_Grid>();
        level1 = FindObjectOfType<Level_1_setup>();

        //inializes the grid 
        gridInit.GridInitilization();

        //find all cell objects and set cell counter to 0 for item setting
        cells = FindObjectsOfType<Cell>();
        int cellCounter = 0;

        //loops through all the cells to fill them with the appropriate item 
        for(int rowCount  = 0; rowCount < numOfRows; rowCount++)
        {
            for(int colCount = 0; colCount < numOfCols; colCount++)
            {
                Cell cellToSet = cells[cellCounter]; 
                switch (level1.itemToContain[rowCount, colCount])
                {
                    case 0:
                        cellToSet.setContainedItem(item1);
                        break;
                    case 1:
                        cellToSet.setContainedItem(item2);
                       break;
                }
                cellCounter++;
            }
        }
    }

    
}
