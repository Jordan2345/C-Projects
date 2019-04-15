using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RetrieveName:MonoBehaviour  {

    public static RetrieveName save_Names;
    public TMP_InputField inputField;
    public GameObject input;
    public string userName;

    private void Awake()
    {
        input = GameObject.Find("NameInput");
        inputField = input.GetComponent<TMP_InputField>();
        save_Names = this;
    }

    public void SetName()
    {
        userName = inputField.text;
    }
}
