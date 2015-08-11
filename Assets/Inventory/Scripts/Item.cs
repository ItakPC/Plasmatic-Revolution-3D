using UnityEngine;
using System.Collections;

public enum ItemType { FISH };

public class Item : MonoBehaviour {

	public ItemType type;

	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;

	public int maxStackSize;

	public void Use() {
		switch(type){

		}
	}
}
