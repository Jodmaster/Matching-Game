using System.Collections.Generic;
using UnityEngine;

public class UsableItems_Logic : MonoBehaviour {
    Game_manager manager;
    Rule_checks rules;

    // Start is called before the first frame update
    void Start() {
        manager = FindObjectOfType<Game_manager>();
        rules = FindObjectOfType<Rule_checks>();
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

        manager.shouldBomb = false;
        manager.bombCell = null;
        manager.eliminateJewels(cellsWithJewels);
    }

    public void colourBombExplosion() {

        List<Cell> cellWithCorrectColour = new List<Cell>();

        //loop through all the cells and checks if they contain a same color jewel
        for(int row = 0; row < manager.numOfRows; row++) {
            for(int col = 0; col < manager.numOfCols; col++) {

                Cell cellToCheck = manager.cells[row, col];

                if(cellToCheck.GetComponentInChildren<Jewel>() != null) {
                    if(cellToCheck.GetComponentInChildren<Jewel>().jewelColor == manager.originColor) {
                        cellWithCorrectColour.Add(cellToCheck);
                    }
                }
            }
        }

        //deletes all jewels with same color
        manager.shouldColourBomb = false;
        manager.eliminateJewels(cellWithCorrectColour);
    }

    public void concretion(Cell cellToConcrete) {
        if(cellToConcrete.GetComponentInChildren<Jewel>() != null) {
            cellToConcrete.setContainedItem(manager.blocker);
        }
    }
}

