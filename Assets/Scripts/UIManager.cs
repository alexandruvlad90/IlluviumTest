using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private SimulationManager simManager;
    private HexGridGenerator hexGridGen;
    private bool playerOneZoom = true;
    [SerializeField]
    private Text playerZoomButtonText;
    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject fightIcon;
    [SerializeField]
    private GameObject fightDisclaimer;
    [SerializeField]
    private GameObject RestartButton;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        simManager = SimulationManager.instance;
        hexGridGen = HexGridGenerator.instance;
    }

    public void SwitchPlayerZoom()
    {
        playerOneZoom = !playerOneZoom;
        simManager.SwitchPlayerZoom(!playerOneZoom);
        string playerNumber = "1";
        if (!playerOneZoom)
        {
            playerNumber = "2";
        }
        playerZoomButtonText.text = "Switch to Player " + playerNumber;
    }

    public void ZoomOut()
    {
        simManager.ZoomOut();
    }


    public void StartBattle()
    {
        controlPanel.SetActive(false);
        fightIcon.SetActive(true);
        simManager.FreeCamera();
    }

    public void Restart()
    {
        fightIcon.SetActive(false);
        controlPanel.SetActive(true);
        hexGridGen.StartSimulation();
        playerOneZoom = true;
        playerZoomButtonText.text = "Switch to Player 2";
    }
}
