using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public enum EffectType
    {
        AddAmmo,
        AddEnergy,
        AddAccelration,
        AddShiled,
        AddHP
    }
    public class PowerupStats : Powerup
    {

        [SerializeField] private EffectType m_EffectType;
        [SerializeField] private float m_Value;

        //ƒобавл€ет бонусы в зависимоти от их типа.
        protected override void OnPickedUp(SpaceShip ship)
        {
            if (m_EffectType == EffectType.AddEnergy)
                ship.AddEnergy((int)m_Value);
            if (m_EffectType == EffectType.AddAmmo)
                ship.AddAmmo((int)m_Value);
            if (m_EffectType == EffectType.AddAccelration)
                ship.AddAccelration((int)m_Value);
            if (m_EffectType == EffectType.AddShiled)
                ship.AddShiled((int)m_Value);
            if (m_EffectType == EffectType.AddHP)
                ship.AddHP((int)m_Value);
        }
    }
}