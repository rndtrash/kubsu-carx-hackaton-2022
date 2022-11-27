using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorkManager : MonoBehaviour
{
    public GameObject Wristwatch = null;
    
    void Start()
    {
        if (PoliceManager.FoundWristwatch && Wristwatch != null)
            Wristwatch.SetActive(true);
    }

    public void NextShift()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
