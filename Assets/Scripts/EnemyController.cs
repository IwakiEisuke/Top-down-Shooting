using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _speed = 3;
    [SerializeField] string _playerName = "Player";
    Transform _player;
    Rigidbody _rb;
    NavMeshAgent _agent;
    Vector3 _direction;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.Find(_playerName).transform;
        _agent = _rb.GetComponent<NavMeshAgent>();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        _agent.destination = NewPos();
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        StartCoroutine(Move());
    }

    private Vector3 NewPos()
    {
        var dir = _player.transform.position - transform.position;
        dir.y = 0;
        dir = Quaternion.Euler(0, Random.Range(-90, 90), 0) * dir;
        dir.Normalize();
        return transform.position + dir * _speed;
    }
}
