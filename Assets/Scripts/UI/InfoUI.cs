using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour {
    
    public static InfoUI Instance { get; private set; }
    
    [SerializeField]
    [SceneObjectsOnly]
    [Required]
    [Header("Info Panel Container")]
    private GameObject infoUI;
    [SerializeField] private Button infoCloseButton;
    [SerializeField] private TextMeshProUGUI infoTitleText;
    [SerializeField] private TextMeshProUGUI infoDescriptionText;

    private void Awake() {
        Instance = this;
        
        infoCloseButton.onClick.AddListener(Hide);
        
        Hide();
    }

    public void SetInfo(string title, string description) {
        infoTitleText.text = title;
        infoDescriptionText.text = description;
    }

    public void Toggle() {
        if (infoUI.activeSelf) {
            Hide();
        } else {
            Show();
        }
    }
    
    public void Show() {
        infoUI.SetActive(true);
    }
    
    public void Hide() {
        infoUI.SetActive(false);
    }
}
