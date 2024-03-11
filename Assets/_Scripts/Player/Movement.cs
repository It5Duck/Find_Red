using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour, IGroundChecker
{
    #region Variables
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhysicsMaterial2D slipMaterial;
    [SerializeField] private PhysicsMaterial2D frictionMaterial;
    [SerializeField] private ConstantForce2D cf;
    [SerializeField] private Animator animator;
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
    Vector2 groundDir;
    #endregion

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        EventManager.instance.OnGroundedStateChanged += GroundedStateChanged;
    }
    private void FixedUpdate()
    {
        if (isOnGround)
        {
            if (!wasOnGround)
            {
                hasDoubleJump = true;
                if(input == 0)
                {
                    animator.Play("PlayerIdle");
                }
                else
                {
                    animator.Play("PlayerWalk");
                }
            }
            if(input == 0)
            {
                rb.sharedMaterial = frictionMaterial;
            }
        }
        else
        {
            if(wasOnGround)
            {
                if(additionalGravity == 0f)
                {
                    additionalGravity = -1f;
                }

                animator.Play("PlayerJump");
            }
            Vector2 g = -Vector2.ClampMagnitude(cf.force, 1f);
            rb.velocity += g * additionalGravity;
            angle = Vector2.SignedAngle(new Vector2(-Mathf.RoundToInt(g.x), Mathf.RoundToInt(g.y)), Vector2.up);
        }

        Accelerate();
        RotateInGroundDir();

        wasOnGround = isOnGround;
    }

    void Accelerate()
    {
        if (isOnGround)
        {
            if (cf.force.y > 0f)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(transform.right.x * input * speed, rb.velocity.y), acceleration);
            }
            else
            {
                rb.velocity = Vector2.Lerp(rb.velocity, -groundDir * speed * input, acceleration);
            }
        }
        else
        {
            if(Mathf.Abs(cf.force.y) > 0f)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(transform.right.x * input * speed, rb.velocity.y), acceleration);
            }
            else
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, transform.right.y * input * speed), acceleration);
            }
        }
    }
    void Jump()
    {
        if (Mathf.Abs(cf.force.y) > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(-cf.force.y, -1f, 1f) * jumpForce);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Clamp(-cf.force.x, -1f, 1f) * jumpForce, rb.velocity.y);
        }
    }

    void RotateInGroundDir()
    {
        placeholder = 0f;
        float target = transform.eulerAngles.z;
        if (cf.force.y <= 0f)
        {
            target = (target > 180) ? target - 360 : target;
        }
        target = Mathf.SmoothDamp(target, angle, ref placeholder, 0.032f);
        transform.eulerAngles = new Vector3(0f, 0f, target);
    }

    public void AxisRecieved(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            if (isOnGround)
            {
                animator.Play("PlayerWalk");
            }
            input = value.ReadValue<float>();
            if(input != 0f)
            {
                transform.localScale = input > 0 ? new Vector2(1f,1f) : new Vector2(-1f, 1f);
            }
            rb.sharedMaterial = slipMaterial;
        }
        else if (value.canceled)
        {
            if (isOnGround)
            {
                animator.Play("PlayerIdle");
            }
            input = 0f;
        }
    }

    public void JumpRecieved(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (isOnGround)
            {
                isOnGround = false;
                Jump();
            }
            else if (hasDoubleJump)
            {
                Jump();
                hasDoubleJump = false;
            }
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
    public void ChangeAngle(float angle, Vector2 dir)
    {
        this.angle = angle;
        groundDir = dir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.right);
    }
}
