using UnityEngine;
using System.Collections;

public enum ItemType { FISH, BANDAGE};
public enum Rarity { COMMON, UNCOMMON };

public class Item : MonoBehaviour {

	public ItemType type;
    public Rarity rarity;

	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;

	public int maxStackSize;

    public float damage;

    public string itemName;

    public string description;

	public void Use() {
        switch (type)
        {
            case ItemType.FISH:
                Debug.Log("Fish used");
                break;
            case ItemType.BANDAGE:
                Debug.Log("Bandage used");
                
                break;
        }
    }

    public string GetTooltip() {
        string stats = string.Empty;
        string color = string.Empty;
        string newLine = string.Empty;

        if (description != string.Empty)
        {
            newLine = "\n";
        }

        switch (rarity)
        {
            case Rarity.COMMON:
                color = "gray";
                break;
            case Rarity.UNCOMMON:
                color = "white";
                break;

        }

        if (damage > 0) { stats += "\n" + "Damage: " + damage.ToString(); }

        return string.Format("<color=" + color + "><size=10>{0}</size></color><size=8><i>" + newLine + "{1}</i>{2}</size>", itemName, description, stats);
    }


    public void SetContent(Item item) {

        #region Variables
        this.type = item.type;
        this.rarity = item.rarity;
        this.spriteNeutral = item.spriteNeutral;
        this.spriteHighlighted = item.spriteHighlighted;
        this.maxStackSize = item.maxStackSize;
        this.damage = item.damage;
        this.itemName = item.itemName;
        this.description = item.description;
        #endregion

        switch (type)
        {
            case ItemType.FISH:
                GetComponent<Renderer>().material.color = Color.cyan;
                break;
            case ItemType.BANDAGE:
                GetComponent<Renderer>().material.color = Color.red;
                break;
        }
    }
}
