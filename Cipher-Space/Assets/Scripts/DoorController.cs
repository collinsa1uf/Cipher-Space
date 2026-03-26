using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    public UnityEvent openDoor;
    public UnityEvent closeDoor;

    public bool IsOpen { get; private set; } = false;

    void Start()
    {
        IsOpen = false;
    }

    public void OpenDoor()
    {
        if (IsOpen) return;

        IsOpen = true;
        openDoor?.Invoke();
    }

    public void CloseDoor()
    {
        if (!IsOpen) return;

        IsOpen = false;
        closeDoor?.Invoke();
    }
}
