using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyWiggle : MonoBehaviour {

    [Required]
    [SerializeField] private WiggleSO wiggleSO;
    [Required]
    [SerializeField] private GameObject enemyVisuals;
    private Wiggle enemyWiggle;

    private void Awake() {
        if (enemyVisuals.TryGetComponent(out enemyWiggle)) {
            Initialize();
        } else {
            enemyWiggle = enemyVisuals.AddComponent<Wiggle>();
            Initialize();
        }
    }

    private void Initialize() {
        enemyWiggle.wiggleSpeed = wiggleSO.wiggleSpeed;
        enemyWiggle.posRange = wiggleSO.wigglePosRange;
        enemyWiggle.rotRange = wiggleSO.wiggleRotRange;
    }
}