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
        gridInit = FindObjectOfType<Initialize_Grid>();
        level1 = FindObjectOfType<Level_1_setup>();

        gridInit.GridInitilization();

        cells = FindObjectsOfType<Cell>();
        int cellCounter = 0;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
