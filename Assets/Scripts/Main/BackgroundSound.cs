using UnityEngine;

namespace SpaceShooter
{
    public class BackgroundSound : SingletonBase<BackgroundSound>
    {
        [SerializeField] private AudioSource[] m_AudioSources;
        void Start()
        {
            for (int i = 0; i < m_AudioSources.Length; i++)
            {
                m_AudioSources[i].Play();
            }
        }
    }
}