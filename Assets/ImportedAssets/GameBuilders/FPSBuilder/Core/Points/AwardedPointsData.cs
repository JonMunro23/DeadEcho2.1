using UnityEngine;

[CreateAssetMenu(fileName = "AwardedPointsData", menuName = "Awarded Points Data")]
public class AwardedPointsData : ScriptableObject
{
    public int hit;
    public int kill;
    public int headshotKill;
    public int meleeKill;
}
