using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    #region GeneralInfo

    [SerializeField]
    private MeshRenderer ballMesh;
    private Transform currentTile; //the tile the ball is currently on
    [SerializeField]
    private MeshRenderer muzzle; //the mesh of the muzzle object, where projectiles will be spawned
    [SerializeField]
    private Transform ballBody; //the actual sferical object that I will spin for the illusion of rolling
    private List<Transform> targetTiles = new List<Transform>(); //the list of tiles to move on per simulation step
    private int currentTileIndex = 0; //index to keep track of what tile I am on during the visualization phase
    [SerializeField]
    private float lerpDuration = 0.5f; //time it takes to move between two tiles
    [SerializeField]
    private float rollSpeed = 80f; //rolling speed during visualization phase
    private Vector3 startPos;
    private Vector3 endPos;
    private int ballLayer = 0;
    private bool playerOne = false;
    [SerializeField]
    private GameObject hitEffect; //effect visible when taking damage


    public Ball enemyBall; //reference to the other ball
    public Transform cameraHolder;
    public int tilesPerSimulationCicle = 10; // how many tiles does the ball calculate its path for per simulation step

    #endregion

    #region Combat

    [SerializeField]
    private List<GameObject> pooledBullets;
    [SerializeField]
    private GameObject bulletPrefab;
    private float firingSpeed;// a randomized value indicating how often the projectiles are spawned.
    public int hp; Ball health;

    #endregion


    #region StateMachine

    public State currentState;
    public CalculatePathState calculatePathState = new CalculatePathState();
    public MoveState moveState = new MoveState();
    public FightState fightState = new FightState();

    #endregion


    public void SetupBall(Transform spawnTile, Ball enemyBall, bool isPlayerOne = false)
    {
        
        ballLayer = LayerMask.NameToLayer("Ball");
        playerOne = isPlayerOne;
        currentTile = spawnTile;
        firingSpeed = Random.Range(0.4f, 1f);
        hp = Random.Range(2, 10);
        this.enemyBall = enemyBall;
        if (isPlayerOne)
        {
            ballMesh.materials[0].color = Color.blue;
            muzzle.materials[0].color = Color.blue;
        }
        else
        {
            ballMesh.materials[0].color = Color.red;
            muzzle.materials[0].color = Color.red;

        }
        //set up state machine and start first state upon instantiation
        calculatePathState = new CalculatePathState();
        moveState = new MoveState();
        fightState = new FightState();
        currentState = calculatePathState;
        currentState.StartState(this); //calculate the path
    }

    #region ball mechanics
    /// <summary>
    /// not proud of this but it will have to do
    /// </summary>
    public void CalculatePath()
    {
        targetTiles.Clear();
        //send raycast towards the enemy
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, (enemyBall.transform.position - transform.position), 30);
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance)); //order the objects hit by the raycast
        int tilesBetween = hits.Length;
        //check if one of the objects hit was the enemy ball to indicate that it is close and ignore the tiles behind it
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.layer == ballLayer)
            {
                tilesBetween = i - 2; // -2 because we eliminate 2 unnecessary objects hit by the raycast, the enemy ball and the tile it is on 
            }
        }

        if (tilesBetween == 1) //if there is only one tile between the balls start fighting
        {
            SwitchState(fightState);
        }
        else
        {
            if (tilesBetween % 2 == 1) //if odd number of tiles between balls
            {
                int noOfTilesToTravel = Mathf.FloorToInt(tilesBetween / 2); //how many tiles are available to go through
                if (noOfTilesToTravel >= tilesPerSimulationCicle)
                {
                    for (int i = 0; i < tilesPerSimulationCicle; i++)
                    {
                        targetTiles.Add(hits[i].transform);
                    }
                }
                else
                {
                    for (int i = 0; i < noOfTilesToTravel; i++)
                    {
                        targetTiles.Add(hits[i].transform);
                    }
                }
            }
            else //if even number of tiles between the balls
            {
                int noOfTilesToTravel = Mathf.FloorToInt(tilesBetween / 2); //how many tiles are available to go through
                if (noOfTilesToTravel >= tilesPerSimulationCicle)
                {
                    for (int i = 0; i < tilesPerSimulationCicle; i++)
                    {
                        targetTiles.Add(hits[i].transform);
                    }
                }
                else
                {
                    // to keep at least one tile between them, in case we have an even number of tiles, player one moves noOfTile/2 while player two moves noOfTile/2 -1
                    if (playerOne)
                    {
                        for (int i = 0; i < noOfTilesToTravel; i++)
                        {
                            targetTiles.Add(hits[i].transform);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < noOfTilesToTravel - 1; i++)
                        {
                            targetTiles.Add(hits[i].transform);
                        }
                    }

                }
            }
            currentTileIndex = 0;
            SwitchState(moveState); //switch state to move
        }

    }

    public void MoveWrapper()
    {
        StartCoroutine(Move());
    }

    /// <summary>
    /// coroutine to move the ball between the tiles
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move()
    {
        float timeElapsed = 0;
        startPos = new Vector3(currentTile.position.x, transform.position.y, currentTile.position.z);
        if (currentTileIndex< targetTiles.Count)
        {
            endPos = new Vector3(targetTiles[currentTileIndex].position.x, transform.position.y, targetTiles[currentTileIndex].position.z);
            ballBody.transform.LookAt(targetTiles[currentTileIndex].transform, Vector3.up);
        }
        else
        {
            endPos = startPos;
        }
        
        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / lerpDuration);
            ballBody.Rotate(Vector3.right* rollSpeed * timeElapsed, Space.Self);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
        if (currentTileIndex < targetTiles.Count)
            currentTile = targetTiles[currentTileIndex];
        currentTileIndex++;
        if (currentTileIndex < targetTiles.Count)
        {
            StartCoroutine(Move());
        }
        else //if we reached the last tile calculate path again
        {
            SwitchState(calculatePathState);
        }
    }

    public void FightWrapper()
    {
        Debug.Log("Fight!");
        if (playerOne)
        {
            //do the UI stuff.
            UIManager.instance.StartBattle();
        }
        StartCoroutine(Fight());
    }

    private IEnumerator Fight()
    {
        yield return new WaitForSeconds(1);
        muzzle.gameObject.SetActive(true);
        transform.LookAt(enemyBall.transform, Vector3.up);
        yield return new WaitForSeconds(1);
        //Spawn and fire projectiles - No time!!!
        for (int i = 0; i < enemyBall.hp; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, muzzle.transform);
            bullet.transform.localPosition = Vector3.zero;
            pooledBullets.Add(bullet);
        }
        //this should be triggered by whoever is left standing, but I didn't have time to do the combat so I'm calling it from player one
        if (playerOne)
        {
            //do the UI stuff.
            UIManager.instance.EndBattle();
        }
    }

    #endregion

    private void SwitchState(State state)
    {
        currentState = state;
        currentState.StartState(this);
    }


}
