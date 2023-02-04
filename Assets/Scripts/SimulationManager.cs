using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    Ball playerOne;
    Ball playerTwo;
    [SerializeField]
    private Ball ballPrefab;
    Camera mainCamera;
    [SerializeField]
    private Transform zoomOutPosition;


    public void StartSimulation(HexTile[] cells, int height, int width)
    {
        //pick a random spot for each ball without risking having them too close to eachother
        int playerOneIndex = Random.Range(0, Mathf.RoundToInt(height * width * 0.3f));
        int playerTwoIndex = Random.Range(height * width - Mathf.RoundToInt(height * width * 0.3f), height * width - 1);
        HexTile playerOneTile = cells[playerOneIndex];
        HexTile playerTwoTile = cells[playerTwoIndex];
        mainCamera = Camera.main;
        SpawnBalls(playerOneTile, playerTwoTile);
    }


    /// <summary>
    /// spawns the balls after the grid has been set up
    /// </summary>
    /// <param name="playerOneTile"></param>
    /// <param name="playerTwoTile"></param>
    public void SpawnBalls(HexTile playerOneTile, HexTile playerTwoTile)
    {
        //for a 'Restart' functionality at the end
        if (playerOne != null)
        {
            Destroy(playerOne.gameObject);
        }
        if (playerTwo != null)
        {
            Destroy(playerTwo.gameObject);
        }

        playerOne = Instantiate(ballPrefab, playerOneTile.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        playerTwo = Instantiate(ballPrefab, playerTwoTile.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        mainCamera.transform.SetParent(playerOne.cameraHolder);
        mainCamera.transform.localPosition = Vector3.zero;
        playerOne.SetupBall(playerOneTile.transform, playerTwo, true);
        playerTwo.SetupBall(playerTwoTile.transform, playerOne);
    }
}
