using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhysicsMaterial2D slipMaterial;
    [SerializeField] private PhysicsMaterial2D frictionMaterial;
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float additionalGravity;
    private float input;
    private bool hasDoubleJump = true;
    private bool isOnGround = false;
    private bool wasOnGround = false;
    private float placeholder;
    float angle;
    #endregion

    private void Start()
    {
        EventManager.instance.OnGroundedStateChanged += GroundedStateChanged;
        EventManager.instance.OnAngleChanged += ChangeAngle;
    }
    private void FixedUpdate()
    {
        if (isOnGround)
        {
            if (!wasOnGround)
            {
                hasDoubleJump = true;
            }
            if(input == 0)
            {
                rb.sharedMaterial = frictionMaterial;
            }
        }
        else
        {
            if(wasOnGround && additionalGravity == 0f)
            {
                additionalGravity = -1f;
            }
            rb.velocity += new Vector2(0, additionalGravity);
        }

        Accelerate();

        placeholder = 0f;
        float target = transform.eulerAngles.z;
        target = (target > 180) ? target - 360 : target;

        target = Mathf.SmoothDamp(target, angle, ref placeholder, 0.032f);
        transform.eulerAngles = new Vector3(0f, 0f, target);


        wasOnGround = isOnGround;
    }

    void Accelerate()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(transform.right.x * input * speed, rb.velocity.y), acceleration);
    }
    void Jump()
    {
        if(isOnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if(hasDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            hasDoubleJump = false;
        }
    }

    public void AxisRecieved(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            input = value.ReadValue<float>();
            if(input != 0f)
            {
                transform.localScale = input > 0 ? new Vector2(2f,2f) : new Vector2(-2f, 2f);
            }
            rb.sharedMaterial = slipMaterial;
        }
        else if (value.canceled)
        {
            input = 0f;
            
        }
    }

    public void JumpRecieved(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Jump();
            additionalGravity = -0.5f;
            rb.sharedMaterial = slipMaterial;
        }
        else if (value.canceled)
        {
            additionalGravity = -1f;
        }
    }

    void GroundedStateChanged(bool state)
    {
        isOnGround = state;
    }
    void ChangeAngle(float angle)
    {
        this.angle = angle;
    }
}
