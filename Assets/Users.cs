using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Users 
{
    public List<data> User;
}

[Serializable]
public class data
{
    public string uname, password;
    public bool management;
    public int ndocs;
    public void assign(string n, string p, bool b)
    {
        uname = n;
        password = p;
        management = b;
    }
}
