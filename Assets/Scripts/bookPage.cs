using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bookPage : MonoBehaviour
{
    [Header("Bug Data")]
    [SerializeField] private BugDataSO bugData;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI bugNameText;
    [SerializeField] private TextMeshProUGUI bugScientificNameText;
    [SerializeField] private TextMeshProUGUI bugFactText;
    [SerializeField] private Image bugImage;
    [SerializeField] private TextMeshProUGUI bugCaughtText;

    void Start()
    {
        Populate();
    }

    private void Populate()
    {
        if (bugData == null) return;

        bool hasCaughtAny  = bugData.AmountCaught > 0;
        bool hasCaughtHalf = bugData.AmountCaught >= bugData.catchGoal / 2f;
        bool goalMet       = bugData.GoalReached;

        if (bugImage              != null) bugImage.enabled                    = hasCaughtAny;
        if (bugNameText           != null) bugNameText.enabled                 = hasCaughtHalf;
        if (bugScientificNameText != null) bugScientificNameText.enabled       = hasCaughtAny;
        if (bugFactText           != null) bugFactText.enabled                 = goalMet;

        if (bugImage              != null) bugImage.sprite                     = bugData.catchImage;
        if (bugNameText           != null) bugNameText.text                    = bugData.bugName;
        if (bugScientificNameText != null) bugScientificNameText.text          = bugData.bugScientificName;
        if (bugFactText           != null) bugFactText.text                    = bugData.bugFact;
        if (bugCaughtText         != null) bugCaughtText.text                  = $"Caught: {bugData.AmountCaught} / {bugData.catchGoal}";
    }
}