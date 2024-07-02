//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using GameBuilders.FPSBuilder.Core.Managers;
using GameBuilders.FPSBuilder.Interfaces;
using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.EnvironmentPack
{
    [AddComponentMenu("FPS Builder/Gameplay/Breakable Light"), DisallowMultipleComponent]
    public class BreakableObject : MonoBehaviour, IProjectileDamageable, IExplosionDamageable, IMeleeDamageable
    {
        [SerializeField]
        private GameObject m_Particle;

        [SerializeField]
        private Transform m_ParticleTarget;

        [Space]
        [SerializeField]
        private AudioClip m_BreakSound;

        [SerializeField]
        [Range(0, 1)]
        private float m_BreakVolume = 1f;

        [Space]
        [SerializeField]
        
        private GameObject[] m_AffectedObjects;
        [SerializeField]
        GameObject gameObjectToActivate; 

        public bool IsAlive
        {
            get;
            private set;
        }

        private void Start ()
        {
            IsAlive = true;
        }

        public void ProjectileDamage (float damage, Vector3 targetPosition, Vector3 hitPosition, float penetrationPower, string hitBodyPart = null)
        {
            if (!IsAlive) 
                return;
            
            if(gameObjectToActivate)
            {
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                gameObjectToActivate.SetActive(true);
                Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
                AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
                IsAlive = false;
                return;
            }

            if (damage > 0)
            {
                for (int i = 0; i < m_AffectedObjects.Length; i++)
                {
                    m_AffectedObjects[i].SetActive(false);
                }
            }

            Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
            AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
            IsAlive = false;
        }

        public void ExplosionDamage (float damage, Vector3 targetPosition, Vector3 hitPosition)
        {
            if (!IsAlive) 
                return;

            if (gameObjectToActivate)
            {
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                gameObjectToActivate.SetActive(true);
                Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
                AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
                IsAlive = false;
                return;
            }

            if (damage > 0)
            {
                for (int i = 0; i < m_AffectedObjects.Length; i++)
                {
                    m_AffectedObjects[i].SetActive(false);
                }
            }

            Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
            AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
            IsAlive = false;
        }

        public void Damage (float damage)
        {
            if (!IsAlive) 
                return;

            if (gameObjectToActivate)
            {
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                gameObjectToActivate.SetActive(true);
                Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
                AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
                IsAlive = false;
                return;
            }

            if (damage > 0)
            {
                for (int i = 0; i < m_AffectedObjects.Length; i++)
                {
                    m_AffectedObjects[i].SetActive(false);
                }
            }

            Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
            AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
            IsAlive = false;
        }

        public void Damage (float damage, Vector3 targetPosition, Vector3 hitPosition)
        {
            if (!IsAlive) 
                return;

            if (gameObjectToActivate)
            {
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                gameObjectToActivate.SetActive(true);
                Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
                AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
                IsAlive = false;
                return;
            }

            if (damage > 0)
            {
                for (int i = 0; i < m_AffectedObjects.Length; i++)
                {
                    m_AffectedObjects[i].SetActive(false);
                }
            }

            Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
            AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
            IsAlive = false;
        }

        public void MeleeDamage(float damage, Vector3 targetPosition, Vector3 hitPosition, string hitBodyPartTag = null)
        {
            if (!IsAlive)
                return;

            if (gameObjectToActivate)
            {
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                gameObjectToActivate.SetActive(true);
                Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
                AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
                IsAlive = false;
                return;
            }

            if (damage > 0)
            {
                for (int i = 0; i < m_AffectedObjects.Length; i++)
                {
                    m_AffectedObjects[i].SetActive(false);
                }
            }

            Instantiate(m_Particle, m_ParticleTarget.position, m_ParticleTarget.rotation);
            AudioManager.Instance.PlayClipAtPoint(m_BreakSound, transform.position, 5, 15, m_BreakVolume);
            IsAlive = false;
        }
    }
}
