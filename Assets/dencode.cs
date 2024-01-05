using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dencode : MonoBehaviour
{
    public static dencode instance;
    private void OnEnable()
    {
        instance = this;
    }
    public string EncodeString(string input)
    {
        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(input);
        string encodedText = Convert.ToBase64String(bytesToEncode);
        return encodedText;
    }

    public string DecodeString(string input)
    {
        byte[] bytesToDecode = Convert.FromBase64String(input);
        string decodedText = System.Text.Encoding.UTF8.GetString(bytesToDecode);
        return decodedText;
    }
}
