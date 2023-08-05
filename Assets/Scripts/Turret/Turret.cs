using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        [SerializeField] private float m_VolumeSFX = 1f;

        [SerializeField] private AudioSource m_AudioSource;

        //таймер повторного выстрела
        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;
        /*public bool IsCanFire()
        {
            if (m_RefireTimer <= 0) return true;
            else return false;
        }*/

        private SpaceShip m_Ship;

        #region Unity Event
        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;

        }
        #endregion

        //public API

        /// <summary>
        /// выстрел из пушки.
        /// </summary>
        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if (m_Ship?.DrawEnergy(m_TurretProperties.EnergyUsage) == false) return;

            if (m_Ship?.DrawAmmo(m_TurretProperties.AmoUsage) == false) return;

            //Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;
            projectile.TurretMode = m_TurretProperties.Mode;
            projectile.SetParentShooter(m_Ship);
            m_RefireTimer = m_TurretProperties.RateOfFire;

            //playSFX
            m_AudioSource.PlayOneShot(m_TurretProperties.LaunchSFX, m_VolumeSFX);
        }

        /// <summary>
        /// Меняем свойства турели
        /// </summary>
        /// <param name="props"></param>
        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode) return;
            m_RefireTimer = 0;
            m_TurretProperties = props;
        }
    }
}