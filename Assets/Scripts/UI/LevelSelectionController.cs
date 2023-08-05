using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class LevelSelectionController : MonoBehaviour
    {
        [SerializeField] private Text m_LevelNickName;

        private void Start()
        {
            if (m_LevelNickName != null)
                m_LevelNickName.text = SceneManager.GetActiveScene().name;
        }
    }
}