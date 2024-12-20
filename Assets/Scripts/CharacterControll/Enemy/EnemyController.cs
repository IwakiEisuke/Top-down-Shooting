using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _speed = 3;
    [SerializeField] string _playerName = "Player";
    [SerializeField] EnemyBulletShooter _bulletShooter;
    [SerializeField] float _playerDetectDistance;
    [SerializeField] float _timeOfLoseSight;
    [SerializeField] float _groundDetectDistance;
    [SerializeField] float _canGroundedAngle;
    [Tooltip("射線が通ってないと検知しない")]
    [SerializeField] bool _needSeenForDetection;
    [Tooltip("その場から動かない")]
    [SerializeField] bool _notMoving;
    [Tooltip("落下しない")]
    [SerializeField] bool _flying;
    [SerializeField] float _flyingBouncy;
    [SerializeField] float _cycleTime;
    [SerializeField] float _stoppingDistance;
    [Tooltip("後退しない")]
    [SerializeField] bool _notBackwards;
    float _initialBaseOffset;
    Transform _player;
    Rigidbody _rb;
    NavMeshAgent _agent;
    Coroutine _currentCoroutine;
    float _playerUnDetectedTime;
    bool _playerDetected;

    public void ChangeDetectDistance(float newDistance)
    {
        _playerDetectDistance = newDistance;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.Find(_playerName).transform;
        _agent = _rb.GetComponent<NavMeshAgent>();
        _currentCoroutine = StartCoroutine(Attack());
        if (_notMoving) _agent.enabled = false;
        _initialBaseOffset = _agent.baseOffset;
        _notMoving = !_agent.isOnNavMesh;
    }

    private void Update()
    {
        //　プレイヤーが一定距離に入ったか && プレイヤーを視認できるか
        if ((_player.transform.position - transform.position).sqrMagnitude <= _playerDetectDistance * _playerDetectDistance
            && (!_needSeenForDetection || CheckPassPlayer()))
        {
            //　検知状態に移行
            _playerDetected = true;
            _playerUnDetectedTime = 0;
        }
        else
        {
            _playerUnDetectedTime += Time.deltaTime;

            if (_playerUnDetectedTime > _timeOfLoseSight)
            {
                // 一定時間プレイヤーを認識できなければ検知状態を解除
                _playerDetected = false;
            }
        }

        if (_notMoving)
        {
            // 停止処理
            _agent.enabled = false;
            // 攻撃などの動作は行う
            NextState();
        }
        else if (_flying)
        {
            // フワフワ浮かせる
            _agent.baseOffset = _initialBaseOffset + Mathf.Cos(2 * Mathf.PI * Time.time / _cycleTime) * _flyingBouncy;
            NextState();

            var distance = (_player.transform.position - transform.position).magnitude;

            if (CheckPassPlayer() && distance < _stoppingDistance)
            {
                // エージェントのブレーキを使って停止させる
                _agent.stoppingDistance = 1000000;
            }
            else
            {
                _agent.stoppingDistance = 0;
            }
        }
        else // 飛んでいない場合、地面検知を行う
        {
            if (_agent.isOnNavMesh)
            {
                // notMoving 解除用
                _agent.updatePosition = true;
                _agent.isStopped = false;
            }

            var cos = Mathf.Cos(Mathf.PI * _canGroundedAngle / 180);
            NavMesh.SamplePosition(transform.position, out var navHit, _groundDetectDistance, NavMesh.AllAreas);
            Physics.Raycast(transform.position, transform.up * -1, out var hit, _groundDetectDistance);

            // 一定距離以内にナビメッシュがある && 足元方向の一定距離内に地面がある && 地面の傾きが少ない
            if (navHit.hit && hit.collider && Vector3.Dot(hit.normal, Vector3.up) > cos)
            {
                _agent.enabled = true;
                _rb.isKinematic = true;
                NextState();
            }
            else
            {
                // エージェントを切り、現在の行動をキャンセル
                _agent.enabled = false;
                _rb.isKinematic = false;
                _agent.Warp(transform.position);
                if (_currentCoroutine != null)
                {
                    StopCoroutine(_currentCoroutine);
                    _currentCoroutine = null;
                }
            }
        }
    }

    IEnumerator Attack()
    {
        // プレイヤーと射線が通っていればランダムに動く
        var moveDir = transform.position - _player.transform.position;
        moveDir.y = 0;
        moveDir = Quaternion.Euler(0, Random.Range(-120, 120), 0) * moveDir;
        moveDir.Normalize();
        var target = Vector3.zero;

        if (_notBackwards)
        {
            if (CheckPassPlayer() && (_player.transform.position - transform.position).sqrMagnitude < _stoppingDistance * _stoppingDistance)
            {
                target = transform.position;
            }
            else
            {
                target = _player.transform.position;
            }
        }
        else
        {
            target = transform.position + moveDir * _speed;
        }

        yield return new WaitUntil(() => _playerDetected);
        if(_agent.isOnNavMesh) _agent.destination = target;
        yield return new WaitForSeconds(0.5f);

        if (CheckPassPlayer())
        {
            _bulletShooter.Shoot();
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }

        _currentCoroutine = null;
    }

    private void NextState()
    {
        if (_playerDetected)
        {
            if (CheckPassPlayer())
            {
                _currentCoroutine ??= StartCoroutine(Attack());
            }
            else if (_currentCoroutine == null && _agent.isOnNavMesh)
            {
                _agent.destination = _player.transform.position; // プレイヤーを追いかける
            }
        }
    }

    private bool CheckPassPlayer()
    {
        return CheckPassPlayer(out _);
    }

    private bool CheckPassPlayer(out RaycastHit hit)
    {
        var dir = _player.transform.position - transform.position;
        Physics.Raycast(transform.position, dir, out hit, float.MaxValue); // プレイヤー方向にレイを飛ばす
        return hit.collider && LayerMask.LayerToName(hit.collider.gameObject.layer) == "Player"; //　レイがヒットした　＆＆　プレイヤー以外に当たらなかった
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!_rb.isKinematic)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (UnityEditor.EditorApplication.isPlaying && enabled)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

            if (_agent.enabled)
            {
                var color = Color.green;
                color.a = 0.5f;
                Gizmos.color = color;
                Gizmos.DrawCube(Vector3.zero, Vector3.one * 1.01f);
            }

            if (CheckPassPlayer())
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(Vector3.up, Vector3.one * 0.5f);
            }
        }
    }
}