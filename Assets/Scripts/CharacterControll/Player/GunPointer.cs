using UnityEngine;

public class GunPointer : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] Transform _muzzle;
    [SerializeField] float _maxDistance = 100;

    [Header("�Ə��␳�@�\")]
    [SerializeField] bool _assist;
    [SerializeField] float _yAssistRange = 0.5f;
    [SerializeField] float _distanceToGround = 0.5f;

    /// <summary>
    /// �I�[�ʒu
    /// </summary>
    public Vector3 HitPosition { get; private set; }

    LineRenderer _line;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _line.SetPosition(0, _muzzle.transform.position);
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out var cameraHit, float.MaxValue, _layerMask)) // �J��������}�E�X�ʒu�Ƀ��C���΂�
        {
            // �����ȊO�̃_���[�W���󂯎��I�u�W�F�N�g�̏ꍇ�A�V�X�g�̉e�����󂯂Ȃ�
            if (cameraHit.transform != this.transform && cameraHit.collider.GetComponent<IDamageable>() != null)
            {
                SetPoint(cameraHit.point);
            }
            // �}�E�X�ʒu�̍������v���C���[�̖ڐ����痧���Ă���n�ʂ܂ł̊Ԃɂ���ꍇ�������v���C���[�̖ڐ��ɂ���
            else if (_assist && Mathf.Abs(transform.position.y - _distanceToGround - cameraHit.point.y) < _yAssistRange)
            {
                var assistedPos = cameraHit.point;
                assistedPos.y = transform.position.y;
                SetPoint(assistedPos);
            }
            else
            {
                SetPoint(cameraHit.point);
            }
        }
        else // ���C���q�b�g���Ȃ�������v���C���[�Ƒ�̓��������̃}�E�X�ʒu��^����
        {
            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(_muzzle.transform.position, Camera.main.transform.position);
            var targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            SetPoint(targetPos);
        }
    }

    /// <summary>
    /// �v���C���[�ʒu����^�[�Q�b�g�Ƀ��C���΂��A�Փ˂���|�C���^�[�̏I�_�ʒu�����肷��
    /// </summary>
    /// <param name="target"></param>
    void SetPoint(Vector3 target)
    {
        // �v���C���[�ʒu����^�[�Q�b�g�Ƀ��C���΂�
        Ray lineRay = new(_muzzle.transform.position, target - _muzzle.transform.position);
        if (Physics.Raycast(lineRay, out var lineHit, _maxDistance, _layerMask))
        {
            // ���C��������΂��̈ʒu���I�[�ɂ���
            HitPosition = lineHit.point;
        }
        else
        {
            // ���C��������Ȃ���΃v���C���[����^�[�Q�b�g�����ֈ�苗���L�΂����ʒu���I�[�ɂ���
            HitPosition = _muzzle.transform.position + (target - _muzzle.transform.position).normalized * _maxDistance;
        }
        _line.SetPosition(1, HitPosition);
    }
}
