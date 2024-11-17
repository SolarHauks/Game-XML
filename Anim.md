L'idée est de procéder par Automate

### Code

string currentState;	// Stock l'état courant 
dico anim<string, anim>;	Dico d'anim, déja présent

void changeState(string newState) 
{
	si (newState est dans dico)
		currentState = newState;
	sinon
		afficher erreur;
		retour;
}

void Update() 
{
	dico[currentState].Update;
}

### Notes
Mais ducoup, comment on gère les anims ?
Dans l'état, ça anim résum quand on passe dessus

Comment faire pour les anims type jump, où ça dépend de la physique du joueur ?

3 types d'anim :
- Anim continu : walk, run, climb
  -> pas de pb, juste jouer tant que c'est actif

- Anim ponctuel : slash, hit, punch
  -> joueur l'anim puis revenir à l'anim continu d'avant

- Anim dépendant de la physique : jump
  -> ???

Ducoup :
Faut t'il différencier les types d'anim ? Avec 3 comportements différents ?
Plusieurs framerates ?

continu -> isok
ponctuel -> just joueur l'anim une fois puis résum celle d'avant

physique -> faudrait passer la physique du joueur en para de l'Update.
Sachant que pour le saut :
JUMP: frame 5 for "jump preparation", frame 6 for moving upwards, frame 7 for moving downards and frame 8 for landing
pression sur jump -> 5 for a frame duration
moving upward -> 6
moving downward -> 7
landing -> 8 for a frame duration
A voir pour les frame duration
Donc juste besoin de Velocity
Donc 3 types d'anim

Quel structure alors ?
Interface / master class abstraite + sous classes spécialisant
Quel code en commun ?
continu et ponctuel très proche -> continu = ponctuel en boucle
Donc 2 classes ou une classe avec des if ? Probablement plutot 2 classes car plus propre. 
Mais on peut peut etre grouper / réutiliser des trucs. Héritage ?

physique -> frame dépend de physique.
Mais cette logique veut donc dire toujours le meme nombre de frame
Ducoup, une classe spéciale pour le saut en fait
Veut donc dire une classe par anim de ce type -> combien d'anim de ce type
Qui va pouvoir sauter ? Player + ennemi peut etre (si pathfinding notamment)
Pour l'instant on va partir sur ça. On verra après

Ducoup, structure ?
