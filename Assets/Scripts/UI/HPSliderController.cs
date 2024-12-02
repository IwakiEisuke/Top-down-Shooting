using UnityEngine;
using UnityEngine.UI;

public class HPSliderController : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [SerializeField] Texture2D _hpSprite;
    [SerializeField] Health _health;

    private void Start()
    {
        _health.onTakeDamage.AddListener(() => UpdateHPSlider());
        _health.onHeal.AddListener(() => UpdateHPSlider());

        // スライダー初期化
        var maxHP = _health.GetMaxHealth();
        var sliderRect = _hpSlider.GetComponent<RectTransform>();
        sliderRect.sizeDelta = new Vector2(_hpSprite.width * maxHP, sliderRect.sizeDelta.y);
        UpdateHPSlider();
    }

    public void UpdateHPSlider()
    {
        var _currentHP = _health.GetHealth();
        var _maxHP = _health.GetMaxHealth();
        _hpSlider.value = _currentHP / (float)_maxHP;
    }
}
    
