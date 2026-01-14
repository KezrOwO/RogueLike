# MVP Jouable

## 📌 Objectif du MVP

Prouver la boucle de gameplay core et la structure technique :
- Se déplacer
- Attaquer
- Combattre un ennemi
- Progresser à travers plusieurs salles
- Mourir et voir écran GameOver

**Hors MVP** : Items, boss, polish, équilibrage fin

---

## ✅ Checklist MVP

- ✅ Déplacement joueur (8 directions)
- ✅ Tir de projectiles
- ✅ 1 type d'ennemi simple
- ✅ Système de PV / mort (joueur + ennemi)
- ✅ 2-3 salles enchaînées
- ✅ Transitions entre salles (portes)
- ✅ Écran GameOver

---

## 📅 Planning des Itérations

### Itération 1 : Setup + Déplacement ✅

**Objectif** : Fonder le projet et pouvoir se déplacer

**Tâches**
- Organisation projet Unity (dossiers, structure)
- PlayerController avec déplacement 8 directions
- Caméra (follow ou fixe)

**État** : ✅ Complétée
- Déplacement fonctionnel
- Objet joueur créé

---

### Itération 2 : Attaque + Projectiles + Dégâts ✅

**Objectif** : Implémenter l'attaque de base

**Tâches**
- Système de tir de projectile
- Cooldown d'attaque
- Collision projectile → ennemi
- Système de dégâts
- Visée en 4 directions

**État** : ✅ Complétée
- Attaques avec les flèches directionnelles
- Création de projectiles
- Système de visée

**À revoir** : Architecture des inputs (mieux répartir le code)

---

### Itération 3 : Ennemi + Combat + PV/Mort ✅

**Objectif** : Avoir un vrai combat contre un ennemi

**Tâches**
- EnemyController avec IA basique (idle, poursuite, attaque)
- Système de PV pour joueur et ennemi
- Mort et respawn/GameOver
- Feedback visuel de hit

**État** : ✅ Complétée
- Ennemi qui suit le joueur et attaque
- Système de combat fonctionnel
- Écran GameOver activé à la mort du joueur

**Difficultés surmontées** : Feedback du joueur et ennemi, gestion de la mort
**À améliorer** : HealthComponent avec OnDeath() → GameManager gère l'état global

---

### Itération 4 : Salles + Transitions ✅

**Objectif** : Boucle de progression avec plusieurs salles

**Tâches**
- Création de salles préfabriquées
- RoomController (gestion du contenu, spawning ennemis)
- Système de portes (bloquées tant qu'ennemis vivants)
- Transition entre salles

**État** : ✅ Complétée
- Création de plusieurs salles
- Transitions fonctionnelles
- Portes qui se débloquent

**Bugs réglés** : Trigger des portes
**À améliorer** : Qualité du code + refacto, architecture des états

---

### Itération 5 : Nettoyage + Bugs + Refacto Légère 🔄

**Objectif** : Stabiliser et nettoyer le code avant build

**Tâches**
- Correction de bugs critiques
- Refactoring léger (architecture, lisibilité)
- Optimisation des systèmes existants
- Tests de jouabilité

**État** : 🔄 En cours

---

### Itération 6 : Build Jouable 📋

**Objectif** : Préparer une version jouable et stable

**Tâches**
- Tests finaux complets
- Build executables (Windows/Web)
- Documentation pour lancer le jeu

**État** : 📋 Prévue

---

## 🚀 Après le MVP

Une fois le MVP stabilisé, les prochaines étapes post-MVP :

### Post-MVP 1 : Items
- HP up, Damage up, Speed up, Attack speed up
- Spawn d'items après combats
- Application d'effets au joueur
- UI pour afficher les upgrades

### Post-MVP 2 : Boss Simple
- Ennemi plus tanky et patterns d'attaque spéciaux
- Salle finale
- Condition de victoire

### Post-MVP 3 : Génération Procédurale
- Génération dynamique de salles (au lieu de préfab fixes)
- Layouts variés
- Augmentation de la rejouabilité

### Post-MVP 4 : Polissage UI + Feedback
- Interface utilisateur complète
- Menu principal, pausemenu
- Effets visuels (VFX)
- Effets sonores (SFX)

---

## 📊 Logique du MVP

Le MVP minimal = **prouver que la boucle fonctionne** :

```
Spawn → Déplacement → Combat → Salle suivante → Mort/GameOver
```

C'est **suffisant** pour tester :
- Les mécaniques de base
- La structure technique
- La courbe de jouabilité

**C'est PAS le jeu fini** → items, variété, polish arrivent après.

---

## 🔗 Voir aussi

- [README.md](README.md) : présentation générale + roadmap complète
- Contrôles actuels : ZQSD pour se déplacer, Flèches pour attaquer
