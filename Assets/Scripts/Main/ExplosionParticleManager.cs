using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class ExplosionParticleManager : MonoBehaviour
    {
        //[SerializeField] private GameObject m_GameObject;
        [SerializeField] private ParticleSystem m_PrefabParticleExplosion;
        //private Transform m_Pos;
        private Destructible dest;

        private void Awake()
        {
            //m_Pos = gameObject.transform;
            transform.root.TryGetComponent<Destructible>(out dest);
            if (dest != null)
            {
                dest.EventOnDeath.AddListener(PlayParticleDie);
            }
        }
               
        private void PlayParticleDie(Vector3 pos)
        {
            var particleExplosion = Instantiate(m_PrefabParticleExplosion);
            particleExplosion.transform.position = pos;
            //m_PrefabParticleExplosion.Play();
            dest.EventOnDeath.RemoveListener(PlayParticleDie);
            Destroy(particleExplosion.gameObject, 2f);
        }

    }
}