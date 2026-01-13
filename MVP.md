# MVP Jouable

## ✅ Ce que le MVP doit contenir 

- Déplacement joueur 
- Tir de projectiles
- 1 Type d'ennemie
- 2-3 salles enchainées
- Mort du joueur 
- Fin de run 

**Notes** : Sans items, sans boss, sans polish.

---
## 📅 Planing du MVP 

### Première itération (fini)

- Setup projet
- Joueur + déplacement 

**Notes de fin** :
- Déplacement fonctionelle, Objet joeur crée  
- Rien de bloquant
- Pas d'amélioration en vu
### Deuxième itération (fini)

- Attaque + projectiles
- Dégâts

**Notes de fin** : 
- Attaques avec les flèches directionnelles, créations de projectiles 
- Sytème de visée 
- Système d'input de touches à revoir, mieux repartir le code   

### Troisième itération 

- Ennemi simple
- Combat fonctionnel
- Système de PV / mort

**Notes de fin**

- Ennemi qui suit le joueur et attaque , système de combat avec feedback de hit + mort d'un joueur écran de gameOver 
- Vrai difficulté sur le feedback du joueur et ennemie + mort du joueur 
- Améliorer le système de mort (avec cela HealthComponent Un OnDeath() qui notifie un GameManager Et le GameManager gère l’état global (GameOver, UI, reload, etc.))

### Quatrième itération 

- Salles + transitions

### Cinquième itération 

- Nettoyage
- Bugs
- Refacto légère

### Sixième itération 

- Build jouable

--- 

**Lien vers le README**: [Lien vers le README](README.md)
