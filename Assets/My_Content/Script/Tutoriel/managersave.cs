using UnityEngine;
using System.Collections.Generic;

public static class CheckpointData
{
    public static Vector3 savedPosition = Vector3.zero;
    public static bool hasSavedPosition = false;

    public static Dictionary<string, bool> savedStates = new Dictionary<string, bool>();

    // 🔄 Réinitialise toutes les données de checkpoint
    public static void Reset()
    {
        savedPosition = Vector3.zero;
        hasSavedPosition = false;
        savedStates.Clear();
    }
}
