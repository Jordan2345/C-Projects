using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RetrieveName:MonoBehaviour  {

    public static RetrieveName save_Names;
    public TMP_InputField inputField;
    public string userName;

    private void Awake()
    {
        if (save_Names == null)
        {
            save_Names = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SetName()
    {
        userName = inputField.text;
    }
}
