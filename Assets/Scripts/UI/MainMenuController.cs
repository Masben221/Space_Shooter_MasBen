using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class MainMenuController : SingletonBase<MainMenuController>
    {
        [SerializeField] private SpaceShip m_DefaultSpaceShip;
        //public SpaceShip DefaultSpaceShip => m_DefaultSpaceShip; // for testing

        [SerializeField] private GameObject m_EpisodeSelection;

        [SerializeField] private GameObject m_ShipSelectionPanel;

        [SerializeField] private GameObject m_StatisticsPanel;

        [SerializeField] private GameObject m_StatisticsPanelButton;

        private void Start()
        {
           LevelSequenceController.PlayerShip = m_DefaultSpaceShip;

            /*   if (LevelSequenceController.Instance.EpisodeStatistics == null)
                  m_StatisticsPanelButton.GetComponent<Button>().interactable = false;
              else
                  m_StatisticsPanelButton.GetComponent<Button>().interactable = true;*/
        }

        public void OnButtonStartNew()
        {
            m_EpisodeSelection.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }

        public void OnSelectShip()
        {
            m_ShipSelectionPanel.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }

        public void OnButtonExit()
        {
            Application.Quit();
        }

        public void OnShowStatistics()
        {
            if (LevelSequenceController.Instance.EpisodeStatistics == null) return;

            m_StatisticsPanel.gameObject.SetActive(true);

            m_StatisticsPanel.GetComponent<StatisticsPanelController>().RecordResults();
        }
    }
}