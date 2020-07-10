using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class SimplePromptOptionData
    {
        public string buttonText;
        public UnityAction simplePromptCallback;
    }

    public class SimplePrompt : MonoBehaviour
    {
        [SerializeField] private string title;
        [SerializeField] private List<SimplePromptOptionData> options;

        [SerializeField] private Button backButton;
        [SerializeField] private GameObject buttonParentGo;

        [SerializeField] private SimplePromptOption simplePromptOptionPrefab;

        private readonly List<SimplePromptOption> _simplePromptOptions = new List<SimplePromptOption>();

        public static Task<SimplePrompt> Spawn(SimplePrompt prefab, SimplePromptOption simplePromptOptionPrefab,
            string title, List<SimplePromptOptionData> simplePromptOptionDataList)
        {
            var go = Instantiate(prefab);
            var prompt = go.GetComponent<SimplePrompt>();
            prompt.title = title;
            prompt.options = simplePromptOptionDataList;

            foreach (var simplePromptOptionData in prompt.options)
            {
                var optionGo = Instantiate(simplePromptOptionPrefab);
                optionGo.label.text = simplePromptOptionData.buttonText;
                optionGo.button.onClick.AddListener(simplePromptOptionData.simplePromptCallback);

                prompt._simplePromptOptions.Add(optionGo);
            }

            return Task.FromResult(prompt);
        }

        private void Awake()
        {
            GlobalEvents.BackButtonPressed.AddListener(GlobalOnBackButtonPressed);

            backButton.onClick.AddListener(LocalBackButtonPressed);
        }

        private void LocalBackButtonPressed()
        {
            GlobalEvents.BackButtonPressed?.Invoke();
        }

        private void OnDestroy()
        {
            GlobalEvents.BackButtonPressed.RemoveListener(GlobalOnBackButtonPressed);
            foreach (var simplePromptOption in _simplePromptOptions)
            {
                simplePromptOption.button.onClick.RemoveAllListeners();
            }
        }

        private void GlobalOnBackButtonPressed()
        {
            Destroy(gameObject);
        }
    }
}