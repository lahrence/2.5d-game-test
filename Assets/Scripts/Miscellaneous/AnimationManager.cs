using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

    public static Dictionary<float, string> rotationValues =
        new Dictionary<float, string>() {
            { 0f, "Back"},
            { 45f, "BackRight"},
            { 90f, "Right"},
            { 135f, "FrontRight"},
            { 180f, "Front"},
            { 225f, "FrontLeft"},
            { 270f, "Left"},
            { 315f, "BackLeft"},
            { 360f, "Back"}
        };

    public static Dictionary<string, Dictionary<string, string>> animations =
        new Dictionary<string, Dictionary<string, string>>() {
            {"topHat",
                new Dictionary<string, string>() {
                    {"walk", "topHatWalk"},
                    {"idle", "topHatIdle"},
                    {"pickup", "topHatPickUp"}
                }
            }
        };

    Animator animator;
    const float Threshold = 22.5f;
    [SerializeField] GameObject spriteObject;
    PlayerController playerController;
    PlayerSpriteController sprite;
    float angle;
    float speedModifier;
    string currentState;

    void Awake() {
        animator = GetComponent<Animator>();
        playerController = spriteObject.GetComponent<PlayerController>();
        sprite = GetComponent<PlayerSpriteController>();
        sprite.OnAngleChange += PlayerSpriteController_OnAngleChange;
    }

    void Update() {
        speedModifier = playerController.speedModifier;
    }

    public string PlayAnimation(string animation, string spriteName) {
        string newState = currentState;
        animator.speed = playerController.speedModifier;
        animation = animations[spriteName][animation];
        foreach (KeyValuePair<float, string> rotations in rotationValues) {
            // Key value pair <angle in degrees at multiples of 45,
            // camera rotation relative to controller object>
            float rotation = rotations.Key;
            string perspective = rotations.Value;
            // Check if camera is in current octant
            bool inOctant = angle <= rotation + Threshold &&
                            angle >= rotation - Threshold;
            if (inOctant) {
                newState = ChangeAnimationState(animation + perspective,
                                                currentState,
                                                animator);
            }
        }
        return newState;
    }

    string ChangeAnimationState(string newState,
                                string currentState,
                                Animator animator) {
        // Returns if new animation is the same as current
        if (currentState == newState) {
            return currentState;
        }
        float time = AnimationTime(animator);
        // Play animation at normalized animation time
        animator.Play(newState, 0, time);
        return newState;
    }

    float AnimationTime(Animator animator) {
        float rawTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float time = rawTime < 1 ? rawTime : rawTime - (int)rawTime;
        return time;
    }

    private void PlayerSpriteController_OnAngleChange(object sender, PlayerSpriteController.OnAngleChangeEventArgs e) {
        angle = e.angle;
        currentState = e.currentState;
    }
}
