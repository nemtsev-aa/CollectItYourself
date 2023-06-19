[System.Serializable]
public class ProgressData 
{
    private int _coins;
    private int _level;
    private bool _isMusicOn;

    public bool IsMusicOn => _isMusicOn;
    public int Coins => _coins;
    public int Level => _level;

    public ProgressData(Progress progress)
    {
        _coins = progress.Coins;
        _level = progress.Level;
        _isMusicOn = progress.IsMusicOn;
    }
}
