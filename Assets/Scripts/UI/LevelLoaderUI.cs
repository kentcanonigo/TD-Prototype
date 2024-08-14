using System;
using UnityEngine;

public class LevelLoaderUI : MonoBehaviour {
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    private float timeBeforeFade = 2f;

    private bool isFading;
    private float fadeStartTime;

    private void Start() {
        GridManager.Instance.OnGridMapInitialized += GridManager_OnGridMapInitialized;
    }

    private void GridManager_OnGridMapInitialized(object sender, EventArgs e) {
        // Start the fade-out process
        isFading = true;
        fadeStartTime = Time.time;
    }

    private void Update() {
        if (isFading) {
            timeBeforeFade -= Time.deltaTime;
            if (timeBeforeFade < 0) {
                float elapsedTime = Time.time - fadeStartTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                fadeCanvasGroup.alpha = alpha;

                // Check if the fade-out duration has completed
                if (elapsedTime >= fadeDuration) {
                    fadeCanvasGroup.alpha = 0f;
                    isFading = false;
                    fadeCanvasGroup.gameObject.SetActive(false); // Optionally disable the CanvasGroup or its GameObject
                }
            }
        }
    }
}