# A fix
- Quand le joueur colle une plateforme sur le coté, il se tp (au dessus souvent). Vient probablement de GetIntersectingTilesHorizontal
- Depuis la correction des collisions des ennemis, on ne peut plus les taper

# TODO
- A faire : taille affichage (ATTENTION : GROS TRAVAIL)
- Ré-écrire : A plusieurs endroits on fait des if pour vérifier que le contenu de certaines variables n'est pas nul.
  On pourrait remplacer ça par des validations de schéma xml, pour vérifier que nos données sont correct
  Il faut donc faire tous les schémas et intégrer les vérifications au code
- A faire : faire charger les données du joueur depuis un xml
- Ré-écrire : Réorganiser le rectangle logique, qui doit aller dans GameObject. Vérifier qu'est ce qui utilise quoi
  Rectangle d'affichage doit pouvoir passer en private (je crois)



# A demander
- Quand utiliser des interfaces ? Parce que classe abstraite