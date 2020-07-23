using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    //Config Params
    WaveConfig waveConfig;
    float moveSpeed = 0f;
    List<Transform> wayPoints = default;
    int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = waveConfig.GetMoveSpeed();
        wayPoints = waveConfig.GetWaypoints();
        transform.position = wayPoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();

    }

    public void SetWaveConfig(WaveConfig wc)
    {
        waveConfig = wc;
    }

    //Handles enemy movement between waypoints.
    private void EnemyMovement()
    {
        if (waypointIndex <= wayPoints.Count - 1)
        {
            var targetPosition = wayPoints[waypointIndex].transform.position;
            var frameMovement = moveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position,
                targetPosition,
                frameMovement);
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);

        }
    }
}
