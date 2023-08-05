using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace SpaceShooter
{
    public class SNR_Module_ForRockets : MonoBehaviour
    {
        [SerializeField] float m_SpeedAngular;
        private Transform m_nearestEnemy;

        private void Start()
        {
            var enemies = FindObjectsOfType<Enemy>();

            float nearestEnemyDistance = Mathf.Infinity;

            foreach (var enemy in enemies)
            {
                float dist = Vector2.Distance(transform.position, enemy.transform.position);

                if (dist < nearestEnemyDistance)
                {
                    m_nearestEnemy = enemy.transform;

                    nearestEnemyDistance = dist;
                }
            }
        }

        private void Update()
        {
            if (m_nearestEnemy == null) return;

            Vector3 targetVector = m_nearestEnemy.position - transform.position;
            
            transform.up = Vector3.Slerp(transform.up, targetVector, m_SpeedAngular * Time.deltaTime);
        }
    }
}