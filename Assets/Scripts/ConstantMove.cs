using UnityEngine;

public class ConstantMove : MonoBehaviour
{
    public GameObject Follow;
    public float TargetZ = -90;

    private RectTransform _transform;
    private RectTransform _followT;
    
    private void Start()
    {
        _transform = GetComponent<RectTransform>();

        GetTransform();
    }

    public void SetFollow(GameObject follow)
    {
        Follow = follow;
        GetTransform();
    }

    private void GetTransform()
    {
        _followT = Follow.GetComponent<RectTransform>();
    }

    void Update()
    {
        _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, TargetZ);
        if (Follow is not null)
            _transform.anchoredPosition = Vector2.Lerp(_transform.anchoredPosition, _followT.anchoredPosition, Time.deltaTime);
    }
}
