using UnityEngine;
using System.Collections;

public enum ItemType { FISH, BANDAGE};

public class Item : MonoBehaviour {

	public ItemType type;

	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;

	public int maxStackSize;

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
}
