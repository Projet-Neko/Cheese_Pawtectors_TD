using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GiftCode : MonoBehaviour
{

    [SerializeField] private TMP_InputField _code;

    private List<string> _codes = new List<string>();
    private List<string> _oldCodes = new List<string>();

    public void CheckCode()
    {
        if (_code.text == "123" || _code.text == "1234") //Debug code
        {
            CheckOldCodes(_code.text);
        }

        if (_codes.Contains(_code.text))
        {
            CheckOldCodes(_code.text);
        }
        else
        {
            Debug.Log("invalid code");
        }
        _code.text = "";
    }

    private void CheckOldCodes(string code)
    {
        if (_oldCodes.Contains(code))
        {
            Debug.Log("Code already used");
        }
        else
        {
            Debug.Log("Code valid");
            _oldCodes.Add(code);
        }
    }


    public void OnDestroy()
    {
        // Supprimer le code de la base de données ?
    }

}
