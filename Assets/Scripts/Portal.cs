using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    #region Fields

    [Tooltip("All parts must use Magical Portal Shader material.")]
    [SerializeField] private List<GameObject> _parts = new();
    private List<Material> _partsMaterials = new();

    [SerializeField] private float _transitionTimeInSec = 1f;

    private const string _circleClipName = "_CircleClip";
    private const string _circleWidthName = "_CircleWidth";
    private const string _featherName = "_Feather";

    private List<float> _openedCircleClips = new();
    private List<float> _openedCircleWidths = new();
    private List<float> _openedFeathers = new();

    private float _closedCircleClip = 0f;
    private float _closedCircleWidth = 0f;
    private float _closedFeather = 0f;

    private bool _opened = true;
    private bool _hasActiveTransition = false;

    private delegate void TransitionCalculation(int currentPartIndex, float elapsedTime, out float circleClip, out float circleWidth, out float feather);

    #endregion


    #region Private Methods

    private void SetOpened() => _opened = true;
    private void SetClosed() => _opened = false;


    private void Awake()
    {
        foreach(GameObject part in _parts)
        {
            _partsMaterials.Add(part.GetComponent<MeshRenderer>().material);
        }

        foreach (Material currentPartMaterial in _partsMaterials)
        {
            _openedCircleClips.Add(currentPartMaterial.GetFloat(_circleClipName));
            _openedCircleWidths.Add(currentPartMaterial.GetFloat(_circleWidthName));
            _openedFeathers.Add(currentPartMaterial.GetFloat(_featherName));
        }
    }

    void Update()
    {
        if(_hasActiveTransition)
        {
            return;
        }    


        if (Input.GetKeyDown(KeyCode.W) && !_opened)
        {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.Q) && _opened)
        {
            Close();
        }
    }

    private void Open()
    {
        Invoke(nameof(SetOpened), _transitionTimeInSec - 1f);
        StartCoroutine(TransitionRoutine(CalculateOpenValues));
    }

    private void Close()
    {
        Invoke(nameof(SetClosed), _transitionTimeInSec - 1f);
        StartCoroutine(TransitionRoutine(CalculateCloseValues));
    }


    private void CalculateOpenValues(int partIndex, float elapsedTime, out float circleClip, out float circleWidth, out float feather)
    {
        circleClip = Mathf.Lerp(_closedCircleClip, _openedCircleClips[partIndex], elapsedTime / _transitionTimeInSec);
        circleWidth = Mathf.Lerp(_closedCircleWidth, _openedCircleWidths[partIndex], elapsedTime / _transitionTimeInSec);
        feather = Mathf.Lerp(_closedFeather, _openedFeathers[partIndex], elapsedTime / _transitionTimeInSec);
    }

    private void CalculateCloseValues(int partIndex, float elapsedTime, out float circleClip, out float circleWidth, out float feather)
    {
        circleClip = Mathf.Lerp(_openedCircleClips[partIndex], _closedCircleClip, elapsedTime / _transitionTimeInSec);
        circleWidth = Mathf.Lerp(_openedCircleWidths[partIndex], _closedCircleWidth, elapsedTime / _transitionTimeInSec);
        feather = Mathf.Lerp(_openedFeathers[partIndex], _closedFeather, elapsedTime / _transitionTimeInSec);
    }


    private IEnumerator TransitionRoutine(TransitionCalculation CalcTransitionValues)
    {
        _hasActiveTransition = true;

        float elapsedTimeInSec = 0f;

        while (elapsedTimeInSec < _transitionTimeInSec)
        {
            elapsedTimeInSec += Time.deltaTime;

            UpdateEachPartValues(CalcTransitionValues, elapsedTimeInSec);

            yield return null;
        }

        UpdateEachPartValues(CalcTransitionValues, _transitionTimeInSec);
        _hasActiveTransition = false;
    }

    private void UpdateEachPartValues(TransitionCalculation CalcTransitionValues, float elapsedTime)
    {
        float currentCircleClip;
        float currentCircleWidth;
        float currentFeather;

        for (int currentPartIndex = 0; currentPartIndex < _partsMaterials.Count; ++currentPartIndex)
        {
            CalcTransitionValues(currentPartIndex, elapsedTime, out currentCircleClip, out currentCircleWidth, out currentFeather);

            _partsMaterials[currentPartIndex].SetFloat(_circleClipName, currentCircleClip);
            _partsMaterials[currentPartIndex].SetFloat(_circleWidthName, currentCircleWidth);
            _partsMaterials[currentPartIndex].SetFloat(_featherName, currentFeather);
        }
    }

    #endregion
}
