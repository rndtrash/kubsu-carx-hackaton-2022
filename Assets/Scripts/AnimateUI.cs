using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateUI : MonoBehaviour
{
    [System.Serializable]
    public struct Animation
    {
        public float Time;
        public Sprite[] Sprites;
    }

    public Animation[] Animations;

    private int _animation = -1;
    private Image _sprite;
    private DateTime _start;

    private void Start()
    {
        _sprite = GetComponent<Image>();
        _start = DateTime.UtcNow;
    }

    public void SetAnimation(int index)
    {
        if (index >= Animations.Length)
            return;

        _animation = index;
        _start = DateTime.UtcNow;
    }

    private void Update()
    {
        if (_animation == -1)
            return;

        _sprite.sprite = Animations[_animation]
            .Sprites[
                (int)((DateTime.UtcNow - _start).TotalSeconds / Animations[_animation].Sprites.Length *
                      Animations[_animation].Time) % Animations[_animation].Sprites.Length];
    }
}
