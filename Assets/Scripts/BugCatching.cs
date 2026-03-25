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
    [SerializeField] AudioClip bugSavedAudioClip;
    
    

    void Update()
    {
        if (Input.GetKeyDown(catchKey))
        {
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
            Debug.Log($"Caught bug: {identity.bugData.bugName}");
            jarController?.AddButterfly(identity.bugData);
        }
        else
        {
            Debug.LogWarning($"Bug '{bugObject.name}' is missing a BugIdentity or BugDataSO with a catchImage.");
        }

        Destroy(bugObject);
        audioSource.PlayOneShot(bugSavedAudioClip);
        StartCoroutine(ShowImage(bugCaughtImage, displayDuration));
        StartCoroutine(ShowImage(handCaughtImage, displayDuration));
    }

    private IEnumerator ShowImage(Image image, float seconds)
    {
        image.enabled = true;
        yield return new WaitForSeconds(seconds);
        image.enabled = false;
    }

}