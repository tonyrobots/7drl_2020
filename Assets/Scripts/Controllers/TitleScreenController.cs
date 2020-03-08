using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenController : MonoBehaviour
{

    public GameObject blackScreen;
    public InputField nameInput;
    public TextMeshProUGUI startButton;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("name")) nameInput.text = PlayerPrefs.GetString("name");        
    }

    // Update is called once per frame
    void Update()
    {

        // if (Input.anyKeyDown && startButton.isActiveAndEnabled) {

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            StartGame();
        }
        
    }

    public void StartGame() {
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
        blackScreen.SetActive(true);
    }

    public void SaveName(string name) {
        PlayerPrefs.SetString("name", name);
        startButton.text=$"Okay, {Player.LoadName()}. Press any\n key to Start Your Adventure";
        StartGame();
    }

}
