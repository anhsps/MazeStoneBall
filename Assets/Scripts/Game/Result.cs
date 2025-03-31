using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Result : MonoBehaviour
{
    private float timer;
    private int star;
    private Image[] starImgs;
    [SerializeField] private Image star1, star2, star3;
    [SerializeField] private TextMeshProUGUI starTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;

    // Start is called before the first frame update
    void Start()
    {
        UpdateStar(0);
        starImgs = new Image[] { star1, star2, star3 };
        HideStars();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        ShowTime();
    }

    public void IncreaseStar(int point)
    {
        UpdateStar(point + star);
    }

    private void UpdateStar(int star)
    {
        this.star = star;
    }

    private void HideStars()
    {
        foreach (var item in starImgs)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ShowStar()
    {
        if (star == 0) HideStars();
        for (int i = 0; i < star; i++)
        {
            if (i < starImgs.Length)
            {
                starImgs[i].gameObject.SetActive(true);
            }
        }
        starTxt.text = star + "/3";
    }

    private void ShowTime()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timeTxt.text = minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
