using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    private Button[] btns;

    // Start is called before the first frame update
    void Start()
    {
        btns = GetComponentsInChildren<Button>();

        btns[0].gameObject.AddComponent<ButtonInput>().dir = Vector3.forward;
        btns[1].gameObject.AddComponent<ButtonInput>().dir = Vector3.back;
        btns[2].gameObject.AddComponent<ButtonInput>().dir = Vector3.left;
        btns[3].gameObject.AddComponent<ButtonInput>().dir = Vector3.right;
        btns[4].gameObject.AddComponent<ButtonInput>().dir = Vector3.up;
    }
}
