using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController
{
    public static Dictionary<float, string> rotationValues = new Dictionary<float, string>()
        {
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

    public static Dictionary<string, string> animations = new Dictionary<string, string>() 
        {
            {"walk", "topHatWalk"},
            {"idle", "topHatIdle"}
        };

    public static string SpriteAnimationPerspective(float cameraOrbit,
                                                    string animation,
                                                    Animator animator,
                                                    string currentState,
                                                    float speed)
    {
        string newState = currentState;
        float threshold = 22.5f; // 45 / 2
        animator.speed = speed;
        foreach (KeyValuePair<float, string> rotations in rotationValues)
        {
            // Key value pair <angle in degrees at multiples of 45, camera rotation relative to controller object>
            float rotation = rotations.Key;
            string perspective = rotations.Value;

            // Check if camera is in current octant
            bool inOctant = cameraOrbit <= rotation + threshold && cameraOrbit >= rotation - threshold;
            if (inOctant)
            {
                newState = ChangeAnimationState(animation + perspective, currentState, animator);
            }
        }
        return newState;
    }

    public static string ChangeAnimationState(string newState, string currentState, Animator animator)
    {
        // Returns if new animation is the same as current
        if (currentState == newState) return currentState;
        
        // Get normalized animation time
        float time = AnimationTime(animator);

        // Play animation at normalized animation time
        animator.Play(newState, 0, time);

        return newState;
    }

    private static float AnimationTime(Animator animator)
    {
        float rawTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        // Limit time to ranges 0 - 1
        float time = rawTime < 1 ? rawTime : rawTime - (int)rawTime;
        return time;
    }
}