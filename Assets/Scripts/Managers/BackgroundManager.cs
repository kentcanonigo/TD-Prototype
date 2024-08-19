using System;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {
    
    public static BackgroundManager Instance { get; private set; }
    
    private SpriteRenderer backgroundSprite;

    private void Awake() {
        Instance = this;
        backgroundSprite = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        // Get the grid bounds
        Bounds gridBounds = GridManager.Instance.TryGetMainGrid().GetGridBounds();

        // Get the size of the grid
        float gridWidth = gridBounds.size.x;
        float gridHeight = gridBounds.size.y;

        // Get the size of the sprite
        float spriteWidth = backgroundSprite.sprite.bounds.size.x;
        float spriteHeight = backgroundSprite.sprite.bounds.size.y;

        // Calculate scale factors
        float widthScale = gridWidth / spriteWidth;
        float heightScale = gridHeight / spriteHeight;

        // Use the smaller scale factor to preserve aspect ratio
        float scaleFactor = Mathf.Max(widthScale, heightScale);

        // Apply the scale
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

        // Optionally, you can also position the sprite in the center of the grid
        transform.position = gridBounds.center;
    }
}
