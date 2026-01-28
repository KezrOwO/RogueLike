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
- Générer dynamiquement le contenu des salles (ennemis) afin de varier la difficulté entre les runs
- Introduire une montée en difficulté progressive et contrôlée
- ✅ Terminé


---

### Choix techniques
- **Génération procédurale "safe" par assemblage** :
  - Les salles sont des prefabs pré-construits (layouts faits à la main)
  - La génération repose sur l’assemblage de ces prefabs plutôt que sur une génération géométrique brute
- **Séparation claire Data / Logique** :
  - `RunConfig` (ScriptableObject) contient les paramètres de génération (start, boss, pool de rooms, min/max rooms, doublons)
  - `RunGenerator` construit la séquence de salles (liste logique, sans instanciation)
  - `RoomManager` orchestre la run (instancie/détruit une salle à la fois, téléporte le player, gère les transitions)
- **Choix architectural : "1 room active at a time"** :
  - Une seule salle instanciée à la fois
  - Pas de backtracking volontairement (run linéaire)
  - Réduction drastique de la complexité (placement spatial, collisions, références croisées)
- **Génération procédurale des ennemis par salle** :
  - Chaque salle définit uniquement des points de spawn (Transforms)
  - Le nombre d’ennemis est déterminé dynamiquement (min/max)
  - Les ennemis sont instanciés au runtime à partir d’un pool de prefabs
  - Les points de spawn sont mélangés afin d’éviter les doublons
- **Montée en difficulté basée sur la progression** :
  - La difficulté est calculée à partir d’un index de salle normalisé (`progress ∈ [0,1]`)
  - Une `AnimationCurve` permet de contrôler finement la montée en difficulté sans modifier le code
  - Le système reste indépendant du nombre total de salles générées
---

### Difficultés rencontrées
- **Piège Unity : les prefabs figent les transforms**
  - Certains éléments de salle avaient été sauvegardés avec un Z = -10 (hérité de la caméra)
  - Les rooms devenaient invisibles en GameView une fois instanciées
- **Références Unity au runtime**
  - Une référence de Player assignée dans l’Inspector devenait invalide après refactor et génération procédurale
  - Problème accentué par l’ordre d’exécution (`Awake` / `Start`) et le `Rigidbody2D`
- **Compréhension des bornes de `Random.Range`**
  - Le maximum est exclusif pour les `int`, ce qui a provoqué des erreurs de comptage initiales
- **Bugs d’intégration lors du passage "rooms fixes" → "rooms instanciées"**
  - Gestion des transitions
  - Risque de double déclenchement des portes
  - Nécessité de sécuriser l’ordre d’instanciation
- **Erreur de configuration des prefabs**
  - Une mauvaise référence dans le `RunGenerator` entraînait la génération répétée d’un seul prefab de salle
  - Effet indirect : faible variation du contenu et des ennemis
- **Gestion du contenu procédural**
  - Éviter la multiplication de prefabs de salles pour chaque combinaison d’ennemis
  - Comprendre l’impact de l’héritage de prefabs (parent → enfants) dans Unity



---

### Solutions apportées
- **Convention de profondeur (Z)** :
  - Caméra fixée en Z = -10
  - Gameplay / salles / entités en Z = 0
- **Références runtime robustes** :
  - Récupération du Player au runtime via tag (`FindGameObjectWithTag`)
  - Téléportation via `Rigidbody2D` pour éviter les conflits physiques
- **Clarification de la génération**
  - Séparation explicite entre :
    - Start room
    - Rooms normales
    - Boss room
  - Le nombre de rooms normales est indépendant de Start/Boss
- **Sécurisation du système**
  - Validation des configurations (pool non vide, min/max cohérents)
  - Protection contre les doubles transitions
- **Correction des erreurs de configuration**
  - Vérification systématique du pool de prefabs utilisé par le `RunGenerator`
  - Ajout de logs pour visualiser la séquence réelle générée
- **Génération contrôlée des ennemis**
  - Shuffle des points de spawn
  - Limitation du nombre d’ennemis au nombre de spawns disponibles
  - Système extensible sans explosion du nombre de prefabs


---

### Résultat
- À chaque lancement de partie :
  - Une run est générée dynamiquement avec une longueur variable
  - La Start room apparaît toujours en premier
  - Plusieurs rooms normales sont sélectionnées aléatoirement
  - La Boss room est toujours en dernière
- Chaque salle normale génère dynamiquement :
  - Un nombre variable d’ennemis
  - Des positions de spawn différentes
- La difficulté augmente naturellement au fil de la progression :
  - Sans modifier les layouts
  - Sans dépendre d’un nombre fixe de salles
- Le système est prêt à accueillir :
  - De nouveaux layouts
  - De nouveaux types d’ennemis
  - Des variations de difficulté plus avancées

---

### Notes
Ce jalon m’a appris l’importance de raisonner **runtime-first** sur Unity :  
les prefabs figent l’état des objets, et les références Inspector peuvent devenir fragiles lors de refactors ou d’instanciations dynamiques.


### Notes complémentaires
La génération procédurale des ennemis a été intégrée directement au POST-MVP 3 afin de garantir une variation réelle du gameplay entre chaque run, sans multiplier les prefabs ni complexifier l’architecture existante.  

Ce choix permet de conserver une base stable tout en préparant efficacement les étapes de polish et d’extension du jeu.


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