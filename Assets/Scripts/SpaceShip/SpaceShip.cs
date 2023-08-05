using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        /// <summary>
        /// Масса для автомотической установки у ригида.
        /// </summary>
        [Header("Space Ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Толкающая вперед сила.
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// Вращающая сила.
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelosity;
        public float MaxLinearVelosity { get => m_MaxLinearVelosity; set => m_MaxLinearVelosity = value; }

        /// <summary>
        /// Масимальная вращательная скорость. В градусах/сек
        /// </summary>
        [SerializeField] private float m_MaxAngularVelosity;
        public float MaxAngularVelosity { get => m_MaxAngularVelosity; set => m_MaxAngularVelosity = value; }

        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;

        /// <summary>
        /// Сохраненная ссылка на ригид.
        /// </summary>
        private Rigidbody2D m_Rigidbody;

        #region Public API

        /// <summary>
        /// Управление линейной тягой. -1.0 до +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. -1.0 до +1.0
        /// </summary>
        public float TorqueControl { get; set; }
        
        /// <summary>
        /// управление анимациями двигателей
        /// </summary>
        [SerializeField] private ParticleSystem m_ThrustParticle;
        [SerializeField] private float m_MaxCountParticle;
        [SerializeField] private ParticleSystem m_MobilityParticleLeft;
        [SerializeField] private ParticleSystem m_MobilityParticleRight;

        private float m_previousMaxLenearVelosity; //Запоминаем линейную скорость.

        private float m_ratioAcceleration = 2f; //Коэффициент ускорения.

        #endregion

        #region Unity Event
        //[SerializeField] private UnityEvent<int> m_EventOnUpdateHP;
        //public new UnityEvent<int> EventOnUpdateHP => m_EventOnUpdateHP;

        [SerializeField] private UnityEvent<float> m_EventOnUpdateEnergy;
        public UnityEvent<float> EventOnUpdateEnergy => m_EventOnUpdateEnergy;

        [SerializeField] private UnityEvent<float> m_EventOnUpdateAcceleration;
        public UnityEvent<float> EventOnUpdateAcceleration => m_EventOnUpdateAcceleration;

        [SerializeField] private UnityEvent<float> m_EventOnUpdateShield;
        public UnityEvent<float> EventOnUpdateShield => m_EventOnUpdateShield;

        private float m_Speed;
        public float Speed => m_Speed;

        private Vector3 m_oldPosition;

        protected override void Start()
        {
            base.Start();

            m_Rigidbody = GetComponent<Rigidbody2D>();

            m_Rigidbody.mass = m_Mass;
            
            //инерционная сила
            m_Rigidbody.inertia = 1;

            InitOffensive();

            m_oldPosition = transform.position;

            m_previousMaxLenearVelosity = MaxLinearVelosity;
           
        }

        [Obsolete]
        private void FixedUpdate()
        {
            UpdateRigidBody();
            
            PlayShipEffectInjectors();

            UpdateEnergyRegeneration();

            UpdateEnergyRegeneration();

            UpdateAcselerationRegeneration();

            if (m_IsShildEmpty)
                UpdateShieldRegeneration();

            m_Speed = ((transform.position - m_oldPosition) / Time.deltaTime).magnitude;

            m_oldPosition = transform.position;
        }

        [Obsolete]
        private void PlayShipEffectInjectors()
        {
            if (ThrustControl != 0)
                m_ThrustParticle.maxParticles = (int)m_MaxCountParticle;
            else
                m_ThrustParticle.maxParticles = (int)(m_Rigidbody.velocity.y * m_MaxCountParticle / m_MaxLinearVelosity);

            if (TorqueControl < 0)
                m_MobilityParticleLeft.gameObject.SetActive(true);
            else
                m_MobilityParticleLeft.gameObject.SetActive(false);

            if (TorqueControl > 0)
                m_MobilityParticleRight.gameObject.SetActive(true);
            else
                m_MobilityParticleRight.gameObject.SetActive(false);
        }

            #endregion

            /// <summary>
            /// Метод добавления сил кораблю для движения
            /// </summary>
            private void UpdateRigidBody()
        {
            m_Rigidbody.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);
            
            m_Rigidbody.AddForce(-m_Rigidbody.velocity * (m_Thrust / m_MaxLinearVelosity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigidbody.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);
            
            m_Rigidbody.AddTorque(-m_Rigidbody.angularVelocity * (m_Mobility / m_MaxAngularVelosity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        [SerializeField] private Turret[] m_Turrets;

        //Производит выстрел с турелей заданного типа.
        public void Fire(TurretMode mode)
        {
            for ( int i = 0; i < m_Turrets.Length; i++)
            {
                if (m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }

        [SerializeField] private int m_MaxEnergy;
        public int MaxEnergy => m_MaxEnergy;

        [SerializeField] private int m_MaxAcceleration;
        public int MaxAcceleration => m_MaxAcceleration;

        [SerializeField] private int m_MaxShield;
        public int MaxShield => m_MaxShield;

        [SerializeField] private int m_MaxAmmo;

        [SerializeField] private int m_EnergyRegenerationPerSecond;

        [SerializeField] private float m_AccelerationRegenerationPerSecond;

        [SerializeField] private float m_ShieldRegenerationPerSecond;

        [SerializeField] private GameObject m_ShieldParticle;
        public GameObject ShieldParticle => m_ShieldParticle;

        [SerializeField] private GameObject m_AccelrationParticle;
        public GameObject AccelrationParticle => m_AccelrationParticle;

        private float m_PrimaryEnergy;
        
        private int m_SecondaryAmmo;
        public int SecondaryAmmo => m_SecondaryAmmo;

        private float m_Accelration;

        private float m_Shiled;
        public float Shield => m_Shiled;

        private bool m_IsShildEmpty;

        private bool m_IsShieldHold; // Флаг активации щита.
        public bool IsShieldHold { get => m_IsShieldHold; set => m_IsShieldHold = value; }

        private bool m_IsAcceleration; // Флаг активации ускорения.
        public bool IsAcceleration { get => m_IsAcceleration; set => m_IsAcceleration = value; }

        public void AddEnergy(int e)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);

            EventOnUpdateEnergy?.Invoke(m_PrimaryEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        public void AddAccelration(int a)
        {
            m_Accelration = Mathf.Clamp(m_Accelration + a, 0, m_MaxAcceleration);
            EventOnUpdateAcceleration?.Invoke(m_Accelration);
        }
        public void AddShiled(int s)
        {
            m_Shiled = Mathf.Clamp(m_Shiled + s, 0, m_MaxShield);
            m_EventOnUpdateShield?.Invoke(m_Shiled);
        }
        public void AddHP(int hp)
        {
            CurrentHitPoint = Mathf.Clamp(CurrentHitPoint + hp, 0, HitPoints);
            EventOnUpdateHP?.Invoke(CurrentHitPoint);
        }        

        //Первоначальная инициализация.
        public void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;

            m_SecondaryAmmo = m_MaxAmmo;

            m_Accelration = m_MaxAcceleration;

            m_Shiled = m_MaxShield;

            m_EventOnUpdateShield?.Invoke(m_Shiled);
        }

        //Регенерация энергии.
        private void UpdateEnergyRegeneration()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenerationPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
            EventOnUpdateEnergy?.Invoke(m_PrimaryEnergy);
        }

        //Регенерация ускорения.
        private void UpdateAcselerationRegeneration()
        {
            m_Accelration += m_AccelerationRegenerationPerSecond * Time.deltaTime;

            m_Accelration = Mathf.Clamp(m_Accelration, 0, m_MaxAcceleration);

            EventOnUpdateAcceleration?.Invoke(m_Accelration);
        }

        private void UpdateShieldRegeneration()
        {
            m_Shiled += m_ShieldRegenerationPerSecond * Time.deltaTime;

            m_Shiled = Mathf.Clamp(m_Shiled, 0, m_MaxShield);

            if (m_Shiled == m_MaxShield) m_IsShildEmpty = false;

            m_EventOnUpdateShield?.Invoke(m_Shiled);
        }

        //Использование энергии для выстрела первичной турели.
        public bool DrawEnergy(int count)
        {
            if (count == 0) return true;

            if (m_PrimaryEnergy >= count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }

            return false;
        }

        //Использование снарядов.
        public bool DrawAmmo(int count)
        {
            if (count == 0) return true;

            if (m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }

            return false;
        }

        //Использование ускорения.
        public bool DrawAcceleration(float count)
        {
            m_Accelration -= count * Time.deltaTime;

            m_Accelration = Mathf.Clamp(m_Accelration, 0, m_MaxAcceleration);

            if (m_Accelration > 0)
            {
                return true;
            }
            else
                return false;
        }

        //Использование щита.
        public bool DrawShield(float count)
        {
            m_Shiled -= count * Time.deltaTime;

            m_Shiled = Mathf.Clamp(m_Shiled, 0, m_MaxShield);

            m_EventOnUpdateShield?.Invoke(m_Shiled);

            if (m_Shiled > 0)
            {
                return true;
            }

            m_IsShildEmpty = true;

            return false;
        }

        //Назначение типа снарядов.
        public void AssignWeapon(TurretProperties props) //метод задает какое то свойство турели при подборе
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
            /*foreach (var turet in m_Turrets)
            {
                turet.AssignLoadout(props);
            }*/
        }

        //Задает значение ускорения.
        public void ShipAcseleration(bool isActive)
        {
            if (isActive)
            {
                MaxLinearVelosity = m_previousMaxLenearVelosity * m_ratioAcceleration;
            }
            else
            {
                MaxLinearVelosity = m_previousMaxLenearVelosity;
            }
        }
    }
}