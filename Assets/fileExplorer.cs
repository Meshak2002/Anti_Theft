using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using NativeFilePickerNamespace;
using NativeGalleryNamespace;
using System.Collections.Generic;
using TMPro;
using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using System.Collections;

public class fileExplorer : MonoBehaviour
{
    [SerializeField] private List<Image> imageElement;
    [SerializeField] private List<TextMeshProUGUI> filename;
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private Text name;
    [SerializeField] private Image fillimg;
    private Texture2D t;
    public int maxSize = 10000;
    string savePath;
    private void OnEnable()
    {
        name.text = Manager.instance.uname.text;
        for (int i = 0; i < Manager.instance.d.ndocs; i++)
        {
            savePath = Application.persistentDataPath + "/" + Manager.instance.uname.text + "_Doc_" + i + ".PNG";
            Manager.instance.retrievee(Manager.instance.uname.text + "_Doc_" + i + ".PNG", savePath);
        }
        StartCoroutine(wai());
    }
    IEnumerator wai()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < Manager.instance.d.ndocs; i++)
        {
            readd(i);
        }
    }
    public void zoom(Image img)
    {
        fillimg.sprite = img.sprite;
    }
    public void readd(int i)
    {
        savePath = Application.persistentDataPath + "/" + Manager.instance.uname.text + "_Doc_" + i + ".PNG";
        WWW access = new WWW("file://" + savePath);
        t = (Texture2D)access.texture;
        imageElement[i].color = Color.white;
        imageElement[i].sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
        RectTransform firstButtonRectTransform = buttons[i].GetComponent<RectTransform>();
        Vector2 anchoredPosition = firstButtonRectTransform.anchoredPosition;
        anchoredPosition.y -= 1100f;
        buttons[i].GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
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
    public void pickIMG(int imgno)
    {
        RequestStoragePermission();
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                string name = Path.GetFileName(path);
                filename[imgno].text = name;
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                imageElement[imgno].color = Color.white;
                // Assign texture to the Image element
                imageElement[imgno].sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                // Save texture as PNG
                byte[] pngBytes = texture.EncodeToPNG();
                string savePath = Path.Combine(Application.temporaryCachePath, Manager.instance.uname.text + "_Doc_" + Manager.instance.d.ndocs + ".png");
                File.WriteAllBytes(savePath, pngBytes);

                int index = Manager.instance.person.User.IndexOf(Manager.instance.d);
                Manager.instance.person.User[index].ndocs += 1;
                Manager.instance.write();
                StartCoroutine(waii());
            }
        });
        Debug.Log("Permission result: " + permission);
    }

    IEnumerator waii()
    {
        Debug.Log(savePath);
        yield return new WaitForSeconds(1);
        Manager.instance.uploadd(Manager.instance.uname.text + "_Doc_" + Manager.instance.d.ndocs + ".PNG", savePath);
    }
}
