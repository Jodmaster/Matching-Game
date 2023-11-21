using UnityEngine;

public class Level_1_setup : MonoBehaviour, ILevel_Setup
{
    public int[,] itemToContain;
    public int[,] jemColorMap;

    public int[,] jewelColorMap { set {
            itemToContain = new int[6, 6] {
            {0,2,0,2,0,0},
            {0,0,2,1,0,0},
            {0,0,1,1,0,0},
            {0,0,0,1,2,0},
            {0,2,0,0,0,0},
            {0,0,0,0,0,0}};
        } 
    }
    
    int[,] ILevel_Setup.itemToContain { set { return itemToContain; }}

    
    public void Awake()
    {
        //contains which itmes should go where this array is mirrored vertically to where the items will actually end up
        //0 is a gem
        //1 is blocker
        //2 is sand
        itemToContain = new int[6, 6] {
            {0,2,0,2,0,0},
            {0,0,2,1,0,0},
            {0,0,1,1,0,0},
            {0,0,0,1,2,0},
            {0,2,0,0,0,0},
            {0,0,0,0,0,0}
        };


        jemColorMap = new int[6, 6]{
            {0,0,2,0,1,0},
            {1,2,0,1,1,1},
            {2,2,2,2,1,2},
            {0,2,0,1,0,1},
            {0,0,2,1,0,0},
            {0,0,0,2,0,0}
        };
    }
    
}

    
