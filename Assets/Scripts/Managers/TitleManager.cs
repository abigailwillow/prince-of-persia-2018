using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour {

    void Awake()
    {
        TextMeshProUGUI versionText = GetComponent<TextMeshProUGUI>();
        versionText.SetText(Application.productName);
    }

}
