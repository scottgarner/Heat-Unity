using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrawClick : MonoBehaviour
{

    [SerializeField]
    WebSocketBridge webSocketBridge;

    [SerializeField]
    private GameObject particlePrefab;

    [SerializeField]
    private float appearTime = 2;
    [SerializeField]
    private float idleTime = 36;
    [SerializeField]
    private float fadeTime = 6;

    void Start()
    {
        webSocketBridge.OnReceived += Received;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            ClickScreen(Input.mousePosition);
        }
    }

    private void ClickScreen(Vector2 clickPosition)
    {


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(clickPosition);

        if (Physics.Raycast(ray, out hit))
        {

            Vector3 position = new Vector3((hit.textureCoord.x - .5f) * 2.0f, (hit.textureCoord.y - .5f) * 2.0f, 10);
            CreateParticle(position);

            Vector3 positionRight = new Vector3(((hit.textureCoord.x - .5f) * 2.0f) + 2, (hit.textureCoord.y - .5f) * 2.0f, 10);
            CreateParticle(positionRight);

            Vector3 positionLeft = new Vector3(((hit.textureCoord.x - .5f) * 2.0f) - 2, (hit.textureCoord.y - .5f) * 2.0f, 10);
            CreateParticle(positionLeft);


        }

    }

    private void CreateParticle(Vector3 position)
    {

        GameObject particleInstance = Instantiate(particlePrefab, transform, false);

        Particle particle = particleInstance.GetComponent<Particle>();
        particle.Init(appearTime, idleTime, fadeTime);

        particleInstance.transform.localPosition = position;

    }

    void Received(string message)
    {
        Debug.Log(message);
        try
        {
            Click click = JsonUtility.FromJson<Click>(message);

            Vector2 screenPoint = new Vector3(
                Mathf.Floor(click.x * Screen.width),
                Mathf.Floor((1 - click.y) * Screen.height)
            );

            ClickScreen(screenPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

}
