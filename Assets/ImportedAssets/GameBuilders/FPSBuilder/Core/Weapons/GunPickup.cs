//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using GameBuilders.FPSBuilder.Interfaces;
using UnityEngine;
using HighlightPlus;


#pragma warning disable CS0649

namespace GameBuilders.FPSBuilder.Core.Weapons
{
    [AddComponentMenu("FPS Builder/Weapon/Gun Pickup"), DisallowMultipleComponent]
    //[RequireComponent(typeof(HighlightEffect))]
    public class GunPickup : MonoBehaviour, IPickup
    {
        /// <summary>
        /// The Gun Data Asset that this gun pickup represents.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The Gun Data Asset that this gun pickup represents.")]
        private GunData m_GunData;
        
        private int m_CurrentRounds;

        //[Header("Highlight Effect")]
        //[SerializeField]
        //private float highlightSphereRadius = 3;
        //bool isPlayerNear;

        ///// <summary>
        ///// Sphere to detect players presence nearby
        ///// </summary>
        //SphereCollider highlightSphere;
        //Transform playerTransform;
        //float distanceToPlayer;
        //HighlightEffect highlightEffect;

        private void Start()
        {
            //ensures the weapon has full ammo when purchased
            CurrentRounds = m_GunData.HasChamber ? m_GunData.RoundsPerMagazine + 1 : m_GunData.RoundsPerMagazine;

            //SetupHightlightSphere();

            //highlightEffect.highlighted = false;
            //highlightEffect.outline = 0;
            //highlightEffect.overlay = 0;
        }

        public string weaponName => m_GunData != null ? m_GunData.GunName : "";

        /// <summary>
        /// Returns the ID represented by this gun pickup.
        /// </summary>
        public int Identifier => m_GunData != null ? m_GunData.GetInstanceID() : -1;
        
        /// <summary>
        /// Returns the cost of the weapon
        /// </summary>
        public float WeaponCost => m_GunData != null ? m_GunData.WeaponCost : 0;

        /// <summary>
        /// Returns the current rounds loaded into the weapon
        /// </summary>
        public int CurrentRounds
        {
            get => m_CurrentRounds;
            set => m_CurrentRounds = Mathf.Max(value, 0);
        }

        //private void Awake()
        //{
        //    highlightEffect = GetComponentInParent<HighlightEffect>();
        //}

        //void SetupHightlightSphere()
        //{
        //    highlightSphere = gameObject.AddComponent<SphereCollider>();
        //    highlightSphere.radius = highlightSphereRadius;
        //    highlightSphere.isTrigger = true;
        //}

        //private void Update()
        //{
        //    if (isPlayerNear)
        //    {
        //        if (PointsManager.Instance.CurrentPoints < m_GunData.WeaponCost)
        //            highlightEffect.outlineColor = Color.red;
        //        else
        //            highlightEffect.outlineColor = Color.white;

        //        distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        //        float outlineValue = 1 / (distanceToPlayer / 2);
        //        outlineValue = Mathf.Clamp01(outlineValue);
        //        highlightEffect.outline = outlineValue;


        //        //float overlayValue = 1 / distanceToPlayer;
        //        //overlayValue = Mathf.Clamp01(overlayValue);
        //        //highlightEffect.overlay = overlayValue;
        //    }
        //}

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.transform.root.CompareTag("Player"))
        //    {
        //        isPlayerNear = true;
        //        playerTransform = other.transform;
        //        highlightEffect.highlighted = true;
        //    }
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.transform.root.CompareTag("Player"))
        //    {
        //        isPlayerNear = false;
        //        playerTransform = null;
        //        highlightEffect.highlighted = false;
        //    }
        //}
    }
}

