using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField, Range(1, 100)] float resistance = 1;
    readonly ReactiveProperty<float> time = new();
    public bool IsKnockback { get; private set; }

    private void Start()
    {
        time.Where(x => x <= 0)
            .Subscribe(_ => 
            {
                IsKnockback = false;
                particle.Stop();
            })
            .AddTo(this);
    }

    private void Update()
    {
        time.Value -= Time.deltaTime;
    }

    public void ApplyKnockback(float t, Vector3 vel, Rigidbody rb)
    {
        IsKnockback = true;
        particle.Play();
        this.time.Value = t / resistance;
        rb.AddForce(vel, ForceMode.Impulse);
    }
}
