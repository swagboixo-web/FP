using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public Animator beetleAnimator;

    void Update()
    {
        // 1. Check if the attack buttons are currently being held down (.isPressed)
        // This is the New Input System equivalent of Brackeys' Input.GetButton()
        bool rightClick = Mouse.current != null && Mouse.current.rightButton.isPressed;
        bool spaceBar = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

        // 2. If EITHER button is held down, flip the Bool to TRUE
        if (rightClick || spaceBar)
        {
            // The beetle will transition to 'roar'
            beetleAnimator.SetBool("IsAttacking", true);
        }
        // 3. If you let go of the buttons, flip the Bool to FALSE
        else
        {
            // The beetle will transition back to 'flying 1' once the combo finishes
            beetleAnimator.SetBool("IsAttacking", false);
        }

      
    }
}