using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Rule_checks : MonoBehaviour
{
    public bool canSwap;
    public bool canEliminate;
    
    public Game_manager manager;

    public bool colElim;
    public bool rowElim;
    public bool squareElim;

    public List<Cell> threeInCol;
    public List<Cell> threeInRow;
    public Cell[] sqaure;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<Game_manager>();
        canSwap = false;
        canEliminate = false;
    }

    public void canSwapJewels(Cell cell1, Cell cell2)
    {
        Cell[] potentialSwaps = validCellSwaps(cell1);

        for(int i = 0; i < potentialSwaps.Length; i++) {
            if(potentialSwaps[i] == cell2) {
                canSwap = true;
            }
        }
    }

    public Cell[] validCellSwaps(Cell cell) {

        //intialises return array
        Cell[] validCells = new Cell[4];

        //arrays for checking the rows and columns
        int[] validRows = checkRows(cell);
        int[] validCols = checkCols(cell);

        //gets the valid swap cells
        validCells[0] = manager.getCellAtPosition(cell.position[0], validCols[0]);
        validCells[1] = manager.getCellAtPosition(cell.position[0], validCols[1]);
        validCells[2] = manager.getCellAtPosition(validRows[0], cell.position[1]);
        validCells[3] = manager.getCellAtPosition(validRows[1], cell.position[1]);

        return validCells;

        int[] checkRows(Cell celly)
        {

            int[] validRowPos = new int[2];

            //gets the possible rows 
            int[] possibleRowPos = new int[2]{ celly.position[0] + 1, celly.position[0] - 1 };

            //checks that the possible rows are valid and then adds them to the validRows array
            for (int i = 0; i < possibleRowPos.Length; i++){
                if(possibleRowPos[i] > 0 || possibleRowPos[i] < manager.numOfRows){
                    validRowPos[i] = possibleRowPos[i];
                }
            }
            
            //returns the array
            return validRowPos;
        }

        int[] checkCols(Cell celly)
        {
            int[] validColPos = new int[2];

            //gets the possible cols
            int[] possibleColPos = new int[2]{celly.position[1] + 1, celly.position[1] - 1};

            //checks that the possible cols are valid and then adds them to validCols array
            for (int i = 0; i < possibleColPos.Length; i++){
                if (possibleColPos[i] > 0 || possibleColPos[i] < manager.numOfCols){
                    validColPos[i] = possibleColPos[i];
                }
            }

            //returns the array
            return validColPos;
        }
    }

    public void CheckThreeInARow(Cell originCell) {

        Color originJewel;      

        //arrays to hold all the cells to check
        Cell[] colToCheck = new Cell[6];
        Cell[] rowToCheck = new Cell[6];

        //getting origin jewel info
        if(originCell != null) {
            if(originCell.transform.childCount == 1) {
                if(!originCell.GetComponentInChildren<Blocker>() && !originCell.GetComponentInChildren<Sand>()) {
                    originJewel = originCell.transform.GetChild(0).GetComponent<Jewel>().jewelColor;
                } else { originJewel = Color.black; }
            } else { originJewel = Color.black; }

            //gets the potential cells on the cols and rows that should be searched for 3s
            getColumn();
            getRow();

            //gets the cells that should be eliminated in a col or row around the origin and puts them in cell arrays for processing 
            threeInCol = checkCanEliminateColumn();
            threeInRow = checkCanEliminateRow();
        }
            
        void getColumn() {
            for(int colCount = 0; colCount < colToCheck.Length; colCount++) {
                colToCheck[colCount] = manager.getCellAtPosition(colCount, originCell.position[1]);
            }
        }

        void getRow() {
            for(int rowCount = 0; rowCount < rowToCheck.Length; rowCount++) {
                rowToCheck[rowCount] = manager.getCellAtPosition(originCell.position[0], rowCount);
            }
        }

        List<Cell> checkCanEliminateColumn() {

            //unbroken streak tracks how many cells of the right color have been found in a row 
            
            colElim = false;
            threeInCol = new List<Cell>() { originCell };

            for(int x = 1; x < originCell.position[0]; x++) {

                Cell cellColDown;

                if(manager.getCellAtPosition(originCell.position[0] - x, originCell.position[1]) != null) {
                    cellColDown = manager.getCellAtPosition(originCell.position[0] - x, originCell.position[1] );
                } else { break; };

                // we check that it's first not an invalid cell then if it has any children then if those children are either blockers or sand
                // and then finally for the right jewel color if none of these conditions are false we break out of the loop 

                if(cellColDown != null) {
                    if(cellColDown.transform.childCount == 1) {
                        if(!cellColDown.GetComponentInChildren<Blocker>() && !cellColDown.GetComponentInChildren<Sand>()) {
                            if(cellColDown.transform.GetChild(0).GetComponent<Jewel>().jewelColor == originJewel) {
                                threeInCol.Add(cellColDown);
                            } else { break; }
                        } else { break; }
                    } else { break; }
                } else { break; }
            }

            for(int x = 1; x <= manager.numOfRows - (originCell.position[0] - 1); x++) {

                Cell CellColUp;

                if(manager.getCellAtPosition(originCell.position[0] + x, originCell.position[1] ) != null) {
                    CellColUp = manager.getCellAtPosition(originCell.position[0] + x, originCell.position[1]);
                } else { break; };

                // we check that it's first not an invalid cell then if it has any children then if those children are either blockers or sand
                // and then finally for the right jewel color if none of these conditions are false we break out of the loop 

                if(CellColUp != null) {
                    if(CellColUp.transform.childCount == 1) {
                        if(!CellColUp.GetComponentInChildren<Blocker>() && !CellColUp.GetComponentInChildren<Sand>()) {
                            if(CellColUp.transform.GetChild(0).GetComponent<Jewel>().jewelColor == originJewel) {
                                threeInCol.Add(CellColUp);
                            } else { break; }
                        } else { break; }
                    } else { break; }
                } else { break; }
            }

            Debug.Log(threeInCol.Count);

            if(threeInCol.Count >= 3) {
                canEliminate = true;
                colElim = true;
                return threeInCol;
            } else {
                colElim = false;
                return null;
            }
        }

       List<Cell> checkCanEliminateRow() {        
            
            rowElim = false;
            threeInRow = new List<Cell>() {originCell};          

            for(int x = 1; x < originCell.position[1] + 1; x++) {

                Cell cellRowLeft;

                if(manager.getCellAtPosition(originCell.position[0], originCell.position[1] - x) != null) {
                    cellRowLeft = manager.getCellAtPosition(originCell.position[0], originCell.position[1] - x);
                } else { break; };

                // we check that it's first not an invalid cell then if it has any children then if those children are either blockers or sand
                // and then finally for the right jewel color if none of these conditions are false we break out of the loop 
                
                if(cellRowLeft != null) {
                    if(cellRowLeft.transform.childCount == 1) {
                        if(!cellRowLeft.GetComponentInChildren<Blocker>() && !cellRowLeft.GetComponentInChildren<Sand>()) {
                            if(cellRowLeft.transform.GetChild(0).GetComponent<Jewel>().jewelColor == originJewel) {
                                threeInRow.Add(cellRowLeft);
                            } else { break; }
                        } else { break; }
                    } else { break; }
                } else { break; }
            }

            for(int x = 1; x <= manager.numOfCols - (originCell.position[1] + 1); x++) {

                Cell cellRowRight;

                if(manager.getCellAtPosition(originCell.position[0], originCell.position[1] + x) != null) {
                    cellRowRight = manager.getCellAtPosition(originCell.position[0], originCell.position[1] + x);
                } else { break; };
                

                // we check that it's first not an invalid cell then if it has any children then if those children are either blockers or sand
                // and then finally for the right jewel color if none of these conditions are false we break out of the loop 

                if(cellRowRight != null) {
                    if(cellRowRight.transform.childCount == 1) {
                        if(!cellRowRight.GetComponentInChildren<Blocker>() && !cellRowRight.GetComponentInChildren<Sand>()) {
                            if(cellRowRight.transform.GetChild(0).GetComponent<Jewel>().jewelColor == originJewel) {
                                threeInRow.Add(cellRowRight);
                            } else { break; }
                        } else { break; }
                    } else { break; }
                } else { break; }
            }

            if(threeInRow.Count >= 3) {
                canEliminate = true;
                rowElim = true;
                return threeInRow;
            } else {
                rowElim = false;
                return null;
            }
        }
    }

    public Cell[] CheckSquare(Cell originCell){
        
        Cell[] squareToCheck = new Cell[9];
        
        int counter = 0;
        int colOffset = -1;
        int rowOffset = -1;
        

        for(int rowCount = 0; rowCount < 3; rowCount++) {
            for(int colCount = 0; colCount < 3; colCount++) {
                squareToCheck[counter] = manager.getCellAtPosition(originCell.position[0] + rowOffset, originCell.position[1] + (colOffset + colCount));
                counter++;
            }

            rowOffset++;
        }

        return (squareToCheck);
    }

    public Cell[] getSquareToEliminate(Cell originCell) {

        int[] startPoses = new int[4] {0, 1, 3, 4};
        Color originalColor;

        if(originCell.transform.childCount == 1) {
            if (originCell.GetComponentInChildren<Jewel>() != null) {
                originalColor = originCell.GetComponentInChildren<Jewel>().jewelColor;
            } else { return null; }
        } else { return null; }

        int unbrokenStreak = 0;
        
        Cell[] cellsToCheck = CheckSquare(originCell);
        Cell[] finalsquareCells = new Cell[4]; 

        //top level checking all 4 2x2 sqaure
        for(int i = 0; i < 4; i++) {
            
            //gets a 2x2 
            Cell[] current2by2 = getFourCells(startPoses[i]);

            //checking each 2x2 to see if it's valid 
            for(int currentJewel = 0; currentJewel < current2by2.Length; currentJewel++) {
                Cell cellToCheck = current2by2[currentJewel];

                if(cellToCheck != null) {
                    if(cellToCheck.transform.childCount == 1 && !cellToCheck.GetComponentInChildren<Blocker>() && !cellToCheck.GetComponentInChildren<Sand>()){
                        if(cellToCheck.GetComponentInChildren<Jewel>().jewelColor == originalColor) {
                            finalsquareCells[currentJewel] = cellToCheck;
                            unbrokenStreak++;
                        }
                    }
                }
            }

            if(unbrokenStreak == 4) {
                canEliminate = true;
                squareElim = true;
                break;
            
            } else { 
                squareElim = false;
                unbrokenStreak = 0;
            }

        }

        if(canEliminate && squareElim) { return finalsquareCells; } else { return null; }
        

        Cell[] getFourCells(int startPos) {
            
            Cell[] currentSquare = new Cell[4];

            currentSquare[0] = cellsToCheck[startPos];
            currentSquare[1] = cellsToCheck[startPos + 1];
            currentSquare[2] = cellsToCheck[startPos + 3];
            currentSquare[3] = cellsToCheck[startPos + 4];

            return currentSquare;
        }
    }
}