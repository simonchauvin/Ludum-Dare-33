using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    public float timeBeforeDeactivation;
    public Material onMaterial;
    public Material offMaterial;

    

    private float deactivationTimer;
    public bool isOn { get; private set; }

	
	void Start ()
    {
        isOn = false;
	}
	
	void Update ()
    {
	    /*if (isOn)
        {
            deactivationTimer += Time.deltaTime;
            if (deactivationTimer >= timeBeforeDeactivation)
            {
                GetComponent<Renderer>().material = offMaterial;
                isOn = false;
            }
        }*/
	}

    void OnTriggerEnter (Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            player.activateSwitch();
            GetComponent<Renderer>().material = onMaterial;
            isOn = true;
            deactivationTimer = 0f;
            AudioManager.instance.playTurnOn();
        }

        if (collider.CompareTag("Monster"))
        {
            GetComponent<Renderer>().material = offMaterial;
            if (isOn)
            {
                AudioManager.instance.playTurnOff();
            }
            isOn = false;
        }
    }
}
