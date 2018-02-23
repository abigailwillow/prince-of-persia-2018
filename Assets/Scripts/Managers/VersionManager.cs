using UnityEngine;
using TMPro;

public class VersionManager : MonoBehaviour {

    void Awake()
    {
        TextMeshProUGUI versionText = GetComponent<TextMeshProUGUI>();
        versionText.SetText("Early Alpha " + Application.version);
    }

}
