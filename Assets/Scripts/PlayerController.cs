using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float moveSpeed = 5f;

    public float pullForce = 100f;
    public float rotateSpeed = 360f;
//    private GameObject closestTower;
    public GameObject hookedTower;
    public bool isPulled = false;

    public Canvas UICanvas;
    private UIControllerScript UIControl;

    private AudioSource myAudio;
    private bool isCrashed = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private bool isClick = false;

    #region Singleton
    public static PlayerController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instances of PlayerController found!");
            return;
        }

        instance = this;
    }
    #endregion

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        UIControl = UICanvas.GetComponent<UIControllerScript>();
        myAudio = GetComponent<AudioSource>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        if (isCrashed)
        {
            if (!myAudio.isPlaying)
            {
                //Restart
                RestartPosition();
            }
        }
        else
        {
            rb2D.velocity = -transform.up * moveSpeed;
        }

        if (Input.GetMouseButtonDown(0)) isClick = true;
        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
            if (hookedTower)
            {
                hookedTower.GetComponent<SpriteRenderer>().color = Color.blue;
                MouseUp();
            }
        }

        if (!isPulled && isClick)
        {
            if (hookedTower)
            {
                hookedTower.GetComponent<SpriteRenderer>().color = Color.green;

                float distance = Vector2.Distance(transform.position, hookedTower.transform.position);
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;

                float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
                rb2D.AddForce(pullDirection * newPullForce);

                float rotateAmount = rotateSpeed * Vector3.Cross(pullDirection, transform.up).z;
                rb2D.angularVelocity = rotateAmount / distance;

                isPulled = true;
            }
        }
    }

    public void MouseUp()
    {
        rb2D.angularVelocity = 0f;
        isPulled = false;
    }

    public void RestartPosition()
    {
        rb2D.angularVelocity = 0f;
        transform.position = startPosition;
        transform.rotation = startRotation;

        isCrashed = false;
        isPulled = false;

        if (hookedTower)
        {
            hookedTower.GetComponent<SpriteRenderer>().color = Color.white;
            hookedTower = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (!isCrashed)
            {
                myAudio.Play();
                rb2D.velocity = Vector2.zero;
                rb2D.angularVelocity = 0f;
                isCrashed = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("Level Cleared!");
            UIControl.EndGame("LEVEL CLEAR!", Color.green);
        }
    }

    public void SelectTower(GameObject newTower)
    {
        if(hookedTower != newTower)
        {
            if (hookedTower != null)
            {
                hookedTower.GetComponent<SpriteRenderer>().color = Color.white;
                hookedTower = null;
                MouseUp();
            }

            hookedTower = newTower;
            hookedTower.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            hookedTower.GetComponent<SpriteRenderer>().color = Color.white;
            hookedTower = null;
            MouseUp();
        }
    }
}
