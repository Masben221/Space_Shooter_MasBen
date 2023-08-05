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

        [SerializeField] private AIBehaviour m_AIBehaviour; // ����� (��� ���������) AI

        private AIPointPatrol m_PatrolPoint; //����� ��������������.

        [SerializeField] private AIPointPatrol[] m_PatrolPoints; //������ ����� ��������������.
        public AIPointPatrol[] PatrolPoints { get => m_PatrolPoints; set => m_PatrolPoints = value; }

        [SerializeField] private float m_AttackRadius; //������ ����������� ��� �����.

        [Range(0f, 1f)]
        [SerializeField] private float m_NavigationLinear; //�������� ��������.

        [Range(0f, 1f)]
        [SerializeField] private float m_NavigationAngular; //�������� �������� ������� ��.

        [SerializeField] private float m_RandomSelectMovePointTime; //�������� ������� ������ ����� � ���� ������������.

        [SerializeField] private float m_ActionAvadeCollisionTime;//�������� ������� �������� �������� ������������.

        [SerializeField] private float m_FindNewTargetTime;//�������� ������� ������ ����.

        [SerializeField] private float m_ShootDelay; //�������� ������� ��������.

        [SerializeField] private float m_EvadeRayLenght; //������ ��� CircleCast


        private SpaceShip m_SpaceShip; //������ �� ������� ������.

        private Vector3 m_MovePosition; // ����� ���� ��������� �������

        private Destructible m_SelectedTarget; // ��������� ����

        private Timer m_RandomizeDirectionTimer;

        private Timer m_ActionAvadeCollisionTimer;

        private Timer m_FireTimer;

        private Timer m_FindNewTargetTimer;

        private float m_previousSpeed; //���������� ��������.

        private int m_currentPatrolPoint; //������� ����� ��������������.

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

        //���������� ������� ��������.
        private void ActionFindNewMovePosition()
        {
            if (Physics2D.CircleCast(transform.position, m_EvadeRayLenght, transform.up, 0.1f))// ��������� ���� ��� ������ �����������
            {
                m_SelectedTarget = null;
                return;
            }

            //if (Physics2D.OverlapCircle(transform.position, m_EvadeRayLenght, 2))
            //{
            //    m_SelectedTarget = null;
            //    return;
            //}

            //���� ���� ����, �� ������ �������� ���������� �������.
            if (m_SelectedTarget != null)
            {
                if (m_SelectedTarget.transform.root.GetComponent<SpaceShip>()) // �������� ���� �� SpaceShip
                {
                    // ���������� ���������� �� ���������� � �������� ������ �������
                    float flightTimeBullet = Vector3.Distance(m_SelectedTarget.transform.position, transform.position) / GetComponent<SpaceShip>().Speed;

                    // ��������� ���������� � ��������� �� �������� ���������� �������
                    float lead = m_SelectedTarget.transform.root.GetComponent<SpaceShip>().Speed * flightTimeBullet / 4.0f;

                    // ���������� ���� � ���������
                    m_MovePosition = m_SelectedTarget.transform.position + m_SelectedTarget.transform.up * lead * m_SelectedTarget.transform.root.GetComponent<SpaceShip>().ThrustControl;
                }
            }

            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                //�������� ��� ����, �� ��������� ����� ��������������.
                float dist = Vector3.Distance(transform.position, m_PatrolPoint.transform.position);

                if (dist <= 1)
                {
                    m_currentPatrolPoint++;


                    if (m_currentPatrolPoint >= m_PatrolPoints.Length)
                        m_currentPatrolPoint = 0;

                    SetPatrolBehaviour(m_PatrolPoints[m_currentPatrolPoint]);
                }

                //������ ����� � ������� ���� ��������������.
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

        //�������� �� �������� ������������.
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

        //������ �������� ������� AI.
        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;

            m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalised(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }

        private const float MAX_ANGLE = 45.0f;

        //������ ������� AI � ������� ������� ����.
        private static float ComputeAliginTorqueNormalised(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            return -angle;
        }

        //����� ���� ��� �����.
        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                m_FindNewTargetTimer.Start(m_FindNewTargetTime);
            }
        }

        //����� ���� ������� ����.
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

        //����� ���� ���� �� ������ TeamId ��� �� �������� TeamIdNeutral.
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

        //������������� ��������.
        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);

            m_ActionAvadeCollisionTimer = new Timer(m_ActionAvadeCollisionTime);

            m_FireTimer = new Timer(m_ShootDelay);

            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        //���������� ��������.
        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);

            m_ActionAvadeCollisionTimer.RemoveTime(Time.deltaTime);

            m_FireTimer.RemoveTime(Time.deltaTime);

            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
        }

        //��������� ����� ��������������.
        public void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.Patrol;

            m_PatrolPoint = point;
        }

        #endregion
    }
}