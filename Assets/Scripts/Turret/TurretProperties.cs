using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public enum TurretMode
    {
        Primary,
        Secondary
    }

    [CreateAssetMenu]
    public sealed class TurretProperties : ScriptableObject
    {   
        //тип турели
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;
        
        //префаб снаряда
        [SerializeField] private Projectile m_ProjectilePrefab;
        public Projectile ProjectilePrefab => m_ProjectilePrefab;

        //скорострельность стрельбы (задержка, чем больше тем медленее стреляет)
        [SerializeField] private float m_RateOfFire;
        public float RateOfFire => m_RateOfFire;

        //потребление энергии
        [SerializeField] private int m_EnergyUsage;
        public int EnergyUsage => m_EnergyUsage;

        //потребление боеприпасов
        [SerializeField] private int m_AmmoUsage;
        public int AmoUsage => m_AmmoUsage;
        [SerializeField] private AudioClip m_LaunchSFX;

        //звук стрельбы
        public AudioClip LaunchSFX => m_LaunchSFX;
    }
}