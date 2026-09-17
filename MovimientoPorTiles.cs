using UnityEngine;

public class MivimientoPorTiles : MonoBehaviour {

    public Vector2 moveDirection;
    public Rigidbody2D rb;
    public Vector3 positionSelected;
    public float speed;

    public bool moving;
    public enum PlayerState {Static, Horizontal, Vertical, Dead}
    public PlayerState actualyState = PlayerState.Static; 


    void Start() {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }
    void Update() {
        float moveY = Input.GetAxisRaw("Vertical");
        float moveX = Input.GetAxisRaw("Horizontal");

        if(!moving) {
            if (Mathf.Abs(moveY) > 0.001f) { 
                Transitions(PlayerState.Vertical);
                if (moveY > 0.001) positionSelected =  new Vector2(Mathf.Ceil(transform.position.x), Mathf.Ceil(transform.position.y + 1));
                else positionSelected =  new Vector2(Mathf.Ceil(transform.position.x), Mathf.Ceil(transform.position.y - 1));
                moving = !moving;
            }
            else if (Mathf.Abs(moveX) > 0.001f) {
                Transitions(PlayerState.Horizontal);
                if (moveX > 0.001) positionSelected =  new Vector2(Mathf.Ceil(transform.position.x + 1), Mathf.Ceil(transform.position.y));
                else positionSelected =  new Vector2(Mathf.Ceil(transform.position.x - 1), Mathf.Ceil(transform.position.y));
                moving = !moving;
            } 
        }


        switch(actualyState) {
            case PlayerState.Static:
            moveDirection = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            break;

            case PlayerState.Horizontal:
            moveDirection = new Vector2(moveX, 0);
            transform.position = Vector3.MoveTowards(transform.position, positionSelected, speed * Time.deltaTime);
            if (transform.position == positionSelected) {
                    moving = !moving;
                    Transitions(PlayerState.Static);
                }
            break;

            case PlayerState.Vertical:
            transform.position = Vector3.MoveTowards(transform.position, positionSelected, speed * Time.deltaTime);
            if (transform.position == positionSelected) {
                    moving = !moving;
                    Transitions(PlayerState.Static);
            }
            break;

            case PlayerState.Dead:
            rb.linearVelocity = Vector2.zero;
            break;
        }
    }

    void Transitions(PlayerState nuevoEstado) {
        if (actualyState == PlayerState.Dead) return;
        if (actualyState == PlayerState.Vertical && nuevoEstado == PlayerState.Horizontal) return;
        if (actualyState == PlayerState.Horizontal && nuevoEstado == PlayerState.Vertical) return;
        actualyState = nuevoEstado;
    }
}