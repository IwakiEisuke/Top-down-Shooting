using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] protected int _maxHP = 5;
    protected int _currentHP;

    virtual protected void Start()
    {
        _currentHP = _maxHP;
    }

    public void DealDamage(int damage)
    {
        _currentHP -= damage;
        OnUpdateHP();

        if (_currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    virtual protected void OnUpdateHP() { }
}