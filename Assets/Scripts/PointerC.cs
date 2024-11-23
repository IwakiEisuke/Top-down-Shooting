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

        if (Physics.Raycast(cameraRay, out var cameraHit)) // マウス位置にレイを飛ばす
        {
            SetPoint(cameraHit.point);
        }
        else // レイがヒットしなかったらプレイヤーと大体同じ高さのマウス位置を与える
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
        if (Physics.Raycast(lineRay, out var lineHit, _maxDistance)) // プレイヤー位置からターゲットにレイを飛ばす
        {
            _line.SetPosition(1, lineHit.point); // レイが当たればその位置を終端にする
        }
        else
        {
            // レイが当たらなければプレイヤーからターゲット方向へ一定距離伸ばした位置を終端にする
            _line.SetPosition(1, _player.transform.position + (target - _player.transform.position).normalized * _maxDistance); 
        }
    }
}
