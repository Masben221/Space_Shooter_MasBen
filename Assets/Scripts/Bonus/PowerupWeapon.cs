using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PowerupWeapon : Powerup
    {
        [SerializeField] private TurretProperties m_Propertites; //�������� ������.

        //��������� ������� ������.
        protected override void OnPickedUp(SpaceShip ship)
        {
            ship.AssignWeapon(m_Propertites);
        }
    }
}