using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PowerAccelration : Powerup
    {
        //�������� �������.
        protected override void OnPickedUp(SpaceShip ship)
        {
            ship.IsAcceleration = true;
        }
    }
}