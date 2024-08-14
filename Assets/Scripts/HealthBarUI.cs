using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
    [SerializeField] private Enemy enemy;
    [SerializeField] private Image barImage;

    private void Start() {
        enemy.OnHealthChanged += Enemy_OnHealthChanged;
        barImage.fillAmount = 1f;
        Hide();
    }

    private void Enemy_OnHealthChanged(object sender, Enemy.OnHealthChangedEventArgs e) {
        barImage.fillAmount = e.healthNormalized;

        if (e.healthNormalized is 0f or 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);

    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
