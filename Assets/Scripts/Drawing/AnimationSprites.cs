using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprites", menuName = "Animation/Sprite List", order = 1)]
public class AnimationSprites : ScriptableObject
{
    public List<Sprite> animationSprites;
    public DrawableAnimations animation;
    public string dropDownName;
    public Sprite dropDownSprite;
    public Sprite resetSprite;
}
