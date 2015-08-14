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

    public float derpage, awesomeness;

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

        if (derpage > 0) { stats += "\n" + "Derpage: " + derpage.ToString(); }
        if (awesomeness > 0) { stats += "\n" + "Awesomeness: " + awesomeness.ToString(); }

        return string.Format("<color=" + color + "><size=10>{0}</size></color><size=8><i>" + newLine + "{1}</i>{2}</size>", itemName, description, stats);
    }
}
