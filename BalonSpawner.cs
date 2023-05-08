using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalonSpawner : MonoBehaviour
{
    [SerializeField] Balon balon;
    [SerializeField] Unicorn unicorn;
    [SerializeField] float initialTimer;
    
    float timer;

    public void Start()
    {
        timer = initialTimer;
        balon.gameObject.SetActive(false);
    }

    void Update()
    {
       if(timer<=0 && balon.gameObject.activeInHierarchy == false) {
            balon.gameObject.SetActive(true);
            balon.transform.position = unicorn.transform.position + new Vector3(0,0,13);
            unicorn.SetMoveable(false);
       }

       timer -= Time.deltaTime;
    }

    public void ResetTimer()
    {
        timer = initialTimer;
    }
}
