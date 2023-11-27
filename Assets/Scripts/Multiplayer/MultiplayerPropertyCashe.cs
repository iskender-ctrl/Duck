using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerPropertyCashe : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("Button to initiate matchmaking")]
    public Button playButton;

    [Header("Matchmaking Settings")]
    [Tooltip("Maximum time to wait for matchmaking (in seconds)")]
    public float matchmakingTimeout = 10f;


}
