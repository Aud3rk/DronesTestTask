using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public event Action<int> DroneCountChange; 
    public event Action<int> DroneSpeedChange;
    public event Action<bool> DrawGizmo;
    public event Action<int> SpawnCount;

    [SerializeField] private TMP_InputField inputField;
    
    [SerializeField] private TMP_Text textBarCount;
    [SerializeField] private TMP_Text textBarSpeed;

    [SerializeField] private TMP_Text textBlueTeam;
    [SerializeField] private TMP_Text textRedTeam;

    [SerializeField] private Slider droneCountSlider;
    [SerializeField] private Slider droneSpeedSlider;

    [SerializeField] private Toggle toggleGizmo;
    [SerializeField] private Toggle toggleScore;

    [SerializeField] private GameObject ScoreMenu;
    
    
    void Start()
    {
        droneCountSlider.onValueChanged.AddListener(ChangeDroneCount);
        droneCountSlider.onValueChanged.AddListener(BarCount);
        droneSpeedSlider.onValueChanged.AddListener(ChangeDroneSpeed);
        droneSpeedSlider.onValueChanged.AddListener(BarSpeed);
        toggleGizmo.onValueChanged.AddListener(TurnGizmo);
        toggleScore.onValueChanged.AddListener(TurnScore);
        inputField.onValueChanged.AddListener(InputValueToSpawn);
    }

    private void InputValueToSpawn(string arg0)
    {
        int count;
        if (Int32.TryParse(arg0,out count)) 
            SpawnCount?.Invoke(count);

    }

    private void TurnScore(bool turn) => 
        ScoreMenu.SetActive(turn);

    private void TurnGizmo(bool turn) => 
        DrawGizmo?.Invoke(turn);

    public void ChangeRedScore(int score) => 
        textRedTeam.text = score.ToString();
    public void ChangeBlueScore(int score) => 
        textBlueTeam.text = score.ToString();

    private void BarCount(float count) => 
        textBarCount.text = count.ToString();
    private void BarSpeed(float count) => 
        textBarSpeed.text = count.ToString();

    private void ChangeDroneCount(float arg) => 
        DroneCountChange?.Invoke((int)arg);
    private void ChangeDroneSpeed(float arg) => 
        DroneSpeedChange?.Invoke((int)arg);
    
}
