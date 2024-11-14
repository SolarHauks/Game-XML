# A fix
- Fix : quand le joueur colle une plateforme sur le coté, il se tp (au dessus souvent). Vient probablement de GetIntersectingTilesHorizontal
- Fix : taille affichage (ATTENTION : GROS TRAVAIL)
- Optionnel : Dans l'AnimManager, on pourrait rajouter une vérification de l'instance XML des données vis à vis d'un schéma
- Fix collisions ennemies (il faut utiliser la vélocité)



# A demander :
- Pour la classe GameObject, vaut-il mieux créer des méthodes communes qui serviront de briques aux sous-classe
  ou vaut-il mieux prévoir la logique générale et demander aux sous-classes de réimplémenter les fonctions nécéssaires ?