using UnityEngine;

public class ChargePhoneTask : Task
{
    private float _goalValue = 5f;
    private float _timer = 0;

    public override PromptType StartTask(GameManager gameManager)
    {
        ThrowPhone(gameManager);
        return PromptType.Main;
    }

    public override void UpdateTask(GameManager gameManager)
    {
        foreach (Phone phone in gameManager.taskReferences.phones)
        {
            if (phone.touchCustomerQueryEvent.Query(Time.frameCount))
            {
                if (phone.ChargeAmount >= _goalValue)//!phone.OverHeated && 
                {                                       
                    completedTaskEvent.Invoke();
                }
            }
        }

        _timer += Time.deltaTime;

        if (_timer >= gameManager.customer.GetComponent<ThrowPhone>().throwTimer) {
            ThrowPhone(gameManager);
            _timer = 0;
        }
    }

    public override void EndTask(GameManager gameManager)
    {
        //
    }

    public void ThrowPhone(GameManager gameManager) {
        ThrowPhone throwPhone = gameManager.customer.GetComponent<ThrowPhone>();
        throwPhone.Throw(gameManager.taskReferences.phones[0].gameObject);
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
