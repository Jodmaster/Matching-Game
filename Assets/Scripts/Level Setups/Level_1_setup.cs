using UnityEngine;

public class Level_1_setup : MonoBehaviour, ILevel_Setup
{
    //set usable items for the level here
    int _bombLimit = 2;
    int _colorBombLimit = 1;
    int _concreteLimit = 1;
    int _fragileLimit = 2;
    int _turnLimit = 5;

    /**
     * for the jewel color map:
     * 
     * 0 for red jewels
     * 1 for green jewels 
     * 2 for blue jewels
     */
    public int[,] _jewelColorMap =
           {{0,0,2,0,1,0},
            {1,2,0,1,1,1},
            {2,2,2,2,1,2},
            {0,2,0,1,0,1},
            {0,0,2,1,0,0},
            {0,0,0,2,0,0}};

    /**
     * for the item map:
     * 
     * 0 for jewels
     * 1 for blocker 
     * 2 for sand
     */
    public int[,] _itemToContain =
            {{0,2,0,2,0,0},
            {0,0,2,1,0,0},
            { 0,0,1,1,0,0},
            { 0,0,0,1,2,0},
            { 0,2,0,0,0,0},
            { 0,0,0,0,0,0}};
    

    public int[,] jewelColorMap { get => _jewelColorMap; }

    public int[,] itemToContain {  get => _itemToContain; }

    public int bombLimit { get => _bombLimit; }

    public int colorBombLimit { get => _colorBombLimit; }

    public int concreteLimit { get => _concreteLimit; }

    public int fragileLimit { get => _fragileLimit; }

    public int turnLimit { get => _turnLimit; }
}

    
