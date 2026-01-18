using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false; 

    
    public void TriggerGameOver()
    {
        isGameOver = true;
    }
}
