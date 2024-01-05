using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using Firebase;
using Firebase.Storage;
using UnityEngine.Networking;
using UnityEngine.Android;
using TMPro;
using System.Linq;

public class Manager : MonoBehaviour
{
    public Users person;
    FirebaseStorage storage;
    StorageReference sref;
    public TMP_InputField uname, password;
    [SerializeField] private Toggle managBool;
    public data d;
    private bool retieved;
    [SerializeField] private GameObject userPanel,ManagementPanel,Wrong;
    public static Manager instance;
    public GameObject filebrowser,login_panel,sign_panel,common;
    void OnEnable()
    {
        instance = this;
        StartCoroutine(wait());
       
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseApp app = FirebaseApp.Create(new AppOptions
            {
                ApiKey = "AIzaSyDVKXZcgpCJJLdwgAr3DkDZNmWOb4UJJNc",
                AppId = "1:552876456793:android:fe3e12b1c4d512e977aa48",
                ProjectId = "antitheft-eb242",
                StorageBucket = "antitheft-eb242.appspot.com"
            });
            // Use the Firebase app here
        });
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                storage = FirebaseStorage.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void login()
    {
        if (person.User.Any(data => data.uname == uname.text && data.password == password.text))
        {
            d = person.User.Find(data => data.uname == uname.text);
            if (d.management){
                ManagementPanel.SetActive(true);
                login_panel.SetActive(false);
                common.SetActive(false);
            }
            else
            {
                login_panel.SetActive(false);
                common.SetActive(false);
                userPanel.SetActive(true);
                filebrowser.SetActive(true);
            }
        }
        else
        {
            Wrong.SetActive(true);
            Wrong.GetComponent<TextMeshProUGUI>().text = "Wrong Username or Password";
            StartCoroutine(wrongWai());
        }
    }
    public void signin()
    {
        d = new data();
        if (uname.text.Length > 10 || password.text.Length > 10 )
        {
            Wrong.SetActive(true);
            Wrong.GetComponent<TextMeshProUGUI>().text = "length should be < 10";
            StartCoroutine(wrongWai());
            return;
        }

        d.assign(uname.text, password.text, managBool.isOn);
        if (!person.User.Any(data => data.uname==uname.text))
        {
            person.User.Add(d);
            write();
            if (managBool.isOn)
            {
                ManagementPanel.SetActive(true);
                sign_panel.SetActive(false);
                common.SetActive(false);
            }
            else
            {
                common.SetActive(false);
                sign_panel.SetActive(false);
                userPanel.SetActive(true);
                filebrowser.SetActive(true);
            }
        }
        else
        {
            Wrong.SetActive(true);
            Wrong.GetComponent<TextMeshProUGUI>().text = "Person already registered";
            StartCoroutine(wrongWai());
        }
    }
    IEnumerator wrongWai()
    {
        yield return new WaitForSeconds(1.5f);
        Wrong.SetActive(false);
    }
    public static void RequestStoragePermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
    }
    public void uploadd(string name, string path)
    {
       Debug.Log(path);
       Debug.Log(name);
        RequestStoragePermission();
        FirebaseStorage.DefaultInstance.RootReference.Child(name).PutFileAsync(path).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Upload failed: " + task.Exception);
            }
            if (task.IsCompleted)
            {
                Debug.Log("Sucesssss");
            }
            else
            {
                Debug.Log("Failed");
            }
        });
    }
    public void retrievee(string name, string path)
    {
        //sref = storage.RootReference.Child(name);
        FirebaseStorage.DefaultInstance.RootReference.Child(name).GetFileAsync(path).ContinueWith(task => {
            if (task.IsCompleted)
            {
                retieved = true;
                Debug.Log("Sucesssss");
            }
        });

    }
    public void retrieve()
    {
        Debug.Log("Rode");
        string path = Application.temporaryCachePath + "/Users.json";
        string content = File.ReadAllText(path);
        person = JsonUtility.FromJson<Users>(content);
    }
    public void write()
    {
        string path = Application.temporaryCachePath + "/Users.json";
        File.WriteAllText(path, JsonUtility.ToJson(person));
        Debug.Log(path);
      //  string pat = Application.temporaryCachePath + "/Users.json"; !!!!!!!!!!!!!!! Research about this and implement this
        uploadd("Users.json", Application.temporaryCachePath + "/Users.json");
    }
    IEnumerator wait()
    {
      
        retrievee("Users.json", Application.temporaryCachePath + "/Users.json");
        yield return new WaitUntil(() => retieved);
        retrieve();
    }
}
