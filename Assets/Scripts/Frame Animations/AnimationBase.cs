using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// Базовый класс анимации.
    /// </summary>
    public abstract class AnimationBase : MonoBehaviour
    {
        [SerializeField] protected float m_AnimationTime; // Полное время анимации.

        [SerializeField] protected float m_AnimationScale; // Скелинг времени анимации.
        public float AnimationTime => m_AnimationTime / m_AnimationScale; // Полное время анимации с учетом скецлинга.

        [SerializeField] private bool m_Looping; // Флаг повторяющейся анимации.

        [SerializeField] private bool m_Reverse;

        [SerializeField] private UnityEvent m_EventStart;
        public UnityEvent OnEventStart => m_EventStart;

        [SerializeField] private UnityEvent m_EventEnd;
        public UnityEvent OnEventEnd => m_EventEnd;

        private float m_Timer;

        private bool m_IsAnimationPlaying = false;

        public void SetAnimationScale(float scale)
        {
            m_AnimationScale = scale;
        }

        // Нормализованное время анимации от 0 до 1
        public float NormalizeAnimationTime
        {
            get
            {
                var t = Mathf.Clamp01(m_Timer / AnimationTime);

                return m_Reverse ? (1.0f - t) : t;
            }
        }

        #region Unity Events
        private void Update()
        {
            if (m_IsAnimationPlaying)
            {
                m_Timer += Time.deltaTime;

                AnimateFrame();

                if (m_Timer > AnimationTime)
                {
                    if (m_Looping)
                    {
                        m_Timer = 0;
                    }
                    else
                    {
                        StopAnimation();
                        m_Timer = 0;
                    }
                }
            }
        }

        #endregion

        #region Public API

        // Метод запуска анимации.
        public void StartAnimation(bool prepare = true)
        {
            if (m_IsAnimationPlaying)
                return;

            if (prepare)
                PrepareAnimation();

            m_IsAnimationPlaying = true;

            OnAnimationStart();

            m_EventStart?.Invoke();
        }

        // Метод приостановки анимации.
        public void StopAnimation()
        {
            if (!m_IsAnimationPlaying)
                return;

            m_IsAnimationPlaying = false;

            OnAnimationEnd();

            m_EventEnd?.Invoke();
        }

        #endregion

        // Анимируе текущий фрейм анимации.
        protected abstract void AnimateFrame();

        protected abstract void OnAnimationStart();

        protected abstract void OnAnimationEnd();

        // Подготовка начального состояния анимации.
        public abstract void PrepareAnimation();
    }
}
