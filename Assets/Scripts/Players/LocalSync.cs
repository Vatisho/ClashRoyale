using UnityEngine;

public class LocalSync : MonoBehaviour
{
 static float syncTime= 0.005f;

 private Player _player;

    private void Start() {
        _player = GetComponent<Player>();
    }
    public void Sync(Vector2 velocity, Vector2 position)
    {
        transform.position = position;
        _player.Move(velocity);
    }
}