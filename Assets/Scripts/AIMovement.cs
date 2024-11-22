using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIMovement : MonoBehaviour
{

    public Transform player;
    public float roamingSpeed = 0.0f;//3.5f;
    public float roamingRange = 10f;
    public float chaseSpeed = 5.0f;
    public float detectionRange = 10f;
    public float soundDetectionRange = 15f;
    public float timeInactive = 3f;

    private NavMeshAgent agent;
    private Vector3 roamingVector;
    private bool isChasing = false;
    private float playerLostTime;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        SetNewRoamingVector();
    }

    void Awake() 
    {
    }

    void Update()
    {
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        animator.SetBool("IsRunning", isChasing);

        if(isChasing)
        { 
            ChasePLayer(distanceToPlayer);
        }
        else
        {
            RoamAround();
            if(distanceToPlayer <= detectionRange) 
            {
                StartChase();
            }
        }
        
    }

    void RoamAround() {

        agent.speed = roamingSpeed;

        if(!agent.pathPending && agent.remainingDistance < 0.5f) 
        {
            SetNewRoamingVector();
        }
    }

    void SetNewRoamingVector() 
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamingRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            roamingVector = hit.position;
            //Debug.DrawLine(agent.transform.position, roamingVector, Color.green, 10f);
            agent.SetDestination(roamingVector);
        }
    }

    void ChasePLayer(float distanceToPlayer) 
    {
        agent.SetDestination(player.position);
        

        if(distanceToPlayer > detectionRange) 
        {
            playerLostTime += Time.deltaTime;
            if(playerLostTime >= timeInactive)
            {
                isChasing = false;
                SetNewRoamingVector();
            }
        }
    }

    void StartChase()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        playerLostTime = 0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Collision");
        if(collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Collision with player");
            agent.isStopped = true;

            // GameOver, temporary just load back the scene
            // TODO: proper game over
            // SomeClass.GameOver();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
