using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLEAR_BTN : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClick(GameObject parent)
    {
        Destroy(parent);
    }  
    
}
