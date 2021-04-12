using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour
{
    public static TextUpdater instance;

    [SerializeField] private Text healthText;
    [SerializeField] private Text bombAmountText;

    private PlayerStatistics playerStats;

    private void Awake() {
        if(!instance) {
            instance = this;
        }
    }

    private void Start() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatistics>();
        TextUpdater.instance.UpdateHealthText();
        TextUpdater.instance.UpdateBombAmountText();
    }

    public void UpdateHealthText() {
        healthText.text = playerStats.GetHealth().ToString();
    }

    public void UpdateBombAmountText() {
        bombAmountText.text = playerStats.GetBombAmount().ToString();
    }
}
