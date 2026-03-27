using UnityEngine;

public class outdoorMusic : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip[] songs;

    private int _lastIndex = -1;

    void Start()
    {
        PlayRandomSong();
    }

    void Update()
    {
        if (musicSource != null && !musicSource.isPlaying)
            PlayRandomSong();
    }

    private void PlayRandomSong()
    {
        if (songs == null || songs.Length == 0 || musicSource == null)
            return;

        int index = GetRandomIndex();
        _lastIndex = index;

        musicSource.clip = songs[index];
        musicSource.Play();
    }

    private int GetRandomIndex()
    {
        if (songs.Length == 1)
            return 0;

        int index;
        do
        {
            index = Random.Range(0, songs.Length);
        }
        while (index == _lastIndex);

        return index;
    }
}
