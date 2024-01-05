using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TBEasyWebCam;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;

public class QRDecodeTest : MonoBehaviour
{
	public QRCodeDecodeController e_qrController;

	public Text UiText;

	public GameObject resetBtn;

	public GameObject scanLineObj;
    
	public Image torchImage;
	/// <summary>
	/// when you set the var is true,if the result of the decode is web url,it will open with browser.
	/// </summary>
	public bool isOpenBrowserIfUrl;

	

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void qrScanFinished(string dataText)
	{
        // Get the current real world time in a 24-hour format
        string currentTime = System.DateTime.Now.ToString("HH:mm");
        string decoded=dencode.instance.DecodeString(dataText);
		string[] splitt = decoded.Split('|');
		if (management_tru.instance.listname.Contains(splitt[0]))
		{
			for(int i=0;i< management_tru.instance.listname.Count; i++)
			{
				if (management_tru.instance.listname[i] == splitt[0])
				{
					int je = i;
					Destroy(management_tru.instance.listG[je]);
					management_tru.instance.listG.RemoveAt(je);
					management_tru.instance.listname.RemoveAt(je);

                }
			}
		}
		else
		{
			GameObject g = Instantiate(management_tru.instance.customer.gameObject, management_tru.instance.parent.transform);
			g.transform.name = splitt[0];
            management_tru.instance.listname.Add(g.transform.name);
			management_tru.instance.listG.Add(g.gameObject);
            g.GetComponent<TextMeshProUGUI>().text = splitt[0] + " : " + currentTime;
			Debug.Log(splitt[0] + " : " + currentTime);
		}
		management_tru.instance.scannerPanel.SetActive(false);
		resetBtn.SetActive(false);
		resetBtn.SetActive(true);

		if (isOpenBrowserIfUrl) {
			if (Utility.CheckIsUrlFormat(dataText))
			{
				if (!dataText.Contains("http://") && !dataText.Contains("https://"))
				{
					dataText = "http://" + dataText;
				}
				Application.OpenURL(dataText);
			}
		}
		this.UiText.text = dataText;
		if (this.resetBtn != null)
		{
			this.resetBtn.SetActive(true);
		}
		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(false);
		}

	}

	public void Reset()
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.Reset();
		}

		if (this.UiText != null)
		{
			this.UiText.text = string.Empty;
		}
		if (this.resetBtn != null)
		{
			//this.resetBtn.SetActive(false);
		}
		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(true);
		}
	}

	public void Play()
	{
		Reset ();
		if (this.e_qrController != null)
		{
			this.e_qrController.StartWork();
		}
	}

	public void Stop()
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.StopWork();
		}

		if (this.resetBtn != null)
		{
			//this.resetBtn.SetActive(false);
		}
		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(false);
		}
	}

	public void GotoNextScene(string scenename)
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.StopWork();
		}
		//Application.LoadLevel(scenename);
		SceneManager.LoadScene(scenename);
	}
    

}
