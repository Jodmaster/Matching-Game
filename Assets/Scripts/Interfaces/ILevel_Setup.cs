using UnityEngine;

public interface ILevel_Setup {

    public int[,] itemToContain { set; }
    public int[,] jewelColorMap { set; }


    public void Awake() {
        
        //contains which itmes should go where this array is mirrored vertically to where the items will actually end up
        //0 is a gem
        //1 is blocker
        //2 is sand
        itemToContain = new int[,] { };

        //controls gem color
        //0 is red
        //1 is blue
        //2 is green
        jewelColorMap = new int[,] { };
    }
}
