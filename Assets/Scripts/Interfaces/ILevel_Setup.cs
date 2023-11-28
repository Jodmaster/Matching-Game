
public interface ILevel_Setup {

    public int bombLimit { get; }
    public int colorBombLimit { get; }
    public int concreteLimit { get; }
    public int fragileLimit { get; }
    
    public int turnLimit { get; }

    public int[,] itemToContain { get; }
    public int[,] jewelColorMap { get; }

}
