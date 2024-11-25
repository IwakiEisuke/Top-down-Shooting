using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : StatsManager
{
    [SerializeField] Slider _hpSlider;
    [SerializeField] Texture2D _hpSprite;

    override protected void Start()
    {
        base.Start();
        var rect = _hpSlider.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(_hpSprite.width * _maxHP, rect.sizeDelta.y);
    }

    override protected void OnUpdateHP()
    {
        _hpSlider.value = _currentHP / (float)_maxHP;
    }
}
    
