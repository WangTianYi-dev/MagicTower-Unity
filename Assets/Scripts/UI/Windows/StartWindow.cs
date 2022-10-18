using UnityEngine;
using System.Collections;

public class StartWindow : BaseWindow
{
	public void OnStartButtonClicked()
    {
        GameManager.instance.NewGame();
    }

}

