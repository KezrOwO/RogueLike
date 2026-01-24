using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGenerator : MonoBehaviour
{
    [SerializeField] private RunConfig runConfig;
    [SerializeField] private int n = 0;
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
        n = Random.Range(runConfig.MinRooms, runConfig.MaxRooms + 1 );
        Debug.Log("RunGenerator: Generating run with " + n + " normal rooms.");
        if (runConfig.AllowDuplicateRooms)
        {
            for (int i = 1; i < n; i++)
            {
                // Correction : choisir un prefab individuel dans la liste
                if (runConfig.RoomPrefabs != null && runConfig.RoomPrefabs.Count > 0)
                {
                    // choisir un prefab aléatoire
                    var prefab = runConfig.RoomPrefabs[Random.Range(0, runConfig.RoomPrefabs.Count)];
                    generatedRooms.Add(prefab);
                }
            }
            Debug.Log("RunGenerator: Generated rooms with duplicates allowed and with n room." + generatedRooms.Count);
        }
        else
        {
            var availableRooms = new List<GameObject>(runConfig.RoomPrefabs);
            for (int i = 1; i < n; i++)
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
        return true;
    }
}
