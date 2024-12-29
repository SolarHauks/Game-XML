# Projet de Jeu Vidéo  

## Description  
Ce projet est un jeu vidéo développé en C# utilisant le framework Monogame.   
Il a été réalisé dans le cadre d'un cours universitaire sur la formalisation des données, en groupe.   

Ce projet nous a servi à comprendre et à manipuler les différents concepts du cours, qui inclut :
- la formalisation de donnée en utilisant les technologies XML
- la contrainte de ces fichiers avec des schémas XSD
- la transformation de ces fichiers avec des fichiers XSLT
- l'usage de ces données dans le code par :
	- les parseurs forwards
	- les parseurs DOM
	- la sérialisation

Ce projet nous a aussi permis une première approche du framework .NET et de ses spécificités.
  

## Exécution du Jeu    
### Prérequis  
- **.NET SDK** : Assurez-vous d'avoir le SDK .NET installé sur votre machine. Vous pouvez le télécharger depuis [le site officiel de .NET](https://dotnet.microsoft.com/download).  
- **Monogame Framework** : Installez le framework Monogame en suivant les instructions sur [le site officiel de Monogame](http://www.monogame.net/downloads/).  
  
### Étapes pour Exécuter le Jeu  
1. **Cloner le dépôt** :  
	```git clone <URL_DU_DEPOT>```
	```cd <NOM_DU_DEPOT>```
	
1. **Restaurer les dépendances** :    
    ```dotnet restore```  
    
1. **Compiler le projet** :  
    ```dotnet build```  
    
1. **Exécuter le jeu** :    
    ```dotnet run```  
  
## Commandes 
### Controles de Jeu  
- **Flèches directionnelles** : Déplacer le personnage  
- **Espace** : Sauter  
- **C** : Attaque de base  
- **V** : Attaque Spéciale (consomme du mana)  
- **E** : Interagir avec les objets et les personnages  
- **A** : Réapparaître après la mort du personnage  
  
### Autres commandes  
- **P** : Mettre le jeu en pause  
- **R, T, Y** : Changer la résolution du jeu  
- **Échap** : Quitter le jeu
