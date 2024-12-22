using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField, Range(0.01f, 1000)] float particleSpeed = 20;
    [SerializeField, Range(1, 100)] float resistance = 1;
    readonly ReactiveProperty<float> _time = new();
    public bool IsKnockback { get; private set; }
    Rigidbody _rb;
    NavMeshAgent _agent;
    float _originalRateOverTime;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _originalRateOverTime = particle.emission.rateOverTimeMultiplier;
        _time.Where(x => x <= 0 && IsKnockback)
            .Subscribe(_ => ChangeState(false))
            .AddTo(this);
    }

    private void Update()
    {
        _time.Value -= Time.deltaTime;
        
        if (particle)
        {
            var speedRate = (_agent ? _agent.velocity : _rb.linearVelocity).magnitude / particleSpeed;
            var emission = particle.emission;
            emission.rateOverTime = Mathf.Lerp(0, _originalRateOverTime, speedRate);
            var main = particle.main;
            main.startSizeMultiplier = Mathf.Lerp(0.5f, 1, speedRate);
        }
    }

    public void ApplyKnockback(float t, Vector3 impulse)
    {
        ChangeState(true);
        _time.Value = t / resistance;
        if (_agent) _agent.velocity = impulse / _rb.mass;
        else _rb.AddForce(impulse / _rb.mass, ForceMode.Impulse);
    }

    private void ChangeState(bool isKnockback)
    {
        IsKnockback = isKnockback;
        if (_agent)
        {
            _agent.ResetPath();
        }

        if (particle)
        {
            if (isKnockback) particle.Play();
            else particle.Stop();
        }
    }
}
