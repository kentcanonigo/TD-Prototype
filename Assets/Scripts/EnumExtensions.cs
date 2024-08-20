public static class EnumExtensions
{
    public static string ToModeString(this TurretTargetSelection.TargetingPreference mode)
    {
        switch (mode)
        {
            case TurretTargetSelection.TargetingPreference.Closest:
                return "Closest";
            case TurretTargetSelection.TargetingPreference.Furthest:
                return "Furthest";
            case TurretTargetSelection.TargetingPreference.LowestHealth:
                return "Lowest Health";
            case TurretTargetSelection.TargetingPreference.HighestHealth:
                return "Highest Health";
            case TurretTargetSelection.TargetingPreference.FirstEntered:
                return "First Entered";
            case TurretTargetSelection.TargetingPreference.LastEntered:
                return "Last Entered";
            default:
                return "Invalid";
        }
    }
}