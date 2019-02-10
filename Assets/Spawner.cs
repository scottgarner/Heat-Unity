using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click
{
    public int channel;
    public float x;
    public float y;
}

public class Spawner : MonoBehaviour
{

    [SerializeField]
    private WebSocketBridge webSocketBridge;

    [SerializeField]
    private GameObject spawnPrefab;

    void Start()
    {

        webSocketBridge.OnReceived += Received;

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }

        }

    }

    void Received(string message)
    {
        Debug.Log(message);
        try
        {
            Click click = JsonUtility.FromJson<Click>(message);
            Debug.Log(click.x);

            Spawn(click.x, click.y);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void Spawn(float x, float y)
    {

        Camera camera = Camera.main;

        Vector3 screenPoint = new Vector3(
            Mathf.Floor(x * Screen.width),
            Mathf.Floor((1 - y) * Screen.height),
            10
        );

        Debug.Log(screenPoint);

        Vector3 worldPoint = camera.ScreenToWorldPoint(screenPoint);
        GameObject spawnInstance = Instantiate(spawnPrefab, worldPoint, Quaternion.identity, transform);

    }

}
