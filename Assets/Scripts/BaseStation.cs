using System;
using System.Collections.Generic;
using System.Linq;
using Drones;
using Fabric;
using UnityEngine;
using Object = UnityEngine.Object;

public class BaseStation
{
    public event Action<Loot> TakenLoot;
    public event Action<int> ChangeScore;
    public Transform _spawnPoint;

    private int _points=0;
    private Dictionary<Loot, Drone> _targets;
    private List<Drone> _drones;
    private DroneFabric _DroneFabric;
    private Vector3 _highPoint;
    private Vector3 _lowPoint;

    public BaseStation(DroneFabric DroneFabric, Transform spawnPoint, Vector3 lowPoint, Vector3 highPoint)
    {
        _DroneFabric = DroneFabric;
        _DroneFabric.SpawnPoint = spawnPoint;
        _spawnPoint = spawnPoint;

        _highPoint = highPoint;
        _lowPoint = lowPoint;
            
        _targets = new Dictionary<Loot, Drone>();
        _drones = new List<Drone>();
            
    }

    public void Start()
    {
        SpawnDrone(5);
    }


    public bool TryToLoot(Loot loot,Drone drone)
    {
        if (_targets.ContainsKey(loot))
            return false;
        _targets.Add( loot,drone);
        return true;
    }

    public void ChangeDroneCount(int count)
    {
        count = _drones.Count - count;
        if (count > 0)
            for (int i = 0; i < count; i++)
            {
                Drone drone = _drones[i];
                _drones.Remove(drone);
                Object.Destroy(drone.gameObject);
            }
        else if (count < 0) 
            SpawnDrone(Math.Abs(count));
    }

    public void ChangeDroneSpeed(int speed)
    {
        foreach (Drone drone in _drones) 
            drone.SetSpeed(speed);
    }

    private void SpawnDrone(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var drone = _DroneFabric.GenerateDrone();
            drone.Init(_lowPoint, _highPoint, _spawnPoint.position, this);
            drone.Unload += TakeLoot;
            drone.StartSearchingLoot();
            drone.TookLoot += TakenLoot.Invoke;
            _drones.Add(drone);
        }
    }

    public void ChangeTargetToDrone(Loot loot)
    {
        if (_targets.ContainsKey(loot))
        {
            var pare = _targets.First(x => x.Key == loot);
            pare.Value.StartSearchingLoot();
            _targets.Remove(pare.Key);
        }
    }

    public void TurnGizmo(bool turn)
    {
        foreach (Drone drone in _drones)
        {
            var component = drone.GetComponent<LineRenderer>();
            component.enabled = turn;
        }
    }

    private void TakeLoot(int points)
    {
        _points += points;
        ChangeScore?.Invoke(_points);
    }

    ~BaseStation()
    {
        foreach (Drone drone in _drones)
        {
            drone.Unload -= TakeLoot;
            drone.TookLoot -= TakenLoot.Invoke;
        }
    }
}