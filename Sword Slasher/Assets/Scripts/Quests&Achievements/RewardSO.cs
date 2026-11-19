using UnityEngine;

public abstract class RewardSO : ScriptableObject
{
    public abstract string Describe();
    public abstract void Grant();
}
