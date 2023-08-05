using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Destructible))]
    public class AsteroidDestroyController : MonoBehaviour
    {
        [SerializeField] private GameObject m_DestroyAsteroidPrefab; //Префаб разрушенного астероида

        [SerializeField] private List<Sprite> m_Sprite; //Список спрайтов кусков астероида.

        [SerializeField] private float m_Speed; //Скорость осколков астероида.

        [SerializeField] private float m_AngularOffset; //Скорость вращения осколков.

        //[SerializeField] private float m_MaxDistanse; //Максимальная дистанция для самоуничтожения.

        private Destructible m_Destructible;

        private Vector2 m_startPosition; //Стартовая позиция.

        public Vector2 StartPosition { get => m_startPosition; set => m_startPosition = value; }

        private void Start()
        {
            //m_Destructible = GetComponent<Destructible>();
        }

        private void Update()
        {
            //float distance = ((Vector2)transform.position - m_startPosition).magnitude;

            //Уничтожение при достижении максимальной дистанции.
            /*if (distance > m_MaxDistanse)
            {
                m_Destructible.EventOnDeath?.Invoke();

                Destroy(gameObject);
            }*/
        }

        //Создает осколки астероида при его уничтожении. Метод вызывается в инспекторе в скрипте Destructible по событию EventOnDeath. 
        public void InstanceDestroyAsteroids()
        {
            for (int i = 0; i < m_Sprite.Count - 1; i++)
            {
                var newDestAsteroid = Instantiate(m_DestroyAsteroidPrefab, transform.position, Quaternion.identity);

                newDestAsteroid.GetComponentInChildren<SpriteRenderer>().sprite = m_Sprite[i];

                newDestAsteroid.transform.Rotate(0, 0, i * m_AngularOffset);

                Rigidbody2D rb = newDestAsteroid.GetComponent<Rigidbody2D>();

                rb.velocity = newDestAsteroid.transform.right * Random.Range(0, m_Speed);
            }
        }
    }
}