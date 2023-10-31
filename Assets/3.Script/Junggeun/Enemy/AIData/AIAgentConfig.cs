using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AIState", menuName = "AIState/AIState")]
public class AIAgentConfig : ScriptableObject
{
    public float maxTime = 0.5f; // �־��� �� ���� �� �󸶳� ���� ������
    public float maxDistance = 1f; // �־����� ���� �� �ִ� �־����� �Ÿ�
    public float dieForce = 20f;
}
