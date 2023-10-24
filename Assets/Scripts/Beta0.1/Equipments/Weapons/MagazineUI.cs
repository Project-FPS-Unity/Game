using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagazineUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI magazine;
    public static int frontMagazine;
    public static int backMagazine;

    // Update is called once per frame
    void FixedUpdate()
    {
        MagazineTextUpdater();
        //InvokeRepeating("MagazineTextUpdater", 0, 1);
    }

    public void MagazineTextUpdater()
    {
        magazine.text = "Bullet : " + string.Format("{0:000}", Mathf.Round(frontMagazine)) + "/" + string.Format("{0:000}", Mathf.Round(backMagazine));
    }
}
