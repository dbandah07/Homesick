using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
 
public class PackingCrate : MonoBehaviour
{
    [SerializeField] private int objectsToPack = 5;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private float endScreenDelay = 1.5f;
 
    private int packedCount;
 
    private void Start()
    {
        UpdateCounter();
 
        if (endScreen != null)
        {
            endScreen.SetActive(false);
        }
    }
 
    private void OnTriggerStay(Collider other)
    {
        ObjectGrab item = other.GetComponentInParent<ObjectGrab>();
 
        if (item == null || item.IsHeld || item.IsPacked) return;
 
        item.MarkPacked();
        packedCount++;
        UpdateCounter();
 
        if (packedCount >= objectsToPack)
        {
            Invoke(nameof(ShowEndScreen), endScreenDelay);
        }
    }
 
    private void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = $"Packed: {packedCount}/{objectsToPack}";
        }
    }
 
    private void ShowEndScreen()
    {
        if (endScreen != null)
        {
            endScreen.SetActive(true);
        }
 
        Time.timeScale = 0f;
 
        StarterAssets.StarterAssetsInputs inputs = FindFirstObjectByType<StarterAssets.StarterAssetsInputs>();
        if (inputs != null)
        {
            inputs.cursorLocked = false;
            inputs.cursorInputForLook = false;
        }
 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
 
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

