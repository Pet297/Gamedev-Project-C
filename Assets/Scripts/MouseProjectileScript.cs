using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseProjectileScript : MonoBehaviour
{
    public float WearOffStart = 2f;
    public float WearOffLength = 2f;
    public float Speed = 4f;


    private float CurAngle = 0f;
    private float FollowStrength = 1f;

    private GameObject Player;
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;

        Vector2 toTarget = new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y - transform.position.y);
        float targetAngle = Mathf.Atan2(toTarget.x, toTarget.y);

        float angleDiff1 = CurAngle - targetAngle;
        float angleDiff2 = (6.28f + CurAngle) - targetAngle;

        float angleDiff = 0;
        if (Mathf.Abs(angleDiff1) < Mathf.Abs(angleDiff2)) angleDiff = angleDiff1;
        else angleDiff = angleDiff2;

        angleDiff *= FollowStrength;
        CurAngle -= angleDiff;

        gameObject.transform.Translate(Speed * Time.deltaTime * new Vector3(Mathf.Sin(CurAngle), Mathf.Cos(CurAngle)));

        if (time > WearOffStart + WearOffLength) FollowStrength = 0f;
        else if (time > WearOffStart) FollowStrength = (time - WearOffStart) / WearOffLength;

        if (toTarget.magnitude > 50) gameObject.SetActive(false);
    }

    public void ResetTarget(GameObject go)
    {
        Player = go;
        time = 0;
        FollowStrength = 1f;
    }
}
