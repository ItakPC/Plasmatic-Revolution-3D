using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Inventory inventory;

	void Start () {
	
	}
	
	void Update () {
	
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Item")
        {
            inventory.AddItem(other.GetComponent<Item>());
        }
    }
}
