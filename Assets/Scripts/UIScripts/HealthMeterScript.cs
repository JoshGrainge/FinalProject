using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeterScript : MonoBehaviour
{
    [SerializeField]
    HealthScript playerHealth;

    [SerializeField]
    Text healthTextNum;

    [SerializeField]
    Image healthFillImage;

    void Update()
    {
        // Set fill amount of foreground health image (range of 0-1) and set 
        // text on health to be number of players health
        healthFillImage.fillAmount = playerHealth.health / playerHealth.maxHealth;
        healthTextNum.text = playerHealth.health.ToString();
    }
}
