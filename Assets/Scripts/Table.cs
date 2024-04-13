using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private CustomerGroup customerGroup;

    private int seats = 2;

    private Table linkedTable;

    [SerializeField]
    private bool Linkable = false;

    void Start() {
        GameController.Instance.onInteractableClickedEvent += OnInteractableClick;
    }

    void SetUp (int seats) {
        this.seats = 2 * (Random.Range (1, 2));
    }

    bool SeatCustomers (CustomerGroup customerGroup) {
        if (this.customerGroup) {
            Debug.LogWarning ("Table is already occupied");
            return false;
        }

        if (GetTableSeatCount () < this.customerGroup.GroupSize) {
            Debug.LogWarning ("Table cannot occupy all customers");
            return false;
        }

        this.customerGroup = customerGroup;

        return true;
    }

    public void OnInteractableClick (GameObject target) {
        if (target == this.gameObject) {
            Debug.LogWarning ("Clicked this target");
        }
        else if (target.GetComponent<CustomerGroup> ()) {
            var customerGroup = target.GetComponent<CustomerGroup> ();

            if (SeatCustomers (customerGroup)) {
                customerGroup.OnSeated ();
            }
        }
        else if (target.GetComponent<Table> ()) {
            Debug.LogWarning ("Clicked on another table");
        }
    }

    public void LinkTable (Table table) {
        if (table == null || table == this) return;
        if (linkedTable != null) return;

        linkedTable = table;
        table.linkedTable = this;
    }

    public int GetTableSeatCount () {
        if (linkedTable) return linkedTable.seats + seats;

        return seats;
    }

    void OnDestroy () {
        GameController.Instance.onInteractableClickedEvent -= OnInteractableClick;
    }
}
