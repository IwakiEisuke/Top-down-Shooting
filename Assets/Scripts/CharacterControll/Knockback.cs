using DG.Tweening;
using UniRx;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] float particleSpeed = 20;
    [SerializeField, Range(1, 100)] float resistance = 1;
    readonly ReactiveProperty<float> time = new();
    public bool IsKnockback { get; private set; }
    Rigidbody rb;
    float _originalRateOverTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _originalRateOverTime = particle.emission.rateOverTimeMultiplier;
        time.Where(x => x <= 0)
            .Subscribe(_ =>
            {
                IsKnockback = false;
                if (particle) particle.Stop();
            })
            .AddTo(this);
    }

    private void Update()
    {
        time.Value -= Time.deltaTime;
        if (particle)
        {
            var speedRate = rb.linearVelocity.magnitude / particleSpeed;
            var emission = particle.emission;
            emission.rateOverTime = Mathf.Lerp(0, _originalRateOverTime, speedRate);
            var main = particle.main;
            main.startSizeMultiplier = Mathf.Lerp(0.5f, 1, speedRate);
        }
    }

    public void ApplyKnockback(float t, Vector3 vel, Rigidbody rb)
    {
        IsKnockback = true;
        if (particle) particle.Play();
        this.time.Value = t / resistance;
        rb.AddForce(vel, ForceMode.Impulse);
    }
}
