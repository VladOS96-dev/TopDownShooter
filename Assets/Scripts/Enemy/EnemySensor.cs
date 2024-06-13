using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : SensorBase
{
    public const float SIGHT_DIRECT_ANGLE = 120.0f, SIGHT_MIN_DISTANCE = 0.2f, SIGHT_MAX_DISTANCE = 20.0f;
   
    public LayerMask hitTestMask;

    public float TARGET_LOST_COOLDOWN = 1.0f, ALERTED_COOLDOWN = 10.0f, lastTargetTime = float.MinValue, lastAlertTime = float.MinValue;

    public bool alerted = false;
    public bool targetSpotted = false;
    public Color idleColor, alertedColor, attackColor;
    public Material material;
    public MeshRenderer meshModel;
    public ISensor sensor;
    [SerializeField]
    private bool isVisibleSensor = false;
    private Mesh mesh;
    private int quality = 100;
    private float height = 2.0f;
    protected override void StartSensor()
    {
        sensor = new DirectSight();
        material = meshModel.GetComponent<Renderer>().material;
        material.SetColor("_Color", idleColor);
        InitFoV();
    }

    protected override void UpdateSensor()
    {
        sensor.GetTargetInSight(this);
        UpdateFoV();
    }

    public bool TargetInSight(Transform target, float distance)
    {
        Vector3 sightPosition = transform.position;
        sightPosition.y += height;
        RaycastHit hit;
        Vector3 dir = target.position - sightPosition;

        if (Physics.Raycast(sightPosition, dir, out hit, distance, hitTestMask))
        {
            return hit.collider != null && target.gameObject == hit.collider.gameObject;
        }
        return false;
    }

    private void InitFoV()
    {
        mesh = new Mesh();
        mesh.vertices = new Vector3[2 * quality + 2];
        mesh.triangles = new int[3 * 2 * quality];

        Vector3[] normals = new Vector3[2 * quality + 2];
        Vector2[] uv = new Vector2[2 * quality + 2];

        for (int i = 0; i < uv.Length; i++)
            uv[i] = new Vector2(0, 0);
        for (int i = 0; i < normals.Length; i++)
            normals[i] = new Vector3(0, 1, 0);

        mesh.uv = uv;
        mesh.normals = normals;
        transform.Rotate(0.0f, 360 * -0.05f, 0.0f);
    }

    private void UpdateFoV()
    {
        float angleLookAt = GetEnemyAngle();

        float angleStart = angleLookAt - SIGHT_DIRECT_ANGLE;
        float angleEnd = angleLookAt + SIGHT_DIRECT_ANGLE;
        float angleDelta = (angleEnd - angleStart) / quality;

        Vector3 posCurrMin = Vector3.zero;
        Vector3 posCurrMax = Vector3.zero;
        Vector3 posNextMin = Vector3.zero;
        Vector3 posNextMax = Vector3.zero;

        Vector3[] vertices = new Vector3[4 * quality];
        int[] triangles = new int[3 * 2 * quality];
        Vector3 fovPos = transform.position;
        fovPos.y += height;

        float angleCurr = angleStart;
        float angleNext = angleStart + angleDelta;

        for (int i = 0; i < quality; i++)
        {
            Vector3 sphereCurr = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * angleCurr), 0.0f,
                Mathf.Cos(Mathf.Deg2Rad * angleCurr));

            Vector3 sphereNext = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * angleNext), 0.0f,
                Mathf.Cos(Mathf.Deg2Rad * angleNext));

            posCurrMin = fovPos + sphereCurr * SIGHT_MIN_DISTANCE;
            posCurrMax = fovPos + sphereCurr * SIGHT_MAX_DISTANCE;
            posNextMin = fovPos + sphereNext * SIGHT_MIN_DISTANCE;
            posNextMax = fovPos + sphereNext * SIGHT_MAX_DISTANCE;

            int a = 4 * i;
            int b = 4 * i + 1;
            int c = 4 * i + 2;
            int d = 4 * i + 3;

            RaycastHit currRay, nextRay;
            if (Physics.Raycast(posCurrMin, posCurrMax - posCurrMin, out currRay, SIGHT_MAX_DISTANCE - SIGHT_MIN_DISTANCE))
            {
                float dist = Vector3.Distance(currRay.point, posCurrMin) + SIGHT_MIN_DISTANCE;
                posCurrMax = fovPos + sphereCurr * dist;
            }
            if (Physics.Raycast(posNextMin, posNextMax - posNextMin, out nextRay, SIGHT_MAX_DISTANCE - SIGHT_MIN_DISTANCE))
            {
                float dist = Vector3.Distance(nextRay.point, posNextMin) + SIGHT_MIN_DISTANCE;
                posNextMax = fovPos + sphereNext * dist;
            }

            vertices[a] = posCurrMin;
            vertices[b] = posCurrMax;
            vertices[c] = posNextMax;
            vertices[d] = posNextMin;

            triangles[6 * i] = a;
            triangles[6 * i + 1] = b;
            triangles[6 * i + 2] = c;
            triangles[6 * i + 3] = c;
            triangles[6 * i + 4] = d;
            triangles[6 * i + 5] = a;

            angleCurr += angleDelta;
            angleNext += angleDelta;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        if(isVisibleSensor)
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
    }

    private float GetEnemyAngle()
    {
        return 90 - Mathf.Rad2Deg * Mathf.Atan2(transform.forward.z, transform.forward.x);
    }
}