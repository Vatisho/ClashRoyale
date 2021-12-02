using UnityEngine;

public abstract class InputHandler : MonoBehaviour
{
    protected float i_axis = 0f;
    protected bool i_atack = false;
    protected bool i_jump = false;
    protected bool i_special_atack = false;
    // public bool fireButton => i_atack;
    // public bool sPButton => i_special_atack;
}