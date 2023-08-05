using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PowerupWeapon : Powerup
    {
        [SerializeField] private TurretProperties m_Propertites; //Свойства турели.

        //Назначает кораблю турель.
        protected override void OnPickedUp(SpaceShip ship)
        {
            ship.AssignWeapon(m_Propertites);
        }
    }
}