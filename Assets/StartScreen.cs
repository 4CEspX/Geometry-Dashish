using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject myGameObject;
    public GameObject myPrefab;
    // Start is called before the first frame update
    void Start()
    {
        myGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false);
            myGameObject.SetActive(true);
        }
    }
}
