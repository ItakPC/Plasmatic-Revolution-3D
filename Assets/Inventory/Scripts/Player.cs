using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Inventory inventory;
    public Inventory Chest;
    public Canvas canvas;
    public static DroppedItem dItem;

    public Text healthText;
    public Image healthDisplay;

    private int currentHealth;
    public int maxHealth;
    public float cooldown;
    private bool onCD;

    public float lerpSpeed;

    private int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            currentHealth = value;
        }
    }

    void Start () {
        currentHealth = maxHealth;
        onCD = false;
	}
	
	void Update () {
        HandleHealth();
        RaycastHit hit;

        Ray ray = new Ray(transform.position, Vector3.forward);


        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.Open();

        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit, 2))
            {
                if (hit.collider.name == "Chest")
                {
                    Chest.Open();
                }     
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Item")
        {
            inventory.AddItem(other.GetComponent<Item>());
        }
        if (other.name == "Damage")
        {
            if (!onCD && currentHealth >= 5)
            {
                CurrentHealth -= 5;
            }
            else if (!onCD && currentHealth > 0 )
            {
                CurrentHealth -= currentHealth;
            }
        }

        if (other.name == "Heal")
        {
            if (!onCD && currentHealth < maxHealth)
             {
                StartCoroutine(CoolDown());
                CurrentHealth += 1;
            }
        }
    }

    IEnumerator CoolDown() {
        onCD = true;
        yield return new WaitForSeconds(cooldown);
        onCD = false;
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax) {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    private void HandleHealth() {
        healthText.text = "Health: " + currentHealth;

        float currentValue = MapValues(currentHealth, 0, maxHealth, 0, 1);

        healthDisplay.fillAmount = Mathf.Lerp(healthDisplay.fillAmount, currentValue, Time.deltaTime * lerpSpeed);
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "DroppedItem" || col.gameObject.name == "DroppedItem(Clone)")
        {
            inventory.AddItem(col.gameObject.GetComponent<Item>());
            Destroy(col.gameObject);
        }
    }
}

