using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField, Range(0.01f, 1000)] float particleSpeed = 20;
    [SerializeField, Range(1, 100)] float resistance = 1;
    readonly ReactiveProperty<float> time = new();
    public bool IsKnockback { get; private set; }
    Rigidbody rb;
    NavMeshAgent agent;
    float _originalRateOverTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
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
            var speedRate = agent ? agent.velocity.magnitude / particleSpeed : rb.linearVelocity.magnitude / particleSpeed;
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
        
        if (agent) agent.velocity = vel / rb.mass;
        else rb.AddForce(vel, ForceMode.Impulse);
    }
}
