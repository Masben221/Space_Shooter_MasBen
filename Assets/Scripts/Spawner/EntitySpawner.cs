using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//спавнер сущности

namespace SpaceShooter
{

    public class EntitySpawner : MonoBehaviour
    {
        public enum SpawnMode //спавнер
        {
            Start, //при старте
            Loop //периодически
        }

        [SerializeField] private Entity[] m_EntityPrefabs; //массив префабов

        [SerializeField] private CircleArea m_Area; //зона в которорой спавним

        [SerializeField] private SpawnMode m_SpawnMode; //ссылка какой мод будет спавнить

        [SerializeField] private int m_NumSpawns; //колличество обьектов

        [SerializeField] private float m_RespawnTime; //как часто ссбрасывать таймер

        private float m_Timer; //таймер

        private void Start()
        {
            if(m_SpawnMode == SpawnMode.Start) //что то спавним
            {
                SpawnEntities();  //метод спавна сущности
            }

            m_Timer = m_RespawnTime; //сбрасываем таймер

        }

        private void Update()
        {
            if (m_Timer > 0)
                m_Timer -= Time.deltaTime; 

            if (m_SpawnMode == SpawnMode.Loop && m_Timer < 0)  //если зацикливаем обьект и заспавнем какйто обьект
            {
                SpawnEntities();  //метод спавна сущности

                m_Timer = m_RespawnTime; //сбрасываем таймер
            }

        }

        private void SpawnEntities()   //метод спавна сущности
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                int index = Random.Range(0, m_EntityPrefabs.Length);

                GameObject e = Instantiate(m_EntityPrefabs[index].gameObject); //спавним наш обьект (e)

                e.transform.position = m_Area.GetRandomInsideZone(); //задаем позицию сущности в случайное место
            }
        }
    }
}
