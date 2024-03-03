using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    #region Variables
    [SerializeField] private Vector3 groundCheckPos; //Position relative to the player which detertmines where the ground check begins (ignore z coordinate)
    [SerializeField] private Vector3 groundCheckSize; //Size of the box in which the ground check will occour
    [SerializeField] private LayerMask groundMask;
    private float slopeAngle;
    private float prevSlopeAngle;
    private float slopeSideAngle;
    private float signedAngle;
    private RaycastHit2D hit;
    private Vector2 slopeNormal = Vector2.up;

    public Vector2 slopeNormalPerp; //A Vector2 that's perpendiclular to the normal of the slope (paralell to the players movement direction)

    private bool isOnSlope;
    private bool isGrounded = false;
    #endregion

    private void Update()
    {
        IsGrounded();
        SlopeCheck();
        prevSlopeAngle = slopeAngle;
    }
    public bool IsGrounded()
    {
        hit = Physics2D.Raycast(transform.position + groundCheckPos, -transform.up, groundCheckSize.y, groundMask);
        if(hit != isGrounded)
        {
            isGrounded = hit;
            EventManager.instance.GroundedChanged(isGrounded);
        }
        return hit;
    }


    private void SlopeCheck()
    {
        SlopeCheckVertical(transform.position + groundCheckPos);
        if (hit)
        {
            slopeNormal = hit.normal;
            slopeNormalPerp = Vector2.Perpendicular(slopeNormal);

            if (slopeAngle != prevSlopeAngle)
            {
                isOnSlope = true;
            }

            slopeAngle = Vector2.Angle(slopeNormal, Vector2.up);
            signedAngle = -Vector2.SignedAngle(slopeNormal, Vector2.up);
        }
        else
        {
            slopeNormalPerp = Vector2.Perpendicular(slopeNormal);
            slopeAngle = 0f;
            signedAngle = 0f;
        }
        GetComponent<IGroundChecker>().ChangeAngle(signedAngle, slopeNormalPerp); //send useful information to the object
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, Vector2.right, groundCheckSize.y, groundMask);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -Vector2.right, groundCheckSize.y, groundMask);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            isOnSlope = false;

            slopeSideAngle = 0f;
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + groundCheckPos, groundCheckSize.y);
    }
}
