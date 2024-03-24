using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTextHandler : MonoBehaviour
{
    [SerializeField] GameManager GM;
    TMPro.TextMeshProUGUI ScoreText;

    private void Start()
    {
        ScoreText = GetComponent<TMPro.TextMeshProUGUI>();
    }
    private void Update()
    {
        ScoreText.text =  GM.Score.ToString();
    }
}
