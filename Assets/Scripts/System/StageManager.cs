using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] string[] _scenes;
    [SerializeField] float _timeOffset;
    int _currentStage = 0;
   
    public void NextStage()
    {
        Debug.Log("clear!");
        if ( _currentStage < _scenes.Length)
        {
            _currentStage++;
            Debug.Log($"next stage => {_currentStage}");
            StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(_timeOffset);
        SceneManager.LoadScene(_scenes[_currentStage]);
    }
}
