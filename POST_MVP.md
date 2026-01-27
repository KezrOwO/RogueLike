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
- ✅ Terminé


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
- Génération procédurale des ennemis par salle :
  - chaque salle définit uniquement des points de spawn (Transforms)
  - le nombre d’ennemis est tiré aléatoirement (min/max)
  - les ennemis sont instanciés au runtime à partir d’un pool de prefabs

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
- Erreur de configuration lors de la génération des rooms :
  - une mauvaise référence de prefab dans le RunGenerator entraînait la génération répétée d’un seul type de salle, ce qui limitait indirectement la variation des ennemis générés
- Gestion de la génération procédurale des ennemis :
  - éviter la duplication de prefabs de salles pour chaque combinaison d’ennemis
  - comprendre et corriger l’héritage involontaire des prefabs (parent → enfants)



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
- Correction de la configuration de génération :
  - vérification et correction du pool de prefabs utilisés par le RunGenerator afin de garantir une réelle variété de salles
  - validation visuelle et via logs que chaque room instanciée correspond bien au prefab attendu
- Séparation claire des responsabilités des salles :
  - salles Start, normales et Boss séparées en prefabs distincts
  - seules les salles normales génèrent des ennemis procéduralement
- Génération contrôlée des ennemis :
  - sélection aléatoire de points de spawn sans doublons
  - limitation du nombre d’ennemis au nombre de points disponibles


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
- Chaque salle normale génère dynamiquement :
  - un nombre variable d’ennemis
  - des positions de spawn différentes à chaque run
- La difficulté varie naturellement au fil de la partie sans modifier les layouts

---

### Notes
Ce jalon m’a appris l’importance de raisonner "runtime-first" sur Unity :
les prefabs figent l’état des objets, et les références Inspector peuvent devenir fragiles lors de refactors ou d’instanciations dynamiques.

### Notes complémentaires
La génération procédurale des ennemis a été intégrée directement au POST-MVP 3 afin de garantir une variation réelle du gameplay entre chaque run, sans multiplier les prefabs ni complexifier l’architecture existante.



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