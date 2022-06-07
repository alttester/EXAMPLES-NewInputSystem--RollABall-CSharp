using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterController : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_Text text;
    int counter=0;
    void Start()
    {
        text.text=counter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonPressed(){
        counter++;
        text.text=counter.ToString();
    }
}
