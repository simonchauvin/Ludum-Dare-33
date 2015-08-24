using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    #region PARAMETERS
    public float maxDistanceBeforeRespawn;
    #endregion

    

    private Player player;
    private Vector3 startPosition;
    private Text distanceToPlayer;
    private Text targetsOn;
    private Image deathFade;
    private Image loseFade;
    private Text loseText;

    public Camera m_Camera { get; private set; }

    #region VARIABLES
    
    #endregion


    void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
        m_Camera = transform.FindChild("MainCamera").GetComponent<Camera>();
        startPosition = GameObject.Find("MonsterStartPosition").transform.position;

        

        RectTransform canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        distanceToPlayer = canvas.FindChild("DistanceToPlayer").GetComponent<Text>();
        targetsOn = canvas.FindChild("TargetsOn").GetComponent<Text>();
        deathFade = canvas.FindChild("DeathFade").GetComponent<Image>();
        loseFade = canvas.FindChild("LoseFade").GetComponent<Image>();
        loseText = canvas.FindChild("LoseText").GetComponent<Text>();

        reset();
	}

    void reset()
    {
        transform.position = new Vector3(startPosition.x, transform.position.y, startPosition.z);
        deathFade.color = new Color(0f, 0f, 0f, 0f);
    }

	void Update ()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= maxDistanceBeforeRespawn)
        {
            respawn();
        }

        updateUI();
	}

    void FixedUpdate ()
    {
        
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player.kill();
            AudioManager.instance.playKill();
        }
    }

    void updateUI ()
    {
        distanceToPlayer.text = Vector3.Distance(transform.position, player.transform.position).ToString();
        targetsOn.text = player.getNumberOfTargetsOn().ToString() + " / " + player.getNumberOfTargets();
    }

    void respawn ()
    {
        
    }

    public void lose (float loseTimer)
    {
        deathFade.color = new Color(0f, 0.1f, 0f, Mathf.Lerp(0f, 1f, loseTimer / 2f));
        loseText.enabled = true;
        AudioManager.instance.playLose();
    }

    public void hit (float timer, float maxTime)
    {
        deathFade.color = new Color(0.1f, 0f, 0f, Mathf.Lerp(0f, 1f, timer / maxTime));
        AudioManager.instance.playDeath();
    }

    public void notHit()
    {
        deathFade.color = new Color(0f, 0f, 0f, 0f);
    }

    public void kill ()
    {
        reset();
        AudioManager.instance.playDeath();
    }
}
