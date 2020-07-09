using UnityEngine;

namespace UI
{
    public class SimplePrompt : MonoBehaviour
    {
        public static SimplePrompt Spawn()
        {
            var go = new GameObject("SimplePrompt");
            var prompt = go.AddComponent<SimplePrompt>();
            return prompt;
        }
        
    }
}