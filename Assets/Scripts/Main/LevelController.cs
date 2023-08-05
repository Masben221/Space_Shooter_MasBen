using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public interface ILevelCondition
    {
        bool IsCompleted { get; }
    }

    public class LevelController : SingletonBase<LevelController>
    {
        [SerializeField] private int m_ReferenceTime; //Время для выполнения условий.
        public int ReferenceTime => m_ReferenceTime; // публичное свойство времени прохождения

        [SerializeField] private UnityEvent m_EventLevelCompleted; // событие прохождения уровня

        //[SerializeField] private UnityEvent m_EventLevelLose; // событие проигрыша уровня

        private ILevelCondition[] m_Conditions; //Массив всех условий выполнения уровня.

        private bool m_IsLevelCompleted; // флаг прохождения уровня

        private float m_LevelTime; //Время затраченное на прохождение уровня.
        public float LevelTime => m_LevelTime; // публичное свойство времени прошедшего с начала уровня

        void Start()
        {
            m_Conditions = GetComponentsInChildren<ILevelCondition>();
        }

        void Update()
        {
            if (!m_IsLevelCompleted)
            {
                m_LevelTime += Time.deltaTime;

                CheckLevelConditions();

                //CheckLevelLose();
            }
        }
       /* private void CheckLevelLose()
        {
            if (Player.Instance != null)
            { 
                if (Player.Instance.NumLives == 0)
                {
                    m_EventLevelLose?.Invoke();
                }
            }
        }  */             
            
            //Проверка на выполнение условий завершения уровня.
            private void CheckLevelConditions()
        {
            if (m_Conditions == null || m_Conditions.Length == 0) return;

            int numCompleted = 0;

            foreach (var v in m_Conditions)
            {
                if (v.IsCompleted == true) numCompleted++;
            }

            if (numCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;

                m_EventLevelCompleted?.Invoke();

                LevelSequenceController.Instance?.FinishCurrentLevel(true);
            }
        }
    }
}