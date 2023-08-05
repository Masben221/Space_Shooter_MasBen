using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PowerShield : Powerup
    {
        //Режим щита.
        protected override void OnPickedUp(SpaceShip ship)
        {
            ship.IsShieldHold = true;
        }
    }
}