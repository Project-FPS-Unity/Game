using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor.Animations;
using UnityEngine;

public class EliteAgent : Agent
{
    [SerializeField] private RayPerceptionSensorComponent3D frontRay;
    [SerializeField] private RayPerceptionSensorComponent3D sideRay;

    private bool isCombat = false;
    private bool inAttackRange = false;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private EliteArms hitboxLeft;
    [SerializeField] private EliteArms hitboxRight;

    private float moveX;
    private float moveY;
    private float moveZ;
    private float moveSpeed = 8f;
    [SerializeField] private Transform faceDirection;
    private Vector3 moveToDirection;

    [SerializeField] private EliteAnimationController animationState;

    private void Start()
    {
        //playerTarget = GameObject.Find("Player").GetComponent<Transform>();
    }

    public override void OnEpisodeBegin()
    {
        PlayerHealth.health.SetHealth(PlayerHealth.health.GetMaxHealth());
        isCombat = false;
        transform.localPosition = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));
        playerTarget.localPosition = new Vector3(Random.Range(-15f, 15f), 1.5f, Random.Range(-15f, 15f));
        transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation.eulerAngles / 180.0f - Vector3.one);
        if (playerTarget != null)
        {
            sensor.AddObservation(playerTarget.localPosition);
            sensor.AddObservation(Vector3.Distance(transform.localPosition, playerTarget.localPosition));
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveY = actions.ContinuousActions[1];
        moveZ = actions.ContinuousActions[2];

        CheckRay();
        Move(moveZ);
        Turn(moveY, moveSpeed);
        if (isCombat)
        {
            InCombat();
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contact = actionsOut.ContinuousActions;
        contact[0] = Input.GetAxisRaw("Horizontal");
        contact[1] = Input.GetAxisRaw("Mouse X");
        contact[2] = Input.GetAxisRaw("Vertical");
    }

    private void GetReward(int number)
    {
        switch (number)
        {
            case 0: //Walk backward or Struggle aim
                AddReward(-5.0f);
                break;
            case 1: //Hitwall
                AddReward(-30.0f);
                break;
            case 2: //Found Player
                AddReward(+4.0f);
                break;
            case 3: //Run toward Player
                AddReward(+10.0f);
                break;
            case 4: //Attack Hit
                AddReward(+10.0f);
                break;
            case 5: //Player Die
                AddReward(+15.0f);
                break;
            default:
                // code block
                break;
        }
    }

    private void Move(float z)
    {
        animationState.AnimationManager("Walk");
        moveToDirection = transform.forward * z;
        if (z < 0) GetReward(0);
        transform.localPosition += moveToDirection * Time.deltaTime * moveSpeed;
    }

    private void Turn(float y, float speed)
    {
        transform.Rotate(0, y * speed, 0);
    }

    private void Attack()
    {
        animationState.AnimationManager("Attack");
    }

    private void InCombat()
    {
        RaycastHit hit;
        Debug.DrawRay(faceDirection.position, faceDirection.forward * 4f, Color.green);
        if (Physics.Raycast(faceDirection.position, faceDirection.forward, out hit, 4f))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                if (PlayerHealth.health.GetCurrentHealth() <= 0)
                {
                    GetReward(4);
                    EndEpisode();
                }
                Attack();
            }

        }
    }

    private void CheckRay()
    {
        //Front vision
        RayPerceptionOutput frontOut = RayPerceptionSensor.Perceive(frontRay.GetRayPerceptionInput());
        int rayFrontLength = frontOut.RayOutputs.Length;
        bool playerFound_F = false;
        bool foundObstacle_F = false;
        for (int i = 0; i < rayFrontLength; i++)
        {
            GameObject goHit = frontOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var EnemyDirection = frontOut.RayOutputs[i].EndPositionWorld - frontOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = EnemyDirection.magnitude;
                float rayHitDistance = frontOut.RayOutputs[i].HitFraction * scaledRayLength;
                if (goHit.gameObject.tag == "Untagged" && rayHitDistance <= 20.0f)
                {
                    foundObstacle_F = true;
                }
                if (goHit.gameObject.tag == "Player")
                {
                    playerFound_F = true;
                }
                if (goHit.gameObject.tag == "Player" && rayHitDistance >= 6f) animationState.AnimationManager("StopAttack");
            }
        }

        //Side hit box
        RayPerceptionOutput sideOut = RayPerceptionSensor.Perceive(sideRay.GetRayPerceptionInput());
        int raySideLength = sideOut.RayOutputs.Length;
        bool playerFound_S = false;
        bool foundObstacle_S = false;
        for (int i = 0; i < raySideLength; i++)
        {
            GameObject goHit = sideOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var rayDirection = sideOut.RayOutputs[i].EndPositionWorld - sideOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = rayDirection.magnitude;
                float rayHitDistance = sideOut.RayOutputs[i].HitFraction * scaledRayLength;

                if (goHit.gameObject.tag == "Untagged")
                {
                    foundObstacle_S = true;
                }
                if (goHit.gameObject.tag == "Player")
                {
                    playerFound_S = true;
                }
            }
        }
        if (foundObstacle_F && foundObstacle_S)
        {
            GetReward(1);
            EndEpisode();
        }
        if (playerFound_F && playerFound_S)
        {
            GetReward(2);
        }
        if (playerFound_F)
        {
            isCombat = true;
            animationState.AnimationManager("PlayerFound");
        }
    }
}
