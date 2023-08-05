using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Задает область для взаимодействия с игроком. По достижени которой происходит действие. Работает  с LevelConditionsPosition.
    /// </summary>

    [RequireComponent(typeof(CircleCollider2D))]
    public class ScopeOfAction : MonoBehaviour
    {
        [SerializeField] private float m_Radius;

        [HideInInspector]
        public bool IsAction;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.transform.root.GetComponent<SpaceShip>() == Player.Instance.ActiveShip) IsAction = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.root.GetComponent<SpaceShip>() == Player.Instance.ActiveShip) IsAction = false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            GetComponent<CircleCollider2D>().radius = m_Radius;
        }
#endif
    }
}