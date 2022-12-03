using UnityEngine;

namespace Script.GameSystems
{
    [System.Serializable]
    public class Dialogue : MonoBehaviour
    {
        public string name;
        
        [TextArea(3,10)]
        public string[] sentences;
    }
}