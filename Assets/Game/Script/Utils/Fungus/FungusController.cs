using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusController : MonoBehaviour
{
    [SerializeField] private Flowchart flowchart;
    [SerializeField] DialogInput dialogInput;
    private string lastActiveBlock;

    public void Init()
    {
        if (GameVariables.DIALOG_START_MESSAGE == null)
        {
            GameVariables.DIALOG_START_MESSAGE = "MISSION_START";
            lastActiveBlock = "MissionStart";
        }

        if (GameVariables.DIALOG_START_MESSAGE.Equals("MISSION_START")) lastActiveBlock = "MissionStart";
        else if (GameVariables.DIALOG_START_MESSAGE.Equals("BOSS_PANEL"))
        {
            lastActiveBlock = "pick Boss";
        }
        else if (GameVariables.DIALOG_START_MESSAGE.Equals("ITEM_PANEL")) lastActiveBlock = "pick Item";

        flowchart.ExecuteBlock(lastActiveBlock);
    }

    public void NextBlock(string blockname)
    {
        flowchart.StopBlock(blockname);
        flowchart.ExecuteBlock(blockname);

        lastActiveBlock = blockname;
    }

    public void StopBlock(string blockname)
    {
        flowchart.StopBlock(blockname);
    }

    public void NextDialog()
    {
        dialogInput.SetButtonClickedFlag();
    }

    public void ChangeClickMode(ClickMode clicked)
    {
        dialogInput.ChangeClickMode(clicked);
    }

    public void ActivateCancelEnabled(bool flag)
    {
        dialogInput.ActivateCancelEnabled(flag);
    }
}
