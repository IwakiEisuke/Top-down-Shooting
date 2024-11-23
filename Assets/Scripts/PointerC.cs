using UnityEngine;

public class PointerC : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] PlayerController _player;
    [SerializeField] float _maxDistance = 100;
    LineRenderer _line;

    /// <summary>
    /// �I�[�ʒu
    /// </summary>
    public Vector3 Position { get; private set; }

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _line.SetPosition(0, _player.transform.position);

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out var cameraHit, float.MaxValue, _layerMask)) // �}�E�X�ʒu�Ƀ��C���΂�
        {
            SetPoint(cameraHit.point);
        }
        else // ���C���q�b�g���Ȃ�������v���C���[�Ƒ�̓��������̃}�E�X�ʒu��^����
        {
            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(_player.transform.position, Camera.main.transform.position);
            var targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            SetPoint(targetPos);
        }
    }

    void SetPoint(Vector3 target)
    {
        Ray lineRay = new(_player.transform.position, target - _player.transform.position);
        if (Physics.Raycast(lineRay, out var lineHit, _maxDistance, _layerMask)) // �v���C���[�ʒu����^�[�Q�b�g�Ƀ��C���΂�
        {
            // ���C��������΂��̈ʒu���I�[�ɂ���
            Position = lineHit.point;
        }
        else
        {
            // ���C��������Ȃ���΃v���C���[����^�[�Q�b�g�����ֈ�苗���L�΂����ʒu���I�[�ɂ���
            Position = _player.transform.position + (target - _player.transform.position).normalized * _maxDistance;
        }
        _line.SetPosition(1, Position);
    }
}
