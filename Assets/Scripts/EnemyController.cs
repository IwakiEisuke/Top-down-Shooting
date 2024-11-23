using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _speed = 3;
    [SerializeField] string _playerName = "Player";
    [SerializeField] EnemyBulletShooter _bulletShooter;
    [SerializeField] float _playerDetectDistance;
    [SerializeField] float _groundDetectDistance;
    [SerializeField] float _canGroundedAngle;
    Transform _player;
    Rigidbody _rb;
    NavMeshAgent _agent;
    Coroutine _currentCoroutine;
    bool _playerDetected;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.Find(_playerName).transform;
        _agent = _rb.GetComponent<NavMeshAgent>();
        _currentCoroutine = StartCoroutine(Attack());
    }

    private void Update()
    {
        //�@�v���C���[����苗���ɓ������猟�m��ԂɈڍs
        if ((_player.transform.position - transform.position).sqrMagnitude <= _playerDetectDistance * _playerDetectDistance)
        {
            _playerDetected = true;
        }

        // ��苗���ȓ��ɒn�ʂ�����A�n�ʂ̌X�������Ȃ�
        var cos = Mathf.Cos(Mathf.PI * _canGroundedAngle / 180);
        if (Physics.Raycast(transform.position, transform.up * -1, out var hit, _groundDetectDistance) && Vector3.Dot(hit.normal, Vector3.up) > cos)
        {
            _agent.isStopped = false;
            _agent.updatePosition = true;
            if (_currentCoroutine == null) NextState();
        }
        else
        {
            // �G�[�W�F���g��؂�A���݂̍s�����L�����Z��
            _agent.isStopped = true;
            _agent.updatePosition = false;
            _agent.Warp(transform.position);
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
        }
    }

    IEnumerator Attack()
    {
        // �v���C���[�Ǝː����ʂ��Ă���΃����_���ɓ���
        var moveDir = transform.position - _player.transform.position;
        moveDir.y = 0;
        moveDir = Quaternion.Euler(0, Random.Range(-120, 120), 0) * moveDir;
        moveDir.Normalize();
        var target = transform.position + moveDir * _speed;

        yield return new WaitUntil(() => _playerDetected);
        _agent.destination = target;
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
        if (CheckPassPlayer())
        {
            _currentCoroutine ??= StartCoroutine(Attack());
        }
        else
        {
            _agent.destination = _player.transform.position; // �v���C���[��ǂ�������
        }
    }

    private bool CheckPassPlayer()
    {
        var dir = _player.transform.position - transform.position;
        // �v���C���[�����Ƀ��C���΂�
        Physics.Raycast(transform.position, dir, out var hit, float.MaxValue);
        //�@���C���q�b�g�����@�����@�v���C���[�ȊO�ɓ�����Ȃ�����
        return  hit.collider && LayerMask.LayerToName(hit.collider.gameObject.layer) == "Player";
    }

    private void OnDestroy()
    {
        _agent.isStopped = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var newForward = transform.forward;
        newForward.y = 0;
        transform.forward = newForward;
    }

    private void OnCollisionExit(Collision collision)
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
    }
}