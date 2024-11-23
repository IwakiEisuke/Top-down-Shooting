using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] int _hp = 5;

    public void DealDamage(int damage)
    {
        _hp -= damage;

        if ( _hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
