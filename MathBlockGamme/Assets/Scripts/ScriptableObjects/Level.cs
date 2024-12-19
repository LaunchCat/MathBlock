using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    [System.Serializable]
    public struct MathBlockData
    {
        public float value;
        public MathBlock.Operation operation;

    }
    
    public float levelTarget;
    public List<MathBlockData> levelData;
    

}
