using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Game_manager : MonoBehaviour
{
    public int turnCounter;
    public int currentTurn;
    
    public int bombsUsed;
    public int bombLimit;

    public int colorBombsUsed;
    public int colorBombsLimit;

    public int concreteUsed;
    public int concreteLimit;

    public Cell[,] cells;
    public Cell[] selectedCells;

    public List<Cell> cellsToEliminate;

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

    bool shouldBomb;
    public Cell bombCell;

    bool shouldColourBomb;
    public Cell colourBombCell;
    public Color originColor;

    public bool shouldConcrete;
    public Cell concreteCell;

    // Start is called before the first frame update
    void Start()
    {
        cells = new Cell[numOfRows, numOfCols];
        GameSetup();

        turnCounter = 5;
        
        bombLimit = 2;
        bombsUsed = 0;

        colorBombsLimit = 2;
        colorBombsUsed = 0;

        concreteLimit = 2;
        concreteUsed = 0;
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

        //check bombs first so new jewels haven't fallen into empty cells
        if(shouldBomb) { bombExplosion(bombCell); }
        if(shouldColourBomb) { colourBombExplosion(colourBombCell); }
        
        /**
         * having these methods in update lead to what i think are clashes where both a jewel 
         * and sand simultaneously go into the same cell because they both think it's empty moving these into
         * fixed update seems to have spaced the calculations out enough that these clashes don't happen
        */
        if(sandToFall.Count > 0) { sandFall(); }
        if(shouldFall.Count > 0) { jewelFall(); }

        //TODO 
        if(currentTurn <= 0) { }
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
                    eliminateJewels(squareToEliminate.ToList<Cell>()) ;
                    hasEliminated = true;

                    rules.squareElim = false;

                    for(int i = 0; i < selectedCells.Length; i++) {
                        selectedCells[i] = null;
                    }
                }
                    
                //runs check three in a row so that if there are cells to eliminate the arrays in rule_check will be filled and canElim set to true
                rules.CheckThreeInARow(selectedCells[0]);
                Cell[] cellsToEliminate = new Cell[3];

                //if we can eliminate a row or a column get then destroy
                if(rules.canEliminate && !hasEliminated) {
                    if(rules.colElim) {
                        cellsToEliminate = rules.threeInCol;
                    } else if(rules.rowElim) {
                        cellsToEliminate = rules.threeInRow;
                    }

                    //then delete the jewels from the cell
                    eliminateJewels(cellsToEliminate.ToList<Cell>());
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
        if(selectedCells[0] != null && selectedCells[1] != null) {
            Transform cellItem1 = selectedCells[0].transform.GetChild(0);
            Transform cellItem2 = selectedCells[1].transform.GetChild(0);

            //checks that the got item is a jewel 
            if(cellItem1.GetComponent<Jewel>() != null && cellItem2.GetComponent<Jewel>() != null) {
                //changes parents of the jewel and then updates the transform
                cellItem1.SetParent(selectedCells[1].transform);
                cellItem1.GetComponent<Jewel>().currentParent = selectedCells[1];
                cellItem1.transform.position = new Vector3(selectedCells[1].transform.position.x, selectedCells[1].transform.position.y, -0.1f);

                cellItem2.SetParent(selectedCells[0].transform);
                cellItem2.GetComponent<Jewel>().currentParent = selectedCells[0];
                cellItem2.transform.position = new Vector3(selectedCells[0].transform.position.x, selectedCells[0].transform.position.y, -0.1f);
            }
        }
        
        turnCounter--;
    }

    public void eliminateJewels(List<Cell> cellsToElim) {
        
        //loops through the cells to Elim and destroys the contained jewels
        for(int i = 0; i < cellsToElim.Count; i++) {
            if(cellsToElim[i] != null) {
                
                Transform jewelToDestroy = cellsToElim[i].transform.GetChild(0);

                //have to use bools for the usable items for functional execution order in fixed update
                //checks if a jewel contains the bomb item
                if(jewelToDestroy.GetComponentInChildren<Bomb>()) {
                    bombCell = jewelToDestroy.GetComponentInParent<Cell>();
                    shouldBomb = true;
                }

                if(jewelToDestroy.GetComponentInChildren<Colour_Bomb>()) {
                    colourBombCell = jewelToDestroy.GetComponentInParent<Cell>();
                    
                    //need to get color here otherwise jewel will be missing in colorBombExplosion method
                    originColor = jewelToDestroy.GetComponent<Jewel>().jewelColor;
                    shouldColourBomb = true;
                }

                if(jewelToDestroy.GetComponentInChildren<Concretion>()) {
                    Concretion(jewelToDestroy.GetComponentInParent<Cell>());
                }
              
                Destroy(jewelToDestroy.gameObject);
            }
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
        } else {
            if(selectedCells[0] != null) { selectedCells[0].setSelected(false); }
            if(selectedCells[1] != null) { selectedCells[1].setSelected(false); }
            Array.Clear(selectedCells, 0, selectedCells.Length); }

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
        
        //loops through the should fall array getting the new parent and adjusting to the right transform
        for(int i = 0; i < shouldFall.Count; i++) {

            Jewel currentJewel = shouldFall[i];
            Cell currentParent = currentJewel.currentParent;
            Cell goalCell;
            
            goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1]));

            if (currentJewel != null)
            {
                currentJewel.transform.SetParent(goalCell.transform);
                currentJewel.currentParent = goalCell;

                Vector3 posToChangeTo = new Vector3(goalCell.transform.position.x, goalCell.transform.position.y, -0.1f);
                currentJewel.transform.position = posToChangeTo;

                shouldFall.Remove(currentJewel);
            }           
        }    
    }

    public void sandFall() {
        
        //loops through the sand to fall array checks which direction it should fall and then updates parent and transform 
        for(int i = 0; i < sandToFall.Count; i++) {
            Sand currentSand = sandToFall[i];
            Cell currentParent = currentSand.currentParent;
            Cell goalCell;

            if(currentSand.fallDown) {
                goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1]));
                if(goalCell) { SetParentAndTransform(currentSand, goalCell); }
            } else if(currentSand.fallLeft) {
                goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1] - 1));
                if(goalCell) { SetParentAndTransform(currentSand, goalCell); }
            } else if(currentSand.fallRight) {
                goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1] + 1));
                if(goalCell) { SetParentAndTransform(currentSand, goalCell); }   
            }                    
        }

        //helper method that sets new parent and transform
        void SetParentAndTransform(Sand currentSand, Cell goalCell) {
            
            currentSand.transform.SetParent(goalCell.transform);
            currentSand.currentParent = goalCell;

            Vector3 posToChangeTo = new Vector3(goalCell.transform.position.x, goalCell.transform.position.y, -0.1f);
            currentSand.transform.position = posToChangeTo;

            sandToFall.Remove(currentSand);
        
        }
    }

    public void bombExplosion(Cell origin) {

        //when called gets the 3x3 square around the origin, loops through to find 
        //cells with jewels and then eliminates them

        Cell[] cellsToElim = rules.CheckSquare(origin);
        List<Cell> cellsWithJewels = new List<Cell>();

        for(int i = 0; i < cellsToElim.Length; i++) {
            if(cellsToElim[i] != null) {
                if(cellsToElim[i].GetComponentInChildren<Jewel>()) {
                    cellsWithJewels.Add(cellsToElim[i]);
                }
            }
        }

        shouldBomb = false;
        bombCell = null;
        eliminateJewels(cellsWithJewels);        
    }

    public void colourBombExplosion(Cell origin) {
        
        List<Cell> cellWithCorrectColour = new List<Cell>();
        
        //loop through all the cells and checks if they contain a same color jewel
        for(int row = 0; row < numOfRows; row++) {
            for(int col = 0; col < numOfCols; col++) {
                
                Cell cellToCheck = cells[row, col];
                
                if(cellToCheck.GetComponentInChildren<Jewel>() != null) {
                    if(cellToCheck.GetComponentInChildren<Jewel>().jewelColor == originColor) {
                        cellWithCorrectColour.Add(cellToCheck);
                    }
                }                                
            }
        }

        //deletes all jewels with same color
        shouldColourBomb = false;
        colourBombCell = null;
        eliminateJewels(cellWithCorrectColour);
    }

    public void Concretion(Cell cellToConcrete) {
        if(cellToConcrete.GetComponentInChildren<Jewel>() != null) {    
            cellToConcrete.setContainedItem(blocker);
        }
    }
}
