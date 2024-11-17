namespace JeuVideo.Animation;

// Interface pour les animations.
public interface IAnimation
{
    // Met à jour l'animation.
    public void Update();
    
    // Obtient la prochaine image de l'animation.
    // retour : Index de la prochaine image.
    public int GetNextFrame();
}