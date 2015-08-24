using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    #region PARAMETERS
    public float timeToKill;
    #endregion

    private Monster monster;
    private Target[] targets;
    private Vector3 startPosition;

    #region COMPONENTS
    public Renderer m_Renderer { get; private set; }
    public NavMeshAgent m_Agent { get; private set; }
    #endregion

    #region VARIABLES
    private Vector3 nextDestination;
    public bool isVisible { get; private set; }
    private float attackTimer, loseTimer;
    private bool attacking;
    #endregion


    void Start ()
    {
        monster = GameObject.FindObjectOfType<Monster>();
        targets = GameObject.FindObjectsOfType<Target>();
        startPosition = GameObject.Find("PlayerStartPosition").transform.position;

        m_Renderer = GetComponent<Renderer>();
        m_Agent = GetComponent<NavMeshAgent>();

        reset();
	}

    void reset ()
    {
        attackTimer = 0f;
        loseTimer = 0f;
        attacking = false;
        m_Agent.Warp(new Vector3(startPosition.x, transform.position.y, startPosition.z));
        findNextDestination();
    }

    void Update()
    {
        //checkVisibility();

        if (m_Agent.remainingDistance <= 0.2f)
        {
            findNextDestination();
        }

        checkVictory();
    }

    void FixedUpdate()
    {
        checkAttack();
    }

    public void findNextDestination ()
    {
        NavMeshPath path = new NavMeshPath();
        int indexTargetChosen = -1;
        bool found = false;
        int count = 0;
        if (getNumberOfTargetsOff() > 0)
        {
            while (!found && count < 99)
            {
                indexTargetChosen = Random.Range(0, targets.Length);
                if (!targets[indexTargetChosen].isOn)
                {
                    m_Agent.CalculatePath(targets[indexTargetChosen].transform.position, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        found = true;
                    }
                }
                count++;
            }
        }
        
        if (found)
        {
            Vector3 destination = targets[indexTargetChosen].transform.position;
            m_Agent.destination = new Vector3(destination.x, transform.position.y, destination.z);
        }
    }

    void checkAttack ()
    {
        bool resume = false;
        Vector3 position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        RaycastHit hitInfo;
        if (Physics.Raycast(position, transform.forward, out hitInfo, Mathf.Infinity) 
            || Physics.Raycast(position, Quaternion.Euler(0f, 1f, 0f) * transform.forward, out hitInfo, Mathf.Infinity) || Physics.Raycast(transform.position, Quaternion.Euler(0f, -1f, 0f) * transform.forward, out hitInfo, Mathf.Infinity)
            || Physics.Raycast(position, Quaternion.Euler(0f, 2f, 0f) * transform.forward, out hitInfo, Mathf.Infinity) || Physics.Raycast(transform.position, Quaternion.Euler(0f, -2f, 0f) * transform.forward, out hitInfo, Mathf.Infinity)
            || Physics.Raycast(position, Quaternion.Euler(0f, 3f, 0f) * transform.forward, out hitInfo, Mathf.Infinity) || Physics.Raycast(transform.position, Quaternion.Euler(0f, -3f, 0f) * transform.forward, out hitInfo, Mathf.Infinity)
            || Physics.Raycast(position, Quaternion.Euler(0f, 4f, 0f) * transform.forward, out hitInfo, Mathf.Infinity) || Physics.Raycast(transform.position, Quaternion.Euler(0f, -4f, 0f) * transform.forward, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.CompareTag("Monster"))
            {
                if (attackTimer < timeToKill)
                {
                    monster.hit(attackTimer, timeToKill);
                    attackTimer += Time.deltaTime;
                    m_Agent.Stop();
                    transform.LookAt(monster.transform);
                }
                else
                {
                    monster.kill();
                    resume = true;
                }
            }
            else
            {
                resume = true;
            }
        }
        else
        {
            resume = true;
        }

        if (resume)
        {
            attackTimer = 0f;
            m_Agent.Resume();
            monster.notHit();
        }
    }

    void checkVisibility ()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(monster.m_Camera);
        if (GeometryUtility.TestPlanesAABB(planes, m_Renderer.bounds))
        {
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }
    }

    void checkVictory ()
    {
        bool allOn = true;
        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].isOn)
            {
                allOn = false;
            }
        }

        if (allOn)
        {
            loseTimer += Time.deltaTime;
            monster.lose(loseTimer);
        }
    }

    public int getNumberOfTargetsOn ()
    {
        int number = 0;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].isOn)
            {
                number++;
            }
        }
        return number;
    }

    public int getNumberOfTargetsOff()
    {
        int number = 0;
        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].isOn)
            {
                number++;
            }
        }
        return number;
    }

    public int getNumberOfTargets ()
    {
        return targets.Length;
    }

    public void activateSwitch ()
    {

    }

    public void kill ()
    {
        reset();
    }
}
