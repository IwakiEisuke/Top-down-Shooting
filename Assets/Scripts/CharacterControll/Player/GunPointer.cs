using UnityEngine;

public class GunPointer : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] Transform _muzzle;
    [SerializeField] float _maxDistance = 100;

    [Header("照準補正機能")]
    [SerializeField] bool _assist;
    [SerializeField] float _yAssistRange = 0.5f;
    [SerializeField] float _distanceToGround = 0.5f;

    LineRenderer _line;

    /// <summary>
    /// 終端位置
    /// </summary>
    public Vector3 HitPosition { get; private set; }

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _line.SetPosition(0, _muzzle.transform.position);

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out var cameraHit, float.MaxValue, _layerMask)) // マウス位置にレイを飛ばす
        {
            SetPoint(cameraHit.point);
            
            if (_assist && Mathf.Abs(transform.position.y - _distanceToGround - cameraHit.point.y) < _yAssistRange)
            {
                var targetPos = cameraHit.point;
                targetPos.y = transform.position.y;
                SetPoint(targetPos);
            }
            else
            {
                SetPoint(cameraHit.point);
            }
        }
        else // レイがヒットしなかったらプレイヤーと大体同じ高さのマウス位置を与える
        {
            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(_muzzle.transform.position, Camera.main.transform.position);
            var targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            SetPoint(targetPos);
        }
    }

    void SetPoint(Vector3 target)
    {
        Ray lineRay = new(_muzzle.transform.position, target - _muzzle.transform.position);
        if (Physics.Raycast(lineRay, out var lineHit, _maxDistance, _layerMask)) // プレイヤー位置からターゲットにレイを飛ばす
        {
            // レイが当たればその位置を終端にする
            HitPosition = lineHit.point;
        }
        else
        {
            // レイが当たらなければプレイヤーからターゲット方向へ一定距離伸ばした位置を終端にする
            HitPosition = _muzzle.transform.position + (target - _muzzle.transform.position).normalized * _maxDistance;
        }
        _line.SetPosition(1, HitPosition);
    }
}
