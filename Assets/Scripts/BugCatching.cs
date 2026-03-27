using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BugCatcher : MonoBehaviour
{
    [Header("Bug Catching Settings")]
    public Transform cameraTransform;
    public float bugCatchingRaycastDistance = 3f;
    public KeyCode catchKey = KeyCode.Mouse0;

    [Header("Catch Display")]
    [SerializeField] Image bugCaughtImage;
    [SerializeField] Image handCaughtImage;
    [Tooltip("How long the caught bug image is shown on screen.")]
    [SerializeField] float displayDuration = 2.5f;

    [Header("Jar")]
    [SerializeField] JarController jarController;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip netSwingAudioClip;
    [SerializeField] AudioClip bugSavedAudioClip;

    [Header("Music Ducking")]
    [SerializeField] AudioSource musicSource;
    [Tooltip("Volume the music ducks down to while the SFX plays (0–1).")]
    [SerializeField] float duckVolume = 0.1f;
    [Tooltip("How quickly the music ducks down and recovers (seconds).")]
    [SerializeField] float duckFadeTime = 0.15f;

    void Update()
    {
        if (Input.GetKeyDown(catchKey))
        {
            audioSource.PlayOneShot(netSwingAudioClip);

            LayerMask playerMask = LayerMask.GetMask("Player");
            LayerMask everythingButPlayerMask = ~playerMask;
            RaycastHit hitInfo;

            bool hitSomething = Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward), out hitInfo, bugCatchingRaycastDistance, everythingButPlayerMask);
            GameObject bugObject = hitSomething ? hitInfo.transform.gameObject : null;
            if (bugObject != null && bugObject.CompareTag("FreeBug"))
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.yellow, 2f);
                CatchBug(bugObject);
            }
        }
    }

    private void CatchBug(GameObject bugObject)
    {
        BugIdentity identity = bugObject.GetComponent<BugIdentity>();
        if (identity != null && identity.bugData != null && identity.bugData.catchImage != null)
        {
            bugCaughtImage.sprite = identity.bugData.catchImage;

            if (BugJar.Instance != null && BugJar.Instance.TryAddBug(identity.bugData))
            {
                identity.bugData.RegisterCatch();
                Debug.Log($"Caught bug: {identity.bugData.bugName} | Total caught: {identity.bugData.AmountCaught} / {identity.bugData.catchGoal}");

                if (identity.bugData.GoalReached)
                    Debug.Log($"Achievement unlocked: caught all {identity.bugData.bugName}!");

                jarController?.SpawnButterflyVisual(identity.bugData);
            }
        }
        else
        {
            Debug.LogWarning($"Bug '{bugObject.name}' is missing a BugIdentity or BugDataSO with a catchImage.");
        }

        Destroy(bugObject);
        audioSource.PlayOneShot(bugSavedAudioClip);
        StartCoroutine(DuckMusic(bugSavedAudioClip.length));
        StartCoroutine(ShowImage(bugCaughtImage, displayDuration));
        StartCoroutine(ShowImage(handCaughtImage, displayDuration));
    }

    private IEnumerator DuckMusic(float sfxDuration)
    {
        if (musicSource == null) yield break;

        float originalVolume = musicSource.volume;

        yield return StartCoroutine(FadeVolume(originalVolume, duckVolume, duckFadeTime));
        yield return new WaitForSeconds(sfxDuration - duckFadeTime * 2f);
        yield return StartCoroutine(FadeVolume(duckVolume, originalVolume, duckFadeTime));
    }

    private IEnumerator FadeVolume(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        musicSource.volume = to;
    }

    private IEnumerator ShowImage(Image image, float seconds)
    {
        image.enabled = true;
        yield return new WaitForSeconds(seconds);
        image.enabled = false;
    }
}