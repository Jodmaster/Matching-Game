using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    public int turnCounter;

    public Cell[,] cells;
    public Cell[] selectedCells;

    public Jewel jewel;
    public Blocker blocker;
    public Sand sand;

    //stores the jewels that report having nothing below them 
    public List<Jewel> shouldFall = new List<Jewel> ();
    public List<Sand> sandToFall = new List<Sand> ();

    //the number of rows and columns that the grid of cells will have
    [SerializeField] public int numOfCols;
    [SerializeField] public int numOfRows;

    public Level_1_setup level1;
    public Initialize_Grid gridInit;
    public Rule_checks rules;

    public Sprite redSprite;
    public Sprite blueSprite;
    public Sprite greenSprite;

    // Start is called before the first frame update
    void Start()
    {
        cells = new Cell[numOfRows, numOfCols];
        GameSetup();

    }

    private void GameSetup()
    {
        //finds both the grid initialization script and level item placement script 
        gridInit = FindObjectOfType<Initialize_Grid>();
        level1 = FindObjectOfType<Level_1_setup>();
        rules = FindObjectOfType<Rule_checks>();

        //inializes the grid 
        gridInit.GridInitilization();

        //find all cell objects and set cell counter to 0 for item setting
        Cell[] cellsTemp = FindObjectsOfType<Cell>();

        int cellCounter = 0;

        //loops through all the cells to fill them with the appropriate item 
        for (int rowCount = 0; rowCount < numOfRows; rowCount++)
        {
            for (int colCount = 0; colCount < numOfCols; colCount++)
            {
                Cell cellToSet = cellsTemp[cellCounter];
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
                    case 2:
                        cellToSet.setContainedItem(sand);
                        break;
                }
                cellCounter++;
            }
        }

        //need to reset for next for loop
        cellCounter = 0;

        //reformats raw cell array into 2d array for easier management of cells and applying rules
        for (int rowCount = 0; rowCount < numOfRows; rowCount++)
        {
            for(int colCount = 0; colCount < numOfCols; colCount++)
            {
                cells[rowCount, colCount] = cellsTemp[cellCounter]; 
                cellCounter++;
            }

        }
        
    }

    public void FixedUpdate() {
        if(sandToFall.Count > 0) { sandFall(); }
        if(shouldFall.Count > 0) { jewelFall(); }
    }

    public void Update()
    {
        //if there are cells in the selected array set them to selected
        if (selectedCells[0] != null){selectedCells[0].setSelected(true);}
        if (selectedCells[1] != null){selectedCells[1].setSelected(true);}

        //calls playerTurn on left click
        if (Input.GetMouseButtonDown(0)) {
            
            playerTurn();
            
            if (selectedCells[0] != null) {
                                              
                rules.getSquareToEliminate(selectedCells[0]);
                bool hasEliminated = false;

                //if we can eliminate a square get then destroy
                if (rules.canEliminate && rules.squareElim) {
                        
                    //initialize array for holding square
                    Cell[] squareToEliminate = new Cell[4];

                    squareToEliminate = rules.getSquareToEliminate(selectedCells[0]);
                    eliminateJewels(squareToEliminate);
                    hasEliminated = true;

                    rules.squareElim = false;

                    for(int i = 0; i < selectedCells.Length; i++) {
                        selectedCells[i] = null;
                    }
                }
                    
                //runs check three in a row so that if there are cells to eliminate the arrays in rule_check will be filled and canElim set to true
                rules.CheckThreeInARow(selectedCells[0]);
                Cell[] cellsToEliminate = new Cell[3];

                //if found any potential eliminations get them in a array
                if(rules.canEliminate && !hasEliminated) {
                    if(rules.colElim) {
                        cellsToEliminate = rules.threeInCol;
                    } else if(rules.rowElim) {
                        cellsToEliminate = rules.threeInRow;
                    }

                    //then delete the jewels from the cell
                    eliminateJewels(cellsToEliminate);
                    hasEliminated = true;     

                    //clears selected cells so that the player can't swap afterwards
                    for(int i = 0; i < selectedCells.Length; i++) {
                        selectedCells[i] = null;
                    }
                } 
                
            }


            if(selectedCells[0] != null && selectedCells[1] != null) { 
                rules.canSwapJewels(selectedCells[0], selectedCells[1]);
            }

            if (rules.canSwap) {
                swapJewels();
            }
        }
    }

    public void swapJewels() {
        //gets the children of the selected cells
        Transform cellItem1 = selectedCells[0].transform.GetChild(0);
        Transform cellItem2 = selectedCells[1].transform.GetChild(0);

        //checks that the got item is a jewel 
        if (cellItem1.GetComponent<Jewel>() != null && cellItem2.GetComponent<Jewel>() != null) { 
            //changes parents of the jewel and then updates the transform
            cellItem1.SetParent(selectedCells[1].transform);
            cellItem1.GetComponent<Jewel>().currentParent = selectedCells[1]; 
            cellItem1.transform.position = new Vector3(selectedCells[1].transform.position.x, selectedCells[1].transform.position.y, -0.1f);

            cellItem2.SetParent(selectedCells[0].transform);
            cellItem2.GetComponent<Jewel>().currentParent = selectedCells[0];
            cellItem2.transform.position = new Vector3(selectedCells[0].transform.position.x, selectedCells[0].transform.position.y, -0.1f);
        }
    }

    public void eliminateJewels(Cell[] cellsToElim) {
        //loops through the cells to Elim and destroys the contained jewels
        for(int i = 0; i < cellsToElim.Length; i++) {
            Transform jewelToDestroy = cellsToElim[i].transform.GetChild (0);
            Destroy(jewelToDestroy.gameObject);
        }

        rules.canEliminate = false;
    }

    public void playerTurn() {
        
        //raycasts where the player clicks
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D cellHit = Physics2D.Raycast(ray.origin, ray.direction, 1000, LayerMask.GetMask("Cell"));

        //if there is a cell and space in the selected cells array puts it in the next empty spot
        //if the array is full it clears the array and then puts the cell in
        if (cellHit){
            if (selectedCells[0] != null && selectedCells[1] != null)
            {
                for (int i = 0; i < 2; i++) {
                    selectedCells[i].setSelected(false);
                    selectedCells[i] = null; 
                }

                rules.canSwap = false;
            }
        
            if (selectedCells[0] == null){
                selectedCells[0] = cellHit.transform.GetComponent<Cell>();
            } else {
                selectedCells[1] = cellHit.transform.GetComponent<Cell>();
            }
        }

  
    }

    public Cell getCellAtPosition(int cellRow, int cellCol) {
        
        //unfortunatly cannot just get using the cells position so i decided to just loop through the whole array and check
        //positions manually as there are only 36 cells

        for(int rowCount = 0; rowCount < numOfRows; rowCount++) {
            for(int colCount = 0; colCount < numOfCols; colCount++) {
                if(cells[rowCount, colCount].position[0] == cellRow && cells[rowCount,colCount].position[1] == cellCol) {
                    return cells[rowCount, colCount];
                }
            }
        }
        
        return null;
    }

    public void jewelFall() {
        
        for(int i = 0; i < shouldFall.Count; i++) {

            Jewel currentJewel = shouldFall[i];
            Cell currentParent = currentJewel.currentParent;
            Cell goalCell;
            
            goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1]));

            currentJewel.transform.SetParent(goalCell.transform);
            currentJewel.currentParent = goalCell;

            Vector3 posToChangeTo = new Vector3(goalCell.transform.position.x, goalCell.transform.position.y, -0.1f);
            currentJewel.transform.position = posToChangeTo;

            shouldFall.Remove(currentJewel);
        }
     
    }

    public void sandFall() {
        
        for(int i = 0; i < sandToFall.Count; i++) {

            Sand currentSand = sandToFall[i];
            Cell currentParent = currentSand.currentParent;
            Cell goalCell;

            if (currentSand.fallDown) {
                goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1]));
                if (goalCell) { SetParentAndTransform(currentSand, goalCell); }     
            } else if (currentSand.fallLeft) {
                goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1] - 1));
                if (goalCell) { SetParentAndTransform(currentSand, goalCell); }
            } else if (currentSand.fallRight) {
                goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1] + 1));
                if (goalCell) { SetParentAndTransform(currentSand, goalCell); }
            }
                                  
        }

        void SetParentAndTransform(Sand currentSand, Cell goalCell) {
            
            currentSand.transform.SetParent(goalCell.transform);
            currentSand.currentParent = goalCell;

            Vector3 posToChangeTo = new Vector3(goalCell.transform.position.x, goalCell.transform.position.y, -0.1f);
            currentSand.transform.position = posToChangeTo;

            sandToFall.Remove(currentSand);
        
        }
    }
}
