using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tooltip : MonoBehaviour
{
    [SerializeField]
    Text tooltipText;

    // Disable text initially
    private void Start()
    {
        TurnOffTooltip();
    }

    public void GenerateTooltip(Item item)
    {
        string statInformation = "";

        statInformation += $"{item.itemName}:\n";

        statInformation += $"{item.description}\n";

        // Get all dictionary string int pairs add them to stat information string separating each with a new line
        foreach(string s in item.stats.Keys)
        {
            item.stats.TryGetValue(s, out int value);

            statInformation += $"{s}: {value}\n";
        }

        //string tooltip = string.Format("<b>{0}<b>\n{1}\n\n{}", item.itemName)

        // Set tooltip text
        tooltipText.text = statInformation;
        tooltipText.transform.parent.gameObject.SetActive(true);
    }

    public void TurnOffTooltip()
    {
        tooltipText.text = "";
        tooltipText.transform.parent.gameObject.SetActive(false);
    }
}
