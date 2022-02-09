using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForObjects : MonoBehaviour
{
    [SerializeField] Lever lever;
    [SerializeField] private Transform parent;
    [SerializeField] private List<GameObject> _objectList = new List<GameObject>();
    [SerializeField] private List<GameObject> _testList = new List<GameObject>();
    [SerializeField] private int _maxObjects = 20;
    [SerializeField] private List<GameObject> _currObjects = new List<GameObject>();
    private List<Vector3> _objSpawnPoints = new List<Vector3>();
    private List<Quaternion> _objRotations = new List<Quaternion>();

    public List<GameObject> _allObjects = new List<GameObject>();

    private void Awake() {
        foreach (GameObject obj in _objectList) {
            //_currObjects.Add(obj);
            //_objSpawnPoints.Add(obj.transform.localPosition);
            //_objRotations.Add(obj.transform.localRotation);
            //_allObjects.Add(obj);

            GameObject firstObj = GameObject.Find(obj.name);
            if (firstObj != null) {
                _currObjects.Add(firstObj);
                _testList.Add(firstObj);
                _objSpawnPoints.Add(firstObj.transform.localPosition);
                _objRotations.Add(firstObj.transform.localRotation);
                _allObjects.Add(firstObj);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (_currObjects.Contains(other.gameObject)) {
            _currObjects.Remove(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        if (_testList.Contains(other.gameObject) && !_currObjects.Contains(other.gameObject)) {
            _currObjects.Add(other.gameObject);
        }
    }

    private void Update() {
        if (lever.LeverValue < 0.1f && _currObjects.Count < _objectList.Count) {
            //Check what item needs to be spawned. 
           
            for (int i = 0; i < _objectList.Count; i++) {
                /*if (!_currObjects.Contains(_objectList[i]) && _allObjects.Count < _maxObjects) {
                    //GameObject spawnedObj = Instantiate(_objectList[i], _objSpawnPoints[i], _objRotations[i], parent);
                    GameObject spawnedObj = Instantiate(_objectList[i], parent, false);
                    spawnedObj.transform.localPosition = _objSpawnPoints[i];
                    spawnedObj.transform.localRotation = _objRotations[i];
                    spawnedObj.SetActive(true);
                    //_objectList[i] = spawnedObj;

                    _currObjects.Insert(i, spawnedObj);
                    _allObjects.Add(spawnedObj);
                }*/

                if (!_currObjects.Contains(_testList[i]) && _allObjects.Count < _maxObjects) {
                    GameObject spawnedObj = Instantiate(_objectList[i], parent, false);
                    spawnedObj.transform.localPosition = _objSpawnPoints[i];
                    spawnedObj.transform.localRotation = _objRotations[i];
                    spawnedObj.SetActive(true);
                    //_objectList[i] = spawnedObj;

                    _currObjects.Insert(i, spawnedObj);
                    _allObjects.Add(spawnedObj);
                    _testList.RemoveAt(i);
                    _testList.Insert(i, spawnedObj);
                }
            }
        }

      
    }
}
