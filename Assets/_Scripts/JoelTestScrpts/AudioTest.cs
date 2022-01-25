using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Loop");
    }

}
