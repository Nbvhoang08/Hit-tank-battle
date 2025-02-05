using UnityEngine;

public class SoundManager : Singleton<SoundManager> 
{
     [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; 
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource moveSource; // Thêm AudioSource cho âm thanh di chuyển

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip ClickSound;
    [SerializeField] private AudioClip[] VFXSound;
    [SerializeField] private AudioClip moveSound; // Thêm âm thanh di chuyển

    [Header("Volume Settings")]
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.3f;
    [SerializeField][Range(0f, 1f)] private float sfxVolume = 0.5f;
    [SerializeField][Range(0f, 1f)] private float clickVolume = 0.5f;
    [SerializeField][Range(0f, 1f)] private float moveVolume = 0.5f; // Volume cho âm thanh di chuyển

    public bool TurnOn = true;

    public override void Awake()
    {
        base.Awake();
        InitializeAudio();
        TurnOn = true;
    }
    void Update()
    {
        if (!TurnOn)
        {
            musicSource.volume = 0;
            sfxSource.volume = 0;
            clickSource.volume = clickVolume;
            moveSource.volume = 0;
        }
        else
        {
            // Thiết lập volume
            musicSource.volume = musicVolume;
            sfxSource.volume = sfxVolume;
            clickSource.volume = clickVolume;
            moveSource.volume = moveVolume;
        }
    }

    private void InitializeAudio()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        if (clickSource == null)
        {
            clickSource = gameObject.AddComponent<AudioSource>();
            clickSource.loop = false;
            clickSource.playOnAwake = false;
        }

        if (moveSource == null)
        {
            moveSource = gameObject.AddComponent<AudioSource>();
            moveSource.loop = true;  // Loop để phát liên tục
            moveSource.playOnAwake = false;
        }

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
        clickSource.volume = clickVolume;
        moveSource.volume = moveVolume;

        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }

        if (moveSound != null)
        {
            moveSource.clip = moveSound; // Gán âm thanh di chuyển
        }
    }

    // Hàm Play click sound
    public void PlayClickSound()
    {
        if (ClickSound != null)
        {
            clickSource.PlayOneShot(ClickSound, clickVolume);
        }
    }

    // Hàm Play SFX
    public void PlayVFXSound(int soundIndex)
    {
        if (VFXSound != null && soundIndex < VFXSound.Length)
        {
            sfxSource.PlayOneShot(VFXSound[soundIndex], sfxVolume);
        }
    }

    // **HÀM QUẢN LÝ ÂM THANH DI CHUYỂN**
    public void PlayMoveSound()
    {
        if (!moveSource.isPlaying) // Kiểm tra xem đã phát hay chưa
        {
            moveSource.Play();
        }
    }

    public void StopMoveSound()
    {
        if (moveSource.isPlaying)
        {
            moveSource.Stop();
        }
    }
}
