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
    public float runningHearRange = 15f;
    public float seeRange = 25f;
    public float seeAngle = 45f;
    public float soundDetectionRange = 15f;
    public float timeInactive = 3f;

    private NavMeshAgent agent;
    private Vector3 roamingVector;
    private bool isChasing = false;
    private float playerLostTime;

    private int currentDifficulty = 0;
    private float roamingBias;
    private bool playerInSafeSpace = false;
    private bool shouldRoamAroundHouse;
    private Vector3 safeSpacePosition;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        SetNewRoamingVector();
    }

    void Update()
    {

        DebugKeys();

        animator.SetBool("IsRunning", isChasing);

        if(isChasing)
        { 
            ChasePLayer();
        }
        else if(shouldRoamAroundHouse) 
        {
            // NEEDS MORE TESTING
            Debug.Log("Player enterd safe space");
            RoamAroundHouse();
            if(!playerInSafeSpace) 
            {
                Debug.Log("Player exit safe space");
                shouldRoamAroundHouse = false;
                SetNewRoamingVector();
            }
        }
        else
        {
            RoamAround();
            if(MonsterSeesPlayer()) 
            {
                StartChase();
            }
        }
    }

    void DebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            IncreaseDifficulty();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            DecreaseDifficulty();
        }
    }

    public void PlayerInSafeSpace(Vector3 position) 
    {
        playerInSafeSpace = true;
        safeSpacePosition = position;
    }

    public void PlayerExitSafeSpace() 
    {
        playerInSafeSpace = false;
    }

    public void IncreaseDifficulty() 
    {
        System.Math.Clamp(currentDifficulty++, 0, 4);
        Debug.Log("Current Difficulty : " + currentDifficulty);
    }

    public void DecreaseDifficulty()
    {
        System.Math.Clamp(currentDifficulty--, 0, 4);
        Debug.Log("Current Difficulty : " + currentDifficulty);
    }

    void RoamAround() {

        agent.speed = roamingSpeed;

        if(!agent.pathPending && agent.remainingDistance < 0.5f) 
        {
            SetNewRoamingVector();
        }
    }

    bool MonsterSeesPlayer() 
    {
        // Script to determine if the player is in the "aggro" range of the monster
        // The monster has 3 ways to detect the player and depending on the aggresivness of the monster, the ranges will change
        // First the detectionRange which is a circle around the monster, if the player is inside of this range, the monster will aggro
        // Second is the hearRange, the monster will hear the player when they run, running makes the radius of the range increase
        // Lastly there is the seeRange, which is a cone like shape facing the front of the monster to a certain distance

        // NOTE that the player can enter "safe spaces" where the monster should roam arround the center of that space without ever entering
        // Only if the player enter the safe space while being chased, otherwise the monster should continue to roam as always

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        directionToPlayer.Normalize();
        float angleOfPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Detection range
        if(distanceToPlayer <= detectionRange)
        {
            Debug.Log("Monster hears the player close");
            return true;
        }

        // Hearing range when the player is running, the detection range is bigger
        if(player.GetComponent<Movement>().isRunning && distanceToPlayer <= runningHearRange)
        {
            Debug.Log("Monster hears the player running");
            return true;
        }

        // See range, distance to where the monster can see, and the angle of the fov for the monster.
        if(distanceToPlayer <= seeRange && angleOfPlayer <= seeAngle)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, directionToPlayer, out hit, seeAngle)) 
            {
                if(hit.transform == player)
                {
                    Debug.Log("Monster sees the player");
                    return true;
                }
            }
        }

        return false;
    }

    void SetNewRoamingVector() 
    {
        //TODO

        // The monster will roam randomly at the start.
        // With increasing monster aggressivity or *dificulty, the roaming algorithm should change
        // The more aggressive the monster is, the more likely the monster will get close to the player and the more likely it is for the monster to catch the player

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

    void ChasePLayer() 
    {
        agent.SetDestination(player.position);

        if(!MonsterSeesPlayer()) 
        {
            playerLostTime += Time.deltaTime;
            if(playerLostTime >= timeInactive)
            {
                isChasing = false;
                SetNewRoamingVector();
            }
        }

        if(playerInSafeSpace) shouldRoamAroundHouse = true;
    }

    // NEEDS MORE TESTING
    void RoamAroundHouse()
    {
        isChasing = false;
        // When the player enters a safe space, the monster will roam around the house until the player exits the safe space
        // When the player exits, the monster will continue to chase the player

        float maxRoamingRange = 20f;
        Vector3 randomDirection;

        randomDirection = Random.insideUnitSphere * maxRoamingRange;
        randomDirection.y = 0;

        randomDirection += safeSpacePosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            roamingVector = hit.position;
            agent.SetDestination(roamingVector);
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
