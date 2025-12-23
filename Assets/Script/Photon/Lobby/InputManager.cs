using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InputManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button cancelButton;

    private Action<string> onSubmit;
    private Action onCancel;

    void Awake()
    {
        submitButton.onClick.AddListener(Submit);
        cancelButton.onClick.AddListener(Cancel);
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    public void RequestInput(Action<string> submitCallback, Action cancelCallback)
    {
        onSubmit = submitCallback;
        onCancel = cancelCallback;

        inputField.text = "";
        inputField.ActivateInputField();
    }

    void OnEndEdit(string value)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            Submit();
    }

    void Submit()
    {
        if (onSubmit == null) return;

        onSubmit.Invoke(inputField.text);
        ClearCallbacks();
    }

    void Cancel()
    {
        if (onCancel == null) return;

        onCancel.Invoke(); 
        ClearCallbacks();
    }

    void ClearCallbacks()
    {
        onSubmit = null;
        onCancel = null;
        inputField.DeactivateInputField();
    }
}
