
namespace JeuVideo.Animation;

public enum AnimationType
{
    Continuous, // Animation qui se joue en continu, par exemple animation de marche
    OneTime // Animation qui se joue une fois, par exemple un coup d'épée
}

// Classe représentant une animation.
public class Animation(int[] frames, AnimationType type, int speed)
{
    // Tableau des indices des frames de l'animation dans le tileset.
    private float _counter; // Compteur pour gérer la vitesse de l'animation.
    private int _activeFrame;   // Indice de la frame actuelle.
    private readonly float _interval = speed * (1.0f / 60.0f); // Intervalle entre chaque frame.
    private readonly int _nbFrames = frames.Length; // Nombre de frames totales dans l'animation.

    public bool IsPlaying { get; set; } = true; // Indique si l'animation est en cours de lecture.

    public AnimationType Type { get; } = type; // Type de l'animation.

    // Constructeur de la classe ContinuousAnimation.
    // param : frames - Tableau des indices des frames de l'animation dans le tileset.

    // Met à jour l'état de l'animation.
    public void Update()
    {
        if (!IsPlaying) return; // Si l'animation n'est pas en cours de lecture, on ne fait rien.

        // On utilise le deltaTime pour indexer la vitesse de l'animation sur le framerate
        float deltaTime = (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
        
        // Logique :
        // Tout les intervalles de temps, on passe à la frame suivante
        // Si on arrive à la dernière frame, on revient à la première
        // Si l'animation est de type OneTime, on arrête l'animation à la fin
        
        _counter += deltaTime;
        if (_counter > _interval)
        {
            _counter -= _interval;
            _activeFrame++;
            if (_activeFrame >= _nbFrames)
            {
                _activeFrame = 0;
                if (Type == AnimationType.OneTime)
                {
                    IsPlaying = false;
                }
            }
        }
    }

    // Obtient la prochaine frame de l'animation.
    // retour : L'indice de la prochaine frame dans le tileset.
    public int GetNextFrame() => frames[_activeFrame] - 1;
}