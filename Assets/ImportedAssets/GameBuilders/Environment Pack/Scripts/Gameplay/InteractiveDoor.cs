//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using GameBuilders.FPSBuilder.Interfaces;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using HighlightPlus;

#pragma warning disable CS0649

namespace FPSBuilder.EnvironmentPack
{
    [AddComponentMenu("FPS Builder/Gameplay/Interactive Door"), DisallowMultipleComponent]
    public class InteractiveDoor : MonoBehaviour, IActionable
    {
        [SerializeField]
        private Transform m_TargetTransform;

        [SerializeField]
        private Vector3 m_OpenedPosition;

        [SerializeField]
        private Vector3 m_OpenedRotation;

        [SerializeField]
        private Vector3 m_ClosedPosition;

        [SerializeField]
        private Vector3 m_ClosedRotation;

        [SerializeField]
        private bool m_Open;
        
        [SerializeField]
        private bool m_RequiresAnimation = true;

        [SerializeField]
        bool m_FadeOutAtEnd;

        [SerializeField]
        float m_FadeOutDuration;

        [SerializeField]
        float m_DelayBeforeFadeOut;

        [SerializeField]
        [MinMax(0.1f, 100f)]
        private float m_Duration = 1;

        [SerializeField]
        private int m_Cost = 750;

        //[Header("Highlight Effect")]
        //[SerializeField]
        ////private float highlightSphereRadius = 5;
        //bool isPlayerNear;

        ///// <summary>
        ///// Sphere to detect players presence nearby
        ///// </summary>
        //SphereCollider highlightSphere;
        //Transform playerTransform;
        //float distanceToPlayer;
        //HighlightEffect highlightEffect;
        [SerializeField]
        InteractiveDoor[] otherDoorsToOpen;

        public bool RequiresAnimation => m_RequiresAnimation;

        public int Cost => m_Cost;

        public bool IsOpen => m_Open;

        private void Awake()
        {
            //highlightEffect = GetComponentInParent<HighlightEffect>();
        }

        private void Start()
        {
            //SetupHightlightSphere();

            //highlightEffect.highlighted = false;
            //highlightEffect.outline = 0;
            //highlightEffect.overlay = 0;
       }

        //void SetupHightlightSphere()
        //{
        //    highlightSphere = gameObject.AddComponent<SphereCollider>();
        //    highlightSphere.radius = highlightSphereRadius;
        //    highlightSphere.isTrigger = true;
        //}

        //private void Update()
        //{
        ////    if (isPlayerNear)
        ////    {
        ////        if (PointsManager.Instance.CurrentPoints < Cost)
        ////            highlightEffect.outlineColor = Color.red;
        ////        else
        ////            highlightEffect.outlineColor = Color.white;

        ////        distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        ////        float outlineValue = 1 / distanceToPlayer;
        ////        outlineValue = Mathf.Clamp01(outlineValue);
        ////        highlightEffect.outline = outlineValue;


        ////        //float overlayValue = 1 / distanceToPlayer;
        ////        //overlayValue = Mathf.Clamp01(overlayValue);
        ////        //highlightEffect.overlay = overlayValue;
        ////    }
        //}

        public void Interact ()
        {
            if (PointsManager.Instance.CurrentPoints < Cost)
                return;

            PointsManager.Instance.PurchaseItem(Cost);

            if(otherDoorsToOpen.Length > 0)
            {
                foreach (InteractiveDoor door in otherDoorsToOpen)
                {
                    door.Open();
                }
            }

            Open();
        }

        public string Message ()
        {
            return "Open ($" + Cost + ")";
        }

        public void Open()
        {
            //highlightEffect.enabled = false;
            m_Open = true;
            GetComponent<BoxCollider>().enabled = false;
            m_TargetTransform.DOLocalMove(m_OpenedPosition, m_Duration);
            m_TargetTransform.DOLocalRotate(m_OpenedRotation, m_Duration);/*.OnComplete(() =>*/
            //{
                if (m_FadeOutAtEnd)
                    m_TargetTransform.DOScale(Vector3.zero, m_FadeOutDuration).SetDelay(m_DelayBeforeFadeOut).OnComplete(() =>
                    {
                        m_TargetTransform.gameObject.SetActive(false);
                    });
            //});
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if(other.transform.root.CompareTag("Player"))
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
