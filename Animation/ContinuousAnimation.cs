namespace JeuVideo.Animation;

// Classe représentant une animation continue.
public class ContinuousAnimation : IAnimation
{
    private readonly int[] _frames;
    private int _counter;
    private int _activeFrame;
    private const int Interval = 15; // TODO : Sera à remplacer par une constante du jeu
    
    // Constructeur de la classe ContinuousAnimation.
    // param : frames - Tableau des indices des frames de l'animation dans le tileset.
    public ContinuousAnimation(int[] frames)
    {
        _frames = frames;
        _activeFrame = 0;
        _counter = 0;
    }

    // Met à jour l'état de l'animation.
    public void Update()
    {
        _counter++;
        if (_counter > Interval)
        {
            _counter = 0;
            _activeFrame++;
            if (_activeFrame >= _frames.Length)
            {
                _activeFrame = 0;
            }
        }
    }

    // Obtient la prochaine frame de l'animation.
    // retour : L'indice de la prochaine frame dans le tileset.
    public int GetNextFrame()
    {
        return _frames[_activeFrame] - 1;
    }
}