using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer ballMesh;
    private Transform currentTile; //the tile the ball is currently on
    [SerializeField]
    private MeshRenderer muzzle; //the mesh of the muzzle object, where projectiles will be spawned
    [SerializeField]
    private Transform ballBody; //the actual sferical object that I will spin for the illusion of rolling
    
    private bool playerOne = false;


    public Ball enemyBall; //reference to the other ball
    public Transform cameraHolder;
    public int tilesPerSimulationCicle = 10; // how many tiles does the ball calculate its path for per simulation step
    
    

    public void SetupBall(Transform spawnTile, Ball enemyBall, bool isPlayerOne = false)
    {
        playerOne = isPlayerOne;
        currentTile = spawnTile;
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
    }

}
