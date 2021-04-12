using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour {
    [SerializeField] private int bombAmount;
    [SerializeField] private int health;
    [SerializeField] private float immuneAfterDMGTime;

    private bool isImmune = false;

    public void AddBomb() {
        bombAmount++;
        TextUpdater.instance.UpdateBombAmountText();
    }

    public int GetBombAmount() {
        return bombAmount;
    }

    public void DropBomb() {
        bombAmount--;
        TextUpdater.instance.UpdateBombAmountText();
    }

    public void AddHealth() {
        health++;
        TextUpdater.instance.UpdateHealthText();
    }

    public int GetHealth() {
        return health;
    }

    public void TakeHealth() {
        if(!isImmune) {
            health--;
            if(health <= 0) {
                GameManager.instance.GameOver();
            }
            TextUpdater.instance.UpdateHealthText();
            SetImmuneForSeconds(immuneAfterDMGTime);
        }
    }

    public void SetImmune(bool state) {
        isImmune = state;
    }

    public void SetImmuneForSeconds(float time) {
        StartCoroutine(SetImmune(time));
    }

    private IEnumerator SetImmune(float time) {
        isImmune = true;
        yield return new WaitForSeconds(time);
        isImmune = false;
    }
}
