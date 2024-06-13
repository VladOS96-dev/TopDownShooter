using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectSight : ISensor
{
    public void GetTargetInSight(EnemySensor sensor)
    {
        Collider[] overlapedObjects = Physics.OverlapSphere(sensor.transform.position, EnemySensor.SIGHT_MAX_DISTANCE);

        bool targetFound = false;

        foreach (Collider obj in overlapedObjects)
        {
            Vector3 direction = obj.transform.position - sensor.transform.position;
            float objAngle = Vector3.Angle(direction, sensor.transform.forward);

            if (obj.CompareTag("Player"))
            {

                if (objAngle < EnemySensor.SIGHT_DIRECT_ANGLE && sensor.TargetInSight(obj.transform, EnemySensor.SIGHT_MAX_DISTANCE))
                {
                    sensor.npcBase.SetTargetPos(obj.transform.position);
                    sensor.material.SetColor("_Color", sensor.attackColor);
                    sensor.targetSpotted = true;
                    sensor.lastTargetTime = Time.time;
                    sensor.alerted = true;
                    sensor.lastAlertTime = Time.time;
                    targetFound = true;
                    break; // Exit the loop if the player is found
                }
            }
        }

        if (!targetFound && sensor.targetSpotted && Time.time > sensor.lastTargetTime + sensor.TARGET_LOST_COOLDOWN)
        {
            sensor.material.SetColor("_Color", sensor.alerted ? sensor.alertedColor : sensor.idleColor);
            sensor.targetSpotted = false;
        }

        if (sensor.alerted && Time.time > sensor.lastAlertTime + sensor.ALERTED_COOLDOWN)
        {
            sensor.material.SetColor("_Color", sensor.idleColor);
            sensor.alerted = false;
        }
    }
}