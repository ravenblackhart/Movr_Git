using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskReferences
{
    //TODO: Search through in game pool instead
    public Lever volumeLever;
    public List<SnackBar> snackBars = new List<SnackBar>();
    public List<Phone> phones = new List<Phone>();
    public List<WaterCup> waterCups = new List<WaterCup>();
    public Lever leverAC;
    public Lever windowsLever;
}
