using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SpaceShooter
{
    /// <summary>
    /// ������������ ������ �� �����. ��, ��� ����� ����� ���������. 
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// ������ ���������� ����������� (��������������).
        /// </summary>
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructibe { get => m_Indestructible; set => m_Indestructible = value; }

        /// <summary>
        /// ��������� ���������� ����������.
        /// </summary>
        [SerializeField] private int m_HitPoints;
        public int HitPoints => m_HitPoints;

        /// <summary>
        /// ������� ����������.
        /// </summary>
        private int m_CurrentHitPoints;
        public int CurrentHitPoint { get => m_CurrentHitPoints; set => m_CurrentHitPoints = value; }

        /// <summary>
        /// ������� ����������� �������.
        /// </summary>
        [SerializeField] private UnityEvent<Vector3> m_EventOnDeath;
        public UnityEvent<Vector3> EventOnDeath => m_EventOnDeath;

        /// <summary>
        /// ������� ���������� UI HP.
        /// </summary>
        [SerializeField] private UnityEvent<int> m_EventOnUpdateHP;
        public UnityEvent<int> EventOnUpdateHP => m_EventOnUpdateHP;

        /// <summary>
        /// ������� �����������.
        /// </summary>
        [SerializeField] private UnityEvent<int> m_EventOnDamage;
        public UnityEvent<int> EventOnDamage => m_EventOnDamage;

        //������ �� ������� ����� �����������.
        [SerializeField] private GameObject m_ImpactEffect;

        #endregion


        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;

            EventOnUpdateHP?.Invoke(m_CurrentHitPoints);
        }

        #endregion


        #region Public API

        /// <summary>
        /// ���������� ������ � �������.
        /// </summary>
        /// <param name="damage"> ���� ��������� ������� </param>

        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            EventOnUpdateHP?.Invoke(m_CurrentHitPoints);

            EventOnDamage?.Invoke(m_CurrentHitPoints);

            if (m_CurrentHitPoints <= 0)
            {
                OnDeath();
            }
        }

        /// <summary>
        /// ���������������� ������� ����������� �������, ����� ��������� ���� ����
        /// </summary>
        protected virtual void OnDeath()
        {
            Instantiate(m_ImpactEffect, transform.position, Quaternion.identity);

            var pos = gameObject.transform.position;                      
            m_EventOnDeath?.Invoke(pos);

            Destroy(gameObject);
        }

        #endregion

        #region Teams

        /// <summary>
        /// ����������� ���� ���� �������� ���� Destructible.
        /// </summary>
        private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructible => m_AllDestructibles;

        //��������� � ��������� ����� ��� ��������.
        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
            {
                m_AllDestructibles = new HashSet<Destructible>();
            }

            m_AllDestructibles.Add(this);
        }

        //������� �� ��������� ������ ��� �����������.
        protected virtual void OnDestroy()
        {
            m_AllDestructibles?.Remove(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        #endregion

        #region Score

        //������ �������� ����� �� ���������.
        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        //������ �������� ����� �� �����������.
        [SerializeField] private int m_KillValue;
        public int KillValue => m_KillValue;

        #endregion
    }
}