using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class EpisodeSelectionController : MonoBehaviour
    {
        [SerializeField] private Episode m_Episode;

        [SerializeField] private Text m_EpisodeNickName;

        [SerializeField] private Image m_PreviewImage;

        [SerializeField] private Button m_Button;

        [SerializeField] private int m_LastEpisode;


        private void Start()
        {
            if (m_EpisodeNickName != null)
                m_EpisodeNickName.text = m_Episode.EpisodeName;

            if (m_PreviewImage != null)
                m_PreviewImage.sprite = m_Episode.PreviewImage;

           /* if (LevelSequenceController.Instance.EpisodeCount > m_LastEpisode)
                m_Button.interactable = true;
            else
                m_Button.interactable = false;*/
        }

        public void OnStartEpisodeButtonClick()
        {
            LevelSequenceController.Instance.StartEpisode(m_Episode);
        }
    }
}