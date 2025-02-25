using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_manager : MonoBehaviour
{
   
    //var for turn counter
    public int turnCounter;
    
    //inis the vars for items used and item levels
    public int bombsUsed; public int colorBombsUsed; public int concreteUsed; public int fragileUsed;
    public int bombLimit; public int colorBombsLimit; public int concreteLimit; public int fragileLimit;

    //the number of rows and columns that the grid of cells will have
    public int numOfCols;
    public int numOfRows;

    //arrays for storing all the cells and the selected cells
    public Cell[,] cells;
    public Cell[] selectedCells;

    //stores all the cells that need elinating
    public List<Cell> cellsToEliminate;

    //stores the jewels that report having nothing below them 
    public List<Jewel> shouldFall = new List<Jewel>();
    public List<Sand> sandToFall = new List<Sand>();

    //prefabs for each cell item
    public Jewel jewel; public Blocker blocker; public Sand sand;  

    //the main logic components
    private ILevel_Setup level;
    private Initialize_Grid gridInit;
    private Rule_checks rules;
    private UsableItems_Logic itemLogic;
    private Sprites_Setter spriteSetter;
    
    //pause and end game scripts 
    private Pause_menu pauseMenu; private Game_end gameEndMenu;

    //bools for bomb
    public bool shouldBomb; public Cell bombCell;

    //bools for color bomb
    public bool shouldColourBomb; public Color originColor;
   
    //bools for fragile
    private bool shouldBreak; private Jewel jewelToBreak;

    public bool isPaused = false;  

    //bools for lerping
    public bool isLerping; public bool isFalling;
    public bool gameEnded = false;

    public bool isTutorial;

    // Start is called before the first frame update
    void Start()
    {
        GameSetup();        
    }

    private void GameSetup()
    {
        cells = new Cell[numOfRows, numOfCols];
        
        //finds both the grid initialization script and level item placement script 
        getUIandLogicObjects();
        level = GetComponent<ILevel_Setup>();

        //inializes the grid 
        gridInit.GridInitilization();

        //find all cell objects and set cell counter to 0 for item setting
        Cell[] cellsTemp = FindObjectsOfType<Cell>();
        setCellItems(cellsTemp);
        
        //need to reset for next for loop
        int cellCounter = 0;

        //reformats raw cell array into 2d array for easier management of cells and applying rules
        for (int rowCount = 0; rowCount < numOfRows; rowCount++)
        {
            for(int colCount = 0; colCount < numOfCols; colCount++)
            {
                cells[rowCount, colCount] = cellsTemp[cellCounter]; 
                cellCounter++;
            }
        }

        //sets the limits for the usable items and turn counter
        turnCounter = level.turnLimit;
        bombLimit = level.bombLimit;
        fragileLimit = level.fragileLimit;
        colorBombsLimit = level.colorBombLimit;
        concreteLimit = level.concreteLimit;

        void setCellItems(Cell[] cellsTemp) {
            int counter = 0;

            //loops through all the cells to fill them with the appropriate item 
            for(int rowCount = 0; rowCount < numOfRows; rowCount++) {
                for(int colCount = 0; colCount < numOfCols; colCount++) {
                    Cell cellToSet = cellsTemp[counter];
                    switch(level.itemToContain[rowCount, colCount]) {
                        case 0:

                            cellToSet.setContainedItem(jewel);
                            Jewel jewelToSet = (Jewel)cellToSet.containedItem;

                            switch(level.jewelColorMap[rowCount, colCount]) {
                                case 0:
                                    jewelToSet.jewelColor = Color.red;
                                    jewelToSet.sprite = spriteSetter.redSprite;
                                    jewelToSet.selectedSprite = spriteSetter.redSelectedSprite;
                                    break;
                                case 1:
                                    jewelToSet.jewelColor = Color.blue;
                                    jewelToSet.sprite = spriteSetter.blueSprite;
                                    jewelToSet.selectedSprite = spriteSetter.blueSelectedSprite;
                                    break;
                                case 2:
                                    jewelToSet.jewelColor = Color.green;
                                    jewelToSet.sprite = spriteSetter.greenSprite;
                                    jewelToSet.selectedSprite = spriteSetter.greenSelectedSprite;
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
                    counter++;
                }
            }
        }

        void getUIandLogicObjects() {
            rules = GetComponent<Rule_checks>();
            pauseMenu = FindObjectOfType<Pause_menu>();
            gameEndMenu = FindObjectOfType<Game_end>();
            gridInit = FindObjectOfType<Initialize_Grid>();
            itemLogic = GetComponent<UsableItems_Logic>();
            spriteSetter = GetComponent<Sprites_Setter>();
        }

        
    }

    public void FixedUpdate() {       

        //check bombs first so new jewels haven't fallen into empty cells
        if(shouldBomb) { itemLogic.bombExplosion(bombCell); }
        if(shouldColourBomb) { itemLogic.colourBombExplosion(); }

        /**
         * having these methods in update lead to what i think are clashes where both a jewel 
         * and sand simultaneously go into the same cell because they both think it's empty moving these into
         * fixed update seems to have spaced the calculations out enough that these clashes don't happen
        */
        if(shouldFall.Count > 0 && !isFalling && !isLerping) { jewelFall(); }
        if(sandToFall.Count > 0 && !isFalling && !isLerping) { sandFall(); }

        if(shouldBreak && jewelToBreak != null) {
            List<Cell> jewelToDestroy = new List<Cell>() { jewelToBreak.GetComponentInParent<Cell>() };
            eliminateJewels(jewelToDestroy);
            shouldBreak = false;
        }

        //end conditions
        if(!isLerping && !isPaused && !isFalling && shouldFall.Count == 0 && cellsToEliminate.Count == 0 && sandToFall.Count == 0) {

            List<Cell> cellsWithJewels = findCellsWithJewels();

            //no jewels win
            if(cellsWithJewels.Count == 0) {
                Debug.Log("no jewel win");
                endGame(true);
            } else if (loseGameConditions(cellsWithJewels)) {
                endGame(false);
            }           
        }
    }

    public void Update() {

        if(gameEnded) { pauseMenu.disableButton(); }


        //checking for pause menu and end game onClick events and executing commands       
        if(pauseMenu.isOpen || gameEndMenu.isOpen || isTutorial) { isPaused = true; } else { isPaused = false; }
        if(pauseMenu.reset || gameEndMenu.reset) { resetGame(); }
        if(pauseMenu.shouldQuit || gameEndMenu.shouldquit) { quitGame(); }
        if(gameEndMenu.nextLevel) { nextLevel(); }     

        //if there are cells in the selected array set them to selected
        if (selectedCells[0] != null){selectedCells[0].setSelected(true);}
        if (selectedCells[1] != null){selectedCells[1].setSelected(true);}

        if(!isPaused && !isLerping && !isFalling) {
            //calls playerTurn on left click
            if(Input.GetMouseButtonDown(0)) {

                playerTurn();


                if(selectedCells[0] != null) {
                    playerElim();
                }
                    

                if(selectedCells[0] != null && selectedCells[1] != null) {
                    rules.canSwapJewels(selectedCells[0], selectedCells[1]);
                }

                if(rules.canSwap && turnCounter != 0) {
                    swapJewels();
                }
            }          
        }

        
    }

    private bool loseGameConditions(List<Cell> jewelCells) {
        
        bool shouldEnd = false;
        
        //inits values for end of game checks
        

        //no turns and can't eliminate
        if(turnCounter == 0 && !eliminateSearch()) {
            Debug.Log("no turns no elims loss");
            shouldEnd = true;
            
        }

        //3 or less jewels and no swaps or eliminations
        if(jewelCells.Count < 4 && !eliminateSearch() && !swapSearch()) {
            Debug.Log("3 or less no elims or swap loss");
            shouldEnd = true;
            
        }

        return shouldEnd;
    }

    private void playerElim() {

        rules.CheckThreeInARow(selectedCells[0]);
        rules.getSquareToEliminate(selectedCells[0]);

        //if we can eliminate a square get then destroy
        if(rules.canEliminate && (rules.squareElim || rules.colElim || rules.rowElim)) { 

            List<Cell> cellsToElim = new List<Cell>();

            //initialize array for holding square                       
            if(rules.squareElim) {
                Cell[] squareToElim = new Cell[4];
                squareToElim = rules.getSquareToEliminate(selectedCells[0]);

                for(int i = 0; i < squareToElim.Length; i++) {
                    if(!cellsToElim.Contains(squareToElim[i])) {
                        cellsToElim.Add(squareToElim[i]);
                    }
                }
            }                      

            if(rules.colElim) {
                for(int i = 0; i < rules.threeInCol.Count; i++) {
                    if(!cellsToElim.Contains(rules.threeInCol[i])) {
                        cellsToElim.Add(rules.threeInCol[i]);
                    }
                }
            }

            if(rules.rowElim) {
                for(int i = 0; i < rules.threeInRow.Count; i++) {
                    if(!cellsToElim.Contains(rules.threeInRow[i])) {
                        cellsToElim.Add(rules.threeInRow[i]);
                    }
                }
            }

            for(int i = 0; i < selectedCells.Length; i++) {
                selectedCells[i] = null;
            }

            eliminateJewels(cellsToElim);
        }
    }
    
    private void swapJewels() {
        //gets the children of the selected cells
        if(selectedCells[0] != null && selectedCells[1] != null) {
            if(selectedCells[0].transform.childCount > 0 && selectedCells[1].transform.childCount > 0) { 
                
                Transform cellItem1 = selectedCells[0].transform.GetChild(0);
                Transform cellItem2 = selectedCells[1].transform.GetChild(0);

                //checks that the got item is a jewel 
                if(cellItem1.GetComponent<Jewel>() != null && cellItem2.GetComponent<Jewel>() != null) {
                   
                    //changes parents of the jewel and then updates the transform
                    cellItem1.SetParent(selectedCells[1].transform);
                    cellItem1.GetComponent<Jewel>().currentParent = selectedCells[1];

                    //gets the new position for the jewel to move to 
                    Transform jewelToMove = cellItem1.GetComponent<Jewel>().transform;
                    Vector3 finalPos = new Vector3(selectedCells[1].transform.position.x, selectedCells[1].transform.position.y, -0.1f);
                    
                    //starts the lerp coroutine
                    StartCoroutine(Lerp(jewelToMove.position, finalPos, jewelToMove, false));

                    //repeat the process on the other jewel
                    cellItem2.SetParent(selectedCells[0].transform);
                    cellItem2.GetComponent<Jewel>().currentParent = selectedCells[0];
                    
                    jewelToMove = cellItem2.GetComponent<Jewel>().transform;
                    finalPos = new Vector3(selectedCells[0].transform.position.x, selectedCells[0].transform.position.y, -0.1f);
                                    
                    StartCoroutine(Lerp(jewelToMove.position, finalPos, jewelToMove, false));

                    //decrements the turn counter and sets can swap to false
                    turnCounter--;
                    rules.canSwap = false;

                    //deselects cells so sprites react properly
                    selectedCells[0].setSelected(false);
                    selectedCells[1].setSelected(false);

                    //clear the selected cells for next turn
                    selectedCells[0] = null;
                    selectedCells[1] = null;
                }
            }
        }     
    }

    public void eliminateJewels(List<Cell> cellsToElim) {
        
        //loops through the cells to Elim and destroys the contained jewels
        for(int i = 0; i < cellsToElim.Count; i++) {
            if(cellsToElim[i] != null) {
                
                Transform jewelToDestroy = cellsToElim[i].transform.GetChild(0);

                if(jewelToDestroy.GetComponentInChildren<Colour_Bomb>()) {
                    

                    //need to get color here otherwise jewel will be missing in colorBombExplosion method
                    originColor = jewelToDestroy.GetComponent<Jewel>().jewelColor;
                    shouldColourBomb = true;
                }

                //have to use bools for the usable items for functional execution order in fixed update
                //checks if a jewel contains the bomb item
                if(jewelToDestroy.GetComponentInChildren<Bomb>()) {
                    bombCell = jewelToDestroy.GetComponentInParent<Cell>();
                    shouldBomb = true;
                }

                if(jewelToDestroy.GetComponentInChildren<Concretion>()) {
                    itemLogic.concretion(jewelToDestroy.GetComponentInParent<Cell>());
                }

                jewelToDestroy.GetComponent<Jewel>().animController.SetTrigger("Destroy");            
                
            }
        }

        rules.canEliminate = false;
    }

    private void playerTurn() {
        
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
            Array.Clear(selectedCells, 0, selectedCells.Length); 
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

    private void jewelFall() {        
        /**
            if not paused or lerping and is not also already falling
            trying to make everything fall at the same time causes problems with 
            the raycast and stacking of multiple items in one cell 
        **/

        if(!isPaused && !isLerping && !isFalling) {
            //loops through the should fall array getting the new parent and adjusting to the right transform
            for(int i = 0; i < shouldFall.Count; i++) {

                //ini the jewel to check
                Jewel currentJewel = shouldFall[i];

                //if the current jewel exists
                if(currentJewel != null) {
                   
                    if(currentJewel.checkJewelBelow()) {
                        //get the jewels parent cell and the goal cell
                        Cell currentParent = currentJewel.currentParent;
                        Cell goalCell = getCellAtPosition(currentParent.position[0] - 1, currentParent.position[1]);

                        if(goalCell.transform.childCount == 0) {
                            //set the jewels new parent
                            currentJewel.transform.SetParent(goalCell.transform);
                            currentJewel.currentParent = goalCell;

                            //gets the new position to move to
                            Vector3 posToChangeTo = new Vector3(goalCell.transform.position.x, goalCell.transform.position.y, -0.1f);

                            //starts the lerping coroutine
                            StartCoroutine(Lerp(currentJewel.transform.position, posToChangeTo, currentJewel.transform, true));
                        }

                        //removes the jewel from the should fall array
                        if(currentJewel.checkJewelBelow()) { shouldFall.Remove(currentJewel); }
                    } else { shouldFall.Remove(shouldFall[i]); }
                } else { shouldFall.Remove(shouldFall[i]); }
            }
        }
    }

    private void sandFall() {
        
        if(!isLerping && !isPaused && !isFalling) {
            //loops through the sand to fall array checks which direction it should fall and then updates parent and transform 
            for(int i = 0; i < sandToFall.Count; i++) {
                Sand currentSand = sandToFall[i];
                Cell currentParent = currentSand.currentParent;
                Cell goalCell;

                if(currentSand.fallDown) {
                    goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1]));
                    if(goalCell && goalCell.transform.childCount == 0) { SetParentAndTransform(currentSand, goalCell); }
                } else if(currentSand.fallLeft) {
                    goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1] - 1));
                    if(goalCell && goalCell.transform.childCount == 0) { SetParentAndTransform(currentSand, goalCell); }
                } else if(currentSand.fallRight) {
                    goalCell = getCellAtPosition((currentParent.position[0] - 1), (currentParent.position[1] + 1));
                    if(goalCell && goalCell.transform.childCount == 0) { SetParentAndTransform(currentSand, goalCell); }
                } else {                  
                    sandToFall.Remove(currentSand); 
                }
            }
        }

        //helper method that sets new parent and transform
        void SetParentAndTransform(Sand currentSand, Cell goalCell) {
            
            currentSand.transform.SetParent(goalCell.transform);
            currentSand.currentParent = goalCell;

            Vector3 posToChangeTo = new Vector3(goalCell.transform.position.x, goalCell.transform.position.y, -0.1f);

            StartCoroutine(Lerp(currentSand.transform.position, posToChangeTo, currentSand.transform, true));

            sandToFall.Remove(currentSand);       
        }
    }    

    private void resetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void quitGame() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Title_Screen");
    }

    private void nextLevel() {
        
        int currentSceneIn = SceneManager.GetActiveScene().buildIndex;       
             
        if(currentSceneIn < SceneManager.sceneCountInBuildSettings) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneIn + 1);
        }
    }

    private void endGame(bool wonGame) {
        gameEndMenu.showEnd(wonGame);
        gameEnded = true;
    }

    private bool swapSearch() {

        //get all cells with jewels
        List<Cell> cellsToCheck = findCellsWithJewels();
        bool canSwap = false;
             
        //we first loop through cells with jewels to try and find the valid swaps
        for(int count = 0; count < cellsToCheck.Count; count++) {
            
            Cell[] potentialSwaps = rules.validCellSwaps(cellsToCheck[count]);

            //if there is any valid swaps found we set canSwap to true and break
            for(int swapCheck = 0; swapCheck < potentialSwaps.Length; swapCheck++) {
                if(cellsToCheck.Contains(potentialSwaps[swapCheck])) { 
                    canSwap = true; break;
                }
            }
        }

       

        //if we can swap or eliminate return true
        if(canSwap) { return true; } else { return false; }    
    }

    private bool eliminateSearch() {
        
        List<Cell> cellsToCheck = findCellsWithJewels();
        bool canEliminate = false;
        
        //next we check for elimination of squares 
        for(int elimCheck = 0; elimCheck < cellsToCheck.Count; elimCheck++) {

            rules.getSquareToEliminate(cellsToCheck[elimCheck]);

            //if we can eliminate a square set canEliminate to true
            if(rules.canEliminate && rules.squareElim) {
                canEliminate = true;
                rules.squareElim = false;
                rules.canEliminate = false;
                break;
            }

            rules.CheckThreeInARow(cellsToCheck[elimCheck]);

            //if we can eliminate a row or col set canEliminate to true
            if(rules.canEliminate) {
                if(rules.colElim) {
                    canEliminate = true;

                    //need to reset these values as the checkThreeInARow method sets them
                    rules.colElim = false;
                    rules.canEliminate = false;
                    break;
                } else if(rules.rowElim) {
                    canEliminate = true;
                    rules.rowElim = false;
                    rules.canEliminate = false;
                    break;
                }
            }
        }

        if(canEliminate) { return true; } else { return false; }
    }

    private List<Cell> findCellsWithJewels() {
        
        //init List to store cellswithJewels
        List<Cell> cellsWithJewls = new List<Cell>();
         
        //loops through all cells and checks if they contain a jewel if so add them to the return list
        for(int rows = 0; rows < numOfRows; rows++) {
            for(int cols = 0; cols < numOfCols; cols++) {
                
                Cell cellToCheck = cells[rows, cols];
                
                if(cellToCheck.GetComponentInChildren<Jewel>() != null) {
                    cellsWithJewls.Add(cellToCheck);
                }
            }
        }
        
        return cellsWithJewls;
    }

    IEnumerator Lerp(Vector3 startPos, Vector3 endPos, Transform moveable, bool falling) {        

        //set the time that has passed and init the total time of lerp
        float timeElap = 0;
        float lerpDuration;

        isLerping = true;

        //if the item is falling it should lerp faster dictated by the passed bool
        if(falling) { lerpDuration = 0.2f; } else { lerpDuration = 0.5f; }

        //Lerp
        while(timeElap < lerpDuration) {
            
            

            if(moveable != null) {
                moveable.position = Vector3.Lerp(startPos, endPos, timeElap / lerpDuration);
                timeElap += Time.deltaTime;
            } else {
                timeElap = lerpDuration;
            }

            if(falling) {
                isFalling = true;
            }

            yield return null;
        }

        //moved this from jewel fall to here as there were problems with jewels not breaking due to isLerping         
        if(moveable != null && isFalling) {
            moveable.position = endPos;
            if(moveable.GetComponentInChildren<Fragile>() != null) {
                jewelToBreak = moveable.GetComponent<Jewel>();
                shouldBreak = true;
            }
        }
        
        isFalling = false;
        isLerping = false;       
    }
}
