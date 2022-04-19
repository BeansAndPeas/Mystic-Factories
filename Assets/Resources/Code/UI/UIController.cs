using System;
using System.Collections;
using Resources.Code.Test;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Code {
    public class UIController : MonoBehaviour {
        [SerializeField] 
        private RawImage minimap;
        [SerializeField]
        private Camera minimapCamera;
        [SerializeField]
        private GameObject resourceBar;
        [SerializeField]
        private Transform resourceBarContent;
        [SerializeField]
        private GameObject resourceBarPrefab;
        [SerializeField]
        private Image loadingBar;
        [SerializeField]
        private World world;
        [SerializeField]
        private Generator generator;
        [SerializeField]
        private TextMeshProUGUI loadPercentage;
        [SerializeField]
        private Image loadingScreen;
        internal static bool FinishedGenerating = false;

        private void Start() {
            loadingScreen.gameObject.SetActive(true);
            StartCoroutine(BeginGame());
        }

        private IEnumerator BeginGame() {
            loadingBar.fillAmount = 0;
            
            for (var i = 0; i < 16; i++) {
                Instantiate(resourceBarPrefab, resourceBarContent);
            }

            loadingBar.fillAmount = 0.05f;

            yield return null;

            StartCoroutine(generator.GenerateWorld(loadingBar));
            yield return new WaitForSeconds(10f);
            StartCoroutine(world.GenerateWorld(loadingBar));
        }

        private void Update() {
            if (!loadPercentage.text.Equals("100%")) {
                loadPercentage.SetText(Math.Round(loadingBar.fillAmount * 100, 1) + "%");
            } else {
                if (!loadingScreen.gameObject.activeSelf) return;
                
                foreach (var image in loadingScreen.gameObject.GetComponentsInChildren<Image>()) {
                    image.color = new Color(image.color.r, image.color.g, image.color.b,
                        image.color.a - 1 / 255f);
                }

                foreach (var text in loadingScreen.gameObject.GetComponentsInChildren<TextMeshProUGUI>()) {
                    text.color = new Color(text.color.r, text.color.g, text.color.b,
                        text.color.a - 1 / 255f);
                }

                if (!(loadingScreen.color.a <= 0)) return;
                
                loadingScreen.gameObject.SetActive(false);
            }
        }
    }
}
