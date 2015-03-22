using UnityEngine;
using System.Collections;

public class CameraMovement : Player.MonoBehaviour, Core.IResetable
{
    #region References
    [HideInInspector]
    private Transform SphereTransform;
    [HideInInspector]
    new private Rigidbody rigidbody;
    #endregion

    #region Members
    private bool IsInit = false;

    public float RotationSpeed = 15.0f;
    public float BoomLength = 6.0f;
    private Vector3 CameraOffset;
    private Vector3 DirectionFromPlayer;
    private Vector3 LazySphereTransformPosition;
    #endregion

    #region IResetable
    public void Init()
    {
        if (IsInit)
        {
            return;
        }

        SphereTransform = PlayerData.GameObject.transform.Find("Sphere");
        rigidbody = SphereTransform.GetComponent<Rigidbody>();

        CameraOffset = transform.localPosition;

        IsInit = true;
    }

    public void Reset()
    {
        Init();

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        LazySphereTransformPosition = SphereTransform.position;
    }
    #endregion

	// Use this for initialization
    void Start()
    {
        base.Start();

        Init();
	}

    void OnEnable()
    {
        if (!IsInit)
        {
            return;
        }

        Reset();
    }
	
	// Update is called once per frame
	void Update () {
        // TODO: Implement buffer-collider to stop camera from bumping into things
        UpdateValues();
        CalculateLazyPosition();
        CalculateDirectPosition();
    }

    public float LerpSpeed = 0.2f;

    void UpdateValues()
    {
        BoomLength += Input.GetAxisRaw("Camera_Zoom");
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LerpSpeed += 0.025f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            LerpSpeed -= 0.025f;
        }
    }

    void CalculateDirectPosition()
    {
        transform.position = LazySphereTransformPosition;

        DirectionFromPlayer += new Vector3(
            -Input.GetAxisRaw("Camera_Vertical") * RotationSpeed * Time.deltaTime,
            Input.GetAxisRaw("Camera_Horizontal") * RotationSpeed * Time.deltaTime,
            0
        );

        DirectionFromPlayer = new Vector3(
            Mathf.Max(Mathf.Min(DirectionFromPlayer.x, 89.9f), 0),
            (DirectionFromPlayer.y + 360) % 360,
            (DirectionFromPlayer.z + 360) % 360
        );

        transform.eulerAngles = DirectionFromPlayer;

        transform.position += transform.forward * -BoomLength;
        transform.position += transform.TransformDirection(CameraOffset);
    }

    void CalculateLazyPosition()
    {
        LazySphereTransformPosition = Vector3.Slerp(LazySphereTransformPosition, SphereTransform.position, LerpSpeed);
    }
}
