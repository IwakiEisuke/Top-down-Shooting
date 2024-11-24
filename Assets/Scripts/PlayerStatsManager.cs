using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : StatsManager
{
    [SerializeField] Slider _hpSlider;

    override protected void Start()
    {
        base.Start();
        var rect = _hpSlider.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.y * 2 * _maxHP, rect.sizeDelta.y);
    }

    override protected void OnUpdateHP()
    {
        _hpSlider.value = _currentHP / (float)_maxHP;
    }
}
    
