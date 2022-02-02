public class GiveWaterTask : Task
{
    private float _goalValue = 10f;
    
    public override PromptType StartTask(GameManager gameManager)
    {
        
        //TODO Open door and push out cup
        return PromptType.Main;
    }

    public override void UpdateTask(GameManager gameManager)
    {
        foreach (WaterCup cup in gameManager.taskReferences.waterCups)
        {
            if (cup.TouchedCustomer)
            {
                if (cup.FillAmount >= _goalValue)
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
}
