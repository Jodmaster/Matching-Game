using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_setup : MonoBehaviour
{
    public int[,] itemToContain;
    public int[,] jemColorMap;

    public void Awake()
    {
        //contains which itmes should go where this array is mirrored vertically to where the items will actually end up
        itemToContain = new int[6, 6] {
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0}
        };

        jemColorMap = new int[6, 6]{
            {0,0,0,0,0,0},
            {1,1,1,1,1,1},
            {2,2,2,2,2,2},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0}
        };
    }
}

    
