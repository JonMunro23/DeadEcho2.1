using UnityEngine;
using GameBuilders.FPSBuilder.Core;
using System;
using GameBuilders.FPSBuilder.Core.Managers;

public class PointsManager : Singleton<PointsManager>
{
    /// <summary>
    /// Sound played when the character purchases something.
    /// </summary>
    [SerializeField]
    [Tooltip("Sound played when the character purchases something.")]
    private AudioClip m_ItemPurchaseSound;

    /// <summary>
    /// Defines the volume of Item Purchase Sound played when the character purchases something.
    /// </summary>
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Defines the volume of Item Purchase Sound played when the character purchases something.")]
    private float m_ItemPurchaseVolume = 0.3f;

    private AudioEmitter m_PurchaseItemSource;

    public static event Action<int> onPointsAdded;
    public static event Action<int> onPointsRemoved;

    /// <summary>
    /// Contains a reference to the awardedPointsData
    /// </summary>
    public AwardedPointsData awardedPointsData;

    /// <summary>
    /// Amount of points the player should start with
    /// </summary>
    [SerializeField]
    private int m_StartingPoints = 500;

    /// <summary>
    /// Current points held by the player
    /// </summary>
    private int m_CurrentPoints;

    /// <summary>
    /// Returns the current points held by the player
    /// </summary>
    public int CurrentPoints => m_CurrentPoints;

    /// <summary>
    /// The cost of purchasing ammo for a weapon
    /// </summary>
    [SerializeField]
    private int m_AmmoCost;

    /// <summary>
    /// Returns the cost of purchasing ammo for a weapon
    /// </summary>
    [SerializeField]
    public int AmmoCost => m_AmmoCost;

    [SerializeField] Transform gainedPointsTextSpawnLocation;
    //public GameObject gainedPointsText;

    private void Start()
    {
        m_PurchaseItemSource = AudioManager.Instance.RegisterSource("[AudioEmitter] PurchaseItem", transform.root, spatialBlend: 0);
        AddPoints(m_StartingPoints);
    }

    /// <summary>
    /// Adds points to current points
    /// </summary>
    /// <param name="pointsToAdd"></param>
    public void AddPoints(int pointsToAdd)
    {
        m_CurrentPoints += pointsToAdd;
        onPointsAdded?.Invoke(pointsToAdd);
    }

    /// <summary>
    /// Removes points from current points
    /// </summary>
    /// <param name="costOfItem"></param>
    public void PurchaseItem(int costOfItem)
    {
        m_CurrentPoints -= costOfItem;
        m_PurchaseItemSource.ForcePlay(m_ItemPurchaseSound, m_ItemPurchaseVolume);
        onPointsRemoved?.Invoke(m_CurrentPoints);
    }
}
