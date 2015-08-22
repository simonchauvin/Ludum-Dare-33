using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    #region COMPONENTS
    public float timeToKill;
    #endregion

    private Monster monster;
    private Vector3 startPosition;

    #region COMPONENTS
    public Renderer m_Renderer { get; private set; }
    public NavMeshAgent m_Agent { get; private set; }
    #endregion

    #region VARIABLES
    private Vector3 nextDestination;
    public bool isVisible { get; private set; }
    private float attackTimer;
    #endregion


    void Start ()
    {
        monster = GameObject.FindObjectOfType<Monster>();
        startPosition = GameObject.Find("PlayerStartPosition").transform.position;

        m_Renderer = GetComponent<Renderer>();
        m_Agent = GetComponent<NavMeshAgent>();

        reset();
	}

    void reset ()
    {
        attackTimer = 0f;
        findNewDestination();
    }

    void Update()
    {
        checkVisibility();

        if (m_Agent.remainingDistance <= 0f)
        {
            findNewDestination();
        }
    }

    void FixedUpdate()
    {
        checkAttack();
    }

    void findNewDestination ()
    {
        m_Agent.destination = new Vector3(Random.Range(-1f, 1f) * GameConstants.width, transform.position.y, Random.Range(-1f, 1f) * GameConstants.depth);
    }

    void checkAttack ()
    {
        bool resume = false;
        RaycastHit hitInfo;
        if (Physics.SphereCast(new Ray(transform.position, transform.forward), 4f, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.CompareTag("Monster"))
            {
                if (attackTimer < timeToKill)
                {
                    monster.hit();
                    attackTimer += Time.deltaTime;
                    m_Agent.Stop();
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

    public void kill ()
    {
        transform.position = startPosition;
        reset();
    }
}
