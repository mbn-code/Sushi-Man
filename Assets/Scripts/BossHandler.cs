using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossHandler : MonoBehaviour
{
    [SerializeField] private GameObject EZ;

    [SerializeField] private PlayerController Ply;
    private Transform PlayerTransform;
    [SerializeField] private GameManager GM;
    [SerializeField] private CameraShake CamShaker;

    [SerializeField] private Transform DefaultPos;

    [SerializeField] private Transform[] DashPoints;
    [SerializeField] internal float DashSpeed = 70;

    [SerializeField]
    private GameObject[] ItemDrops;

    private int HP = 3;

    // Dash Attack
    private int CurrentPointIndex = 0;
    private bool IsDoingDashAttack = false;
    private bool Resetting = false;
    private bool DoSpinningAttack = false;

    [SerializeField] private float speed = 10f;
    private bool ReadyToSpin = false;

    private Vector3 CurrentPlayerTransform = Vector3.zero;

    [SerializeField] Sprite KillSprite;

    private bool isDone = false;
    private bool IWon = false;

    private void Start()
    {
        EZ.SetActive(false);
        gameObject.transform.position = new Vector3(1000, 1000, 0);
        PlayerTransform = Ply.GetComponentInParent<Transform>();
        StartCoroutine(SpawnBoss());
    }


    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(2f);
        CamShaker.Shake(2f);
        yield return new WaitForSeconds(1f);
        gameObject.transform.position = DefaultPos.position;
        StartCoroutine(BossManager());
    }

    private bool ReadyToDash = false;

    IEnumerator BossManager()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            if (!IsDoingDashAttack && !DoSpinningAttack && !Resetting && !isDone)
            {
                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    IsDoingDashAttack = true;
                }
                else
                {
                    DoSpinningAttack = true;
                }
            }

            if(isDone && !Resetting && IWon)
            {
                EZ.SetActive(true);
                break;
            }
        }
    }

    private void Update()
    {
        if(IsDoingDashAttack)
        {
            if(CurrentPointIndex == 0 && !ReadyToDash)
            {
                transform.position = Vector2.MoveTowards(transform.position, DashPoints[0].position, DashSpeed/5f * Time.deltaTime);

                if(Vector3.Distance(transform.position, DashPoints[0].position) < 0.1f)
                {
                    ReadyToDash = true;
                }
            }

            if (CurrentPointIndex <= DashPoints.Length - 1 && ReadyToDash)
            {
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, DashPoints[CurrentPointIndex].position, DashSpeed * Time.deltaTime);

                if(CurrentPointIndex == 1 && Vector3.Distance(transform.position, DashPoints[CurrentPointIndex].position) < 0.1f)
                {
                    IsDoingDashAttack = false;
                    ReadyToDash = false;
                    CurrentPointIndex = 0;
                    Resetting = true;
                }

                if (Vector3.Distance(transform.position, DashPoints[CurrentPointIndex].position) < 0.1f)
                {
                    CurrentPointIndex++;
                }
            }
        }

        if(DoSpinningAttack)
        {
            if(CurrentPlayerTransform == Vector3.zero)
            {
                CurrentPlayerTransform = PlayerTransform.position;
            }

            Vector2 StartPoint;

            if(CurrentPlayerTransform.x > 10f && !ReadyToSpin)
            {
                StartPoint = new Vector2(CurrentPlayerTransform.x - 10f, CurrentPlayerTransform.y);
            } else if(CurrentPlayerTransform.x < -10f)
            {
                StartPoint = new Vector2(CurrentPlayerTransform.x + 10f, CurrentPlayerTransform.y);
            } else
            {
                StartPoint = new Vector2(CurrentPlayerTransform.x, CurrentPlayerTransform.y + 10f);
            }

            if(Vector3.Distance(transform.position, StartPoint) > 0.1f && !ReadyToSpin)
            {
                gameObject.transform.position = Vector2.MoveTowards(transform.position, StartPoint, DashSpeed / 5f * Time.deltaTime);
            } else
            {
                ReadyToSpin = true;
                StartCoroutine(ChargeUpSpin());
            }

            if(ReadyToSpin)
            {
                Vector3 direction = CurrentPlayerTransform - transform.position;
                direction = Quaternion.Euler(0, 0, 85) * direction;
                float distanceThisFrame = speed * Time.deltaTime;

                transform.Translate(direction.normalized * distanceThisFrame, Space.World);

                if (Vector3.Distance(transform.position, CurrentPlayerTransform) < 1.5f)
                {
                    DoSpinningAttack = false;
                    ReadyToSpin = false;
                    CurrentPlayerTransform = Vector3.zero;
                    Resetting = true;
                }
            }
        }

        IEnumerator ChargeUpSpin()
        {
            speed = 20f;
            yield return new WaitForSeconds(1f);
            speed = 40f;
            yield return new WaitForSeconds(1f);
            speed = 60f;

        }

        if(Resetting)
        {
            if (Vector3.Distance(transform.position, DefaultPos.position) > 0.1f)
            {
                gameObject.transform.position = Vector2.MoveTowards(transform.position, DefaultPos.position, DashSpeed / 5f * Time.deltaTime);
            }
            else
            {
                Resetting = false;
            }
        }
    }

    internal void ApplyDamage()
    {
        HP -= 1;

        if (HP == 0)
        {
            isDone = true;
            DoSpinningAttack = false;
            IsDoingDashAttack = false;
            Resetting = false;
            ReadyToDash = false;
            CurrentPointIndex = 0;

            StartCoroutine("EndGame");
        }
    }

    IEnumerator EndGame()
    {
        CamShaker.Shake(2f);
        yield return new WaitForSeconds(2f);

        gameObject.GetComponent<SpriteRenderer>().sprite = KillSprite;

        for (int i = 0; i < 100; i++)
        {
            int randomIndex = Random.Range(0, ItemDrops.Length);
            GameObject itemDrop = ItemDrops[randomIndex];
            Instantiate(itemDrop, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<ItemDrop>().DropForce = 10f;
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);

        yield return new WaitForSeconds(10f);

        GM.Restart();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().KillMe();
            IWon = true;
            DoSpinningAttack = false;
            IsDoingDashAttack = false;
            ReadyToDash = false;
            CurrentPointIndex = 0;
            Resetting = true;
            isDone = true;
        }
    }
}
