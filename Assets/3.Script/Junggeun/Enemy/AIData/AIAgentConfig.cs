using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AIState", menuName = "AIState/AIState")]
public class AIAgentConfig : ScriptableObject
{
    public float maxTime = 0.5f; // 멀어진 후 따라갈 때 얼마나 텀이 있을지
    public float maxDistance = 1f; // 멀어지면 따라갈 때 최대 멀어지는 거리
    public float dieForce = 20f;
}
