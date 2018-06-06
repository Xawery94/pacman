using UnityEngine;

public class AudioManager : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip pacmandotEat;
    public AudioClip energizer;
    public AudioClip death;
    public AudioClip ghostEat;
    public AudioClip winSound;
    public static bool win = false;
    public static int win_played = 1;
    private static AudioManager _instance;
    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AudioManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(this != _instance)   
                Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        audioSource = GameObject.FindObjectOfType<AudioSource>();
    }

    public void pacdot_play()
    {
        audioSource.clip = pacmandotEat;
        audioSource.PlayOneShot(pacmandotEat, 0.2f);
    }

    public void energizer_play()
    {
        audioSource.clip = energizer;
        audioSource.Play();
    }

    public void death_play()
    {
        audioSource.clip = death;
        audioSource.Play();
    }

    public void ghostEat_play()
    {
        audioSource.clip = ghostEat;
        audioSource.Play();
    }
    public void win_play()
    {
        if (win && win_played == 1)
        {
            audioSource.clip = winSound;
            audioSource.Play();
            win = false;
            win_played--; 
        }
    }
}
