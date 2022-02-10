using UnityEngine;

public class GiveWaterTask : Task
{
    private CustomerThirst _thirst;
    
    public override PromptType StartTask(GameManager gameManager)
    {
        
        //TODO Open door and push out cup
        return PromptType.Main;
    }

    public override void UpdateTask(GameManager gameManager)
    {
        _thirst = gameManager.customer.GetComponent<CustomerThirst>();
        if (_thirst.FillAmount >= 40f)
        {
            completedTaskEvent.Invoke();
        }
    }
}
