using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//спавнер мусора

namespace SpaceShooter
{

    public class EntitySpawnerDebris : MonoBehaviour
    {
        [SerializeField] private Destructible[] m_DebrisPrefabs; //массив префабов

        [SerializeField] private CircleArea m_Area; //место спавна

        [SerializeField] private int m_NumDebris; //колличество

        [SerializeField] private float m_RandomSpeed; //скорость создавания мусора


        private void Start()
        {
            for (int i = 0; i < m_NumDebris; i++)
            {
                SpawnDebris(); //спавним
            }
        }

        private void SpawnDebris() //метод спавна мусора
        {
            int index = Random.Range(0, m_DebrisPrefabs.Length);

            GameObject debris = Instantiate(m_DebrisPrefabs[index].gameObject); //спавним наш обьект (debris)

            debris.transform.position = m_Area.GetRandomInsideZone();

            debris.GetComponent<Destructible>().EventOnDeath.AddListener(OnDebrisDead); //уничтожается обьект вызывется метод OnDebrisDead

            Rigidbody2D rb = debris.GetComponent<Rigidbody2D>();

            if(rb != null && m_RandomSpeed > 0)
            {
                rb.velocity = (Vector2) UnityEngine.Random.insideUnitSphere * m_RandomSpeed;
            }
        }

        private void OnDebrisDead(Vector3 pos) //метод смерти мусора
        {
            SpawnDebris(); //спавним
        }
    }
}
