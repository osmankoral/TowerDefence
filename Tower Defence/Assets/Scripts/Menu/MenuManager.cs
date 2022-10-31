using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //[SerializeField] private Text stage;
    [SerializeField] private GameObject[] lockPanel;
    [SerializeField] private Text exemineText;
    private void Start()
    {
        
    }
    private void Update()
    {
        for(int i = 0; i < PlayerPrefs.GetInt("UnlockStage"); i++)
        {
            lockPanel[i].SetActive(false);
        }
    }
    public void StartPlay(int _stage)
    {
        PlayerPrefs.SetInt("CurrentStage", _stage);
        SceneManager.LoadScene("GameScene");
    }

    public void LockedExamine(int _beforeNum)
    {
        exemineText.gameObject.SetActive(true);
        exemineText.text = "Lütfen önce " + _beforeNum.ToString() + " bölümü tamamlayın";
    }

    IEnumerator DelayText()
    {
        yield return new WaitForSeconds(0.5f);
        exemineText.gameObject.SetActive(false);
    }

    public void Settings()
    {

    }

    public void Exit()
    {
        
    }

}
