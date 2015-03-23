using UnityEngine;
using System.Collections;

public class SphereMovement : Player.MonoBehaviour, Core.IResetable
{
    #region References
    [HideInInspector]
    private Transform CameraTransform;
    [HideInInspector]
    new public Rigidbody rigidbody;
    [HideInInspector]
    public SphereCollider SphereCollider;
    #endregion

    #region Members
    private bool IsInit = false;

    public float MovementSpeed = 5.0f;
    public float JumpHeight = 5.0f;
    public int AllowedJumpCount = 2;
    public int CurrentJumpCount = 0;
    public Vector3 MaximumUpVelocity = new Vector3(7.5f, 5000.0f, 7.5f);
    public Vector3 MaximumDownVelocity = new Vector3(-7.5f, -150.0f, -7.5f);
    public Vector3 VelocityUpModifier = new Vector3(0.99f, 0.99f, 0.99f);
    public Vector3 VelocityDownModifier = new Vector3(0.99f, 0.99f, 0.99f);

    #region Movement directions
    public Vector3 Forward
    {
        get
        {
            Vector3 cameraDirection = CameraTransform.forward;
            cameraDirection.y = 0;
            return cameraDirection.normalized;
        }
    }
    public Vector3 Right
    {
        get
        {
            Vector3 cameraDirection = CameraTransform.right;
            cameraDirection.y = 0;
            return cameraDirection.normalized;
        }
    }
    public Vector3 Up
    {
        get
        {
            Vector3 cameraDirection = CameraTransform.up;
            return cameraDirection.normalized;
        }
    }
    #endregion
    #endregion

    #region IResetable
    public void Init()
    {
        if (IsInit)
        {
            return;
        }

        CameraTransform = PlayerData.GameObject.transform.Find("Camera");
        rigidbody = GetComponent<Rigidbody>();
        SphereCollider = (SphereCollider)GetComponent<Collider>();
        IsInit = true;
    }

    public void Reset()
    {
        Init();

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
    #endregion

    void OnEnable()
    {
        if (!IsInit)
        {
            return;
        }

        Reset();
    }

    void Start () {
        base.Start();

        Init();
        Reset();
	}
	
    void Update()
    {
        // TODO: Experiment with AddForce(Forward) being "b" instead of "a"
        // a: http://i.vimaster.de/2015-03-20_17-32-36.png
        // b: http://i.vimaster.de/2015-03-20_17-31-38.png
        
        if (Input.GetAxisRaw("Movement_Vertical") != 0.0f)
        {
            rigidbody.AddForce(Forward * MovementSpeed * Input.GetAxisRaw("Movement_Vertical") * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Movement_Horizontal") != 0.0f)
        {
            rigidbody.AddForce(Right * MovementSpeed * Input.GetAxisRaw("Movement_Horizontal") * Time.deltaTime);
        }

        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, Vector3.down, out raycastHit, SphereCollider.radius + 0.02f) && rigidbody.velocity.y <= 0.0f)
        {
            CurrentJumpCount = 0;
        }

        if (Input.GetButtonDown("Movement_Jump") && CurrentJumpCount < AllowedJumpCount)
        {
            rigidbody.AddForce(Up * JumpHeight);
            CurrentJumpCount++;
        }

        rigidbody.velocity = Vector3.Max(Vector3.Min(new Vector3(
             rigidbody.velocity.x * (rigidbody.velocity.x > 0 ? VelocityUpModifier.x : VelocityDownModifier.x),
             rigidbody.velocity.y * (rigidbody.velocity.y > 0 ? VelocityUpModifier.y : VelocityDownModifier.y),
             rigidbody.velocity.z * (rigidbody.velocity.z > 0 ? VelocityUpModifier.z : VelocityDownModifier.z)
        ), MaximumUpVelocity), MaximumDownVelocity);
	}

    #region Debug
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Forward * 10);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Right * 10);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Up * 10);
    }
    #endregion
}
