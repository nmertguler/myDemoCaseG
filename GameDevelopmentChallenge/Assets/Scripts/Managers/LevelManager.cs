using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LevelManager : MonoBehaviour
{
    [Header("Leveller")]
    [SerializeField] private List<LevelHolder> _levelList;

    // bu level numarasi ve sonrasindaki leveller arasinda loop sekilde leveller acilir
    public int levelLoopCount = 1;

    // current level prefab
    private LevelHolder _currentLevel;

    public static int LevelCount
    {
        get => PlayerPrefs.GetInt("LevelCount", 1);
        set => PlayerPrefs.SetInt("LevelCount", value);
    }

    public static int PermenantLevelCount
    {
        get => PlayerPrefs.GetInt("PermenantLevelCount", 1);
        set => PlayerPrefs.SetInt("PermenantLevelCount", value);
    }


    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void LevelLoadingAction()
    {
        EventManager.ActionLevelLoading?.Invoke();
    }

    void LevelLoadedAction()
    {
        EventManager.ActionLevelLoaded?.Invoke();
    }

    void LevelEndAction()
    {
        EventManager.ActionLevelEnded?.Invoke();
    }


    private IEnumerator Start()
    {
        // show loading screen

        SetLevel();

        yield return new WaitUntil(() => _currentLevel != null);

        PreLevelSettings();

        yield return new WaitForSeconds(1);

        LevelLoadedAction();
    }

    private void SetLevel()
    {
        if (LevelHolder.Instance != null)
        {
            Destroy(LevelHolder.Instance.gameObject);
        }

        if (_currentLevel is not null)
        {
            LevelEndAction();
            Destroy(_currentLevel.gameObject);
            _currentLevel = null;
        }
        SpawnLevel();

        LevelLoadingAction();

    }

    private void PreLevelSettings()
    {
        //IsLevelCompleted = false;

        //IsLevelFailed = false;

        //CanvasManager.SetLevelText();

        //SFXManager.Instance.PlayBGSound();
    }

    private void SpawnLevel()
    {
        if (_currentLevel is null)
        {
            if (LevelCount > _levelList.Count)
            {
                LevelCount = levelLoopCount;
            }
            var currentLevel = Instantiate(_levelList[LevelCount - 1]);
            _currentLevel = currentLevel;

        }

        PreLevelSettings();
    }

}
