using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class footstepManager : MonoBehaviour
{
    [Header("Footstep Audio")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] [Range(0f, 1f)] private float volume = 0.5f;

    [Header("Step Rate")]
    [Tooltip("How fast footsteps play while walking. Higher = more frequent.")]
    [SerializeField] private float walkStepRate = 10f;

    [Header("Sprint")]
    [SerializeField] private bool enableSprint = true;
    [Tooltip("How fast footsteps play while sprinting.")]
    [SerializeField] private float sprintStepRate = 16f;

    [Header("Crouch")]
    [SerializeField] private bool enableCrouch = true;
    [Tooltip("How fast footsteps play while crouching.")]
    [SerializeField] private float crouchStepRate = 6f;

    [Header("Controller Reference")]
    [SerializeField] private FirstPersonController playerController;

    // Internal
    private AudioSource _audioSource;
    private float _stepTimer = 0f;
    private bool _hasPlayedThisCycle = false;
    private int _lastClipIndex = -1;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0f;
    }

    private void Update()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;

        bool isWalking = playerController != null && playerController.isWalking;

        if (!isWalking)
        {
            _stepTimer = 0f;
            _hasPlayedThisCycle = false;
            return;
        }

        float stepRate = GetStepRate();
        _stepTimer += Time.deltaTime * stepRate;

        float sinValue = Mathf.Sin(_stepTimer);

        if (sinValue >= 0.99f && !_hasPlayedThisCycle)
        {
            PlayFootstep();
            _hasPlayedThisCycle = true;
        }
        else if (sinValue < 0.99f)
        {
            _hasPlayedThisCycle = false;
        }
    }

    private float GetStepRate()
    {
        if (enableSprint && IsSprinting()) return sprintStepRate;
        if (enableCrouch && IsCrouched())  return crouchStepRate;
        return walkStepRate;
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 1)
        {
            _audioSource.PlayOneShot(footstepClips[0], volume);
            return;
        }

        int index;
        do
        {
            index = Random.Range(0, footstepClips.Length);
        }
        while (index == _lastClipIndex);

        _lastClipIndex = index;
        _audioSource.PlayOneShot(footstepClips[index], volume);
    }

    private bool IsSprinting()
    {
        if (playerController == null) return false;
        return playerController.playerCamera != null &&
               playerController.playerCamera.Lens.FieldOfView > playerController.fov + 0.5f;
    }

    private bool IsCrouched()
    {
        if (playerController == null) return false;
        return playerController.transform.localScale.y < 0.95f;
    }
}
