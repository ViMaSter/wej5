using UnityEngine;
using System.Collections;

public class DeathConditionHandle : Player.MonoBehaviour
{
    public Transform SphereTransform;
    public Transform CameraTransform;

    public float KillY = -50.0f;

	void Start () {
        base.Start();

        PlayerData.State.CurrentConditionChanged += OnConditionChange;

        SphereTransform = PlayerData.GameObject.transform.Find("Sphere");
        CameraTransform = PlayerData.GameObject.transform.Find("Camera");
    }

    void OnConditionChange(object sender, Player.State.ConditionChangedEventArgs e)
    {
        switch (e.Condition)
        {
            case Player.State.Condition.Alive:
                Spawn();
                break;
            case Player.State.Condition.Dead:
                Die();
                break;
        }
    }

    void Spawn()
    {
        Spawn(new Vector3(0, 5, 0));
    }

    void Spawn(Vector3 spawnPosition)
    {
        SphereTransform.position = spawnPosition;

        SphereTransform.gameObject.SetActive(true);
        CameraTransform.gameObject.SetActive(true);
    }

    void Die()
    {
        SphereTransform.gameObject.SetActive(false);
        CameraTransform.gameObject.SetActive(false);
    }
	
	void Update () {
        if (SphereTransform.position.y < KillY)
        {
            PlayerData.State.CurrentCondition = Player.State.Condition.Dead;
        }

        if (PlayerData.State.CurrentCondition == Player.State.Condition.Dead && Input.GetKeyDown(KeyCode.Return))
        {
            PlayerData.State.CurrentCondition = Player.State.Condition.Alive;
        }
	}
}
