using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    public int turnCounter;

    public Cell[] cells;
    public Cell[] selectedCells;

    public Jewel jewel;
    public Blocker blocker;

    [SerializeField] public int numOfCols;
    [SerializeField] public int numOfRows;

    public Level_1_setup level1;
    public Initialize_Grid gridInit;

    // Start is called before the first frame update
    void Start()
    {
        GameSetup();
    }

    private void GameSetup()
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
        for (int rowCount = 0; rowCount < numOfRows; rowCount++)
        {
            for (int colCount = 0; colCount < numOfCols; colCount++)
            {
                Cell cellToSet = cells[cellCounter];
                switch (level1.itemToContain[rowCount, colCount])
                {
                    case 0:

                        cellToSet.setContainedItem(jewel);
                        Jewel jewelToSet = (Jewel)cellToSet.containedItem;
                        
                        switch(level1.jemColorMap[rowCount, colCount]){  
                            case 0:
                                jewelToSet.jewelColor = Color.red;
                                break;
                            case 1:
                                jewelToSet.jewelColor = Color.blue;
                                break;
                            case 2:
                                jewelToSet.jewelColor = Color.green; 
                                break;
                        }

                        cellToSet.containedItem = jewelToSet;
                        
                        break;
                    
                    case 1:
                        cellToSet.setContainedItem(blocker);
                        break;
                }
                cellCounter++;
            }
        }
    }

    public void Update()
    {

        if (selectedCells[0] != null){selectedCells[0].setSelected(true);}
        if (selectedCells[1] != null){selectedCells[1].setSelected(true);}

        if (Input.GetMouseButtonDown(0))
        {
            playerTurn();
        }
    }

    public void playerTurn() {
                
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D cellHit = Physics2D.Raycast(ray.origin, ray.direction, 1000);

        Debug.Log(cellHit.collider.gameObject.name);

        if (cellHit){
            if (selectedCells[0] != null && selectedCells[1] != null)
            {
                Debug.Log("clearing selected");
                for (int i = 0; i < 2; i++) {
                    selectedCells[i].setSelected(false);
                    selectedCells[i] = null; 
                }
            }
        

            if (selectedCells[0] == null){
                selectedCells[0] = cellHit.transform.GetComponent<Cell>();
            } else {
                selectedCells[1] = cellHit.transform.GetComponent<Cell>();
            }
        }

  
    }
}
