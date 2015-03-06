using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour {

	Canvas canvas;

	void Start()
	{
		canvas = GetComponent<Canvas>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
#if UNITY_EDITOR
			canvas.enabled = !canvas.enabled;
			Pause();
#else
            Quit();
#endif
		}
	}

	public void Pause()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
	}

	public void Quit()
	{
#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
#else 
		Application.Quit();
#endif
	}
}
