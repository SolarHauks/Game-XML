# A fix
- Fix : quand le joueur colle une plateforme sur le coté, il se tp (au dessus souvent). Vient probablement de GetIntersectingTilesHorizontal
- Fix : taille affichage (ATTENTION : GROS TRAVAIL)



# A demander :
- Pour la classe GameObject, vaut-il mieux créer des méthodes communes qui serviront de briques aux sous-classe
  ou vaut-il mieux prévoir la logique générale et demander aux sous-classes de réimplémenter les fonctions nécéssaires ?
- "En résumé, le masquage (new) cache la méthode de la classe de base, tandis que la réimplémentation (override) remplace la méthode de la classe de base."
  Quoi, quand, pourquoi ?
