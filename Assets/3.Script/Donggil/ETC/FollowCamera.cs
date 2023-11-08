using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    float speed = 50f;
    void FixedUpdate()
    {
        Vector3 position = new Vector3(player.position.x, transform.position.y, player.position.z - 15.0f);
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
