using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour {

    private static InventoryManager instance;
    private GameObject hoverObject;
    public GameObject tooltip;

    public GameObject slotPrefab;
    public GameObject iconPrefab;

    public GameObject food;
    public GameObject health;
    public GameObject droppedItem;
    public GameObject toolTipObject;

    public Text tooltipSizeObject;
    public Text tooltipTextObject;

    public Canvas canvas;

    private Slot from;
    private Slot to;

    public EventSystem eventSystem;

    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryManager>();
            }
            return instance;
        }
    }
 
    public Slot From
    {
        get
        {
            return from;
        }

        set
        {
            from = value;
        }
    }

    public Slot To
    {
        get
        {
            return to;
        }

        set
        {
            to = value;
        }
    }

    public GameObject SlotPrefab
    {
        get
        {
            return slotPrefab;
        }

        set
        {
            slotPrefab = value;
        }
    }

    public GameObject IconPrefab
    {
        get
        {
            return iconPrefab;
        }

        set
        {
            iconPrefab = value;
        }
    }

    public GameObject HoverObject
    {
        get
        {
            return hoverObject;
        }

        set
        {
            hoverObject = value;
        }
    }

    public GameObject Tooltip
    {
        get
        {
            return tooltip;
        }

        set
        {
            tooltip = value;
        }
    }

    public Canvas Canvas
    {
        get
        {
            return canvas;
        }

        set
        {
            canvas = value;
        }
    }

    public EventSystem EventSystem
    {
        get
        {
            return eventSystem;
        }

        set
        {
            eventSystem = value;
        }
    }

    public Text TooltipSizeObject
    {
        get
        {
            return tooltipSizeObject;
        }

        set
        {
            tooltipSizeObject = value;
        }
    }

    public Text TooltipTextObject
    {
        get
        {
            return tooltipTextObject;
        }

        set
        {
            tooltipTextObject = value;
        }
    }

    public GameObject Mana
    {
        get
        {
            return food;
        }

        set
        {
            food = value;
        }
    }

    public GameObject Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public GameObject DroppedItem
    {
        get
        {
            return droppedItem;
        }

        set
        {
            droppedItem = value;
        }
    }

}
