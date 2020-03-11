using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotGUI : MonoBehaviour {
    public Image[] HealthPots;
    public Image[] ManaPots;

    public void DrawHealthPots(int maxPotNum, int numberofPots) {
        for (var i = 0; i < maxPotNum; i++) {
            HealthPots[i].enabled = (i < numberofPots);
        }
    }

    public void DrawManaPots(int maxPotNum, int numberofPots) {
        for (var i = 0; i < maxPotNum; i++) {
            ManaPots[i].enabled = (i < numberofPots);
        }
    }
}
