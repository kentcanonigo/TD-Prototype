using System;
using UnityEngine;

public class EnemyWiggle : MonoBehaviour {

    [SerializeField] private WiggleSO wiggleSO;
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