# Devlog – Post-MVP

## Post-MVP 1 : Items

### Objectif
Ajouter un système d’items simple et lisible afin d’enrichir le gameplay,  
sans remettre en cause la stabilité ni l’architecture du MVP.

- ✅ Terminé

---

### Choix techniques
- Utilisation de **ScriptableObjects** pour définir les données des items (`ItemData`)
- Séparation claire entre :
  - **ItemData** : données statiques (nom, type, valeur, icône)
  - **ItemPickup** : logique en jeu (spawn, collision, application de l’effet)
- Application directe de l’effet au joueur lors du ramassage
- Système volontairement minimal :
  - pas de rareté
  - pas de synergies
  - pas de génération procédurale avancée

---

### Difficultés rencontrées
- Mise en place d’un **tirage aléatoire contrôlé** des items
- Affichage correct de l’item généré (sprite / icône correspondant à l’`ItemData`)
- Éviter toute dépendance forte entre les items et le joueur

---

### Solutions apportées
- Centralisation de la liste des `ItemData` disponibles
- Sélection aléatoire simple via une liste (sans pondération)
- Référence directe à l’`ItemData` dans le prefab `ItemPickup`
- Mapping clair entre données et représentation visuelle

---

### Résultat
- Système d’items fonctionnel, stable et extensible
- Code lisible et modulaire
- Possibilité d’ajouter de nouveaux items sans modifier la logique existanteotes
- …

## POST-MVP 2 : Boss simple

### Objectif
Introduire un ennemi plus exigeant en fin de run afin de créer un pic de difficulté,
tout en restant cohérent avec le gameplay et l’architecture existants.

- Ennemi plus tanky
- Comportement plus agressif
- Boss placé dans la dernière salle
- ✅ Terminé

---

### Choix techniques
- Mise en place d’une architecture basée sur l’héritage :
  - `EnemyBase` : logique commune à tous les ennemis
  - `EnemyBasic` : ennemi standard
  - `EnemyBoss` : spécialisation avec comportement spécifique
- Réutilisation complète du système d’ennemis existant (déplacement, dégâts, tir)
- Surcharge ciblée de méthodes (`OnShoot`, `OnDeath`) pour éviter toute duplication
- Drop d’un item à la mort du boss via `OnDeath`

---

### Difficultés rencontrées
- Compréhension du fonctionnement de l’héritage en C# dans Unity
- Gestion correcte des méthodes de cycle de vie (`Update`, `Start`) avec une classe parent
- Problèmes d’intégration du boss dans le système de rooms existant
- Bugs liés aux références Unity (scripts multiples sur un même prefab, champs sérialisés cassés)

---

### Solutions apportées
- Centralisation de la logique commune dans `EnemyBase`
- Suppression des méthodes `Update` et `Start` inutiles dans les classes enfants
- Création d’un prefab dédié au boss (variant du prefab ennemi)
- Correction des références cassées dans les contrôleurs de rooms
- Tests en situation réelle dans une salle dédiée au boss

---

### Résultat
- Boss fonctionnel avec un pattern de tir différent (tir en éventail)
- Ennemi identifiable comme un pic de difficulté
- Système extensible permettant l’ajout de nouveaux types d’ennemis
- Aucune régression sur le MVP existant

---

### Notes
Ce Post-MVP m’a permis de mieux comprendre l’architecture orientée objet dans Unity
et l’importance d’un refactoring progressif avant l’ajout de nouvelles fonctionnalités.

## POST-MVP 3 : Génération procédurale

### Objectif
- Génération dynamique et procédurale des salles abec une variété de layouts
- 📋 Prévue

### Choix techniques
- ...

### Difficultés rencontrées
- …
- …

### Notes
- …

## POST-MVP 4 : Polissage UI + Feedback

### Objectif
- Interface utilisateur propre , avec des effets visuels et sonores
- 📋 Prévue

### Choix techniques
- ...

### Difficultés rencontrées
- …
- …

### Notes
- …