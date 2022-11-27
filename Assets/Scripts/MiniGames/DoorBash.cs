using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBash : MonoBehaviour
{
    public AnimateUI GuyAnimator;

    private Minigame _minigame;
    
    // Start is called before the first frame update
    void Start()
    {
        _minigame = GetComponent<Minigame>();
        GuyAnimator.SetAnimation(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
