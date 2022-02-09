using UnityEngine;

public class GiveWaterTask : Task
{
    private float _goalValue = 100f;
    private CustomerThirst _thirst;
    
    public override PromptType StartTask(GameManager gameManager)
    {
        
        //TODO Open door and push out cup
        return PromptType.Main;
    }

    public override void UpdateTask(GameManager gameManager)
    {
        _thirst = gameManager.customer.GetComponent<CustomerThirst>();
        if (_thirst.FillAmount >= _goalValue)
        {
            completedTaskEvent.Invoke();
        }
    }
}
