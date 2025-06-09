using System;
using System.Collections;
using Fabric;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{

    [SerializeField] private Transform redSpawnPoint;
    [SerializeField] private Transform blueSpawnPoint;
        
    [SerializeField] private Transform lowPoint;
    [SerializeField] private Transform highPoint;

    [SerializeField] private UiController uiController;    
    
    private BaseStation _blueStation;
    private BaseStation _redStation;
    private LootSpawner _lootSpawner;
    private int spawnCount=5;

    private void Start() => 
        Init();

    private void OnDestroy()
    {
        _blueStation.TakenLoot -= _redStation.ChangeTargetToDrone;
        _redStation.TakenLoot -= _blueStation.ChangeTargetToDrone;
        
        _blueStation.ChangeScore -= uiController.ChangeBlueScore;
        _redStation.ChangeScore -= uiController.ChangeRedScore;
        
        uiController.DroneCountChange -= _redStation.ChangeDroneCount;
        uiController.DroneCountChange -= _blueStation.ChangeDroneCount;
        
        uiController.DroneSpeedChange -= _redStation.ChangeDroneSpeed;
        uiController.DroneSpeedChange -= _blueStation.ChangeDroneSpeed;

        uiController.DrawGizmo -= _blueStation.TurnGizmo;
        uiController.DrawGizmo -= _redStation.TurnGizmo;

        uiController.SpawnCount -= SetSpawnCount;
    }

    private void Init()
    {
        CreateStations();
        SubscribeUIController();
        InitLootController();
    }

    private void InitLootController()
    {
        _lootSpawner = new LootSpawner(lowPoint.position, highPoint.position);
        StartCoroutine(SpawnLoot());
    }

    private void SubscribeUIController()
    {
        _blueStation.ChangeScore += uiController.ChangeBlueScore;
        _redStation.ChangeScore += uiController.ChangeRedScore;
        
        uiController.DroneCountChange += _redStation.ChangeDroneCount;
        uiController.DroneCountChange += _blueStation.ChangeDroneCount;

        uiController.DroneSpeedChange += _redStation.ChangeDroneSpeed;
        uiController.DroneSpeedChange += _blueStation.ChangeDroneSpeed;

        uiController.DrawGizmo += _blueStation.TurnGizmo;
        uiController.DrawGizmo += _redStation.TurnGizmo;

        uiController.SpawnCount += SetSpawnCount;
    }

    private void CreateStations()
    {
        _redStation = new BaseStation(new DroneFabricRed(), redSpawnPoint, lowPoint.position, highPoint.position);
        _blueStation = new BaseStation(new DroneFabricBlue(), blueSpawnPoint, lowPoint.position, highPoint.position);

        _blueStation.TakenLoot += _redStation.ChangeTargetToDrone;
        _redStation.TakenLoot += _blueStation.ChangeTargetToDrone;

        _blueStation.Start();
        _redStation.Start();
    }

    private IEnumerator SpawnLoot()
    {
        while (true)
        {
            _lootSpawner.SpawnLoot(spawnCount);
            yield return new WaitForSeconds(10);
        }
    }

    private void SetSpawnCount(int count) => 
        spawnCount = count;
}