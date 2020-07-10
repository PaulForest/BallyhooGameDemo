using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public delegate void SimplePromptCallback();

    public class SimplePromptOption
    {
        public string label;
        public SimplePromptCallback simplePromptCallback;
    }

    public class SimplePrompt : MonoBehaviour
    {
        [SerializeField] private string title;
        [SerializeField] private List<SimplePromptOption> options;

        [SerializeField] private Button iOSBackButton;

        public static SimplePrompt Spawn(SimplePrompt prefab, string title,
            List<SimplePromptOption> simplePromptOptions)
        {
            var go = Instantiate(prefab);
            var prompt = go.GetComponent<SimplePrompt>();
            prompt.title = title;
            prompt.options = simplePromptOptions;
            return prompt;
        }

        private void Awake()
        {
            GlobalEvents.BackButtonPressed.AddListener(GlobalOnBackButtonPressed);

            iOSBackButton.enabled = true;
            iOSBackButton.onClick.AddListener(LocalBackButtonPressed);

            iOSBackButtonGo.SetActive(false);
#if UNITY_IOS
#else
#endif
        }

        private void LocalBackButtonPressed()
        {
            GlobalEvents.BackButtonPressed?.Invoke();
        }

        private void OnDestroy()
        {
            GlobalEvents.BackButtonPressed.RemoveListener(GlobalOnBackButtonPressed);
        }

        private void GlobalOnBackButtonPressed()
        {
        }
    }
}
