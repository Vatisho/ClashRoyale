using UnityEngine;
using UnityEngine.UI;

public class UiConnectionManager : MonoBehaviour
{
    [SerializeField] Text serverStatus;
    [SerializeField] Text hostStatus;
    [SerializeField] GameObject findMatchButton;
    [SerializeField] GameObject CancelMatchButton;
    [SerializeField] GameObject findingMatchText;
    [SerializeField] InputField nameInputField;
    [SerializeField] Text selected1;
    [SerializeField] Text selected2;
    [SerializeField] Text selected3;
    [SerializeField] Text selected4;
    [SerializeField] Text selected5;
    [SerializeField] Text selected6;
    [SerializeField] Text selected7;
    [SerializeField] Text selected8;

    [Header("In Game Panel")]
    [SerializeField] GameObject inGameUIPanel;
    [SerializeField] GameObject pickerPanel;
    GameConnectionManager gameConnectionManager;

    private void Awake()
    {
        ResetPickerUI();
        pickerPanel.SetActive(false);
        gameConnectionManager = FindObjectOfType<GameConnectionManager>();
        serverStatus.gameObject.SetActive(true);
        findMatchButton.SetActive(true);
        CancelMatchButton.SetActive(false);
        findingMatchText.SetActive(false);
        inGameUIPanel.SetActive(true);
        hostStatus.gameObject.SetActive(false);

    }

    public void ResetUI()
    {
        pickerPanel.SetActive(false);
        serverStatus.gameObject.SetActive(true);
        findMatchButton.SetActive(true);
        CancelMatchButton.SetActive(false);
        findingMatchText.SetActive(false);
        inGameUIPanel.SetActive(true);
        // FindObjectOfType<MeetingsManager>().ResetMeeting();
    }
    public void FindMatch()
    {
        findMatchButton.SetActive(false);
        CancelMatchButton.SetActive(true);
        findingMatchText.SetActive(true);
    }
    public void CancelMatch()
    {
        findMatchButton.SetActive(true);
        CancelMatchButton.SetActive(false);
        findingMatchText.SetActive(false);
    }

    public void ActivateMatchFound()
    {
        CancelMatchButton.SetActive(false);
        findingMatchText.SetActive(false);
        inGameUIPanel.SetActive(true);
    }
    public void StartGame()
    {
        ResetUI();
        inGameUIPanel.SetActive(false);
        pickerPanel.SetActive(false);
    }
    public void StartPicker()
    {
        ResetUI();
        inGameUIPanel.SetActive(false);
        pickerPanel.SetActive(true);
    }
    public void SetHost()
    {
        hostStatus.gameObject.SetActive(true);
    }
    public void ResetPickerUI()
    {
        selected1.text = "";
        selected2.text = "";
        selected3.text = "";
        selected4.text = "";
        selected5.text = "";
        selected6.text = "";
        selected7.text = "";
        selected8.text = "";
    }

    public void SetSelected(int index)
    {
        Debug.Log("SetSelected: " + index);
        string name = "Selected";
        switch (index)
        {
            case 1:
                selected1.text = name;
                break;
            case 2:
                selected2.text = name;
                break;
            case 3:
                selected3.text = name;
                break;
            case 4:
                selected4.text = name;
                break;
            case 5:
                selected5.text = name;
                break;
            case 6:
                selected6.text = name;
                break;
            case 7:
                selected7.text = name;
                break;
            case 8:
                selected8.text = name;
                break;
        }
    }
}