using UnityEngine;

public class Level_1_setup : MonoBehaviour, ILevel_Setup
{

    public int[,] _jewelColorMap =
           {{0,0,2,0,1,0},
            {1,2,0,1,1,1},
            {2,2,2,2,1,2},
            {0,2,0,1,0,1},
            {0,0,2,1,0,0},
            {0,0,0,2,0,0}};
    
    public int[,] _itemToContain =
            {{0,2,0,2,0,0},
            {0,0,2,1,0,0},
            { 0,0,1,1,0,0},
            { 0,0,0,1,2,0},
            { 0,2,0,0,0,0},
            { 0,0,0,0,0,0}};
    

    public int[,] jewelColorMap { get => _jewelColorMap; }

    public int[,] itemToContain {  get => _itemToContain; }
    
}

    
