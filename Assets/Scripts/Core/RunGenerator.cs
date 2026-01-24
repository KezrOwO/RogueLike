using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGenerator : MonoBehaviour
{
    [SerializeField] private RunConfig runConfig;
    private List<GameObject> generatedRooms;

    public List<GameObject> GenerateRun()
    {
        // Validation de la configuration
        if (!ValidateRunConfig())
        {
            Debug.LogError("RunGenerator: Invalid RunConfig. Cannot generate run.");
            return null;
        }

        generatedRooms = new List<GameObject>();
        generatedRooms.Add(runConfig.RoomStart);

        if (runConfig.AllowDuplicateRooms)
        {
            for (int i = 1; i < runConfig.RunLength - 1; i++)
            {
                // Correction : choisir un prefab individuel dans la liste
                if (runConfig.RoomPrefabs != null && runConfig.RoomPrefabs.Count > 0)
                {
                    // choisir un prefab aléatoire
                    var prefab = runConfig.RoomPrefabs[Random.Range(0, runConfig.RoomPrefabs.Count)];
                    generatedRooms.Add(prefab);
                }
            }
        }
        else
        {
            var availableRooms = new List<GameObject>(runConfig.RoomPrefabs);
            for (int i = 1; i < runConfig.RunLength - 1; i++)
            {
                if (availableRooms.Count == 0)
                {
                    Debug.LogWarning("Not enough unique rooms to generate the run.");
                    break;
                }
                int randomIndex = Random.Range(0, availableRooms.Count);
                generatedRooms.Add(availableRooms[randomIndex]);
                availableRooms.RemoveAt(randomIndex);
            }
        }

        generatedRooms.Add(runConfig.RoomBoss);
        return generatedRooms;
    }

    private bool ValidateRunConfig()
    {
        if (runConfig == null)
        {
            Debug.LogError("RunGenerator: RunConfig is null! Please assign a RunConfig ScriptableObject.");
            return false;
        }

        // Vérifier que RunLength >= 2 (minimum Start + Boss)
        if (runConfig.RunLength < 2)
        {
            Debug.LogError($"RunGenerator: RunLength must be at least 2 (Start + Boss). Current value: {runConfig.RunLength}");
            return false;
        }

        // Vérifier que RoomStart n'est pas null
        if (runConfig.RoomStart == null)
        {
            Debug.LogError("RunGenerator: RoomStart is null! Please assign a start room prefab in RunConfig.");
            return false;
        }

        // Vérifier que RoomBoss n'est pas null
        if (runConfig.RoomBoss == null)
        {
            Debug.LogError("RunGenerator: RoomBoss is null! Please assign a boss room prefab in RunConfig.");
            return false;
        }

        // Vérifier que RoomPrefabs n'est pas vide si RunLength > 2
        if (runConfig.RunLength > 2)
        {
            if (runConfig.RoomPrefabs == null || runConfig.RoomPrefabs.Count == 0)
            {
                Debug.LogError($"RunGenerator: RoomPrefabs is empty but RunLength is {runConfig.RunLength}. You need at least {runConfig.RunLength - 2} room prefabs for intermediate rooms.");
                return false;
            }

            // Si duplicates désactivés, vérifier qu'il y a assez de salles uniques
            if (!runConfig.AllowDuplicateRooms)
            {
                int requiredRooms = runConfig.RunLength - 2; // Nombre de salles intermédiaires
                if (runConfig.RoomPrefabs.Count < requiredRooms)
                {
                    Debug.LogError($"RunGenerator: Not enough unique room prefabs. RunLength={runConfig.RunLength} requires {requiredRooms} intermediate rooms, but only {runConfig.RoomPrefabs.Count} prefabs available. Enable AllowDuplicateRooms or add more room prefabs.");
                    return false;
                }
            }
        }

        return true;
    }
}
