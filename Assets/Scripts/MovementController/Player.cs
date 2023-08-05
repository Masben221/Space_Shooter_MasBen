using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        [SerializeField] private int m_MaxLives; //������������ ����������� ������.

        private int m_NumLives; // ������� ���������� ������.
        public int NumLives => m_NumLives; //������ �� ������� ���������� ������.

        [SerializeField] private SpaceShip m_Ship; //������ �� ������� ������.
        public SpaceShip ActiveShip => m_Ship; //������ �� ������� ������.

        [SerializeField] private GameObject m_PlayerShipPrefab; //������ ������� ������.

        [SerializeField] private CameraController m_CameraController;

        [SerializeField] private MovementController m_MovementController;

        [SerializeField] private Transform m_RespawnPoint;

        [SerializeField] private LivesUI m_LivesUI; //������ �� UI ����������� ����������� ������.

        [SerializeField] private UIEnergyProgress m_UIEnergyProgress; //UI ������ �������.

        [SerializeField] private UIHPProgress m_UIHPProgress; //UI ������ HP.

        [SerializeField] private UIAccelerationProgress m_UIAccelerationProgress; //UI ������ ���������.

        [SerializeField] private UIShieldProgress m_UIShieldProgress;//UI ������ ����.

        [SerializeField] private UnityEvent m_EventLevelLose; // ������� ��������� ������

        //[SerializeField] private UnityEvent<GameObject> m_EventOnSpawn;

        //public UnityEvent<GameObject> EventOnSpawn => m_EventOnSpawn;

        //private float timerRespawn = 1f;
        //private bool isTimerRespawn = false;

        protected override void Awake()
        {
            base.Awake();

            m_NumLives = m_MaxLives;

            m_LivesUI.Setup(m_MaxLives);

            m_LivesUI.UpdateLivesUI(m_NumLives);

            if (m_Ship != null)
            {
                Destroy(m_Ship.gameObject);
            }
        }

        private void Start()
        {
            Respawn();
        }
        private void Update()
        {
           /* if (isTimerRespawn == false)
            {
                return;
            }
           
            timerRespawn -= Time.deltaTime;
           
            if (timerRespawn <= 0)
            {
                Respawn();
                timerRespawn = 1f;
                isTimerRespawn = false;
            }*/
        }

        //���������� ��� ����������� ������� ������.
        private void OnShipDeath(Vector3 pos)
        {
            m_NumLives--;

            LevelSequenceController.Instance.EpisodeStatistics.NumberOfDeaths++;

            m_LivesUI.UpdateLivesUI(m_NumLives);

            if (m_NumLives > 0)
            {
                // isTimerRespawn = true;
                Invoke(nameof(Respawn), 1f);
            }

            else
            {
                LevelSequenceController.Instance.FinishCurrentLevel(false);
                m_EventLevelLose?.Invoke();
            }
        }

        /// <summary>
        /// ����������� ������� ������.
        /// </summary>
        private void Respawn()
        {
            var newPlayerShip = Instantiate(LevelSequenceController.PlayerShip);
            newPlayerShip.transform.position = m_RespawnPoint.position;
            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            m_UIHPProgress.UpdateShip(m_Ship);

            m_UIEnergyProgress.UpdateShip(m_Ship);

            m_UIAccelerationProgress.UpdateShip(m_Ship);

            m_UIShieldProgress.UpdateShip(m_Ship);

            m_CameraController.SetTarget(m_Ship.transform);

            m_MovementController.SetTargetShip(m_Ship);

            m_Ship.EventOnDeath?.AddListener(OnShipDeath);
            //m_EventOnSpawn?.Invoke(newPlayerShip);
        }

        #region Score

        public int Score { get; private set; } //����.
        public int NumKills { get; private set; } //����������� ������������ ��������.

        //��������� ����������� ������������ ��������.
        public void AddKill()
        {
            NumKills++;
        }

        //����������� ����.
        public void AddScore(int num)
        {
            Score += num;
        }

        #endregion

    }
}
