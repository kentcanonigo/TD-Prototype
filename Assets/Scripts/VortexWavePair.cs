using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VortexWavePair {
    public Vector2Int vortexPosition;    // Vortex position
    public List<WaveSO> waveSOList;      // List of WaveSOs for this vortex

    public VortexWavePair(Vector2Int vortexPosition) {
        this.vortexPosition = vortexPosition;
        waveSOList = new List<WaveSO>();
    }
}