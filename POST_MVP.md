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
- Ennemi plus tanky a la fin de la run 
- 📋 Prévue

### Choix techniques
- héritage / composition
- réutilisation du système existant

### Difficultés rencontrées
- …
- …

### Notes
- …

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