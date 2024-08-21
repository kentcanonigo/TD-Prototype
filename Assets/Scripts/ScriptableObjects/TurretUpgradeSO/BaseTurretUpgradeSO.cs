using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
public abstract class BaseTurretUpgradeSO : ScriptableObject {
    [Space]
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeSprite;
    public Color upgradeColor;
    [Space]
    public int baseCreditsCost;
    
    [Space]
    [InfoBox("Determines if the upgrade is applied as a multiplier or a flat bonus.")]
    public bool isMultiplier;
    
    [ShowIf("isMultiplier")]
    [InfoBox("The factor by which each subsequent upgrade is scaled. A value less than 1 results in diminishing returns, while a value greater than 1 results in amplifying returns.")]
    public float effectScalingFactor; // Only relevant for multipliers
    
    [HideIf("isMultiplier")]
    [InfoBox("The factor by which each subsequent upgrade is scaled. A value less than 1 results in diminishing returns, while a value greater than 1 results in amplifying returns.")]
    public float flatBonusScalingFactor = 1f; // Scaling factor for flat bonuses
    
    [InfoBox("The cost scaling factor determines how much the upgrade cost increases with each application. A higher value means the cost increases more rapidly.")]
    public float costScalingFactor;


    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string MultiplierFor1stApplication => FormatUpgradeValue(CalculateUpgradeValue(1));

    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string MultiplierFor2ndApplication => FormatUpgradeValue(CalculateUpgradeValue(2));

    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string MultiplierFor3rdApplication => FormatUpgradeValue(CalculateUpgradeValue(3));

    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string MultiplierFor4thApplication => FormatUpgradeValue(CalculateUpgradeValue(4));
    
    // Add this property for the total multiplier value
    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string TotalMultiplier => FormatUpgradeValue(CalculateTotalMultiplier());

    private float CalculateTotalMultiplier() {
        float total = 0f;
        total += CalculateUpgradeValue(1);
        total += CalculateUpgradeValue(2);
        total += CalculateUpgradeValue(3);
        total += CalculateUpgradeValue(4);
        return total;
    }
    
    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Upgrade Cost")]
    public string CostFor1stApplication => $"{GetCurrentCost(0)} Credits";

    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Upgrade Cost")]
    public string CostFor2ndApplication => $"{GetCurrentCost(1)} Credits";

    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Upgrade Cost")]
    public string CostFor3rdApplication => $"{GetCurrentCost(2)} Credits";

    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Upgrade Cost")]
    public string CostFor4thApplication => $"{GetCurrentCost(3)} Credits";
    
    // Add this property for the total upgrade cost
    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Upgrade Cost")]
    public string TotalCost => $"{CalculateTotalCost()} Credits";

    private int CalculateTotalCost() {
        int total = 0;
        total += GetCurrentCost(0);
        total += GetCurrentCost(1);
        total += GetCurrentCost(2);
        total += GetCurrentCost(3);
        return total;
    }

    public abstract void ApplyUpgrade(Turret turret, int applicationCount);
    public abstract void RevertUpgrade(Turret turret, int applicationCount);

    public abstract float GetBaseValue();

    // Calculate the upgrade value considering whether it's a multiplier or flat bonus
    public float CalculateUpgradeValue(int applicationCount) {
        if (isMultiplier) {
            return CalculateDiminishingReturn(GetBaseValue(), applicationCount, effectScalingFactor);
        } else {
            return CalculateFlatBonus(GetBaseValue(), applicationCount, flatBonusScalingFactor);
        }
    }

    public float CalculateDiminishingReturn(float baseMultiplier, int applicationCount, float diminishingFactor) {
        return 1.0f + (baseMultiplier - 1.0f) * Mathf.Pow(diminishingFactor, applicationCount - 1);
    }
    
    // Method to calculate scaling for flat bonuses
    public float CalculateFlatBonus(float baseBonus, int applicationCount, float scalingFactor) {
        return baseBonus + (applicationCount - 1) * scalingFactor;
    }

    public int GetCurrentCost(int applicationCount) {
        float cost = baseCreditsCost * Mathf.Pow(costScalingFactor, applicationCount);
        return Mathf.CeilToInt(cost / 10f) * 10;
    }

    // Helper method to format the upgrade value based on whether it's a multiplier or flat bonus
    protected string FormatUpgradeValue(float value) {
        return isMultiplier ? $"{value * 100f:F2}%" : $"{value:F2}";
    }
}
