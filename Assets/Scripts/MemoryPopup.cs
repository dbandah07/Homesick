using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemoryPopupManager : MonoBehaviour
{
    public static MemoryPopupManager Instance;

    [Header("UI")]
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private TextMeshProUGUI popupText;

    [Header("Timing (seconds)")]
    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float secondsPerLine = 3.5f;

    private Coroutine currentRoutine;
    private StarterAssets.StarterAssetsInputs playerInputs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        playerInputs = FindFirstObjectByType<StarterAssets.StarterAssetsInputs>();

        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.blocksRaycasts = false;
        }
        HideLine();
    }

    public void ShowMemory(MemoryData memory)
    {
        if (memory == null) return;
        Play(memory.openingLine, memory.lines);
    }

    public void ShowMemory(string line)
    {
        Play(line, null);
    }

    private void Play(string openingLine, IList<string> lines)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        currentRoutine = StartCoroutine(MemoryRoutine(openingLine, lines));
    }

    private IEnumerator MemoryRoutine(string openingLine, IList<string> lines)
    {
        Time.timeScale = 0f; // pause
        SetPlayerControls(false);

        // system: 
        // 1. "This reminds me of..." over the room.
        yield return ShowLine(openingLine);

        // 2. Fade to black and play the memory, one line at a time.
        if (lines != null && lines.Count > 0)
        {
            HideLine();
            yield return Fade(0f, 1f);

            foreach (string line in lines)
            {
                yield return ShowLine(line);
            }

            // 3. Fade back to the room.
            HideLine();
            yield return Fade(1f, 0f);
        }

        HideLine();
        SetPlayerControls(true);
        Time.timeScale = 1f; // resume 
        currentRoutine = null;
    }

    private IEnumerator ShowLine(string text)
    {
        popupText.text = text;
        popupText.gameObject.SetActive(true);

        yield return WaitOrSkip(secondsPerLine);
    }

    private void HideLine()
    {
        if (popupText != null) popupText.gameObject.SetActive(false);
    }

    private IEnumerator WaitOrSkip(float seconds)
    {
        yield return null;

        float elapsed = 0f;
        while (elapsed < seconds)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                yield break;
            }

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private IEnumerator Fade(float from, float to)
    {
        if (fadeGroup == null) yield break;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = to;
    }

    private void SetPlayerControls(bool enabled)
    {
        if (playerInputs == null) return;

        playerInputs.cursorInputForLook = enabled;
        playerInputs.look = Vector2.zero;
        playerInputs.move = Vector2.zero;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Time.timeScale = 1f;
        }
    }
}