using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class MovementController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile,
            Keyboard_and_Mobile
        }
        [SerializeField] private SpaceShip m_TargetShip; //Ссылка на класс корабля.

        public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;

        [SerializeField] private VirtualJoystick m_MobileJoystick; //Ссылка на класс джойстика.

        [SerializeField] private ControlMode m_ControlMode; //Выбор типа управления.

        [SerializeField] private PointerClickHold m_MobileFirePrimary; //Ссылка на кнопку основного оружия.
        [SerializeField] private PointerClickHold m_MobileFireSecondary; //Ссылка на кнопку дополнительного оружия.          

        float thrust;
        float torgue;

        private void Start()
        {
            /* if (Application.isMobilePlatform)
             {
                 m_ControlMode = ControlMode.Mobile;
                 m_MobileJoystick.gameObject.SetActive(true);
             }
             else
             {
                 m_ControlMode = ControlMode.Keyboard;
                 m_MobileJoystick.gameObject.SetActive(false);
             }
             */
            if (m_ControlMode == ControlMode.Mobile || m_ControlMode == ControlMode.Keyboard_and_Mobile)
            {
                m_MobileJoystick.gameObject.SetActive(true);

                m_MobileFirePrimary.gameObject.SetActive(true); 
                m_MobileFireSecondary.gameObject.SetActive(true);
            }
            if (m_ControlMode == ControlMode.Keyboard)
            {
                m_MobileJoystick.gameObject.SetActive(false);

                m_MobileFirePrimary.gameObject.SetActive(false);
                m_MobileFireSecondary.gameObject.SetActive(false);               
            }
        }

        private void Update()
        {
            if (m_TargetShip == null) return;

            if (m_ControlMode == ControlMode.Mobile)
                ControlMobile();

            if (m_ControlMode == ControlMode.Keyboard)
                ControlKeyboard();
            
            if (m_ControlMode == ControlMode.Keyboard_and_Mobile) 
            {
               if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) 
                                                    || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) 
                                                    || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F) 
                                                    || Input.GetKey(KeyCode.LeftShift)) 
                    ControlKeyboard();
               else
                    ControlMobile();
            }

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torgue;

            if (m_TargetShip.IsShieldHold == true)
            {
                if (m_TargetShip.DrawShield(0.1f) == true)
                {
                    m_TargetShip.IsIndestructibe = true;
                    m_TargetShip.ShieldParticle.SetActive(true);
                }

                else
                {
                    m_TargetShip.IsIndestructibe = false;

                    m_TargetShip.IsShieldHold = false;

                    m_TargetShip.ShieldParticle.SetActive(false);
                }
            }
            if (m_TargetShip.IsAcceleration == true)
            {
                if (m_TargetShip.DrawAcceleration(1f) == true)
                {
                    m_TargetShip.ShipAcseleration(true);
                    m_TargetShip.AccelrationParticle.SetActive(true);
                }
                else
                {
                    m_TargetShip.ShipAcseleration(false);
                    m_TargetShip.IsAcceleration = false;
                    m_TargetShip.AccelrationParticle.SetActive(false);
                }
            }
        }
        private void ControlMobile()
        {
            /* Vector3 dir = m_MobileJoystick.Value;

             var dot = Vector2.Dot(dir, m_TargetShip.transform.up);
             var dot2 = Vector2.Dot(dir, m_TargetShip.transform.right);

             thrust = Mathf.Max(0, dot);
             torgue = -dot2;*/

             var dir = m_MobileJoystick.Value;
             thrust = dir.y;
             torgue = -dir.x;

            if (m_MobileFirePrimary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if (m_MobileFireSecondary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }

        }
        private void ControlKeyboard()
        {
            thrust = 0;
            torgue = 0;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                thrust = 1.0f;

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                thrust = -1.0f;

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                torgue = 1.0f;

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                torgue = -1.0f;

            if (Input.GetKey(KeyCode.Space))
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if (Input.GetKey(KeyCode.E))
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                // if (m_TargetShip.Shield != m_TargetShip.MaxShield) return;

                m_TargetShip.IsShieldHold = true;
            }
           
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_TargetShip.IsAcceleration = true;
            }
                        
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                m_TargetShip.IsAcceleration = false;
            }
        }
    }
}