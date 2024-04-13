using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerGroup : MonoBehaviour
{
    [SerializeField]
    private Slider patienceBar;

    [SerializeField]
    private GameObject highlightGO;

    private float currentPatienceMeter = 0;

    private float patience = 0;

    private List<Customer> customers = new List<Customer> ();

    private Customer.CustomerState state = Customer.CustomerState.Queueing;

    private const float waitingPatience = 10;

    private const float seatedPatience = 15;

    public int GroupSize => customers.Count;

    private bool selected = false;

    void Start () {
        GameController.Instance.onInteractableClickedEvent += OnInteractableClick;
    }

    void SetUp () {
        state = Customer.CustomerState.Queueing;
        SetPatience (waitingPatience);
    }

    void SetPatience (float patience) {
        this.patience = patience;
        currentPatienceMeter = patience;
    }

    void UpdatePatience (float newValue) {
        currentPatienceMeter = Mathf.Max (0, newValue);

        patienceBar.value = currentPatienceMeter/patience;

        if (currentPatienceMeter == 0) {
            OnPatienceEmptied ();
        }
    }

    void OnPatienceEmptied () {
        Debug.LogWarning ("OnPatienceEmptied");
    }

    public void OnSeated () {
        if (state == Customer.CustomerState.Seated) return;

        state = Customer.CustomerState.Seated;
        SetPatience (seatedPatience);

        // Move customers to tables
    }

    void Dismiss () {

    }

    public void SetSelected (bool isSelected) {
        selected = isSelected;
        
        highlightGO?.SetActive (isSelected);
    }

    public void OnInteractableClick (GameObject target) {
        if (target == this.gameObject) {
            Debug.LogWarning ($"Clicked [{target.transform.GetSiblingIndex ()}]");
            SetSelected (true);
        }
        else if (target.GetComponent<CustomerGroup> ()) { // If clicked on another customer group do something else
            Debug.LogWarning ($"I am [{gameObject.transform.GetSiblingIndex ()}]. Clicked on [{target.transform.GetSiblingIndex ()}] customer group");
            SetSelected (false);
        }
        else if (target.GetComponent<Table> ()) {
            Debug.LogWarning ("Clicked on a table");
        }
    }

    void Update() {
        if (Input.GetKeyDown (KeyCode.Space)) {
            OnSeated ();
        }

        if (currentPatienceMeter > 0) {
            UpdatePatience (currentPatienceMeter - Time.deltaTime);
        }
    }

    void OnDestroy () {
        GameController.Instance.onInteractableClickedEvent -= OnInteractableClick;
    }
}
