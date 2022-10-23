using UnityEngine;
using System.Collections;

public class StartWindow : BaseWindow
{
	public void OnStartButtonClicked()
    {
        GameManager.instance.NewGame();
    }

    public void OnLoadButtonClicked()
    {
        UIManager.instance.OpenWindow("ArchiveWindow");
        ArchiveWindow.instance.Refresh(GameManager.instance.GetMetasOrdered(), ArchiveEntry.Type.Load);
    }
}

