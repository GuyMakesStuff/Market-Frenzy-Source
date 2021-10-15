using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PopUpText : MonoBehaviour
{
    public string Text;
    public Color color;
    public float Size;
    TextMeshProUGUI TXT;
    float Anim;

    // Start is called before the first frame update
    void Awake()
    {
        TXT = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        TXT.text = Text;
        transform.position += new Vector3(0, Time.deltaTime * 100, 0);
        TXT.fontSize = Size;
        Anim += Time.deltaTime;
        TXT.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), Anim);
    }
}
