using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] PlayerController Ply;
    [SerializeField] Transform Target;
    [SerializeField] GameManager GM;
    [SerializeField] GameObject ShopUI;
    [SerializeField] GameObject Cross;

    private void Start()
    {
        ShopUI.SetActive(false);
        Cross.SetActive(false);
    }

    private List<Coroutine> CurrentCrossRoutine = new List<Coroutine>();

    private void Update()
    {
        if(Vector3.Distance(transform.position, Target.position) < 3f && !Ply.DashingUnlocked)
        {
            ShopUI.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E))
            {
                if(GM.Score >= 100)
                {
                    PlayerPrefs.SetInt("HasDash", 1);
                    PlayerPrefs.Save();
                    GM.Score -= 100;
                    Ply.DashingUnlocked = true;
                } else
                {
                    CurrentCrossRoutine.ForEach(x => StopCoroutine(x));
                    CurrentCrossRoutine.Add(StartCoroutine("ShowCross"));
                }
            }


        } else
        {
            if (ShopUI.activeSelf)
            {
                ShopUI.SetActive(false);
                CurrentCrossRoutine.ForEach(x => StopCoroutine(x));
                Cross.SetActive(false);
            }
        }
    }

    private IEnumerator ShowCross()
    {
        Cross.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Cross.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Cross.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Cross.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Cross.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Cross.SetActive(false);
    }
}
