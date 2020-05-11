using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Camera cam;
    public float camMoveSpeed;
    public float scrollSpeed;

    public float zoomInLimit;
    public float zoomOutLimit;

    private float maxZ = 70.0f;
    private float minZ = -70.0f;
    private float maxX = 90.0f;
    private float minX = -40.0f;

   // public MapManager mapman;

    // Start is called before the first frame update
    void Start()
    {
        camMoveSpeed = .5f;
        scrollSpeed = 1.5f;

        zoomInLimit = 1.0f;
        zoomOutLimit = 99.0f;

        //mapman = new MapManager();
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the right mouse button is pressed
       if (Input.GetMouseButton(1))
        {
            moveCameraXZ();
        }

       //checks if the mouse wheel is scrolling
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zoomInOut();
        }

        //checks if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            typeOfTile();
        }
    }


    //holding the right button, and moving the mouse pans the camera on XZ-plane
    void moveCameraXZ()
    {
        if(Input.GetAxis("Mouse X") < 0 && Input.GetAxis("Mouse Y") < 0 && camMoveAllowedTopRight() == true)
        {
            //move to top right
            cam.transform.position = new Vector3(cam.transform.position.x - camMoveSpeed, cam.transform.position.y,
                cam.transform.position.z + camMoveSpeed);
        }
        else if(Input.GetAxis("Mouse X") < 0 && Input.GetAxis("Mouse Y") > 0 && camMoveAllowedBottomRight() == true)
        {
            //move to bottom right
            cam.transform.position = new Vector3(cam.transform.position.x + camMoveSpeed, cam.transform.position.y,
                cam.transform.position.z + camMoveSpeed);
        }
        else if (Input.GetAxis("Mouse X") > 0 && Input.GetAxis("Mouse Y") < 0 && camMoveAllowedTopLeft() == true)
        {
            //move to top left
            print(3);
            cam.transform.position = new Vector3(cam.transform.position.x - camMoveSpeed, cam.transform.position.y,
                cam.transform.position.z - camMoveSpeed);
        }
        else if (Input.GetAxis("Mouse X") > 0 && Input.GetAxis("Mouse Y") > 0 && camMoveAllowedBottomLeft() == true)
        {
            //move to bottom left
            print(4);
            cam.transform.position = new Vector3(cam.transform.position.x + camMoveSpeed, cam.transform.position.y,
                cam.transform.position.z - camMoveSpeed);
        }
    }


    //camera cannot be moved outside of the borders top right
    private bool camMoveAllowedTopRight()
    {
        if(cam.transform.position.x > minX && cam.transform.position.z < maxZ)
        {
            return true;
        }
        return false;
    }


    //camera cannot be moved outside of the border bottom right
    private bool camMoveAllowedBottomRight()
    {
        if (cam.transform.position.x < maxX && cam.transform.position.z < maxZ)
        {
            return true;
        }
        return false;
    }

    //camera cannot be moved outside of the border top left
    private bool camMoveAllowedTopLeft()
    {
        if (cam.transform.position.x > minX && cam.transform.position.z > minZ)
        {
            return true;
        }
        return false;
    }

    //camera cannot be moved outside of the border bottom left
    private bool camMoveAllowedBottomLeft()
    {
        if (cam.transform.position.x < maxX && cam.transform.position.z > minZ)
        {
            return true;
        }
        return false;
    }



    //scrolling the mouse wheel zooms the camera in and out
    void zoomInOut()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && limitZoomIn() == true)
        {
            cam.transform.position = new Vector3(cam.transform.position.x-scrollSpeed,
                cam.transform.position.y - scrollSpeed, cam.transform.position.z);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && limitZoomOut() == true) 
        {
            cam.transform.position = new Vector3(cam.transform.position.x + scrollSpeed,
                cam.transform.position.y + scrollSpeed, cam.transform.position.z);
        }
            
    }

    //limit for min zoom
    private bool limitZoomIn()
    {
        if (cam.transform.position.y < zoomInLimit)
        {
            return false;
        } 
        return true;
    }

    //limit for max zoom
    private bool limitZoomOut()
    {
        if(cam.transform.position.y > zoomOutLimit)
        {
            return false;
        }
        return true;
    }

    //left clicking a tile outputs the type of tile as text on the console
    void typeOfTile()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);
        }

    }
}
