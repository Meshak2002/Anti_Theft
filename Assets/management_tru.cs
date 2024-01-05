using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class management_tru : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI uname, customer,RealTime;
    public GameObject scannerPanel,parent,resetbtn;
    public static management_tru instance;
    public List<string> listname;
    public List<GameObject> listG;
    private void OnEnable()
    {
        instance = this;
        uname.text = Manager.instance.uname.text;
    }
    private void Update()
    {
        // Get the current real world time in a 24-hour format
        string currentTime = System.DateTime.Now.ToString("HH:mm");

        // Update the text of the UI element to display the current time
        RealTime.text = currentTime;
    }
}
