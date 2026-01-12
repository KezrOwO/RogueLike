# RogueLike 2D - Unity Project

## 📖 Description du Projet

Réalisation d'un roguelike 2D en vue top-down sous Unity, avec caméra fixe.

Le gameplay s'inspire de **The Binding of Isaac** pour la structure des salles et de **FTL** uniquement pour l'ambiance visuelle et narrative.

---

## 🎮 Gameplay

### Mécaniques Principales

- **Déplacement** : 8 directions
- **Attaque** : Attaque à distance par projectile
- **Santé** : Points de vie (PV) limités
- **Fin de run** : À la mort, le jeu s'arrête

### Entités

#### Joueur
- Un seul personnage jouable (pas de système de classe pour le début)
- Attaque de base à distance
- Système de PV

#### Ennemis
- IA simples (3-4 types maximum)
- Comportements :
  - Mouvement
  - Patrouille
  - Attaque
  - Poursuite du joueur
- Caractéristiques : PV, dégâts
- Un ennemi plus tanky en guise de **boss**

#### Salle & Génération
- Plusieurs salles avec enchaînement
- **Génération procédurale simple** : assemblage aléatoire de salles prédéfinies
- Transitions via portes

#### Items
- **Augmentations disponibles** :
  - HP up
  - Damage up
  - Speed up
  - Attack speed up
- Avec limites/caps
- Changements visuels légers pour indiquer la récupération

---

## 🔄 Boucle de Jeu

```
Spawn → Survie + Combat → Loot → Boss → Fin (mort ou vivant)
```

---

## 🏗️ Architecture Générale

### Structure des Dossiers

```
Assets/
  Scripts/
    Player/
    Enemy/
    Core/
    Rooms/
    Items/
  Prefabs/
  ScriptableObjects/
  Scenes/
  Art/
```

### Composants Principaux

| Composant | Responsabilités |
|-----------|-----------------|
| **PlayerController** | PV, vitesses, dégâts, état de vie, comportement |
| **EnemyController** | PV, vitesses, dégâts, état de vie, IA/comportement |
| **GameManager** | Gestion des items, boucle de jeu |
| **RoomController** | Génération des salles, contenu des salles |

### Ressources
- **ScriptableObjects** pour les statistiques et les items

**Lien vers le MVP**: [Lien vers le MVP](MVP.md)

---

## 📋 Phases de Développement

### 🧱 Phase 0 — Setup du Projet (rapide)

**Objectif** : partir propre

**Tâches**
- Création projet Unity 2D (URP non nécessaire)
- Organisation des dossiers (voir structure ci-dessus)
- Création d'une scène principale (MainScene)

**Résultat attendu** : Projet structuré et prêt à développer



---

### 🧍 Phase 1 — Joueur (fondation du jeu)

**Objectif** : se déplacer et mourir

**Tâches**
- **PlayerController**
  - Déplacement 8 directions
  - Gestion vitesse
- **Système de PV**
  - Prise de dégâts
  - Mort
- **Caméra** simple (follow ou fixe)

**Résultat attendu** : Tu peux te déplacer dans une scène vide et mourir.


---

### 🔫 Phase 2 — Attaque & Combat de Base

**Objectif** : interagir avec le monde

**Tâches**
- Tir de projectile
- Cooldown d'attaque
- Collision projectile → ennemi
- Application des dégâts

**Résultat attendu** : Tu tires, tu touches, quelque chose perd des PV.


---

### 👾 Phase 3 — Ennemi Simple

**Objectif** : avoir un combat

**Tâches**
- **EnemyController**
- **IA basique** :
  - Idle
  - Poursuite du joueur
  - Attaque simple
- PV ennemi + mort

**Résultat attendu** : Un ennemi te poursuit, t'attaque, peut mourir.


---

### 🗺️ Phase 4 — Salles & Transitions

**Objectif** : boucle de progression

**Tâches**
- Création de salles préfabriquées
- **RoomController**
- **Portes** :
  - Bloquées tant que les ennemis vivent
  - S'ouvrent quand la salle est nettoyée
- Transition entre salles

**Résultat attendu** : Tu passes de salle en salle.


---

### 🧰 Phase 5 — Items & Progression

**Objectif** : récompense

**Tâches**
- ScriptableObject Item
- Application d'effets au joueur
- Spawn d'item après combat
- Feedback visuel simple

**Résultat attendu** : Tu deviens plus fort au fil de la run.


---

### 👑 Phase 6 — Boss Simple & Fin de Run

**Objectif** : avoir une fin

**Tâches**
- Ennemi plus tanky (boss)
- Salle finale
- Condition de victoire
- Écran fin de run

**Résultat attendu** : La boucle est complète.

---

## 📌 Notes Supplémentaires


- Approche itérative : chaque phase ajoute une couche de gameplay
- Focus sur les mécaniques avant le contenu
- ScriptableObjects pour faciliter la maintenabilité et l'équilibrage

---


## ✍️ Auteur 

Projet réalisé par **Léo Gengembre**  
BUT informatique - Parcours développement d’applications  
Projet personnel orienté game programming 
