# Ennemie

L'idée est de faire une classe générale d'ennemi, puis de faire des sous classes pour chaque type d'ennemi

## Classe générale
Dérive de Game Object

Doit pouvoir bouger, collider avec les tiles 

### Deplacements


Attributs :
- current HP
- max HP
- Speed




# Player Animation :
WALK: frames 1,2,3,4, cycle
JUMP: frame 5 for "jump preparation", frame 6 for moving upwards, frame 7 for moving downards and frame 8 for landing
HIT: frames 9,10,9
SLASH: frames 11,12,13 (you might use them in the order 12,11,12,13 if you want an extra "preparation" frame before the actual slash)
PUNCH: 14,12 (again, you might use them in the order 12,14,12)
RUN: 15-18
CLIMB: 19-22