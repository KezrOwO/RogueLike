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
- **Prévue** : génération procédurale simple par assemblage de salles prédéfinies
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



---

## 🚧 État Actuel

**MVP** : En cours (itération 4 complétée)

Pour le détail complet du planning et de l'avancement, voir [MVP.md](MVP.md)

---

## 🎯 Itérations MVP

Le MVP définit le minimum pour prouver la boucle du jeu.

### Itération 1 : Setup + Déplacement
- Organisation projet
- Joueur et déplacement 8 directions
- ✅ Complétée

### Itération 2 : Attaque + Projectiles + Dégâts
- Tir de projectiles
- Système de dégâts
- Visée et cooldown
- ✅ Complétée

### Itération 3 : Ennemi + Combat + PV/Mort
- IA simple (poursuite & attaque)
- Système de combat avec feedback
- Mort du joueur et écran GameOver
- ✅ Complétée

### Itération 4 : Salles + Transitions
- Création de salles préfabriquées
- Portes (bloquées/ouvertes selon ennemis)
- Transition entre salles
- ✅ Complétée

### Itération 5 : Nettoyage + Bugs + Refacto Légère
- Amélioration qualité du code
- Correction de bugs
- Optimisation légère
- ✅ Complétée

### Itération 6 : Build Jouable
- Préparation version jouable
- Tests finaux
- ✅ Complétée

---

## 🚀 Après le MVP

Une fois le MVP terminé, la roadmap post-MVP prévoit :

### Post-MVP 1 : Items
- HP up, Damage up, Speed up, Attack speed up
- Spawn d'items après combats
- Application d'effets au joueur
- ✅ Complétée 

### Post-MVP 2 : Boss Simple
- Ennemi plus tanky et agressif
- Combat final de run
- 🔄 En cours 

### Post-MVP 3 : Génération Procédurale
- Génération dynamique de salles
- Variété de layouts
- 📋 Prévue

### Post-MVP 4 : Polissage UI + Feedback
- Interface utilisateur propre
- Effets visuels (VFX)
- Effets sonores (SFX)
- 📋 Prévue
---

## 🎮 Contrôles

- **Déplacement** : ZQSD
- **Attaque** : Flèches directionnelles 
- **Interagir/Avancer** : Entrée (transitions de salles)

---

## 🔧 Comment lancer le projet

1. Ouvrir le projet dans **Unity 2022+**
2. Charger la scène **MainScene** (Assets/Scenes/)
3. Appuyer sur Play

---

## 📋 Notes de Développement

- Approche itérative : chaque itération ajoute une couche de gameplay
- Focus sur les mécaniques avant le contenu/polish
- ScriptableObjects pour faciliter la maintenabilité et l'équilibrage
- MVP.md contient le planning détaillé et l'état d'avancement

---

## ✍️ Auteur 

Projet réalisé par **Léo Gengembre**  
BUT informatique - Parcours développement d'applications  
Projet personnel orienté game programming
