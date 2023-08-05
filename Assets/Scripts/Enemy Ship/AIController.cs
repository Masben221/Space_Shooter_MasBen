using System;
using UnityEngine;

namespace SpaceShooter
{
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol
        }

        [SerializeField] private AIBehaviour m_AIBehaviour; // режим (тип поведения) AI

        private AIPointPatrol m_PatrolPoint; //Точка патрулирования.

        [SerializeField] private AIPointPatrol[] m_PatrolPoints; //Массив точек патрулирования.
        public AIPointPatrol[] PatrolPoints { get => m_PatrolPoints; set => m_PatrolPoints = value; }

        [SerializeField] private float m_AttackRadius; //Радиус обнаружения для атаки.

        [Range(0f, 1f)]
        [SerializeField] private float m_NavigationLinear; //Линейная скорость.

        [Range(0f, 1f)]
        [SerializeField] private float m_NavigationAngular; //Скорость вращения корабля ИИ.

        [SerializeField] private float m_RandomSelectMovePointTime; //Величина таймера выбора точки в зоне патрулировая.

        [SerializeField] private float m_ActionAvadeCollisionTime;//Величина таймера проверки коллизии столкновения.

        [SerializeField] private float m_FindNewTargetTime;//Величина таймера поиска цели.

        [SerializeField] private float m_ShootDelay; //Величина таймера стрельбы.

        [SerializeField] private float m_EvadeRayLenght; //Радиус для CircleCast


        private SpaceShip m_SpaceShip; //Ссылка на корабль игрока.

        private Vector3 m_MovePosition; // точка куда двигается корабль

        private Destructible m_SelectedTarget; // выбранная цель

        private Timer m_RandomizeDirectionTimer;

        private Timer m_ActionAvadeCollisionTimer;

        private Timer m_FireTimer;

        private Timer m_FindNewTargetTimer;

        private float m_previousSpeed; //Предыдущая скорость.

        private int m_currentPatrolPoint; //Текущая точка патрулирования.

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            m_MovePosition = new Vector3(0, 0, 0);

            m_previousSpeed = m_NavigationLinear;

            m_currentPatrolPoint = 0;

            m_PatrolPoint = m_PatrolPoints[m_currentPatrolPoint];

            //m_PatrolPoint.transform.position = transform.position;
            
            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
               UpdateBevaviourPatrol();
            }
        }

        public void UpdateBevaviourPatrol()
        {
            ActionFindNewMovePosition();

            ActionControlShip();

            ActionFindNewAttackTarget();

            ActionFire();

            ActionEvadeCollision();
        }

        //Нахождение позиции движения.
        private void ActionFindNewMovePosition()
        {
            if (Physics2D.CircleCast(transform.position, m_EvadeRayLenght, transform.up, 0.1f))// обнуление цели при обходе препятствия
            {
                m_SelectedTarget = null;
                return;
            }

            //if (Physics2D.OverlapCircle(transform.position, m_EvadeRayLenght, 2))
            //{
            //    m_SelectedTarget = null;
            //    return;
            //}

            //Если есть цель, то расчет движения упреждения корабля.
            if (m_SelectedTarget != null)
            {
                if (m_SelectedTarget.transform.root.GetComponent<SpaceShip>()) // проверка цели на SpaceShip
                {
                    // коффициент упреждения по расстоянию и скорости своего корабля
                    float flightTimeBullet = Vector3.Distance(m_SelectedTarget.transform.position, transform.position) / GetComponent<SpaceShip>().Speed;

                    // кофициент упреждения с поправкой на скорость вражеского корабля
                    float lead = m_SelectedTarget.transform.root.GetComponent<SpaceShip>().Speed * flightTimeBullet / 4.0f;

                    // координаты цели с поправкой
                    m_MovePosition = m_SelectedTarget.transform.position + m_SelectedTarget.transform.up * lead * m_SelectedTarget.transform.root.GetComponent<SpaceShip>().ThrustControl;
                }
            }

            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                //движение без цели, до следующей точки патрулирования.
                float dist = Vector3.Distance(transform.position, m_PatrolPoint.transform.position);

                if (dist <= 1)
                {
                    m_currentPatrolPoint++;


                    if (m_currentPatrolPoint >= m_PatrolPoints.Length)
                        m_currentPatrolPoint = 0;

                    SetPatrolBehaviour(m_PatrolPoints[m_currentPatrolPoint]);
                }

                //Задает точку в радиусе зоны патрулирования.
                if (m_PatrolPoint != null)
                {
                    bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                    if (isInsidePatrolZone == true)
                    {
                        if (m_RandomizeDirectionTimer.IsFinished)
                        {
                            Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;

                            m_MovePosition = newPoint;

                            m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                        }
                    }
                    else
                    {
                        m_MovePosition = m_PatrolPoint.transform.position;
                    }
                }
            }
        }

        //Проверка на коллизию столкновения.
        private void ActionEvadeCollision()
        {
            if (m_ActionAvadeCollisionTimer.IsFinished == true)
            {
                var hit = Physics2D.CircleCast(transform.position, m_EvadeRayLenght, transform.up, 0.1f);

                //var hit = Physics2D.OverlapCircle(transform.position, m_EvadeRayLenght, 2);

                //Debug.Log(hit);

                if (hit)
                {
                    m_NavigationLinear = -0.2f;

                    //float angle = Vector2.SignedAngle(transform.up, hit.point - (Vector2)transform.position);

                    //ContactPoint2D[] contacts = new ContactPoint2D[1];

                    //float angle = Vector2.SignedAngle(transform.up, contacts[hit.GetContacts(contacts)].point - (Vector2)transform.position);

                    float angle = Vector2.SignedAngle(transform.up, hit.point - (Vector2)transform.position);

                    var direction = hit.transform.position - transform.position;

                    if (angle > 0)
                    {
                        m_MovePosition = transform.position + transform.right * 50f + -transform.up * 20;
                    }
                    else
                    {
                        m_MovePosition = transform.position + -transform.right * 50f + -transform.up * 20;
                    }

                    m_ActionAvadeCollisionTimer.Start(m_ActionAvadeCollisionTime);

                    m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);

                    m_FindNewTargetTimer.Start(m_FindNewTargetTime);
                }
                else m_NavigationLinear = m_previousSpeed;
            }
        }

        //Задает движение объекту AI.
        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;

            m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalised(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }

        private const float MAX_ANGLE = 45.0f;

        //Задает поворот AI в сторону позиции цели.
        private static float ComputeAliginTorqueNormalised(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            return -angle;
        }

        //Поиск цели для атаки.
        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                m_FindNewTargetTimer.Start(m_FindNewTargetTime);
            }
        }

        //Атака если найдена цель.
        private void ActionFire()
        {
            if (m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinished == true)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    m_FireTimer.Start(m_ShootDelay);

                   // m_SpaceShip.Fire(TurretMode.Secondary);
                    
                }
            }
        }

        //Поиск цели если не разные TeamId или не является TeamIdNeutral.
        private Destructible FindNearestDestructibleTarget()
        {
            float minDist = float.MaxValue;
             
            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructible)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;

                if (v.TeamId == Destructible.TeamIdNeutral) continue;

                if (v.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

                if (dist < minDist)
                {
                    minDist = dist;
                    potentialTarget = v;
                }
            }

            if (minDist < m_AttackRadius)
            {
                return potentialTarget;
            }

            return null;
        }

        #region Timers

        //Инициализация таймеров.
        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);

            m_ActionAvadeCollisionTimer = new Timer(m_ActionAvadeCollisionTime);

            m_FireTimer = new Timer(m_ShootDelay);

            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        //Обновление таймеров.
        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);

            m_ActionAvadeCollisionTimer.RemoveTime(Time.deltaTime);

            m_FireTimer.RemoveTime(Time.deltaTime);

            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
        }

        //Установка точки патрулирования.
        public void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.Patrol;

            m_PatrolPoint = point;
        }

        #endregion
    }
}