using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QR : MonoBehaviour
{
    public string inputString;
    public RawImage qrcodeImage;

    private Texture2D qrTexture;

    void Start()
    {
        Texture2D qrCodeTexture = GenerateQRCodeTexture(inputString);
        qrcodeImage.texture = qrCodeTexture;
        qrcodeImage.rectTransform.sizeDelta = new Vector2(qrCodeTexture.width, qrCodeTexture.height);
    }

    Texture2D GenerateQRCodeTexture(string inputString)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = 1024,
                Width = 1024,
                Margin = 4,
                CharacterSet = "UTF-8"
            }
        };

        Texture2D qrCodeTexture = new Texture2D(1024, 1024);
        Color32[] pixels = writer.Write(inputString);
        qrCodeTexture.SetPixels32(pixels);
        qrCodeTexture.Apply();

        return qrCodeTexture;
    }
}
