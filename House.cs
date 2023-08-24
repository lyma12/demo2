using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;
public class House : MonoBehaviour
{
    private bool start = false;
    private float duration = 1f;
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("train") && coll != null)
        {
            GameObject other = coll.gameObject;
            Color trainColor = other.GetComponent<Renderer>().material.color;
            Color t = GetComponent<Renderer>().material.color;
            if (t == trainColor)
            {
                finish();
            }
            else wrongWay();
            other.GetComponent<train>().DestroyTrain();
        }
        
    }
    private void Start()
    {
        ParticleSystem part = GetComponent<ParticleSystem>();
        part.Stop();
    }
    private void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }
    private void finish()
    {
        DataGame.count++;
        ParticleSystem part = GetComponent<ParticleSystem>();
        part.Play();
    }
    private void wrongWay()
    {
        DataGame.count--;
        start = true;
    }
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere;
            yield return null;
        }
        transform.position = startPosition;
    }
}
