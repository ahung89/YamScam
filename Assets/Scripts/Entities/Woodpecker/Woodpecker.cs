using UnityEngine;

public class Woodpecker : MonoBehaviour {

    // body
    private GameObject joint = null;
    private GameObject body = null;
    private PolygonCollider2D bodyCollider = null;

    // transform
    private bool isBodyInRotation = false;
    private bool isRotationCounterclockwise = true;
    private float totalSecondsElapsed = 0f;
    private const float SecondsToRotate = 0.3f;
    private const float DegreesToRotate = 65f;
    private const float DegreesPerSecond = DegreesToRotate / SecondsToRotate;

    private Vector3 zAxis = new Vector3(0, 0, 1);

    void Start()
    {
        this.joint = GameObject.Find("WoodpeckerJoint");
        this.body = GameObject.Find("WoodpeckerBody");
        this.bodyCollider = this.body.GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        if (this.body != null && this.bodyCollider != null) 
        {
            if (!this.isBodyInRotation) 
            {
                if (Input.GetMouseButtonDown(0)) 
                {
                    Vector3 touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (this.bodyCollider.OverlapPoint(touchPoint)) 
                    {
                        this.isBodyInRotation = true;
                        this.isRotationCounterclockwise = true;
                    }
                }
            } 
            else 
            {
                int directionFactor = this.isRotationCounterclockwise ? 1 : -1;
                this.body.transform.RotateAround(this.joint.transform.position, zAxis, 
                    directionFactor * DegreesPerSecond * Time.deltaTime);

                this.totalSecondsElapsed += Time.deltaTime;
                if (this.totalSecondsElapsed >= SecondsToRotate) 
                {
                    if (!this.isRotationCounterclockwise) 
                    {
                        this.isBodyInRotation = false;
                    }
                    this.isRotationCounterclockwise = !this.isRotationCounterclockwise;
                    this.totalSecondsElapsed = 0;
                }
            }
        }
    }
}
