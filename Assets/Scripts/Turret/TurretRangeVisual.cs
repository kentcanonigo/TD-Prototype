using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TurretRangeVisual : MonoBehaviour {
    [SerializeField] [AssetsOnly] [Required] private GameObject rangeSpritePrefab;
    private GameObject rangeSpriteInstance;
    
    public void ShowRange(float range) {
        if (!rangeSpriteInstance) {
            rangeSpriteInstance = Instantiate(rangeSpritePrefab, transform.position, Quaternion.identity, transform);
        }
        rangeSpriteInstance.transform.localScale = new Vector3(range, range, 1); // Ensure the sprite's diameter matches the range
    }

    public void HideRange() {
        if (rangeSpriteInstance) {
            Destroy(rangeSpriteInstance);
        }
    }
}
