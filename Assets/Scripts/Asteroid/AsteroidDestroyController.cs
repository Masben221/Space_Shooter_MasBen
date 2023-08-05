using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Destructible))]
    public class AsteroidDestroyController : MonoBehaviour
    {
        [SerializeField] private GameObject m_DestroyAsteroidPrefab; //������ ������������ ���������

        [SerializeField] private List<Sprite> m_Sprite; //������ �������� ������ ���������.

        [SerializeField] private float m_Speed; //�������� �������� ���������.

        [SerializeField] private float m_AngularOffset; //�������� �������� ��������.

        //[SerializeField] private float m_MaxDistanse; //������������ ��������� ��� ���������������.

        private Destructible m_Destructible;

        private Vector2 m_startPosition; //��������� �������.

        public Vector2 StartPosition { get => m_startPosition; set => m_startPosition = value; }

        private void Start()
        {
            //m_Destructible = GetComponent<Destructible>();
        }

        private void Update()
        {
            //float distance = ((Vector2)transform.position - m_startPosition).magnitude;

            //����������� ��� ���������� ������������ ���������.
            /*if (distance > m_MaxDistanse)
            {
                m_Destructible.EventOnDeath?.Invoke();

                Destroy(gameObject);
            }*/
        }

        //������� ������� ��������� ��� ��� �����������. ����� ���������� � ���������� � ������� Destructible �� ������� EventOnDeath. 
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