using UnityEditor.Rendering;
using UnityEngine;

public class PointerC : MonoBehaviour
{
    [SerializeField] PlayerController _player;
    [SerializeField] float _maxDistance = 100;
    LineRenderer _line;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        _line.SetPosition(0, _player.transform.position);

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out var cameraHit)) // �}�E�X�ʒu�Ƀ��C���΂�
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
        if (Physics.Raycast(lineRay, out var lineHit, _maxDistance)) // �v���C���[�ʒu����^�[�Q�b�g�Ƀ��C���΂�
        {
            _line.SetPosition(1, lineHit.point); // ���C��������΂��̈ʒu���I�[�ɂ���
        }
        else
        {
            // ���C��������Ȃ���΃v���C���[����^�[�Q�b�g�����ֈ�苗���L�΂����ʒu���I�[�ɂ���
            _line.SetPosition(1, _player.transform.position + (target - _player.transform.position).normalized * _maxDistance); 
        }
    }
}
