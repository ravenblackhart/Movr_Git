using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePhoneTask : Task
{
    public override PromptType StartTask(GameManager gameManager)
    {
        return PromptType.Main;
    }

    public override void UpdateTask(GameManager gameManager)
    {
        //
    }

    public override void EndTask(GameManager gameManager)
    {
        //
    }

    public override bool CheckValid(GameManager gameManager)
    {
        return true;
    }
}
