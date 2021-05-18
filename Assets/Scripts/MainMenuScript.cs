using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
	[SerializeField] private string gameplaySceneName;

	public void PlayButtonClick()
	{
		SceneManager.LoadSceneAsync(gameplaySceneName);
	}

	public void ExitButtonClick()
	{
		Debug.Log("Quit lgo");
		Application.Quit();
	}
}
