
namespace JeuVideo.Animation;

// Représente une animation qui s'exécute une seule fois.
public class OneTimeAnimation : IAnimation
{
    private readonly int[] _frames;
    private int _counter;
    private int _activeFrame;
    private const int Interval = 15; // TODO : Sera à remplacer par une constante du jeu

    public bool IsPlaying { get; private set; }
    
    // Constructeur de la classe OneTimeAnimation.
    // param : frames - Tableau des indices des frames de l'animation dans le tileset.
    public OneTimeAnimation(int[] frames)
    {
        _frames = frames;
        IsPlaying = false;
    }
    
    // Met à jour l'état de l'animation.
    public void Update()
    {
        if (!IsPlaying) return;

        _counter++;
        if (_counter > Interval)
        {
            _counter = 0;
            _activeFrame++;
            if (_activeFrame >= _frames.Length)
            {
                _activeFrame = _frames.Length - 1; // Reste sur la dernière frame
                IsPlaying = false; // Stop l'animation
            }
        }
    }
    
    // Obtient la prochaine frame de l'animation.
    // retour : L'indice de la prochaine frame dans le tileset.
    public int GetNextFrame()
    {
        return _frames[_activeFrame] - 1;
    }
    
    // Démarre l'animation.
    public void Play()
    {
        IsPlaying = true;
        _counter = 0;
        _activeFrame = 0;
    }
    
}