using UnityEngine;

public class lockXY : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0,transform.eulerAngles.y,0);
    }
}