using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip death;
    public AudioClip lose;
    public AudioClip kill;
    public AudioClip turnOn;
    public AudioClip turnOff;

	/// <summary>
	/// The only instance of the audio manager.
	/// </summary>
	private static AudioManager _instance;

	/// <summary>
	/// The audio source.
	/// </summary>
	private AudioSource thisAudio;


	/// <summary>
	/// Retrieve the instance of the audio manager.
	/// </summary>
	/// <value>The audio manager.</value>
	public static AudioManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
			}
			return _instance;
		}
	}

	// Use this for initialization
	void Start ()
	{
		// Components
		thisAudio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update ()
	{

	}

    public void playDeath ()
    {
        thisAudio.clip = death;
        thisAudio.Play();
    }

    public void playLose ()
    {
        thisAudio.clip = lose;
        thisAudio.Play();
    }

    public void playKill()
    {
        thisAudio.clip = kill;
        thisAudio.Play();
    }

    public void playTurnOn()
    {
        thisAudio.clip = turnOn;
        thisAudio.Play();
    }

    public void playTurnOff()
    {
        thisAudio.clip = turnOff;
        thisAudio.Play();
    }
}