using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Time.timeScale = 0f;
            FindObjectOfType<SoundManager>().Play("Win Level");
            Debug.Log("You win!!!");
            //test
        }
    }
}
