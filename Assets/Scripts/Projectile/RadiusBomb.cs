using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Projectile))]
    public class RadiusBomb : MonoBehaviour
    {
        [SerializeField] private float m_MaxDistanse;

        [SerializeField] private int m_Damage;

        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

        private Projectile m_projectile;

        void Start()
        {
            m_projectile = GetComponent<Projectile>();

            m_projectile.EventOnDeath.AddListener(UseRadiusBomb);
        }

        public void UseRadiusBomb()
        {
            var enemies = FindObjectsOfType<Enemy>();

            Instantiate(m_ImpactEffectPrefab, transform.position, Quaternion.identity);

            //SecondaryAmmoMMFeedback.Instance.PlayMMFeedback();

            foreach (var enemy in enemies)
            {
                float dist = Vector2.Distance(transform.position, enemy.transform.position);


                if (dist < m_MaxDistanse)
                {
                    Destructible dest = enemy.transform.root.GetComponent<Destructible>();

                    if (dest != null && dest != m_projectile.ParentDestructible)
                    {
                        dest.ApplyDamage(m_Damage);

                        //Player.Instance.AddScore(dest.ScoreValue);
                    }
                    else
                    {
                        dest = enemy.GetComponentInParent<Destructible>();

                        if (dest != null && dest != m_projectile.ParentDestructible)
                        {
                            dest.ApplyDamage(m_Damage);

                            //Player.Instance.AddScore(dest.ScoreValue);
                        }
                    }
                }
            }
        }
    }
}