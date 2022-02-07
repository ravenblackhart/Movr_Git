using UnityEngine;

public class ChargePhoneTask : Task
{
    private float _goalValue = 10f;
    
    public override PromptType StartTask(GameManager gameManager)
    {
        return PromptType.Main;
    }

    public override void UpdateTask(GameManager gameManager)
    {
        foreach (Phone phone in gameManager.taskReferences.phones)
        {
            if (phone.touchCustomerQueryEvent.Query(Time.frameCount))
            {
                if (!phone.OverHeated && phone.ChargeAmount >= _goalValue)
                {
                    completedTaskEvent.Invoke();
                }
                else
                {
                    failedTaskEvent.Invoke();
                }
            }
        }
    }

    public override void EndTask(GameManager gameManager)
    {
        //
    }

    // public override bool CheckValid(GameManager gameManager)
    // {
    //     // return ValueInRange(gameManager);
    // }
    
    // private bool ValueInRange(GameManager gameManager)
    // {
    //     foreach (Phone phone in gameManager.taskReferences.phones)
    //     {
    //         return Mathf.Abs(phone.ChargeAmount - _goalValue) <= 0.5f;
    //     }
    //
    //     return false;
    // }
}
