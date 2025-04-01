using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class GameManager18 : Singleton<GameManager18>
{
    public static int level;
    [SerializeField] private TextMeshProUGUI lvText;
    [SerializeField] private GameObject nextBtn_win;
    [SerializeField] private GameObject winMenu, loseMenu, pauseMenu;
    [SerializeField] private RectTransform winPanel, losePanel, pausePanel;
    [SerializeField] private float topPosY = 250f, middlePosY, tweenDuration = 0.3f;
    private int maxLV = 15;

    [Header("Grid")]
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject[] gridPrefabs;
    [SerializeField] private Transform[] cameraPos;

    protected override void Awake()
    {
        base.Awake();
        level = PlayerPrefs.GetInt("CurrentLevel", 1);
        LoadLevel(level);
    }

    async void Start()
    {
        await HidePanel(winMenu, winPanel);
        await HidePanel(loseMenu, losePanel);
        await HidePanel(pauseMenu, pausePanel);
    }

    private void LoadLevel(int levelIndex)
    {
        if (levelIndex < 1 || levelIndex > maxLV) levelIndex = 1;

        maxLV = gridPrefabs.Length;
        if (levelIndex == maxLV && nextBtn_win) nextBtn_win.SetActive(false);

        PlayerPrefs.SetInt("CurrentLevel", levelIndex);

        if (lvText) lvText.text = "LEVEL " + levelIndex.ToString("00");

        if (gridPrefabs.Length > 0) CreateGrid(levelIndex);
    }

    private void CreateGrid(int levelIndex)
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        if (gridPrefabs[levelIndex - 1] != null)
            Instantiate(gridPrefabs[levelIndex - 1], gridParent);

        Camera.main.transform.rotation = Quaternion.Euler(60, 0, 0);
        if (cameraPos[levelIndex - 1] != null)
            Camera.main.transform.position = cameraPos[levelIndex - 1].transform.position;
    }

    public void Retry() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void NextLV() => SetCurrentLV(level + 1);

    public void UnlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (level >= unlockedLevel && level < maxLV)
            PlayerPrefs.SetInt("UnlockedLevel", level + 1);
    }

    public void SetCurrentLV(int levelIndex)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene("1");
        //SceneManager.LoadScene(levelIndex.ToString());
    }

    public void PauseGame() => OpenMenu(pauseMenu, pausePanel, 1);

    public async void ResumeGame()
    {
        SoundManager18.Instance.SoundClick();
        await HidePanel(pauseMenu, pausePanel);
        Time.timeScale = 1f;
    }

    public void GameWin()
    {
        UnlockNextLevel();
        OpenMenu(winMenu, winPanel, 2);
    }

    public void GameLose() => OpenMenu(loseMenu, losePanel, 3);

    private void OpenMenu(GameObject menu, RectTransform panel, int soundIndex)
    {
        SoundManager18.Instance.PlaySound(soundIndex);
        ShowPanel(menu, panel);
    }

    private void ShowPanel(GameObject menu, RectTransform panel)
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        menu.GetComponent<CanvasGroup>().DOFade(1, tweenDuration).SetUpdate(true);
        panel.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    private async Task HidePanel(GameObject menu, RectTransform panel)
    {
        if (menu == null || panel == null) return;

        menu.GetComponent<CanvasGroup>().DOFade(0, tweenDuration).SetUpdate(true);
        await panel.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        if (menu) menu.SetActive(false);
    }

    public void HackGame()=>GameWin();
}
