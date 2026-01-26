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
Mettre en place une génération de run dynamique afin de varier chaque partie, tout en garantissant une progression stable et sans softlock.

- Générer une séquence de salles automatiquement (Start → salles normales → Boss)
- Varier la longueur de la run (min/max) et les layouts choisis
- Préparer l’extension pour varier le contenu (ennemis/items) par salle
- ✅ Terminé (génération des salles) / 🔄 En cours (contenu procédural si ajouté ensuite)

---

### Choix techniques
- Génération procédurale "safe" par assemblage :
  - Les salles sont des prefabs pré-construits (layouts faits à la main)
  - La run est une liste de prefabs générée au runtime (pool-based)
- Séparation claire Data / Logique :
  - `RunConfig` (ScriptableObject) contient les paramètres de génération (start, boss, pool, min/max rooms, doublons)
  - `RunGenerator` construit la séquence de salles (sans instancier)
  - `RoomManager` orchestre la run (instancie/détruit 1 salle à la fois, téléporte le player, gère les transitions)
- Choix "1 room active at a time" :
  - Une seule salle instanciée à la fois pour limiter la complexité (pas de backtracking), réduire les bugs et éviter la gestion de placement des rooms dans l’espace
- Debug orienté développement :
  - logs clairs sur la séquence générée et les transitions

---

### Difficultés rencontrées
- Piège Unity : les prefabs figent les transforms
  - certains éléments de salle étaient sauvegardés en Z = -10 (comme la caméra), rendant la room invisible en GameView
- Références Unity au runtime :
  - une référence de Player assignée dans l’Inspector n’était pas fiable après refactor/procédural (ordre d’exécution + Rigidbody2D)
- Compréhension des bornes de Random.Range :
  - le max est exclusif pour les int, ce qui peut provoquer des erreurs de comptage
- Bugs d’intégration lors du passage "rooms fixes" → "rooms instanciées" :
  - logique de transition, wiring des DoorTriggers uniquement sur la room instanciée, et gestion anti double transition

---

### Solutions apportées
- Convention Z fixe :
  - Caméra en Z = -10
  - Gameplay / rooms / sprites en Z = 0
- Références runtime robustes :
  - récupération du Player au runtime via tag (pour éviter une référence figée/incorrecte)
  - téléportation via Rigidbody2D (et reset si nécessaire)
- Clarification des variables de génération :
  - séparation entre nombre de rooms normales et ajout Start/Boss en dehors de la boucle
  - ajout de min/max rooms et logs de génération
- Sécurité :
  - validations de config (pool non vide, min/max cohérents)
  - prévention des doubles transitions

---

### Résultat
- À chaque lancement de partie :
  - une run est générée dynamiquement avec une longueur variable
  - la Start room apparaît en premier
  - plusieurs rooms normales sont sélectionnées aléatoirement
  - la Boss room est toujours en dernière
- Système extensible pour ajouter :
  - plus de layouts
  - génération procédurale du contenu (ennemis/items) par salle

---

### Notes
Ce jalon m’a appris l’importance de raisonner "runtime-first" sur Unity :
les prefabs figent l’état des objets, et les références Inspector peuvent devenir fragiles lors de refactors ou d’instanciations dynamiques.

### Extension prévue : Ennemis procéduraux (🔄 En cours)

Dans la continuité de la génération procédurale des salles, une extension est prévue afin de varier également le contenu des rooms.

- Chaque prefab de salle définit uniquement une structure et des points de spawn d’ennemis (Transforms)
- Le nombre d’ennemis est déterminé dynamiquement par salle (min/max)
- Les ennemis sont instanciés au runtime à partir d’une liste de prefabs possibles
- Cette approche évite la multiplication de prefabs de salles pour chaque combinaison de difficulté

Objectif :
- Introduire une variation de difficulté entre les rooms
- Préparer l’ajout de nouveaux types d’ennemis sans modifier la structure des salles
- Garder un système extensible et lisible, compatible avec l’architecture existante


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