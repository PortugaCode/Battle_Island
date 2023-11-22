using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepControl : MonoBehaviour
{
    private AudioSource audioSource;
    private CharacterMovement player;

    private void Awake()
    {
        TryGetComponent(out audioSource);
        player = gameObject.transform.root.GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if(player.isGround)
        {
            if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                audioSource.volume = player.currentSpeed / player.runSpeed;
            }
            else if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
            {
                audioSource.volume = player.currentSpeed / player.runSpeed;
            }
            else if(audioSource.volume <= 1f)
            {
                audioSource.volume = 0f;
            }
        }
        else if(!player.isGround)
        {
            audioSource.volume = 0f;
        }
    }
}
