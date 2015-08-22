using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    private Player player;
    public Camera m_Camera { get; private set; }
    private Renderer thisRenderer;

    #region VARIABLES
    
    #endregion


    void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
        m_Camera = transform.FindChild("MainCamera").GetComponent<Camera>();
        thisRenderer = GetComponent<Renderer>();
	}
	
	
	void Update ()
    {
        
	}

    void FixedUpdate ()
    {
        
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player.kill();
        }
    }

    void respawn ()
    {

    }

    public void hit ()
    {
        
    }

    public void kill ()
    {
        Debug.Log("Monster killed");
    }
}
