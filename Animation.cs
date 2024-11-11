
namespace JeuVideo;

public class Animation(int[] frames)
{
    private int _counter;
    private int _activeFrame;
    private readonly int _interval = 30;    // TODO : Sera à remplacer par une constante du jeu
    private readonly int _nbFrames = frames.Length;

    public void Update()
    {
        _counter++;
        if (_counter > _interval)
        {
            _counter = 0;
            _activeFrame++;
            
            if (_activeFrame >= _nbFrames)
            {
                _activeFrame = 0;
            }
        }
    }

    public int GetNextFrame()
    {
        return frames[_activeFrame] - 1;
    }
}