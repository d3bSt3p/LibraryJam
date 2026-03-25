using UnityEngine;
using UnityEngine.UI;

public class JarButterfly : MonoBehaviour
{
    [HideInInspector] public BugDataSO bugData;

    [Header("Movement Settings")]
    [SerializeField] float minMoveSpeed = 20f;
    [SerializeField] float maxMoveSpeed = 60f;
    [SerializeField] float minWaitTime = 0.5f;
    [SerializeField] float maxWaitTime = 2f;

    private Vector2 _targetPosition;
    private float _currentSpeed;
    private float _waitTimer;
    private bool _isWaiting;
    private RectTransform _boundsRect;
    private RectTransform _rectTransform;

    public void Initialise(BugDataSO data, RectTransform boundsRect)
    {
        bugData = data;
        _boundsRect = boundsRect;
        _rectTransform = GetComponent<RectTransform>();

        Image img = GetComponent<Image>();
        if (img != null && data?.catchImage != null)
            img.sprite = data.catchImage;

        PickNewTarget();
    }

    private void Update()
    {
        if (_rectTransform == null) return;

        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
                PickNewTarget();
            }
            return;
        }

        _rectTransform.anchoredPosition = Vector2.MoveTowards(
            _rectTransform.anchoredPosition,
            _targetPosition,
            _currentSpeed * Time.deltaTime
        );

        if (Vector2.Distance(_rectTransform.anchoredPosition, _targetPosition) < 0.5f)
        {
            _isWaiting = true;
            _waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    private void PickNewTarget()
    {
        Vector3[] corners = new Vector3[4];
        _boundsRect.GetLocalCorners(corners);
        _targetPosition = new Vector2(
            Random.Range(corners[0].x, corners[2].x),
            Random.Range(corners[0].y, corners[2].y)
        );
        _currentSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }
}