using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject customerGroupPrefab;

    [SerializeField]
    private GameObject tablePrefab;
    
    [SerializeField]
    private Transform customerQueueContainer;

    [SerializeField]
    private TextMeshProUGUI revenueLabel;

    private long currentRevenue;
    private long targetRevenue;

    private long currentReputation;
    private long maxReputation;

    [SerializeField]
    private Slider timerBar;

    private float remainingTime = 0;
    private float maxTime = 0;

    private bool started = false;

    public static GameController Instance { get; private set; }

    public delegate void OnInteractableClick (GameObject target);
    public event OnInteractableClick onInteractableClickedEvent;

    void Awake () {
        if (Instance != null) return;

        Instance = this;
    }

    void Start () { StartGame (); }

    void StartGame () {
        if (started) return;

        started = true;

        maxTime = 5;
        remainingTime = maxTime;

        SetRevenueTarget (500);

        SpawnCustomers ();
        SpawnCustomers ();
    }

    #region Revenue
    void SetRevenueTarget (long newTarget) {
        targetRevenue = newTarget;

        UpdateCurrentRevenue (0);
    }

    void UpdateCurrentRevenue (long newValue) {
        currentRevenue = newValue;

        if (revenueLabel != null)
            revenueLabel.text = $"{currentRevenue}/{targetRevenue}";
    }
    #endregion

    #region Spawning
    void SpawnCustomers () {
        GameObject go = Instantiate (customerGroupPrefab, customerQueueContainer);
        var customerGroup = go.GetComponent<CustomerGroup> ();
        var evtTrigger = go.GetComponent<EventTrigger> ();
        evtTrigger.triggers[0].callback.AddListener ((evtData) => {
            onInteractableClickedEvent.Invoke (go);
        });
    }
    #endregion

    void UpdateTimeLeft (float newValue) {
        remainingTime = Mathf.Max (0, newValue);

        if (timerBar != null) timerBar.value = remainingTime/maxTime;

        if (remainingTime == 0) {
            GameOver ();
        }
    }

    void GameOver () {
        Debug.LogWarning ("Game Over");
        started = false;
    }

    // Update is called once per frame
    void Update() {
        if (remainingTime > 0) {
            UpdateTimeLeft (remainingTime -= Time.deltaTime);
        }
    }
}
