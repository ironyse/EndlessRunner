using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 effectMultiplier;
    private Transform cameraTransform;
    private Vector3 lastCameraPos;
    private float textureUnitSizeX;

    void Start(){
        cameraTransform = Camera.main.transform;
        lastCameraPos = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }
    
    void LateUpdate(){
        Vector3 deltaMov = cameraTransform.position - lastCameraPos;
        transform.position += new Vector3(deltaMov.x * effectMultiplier.x, deltaMov.y * effectMultiplier.y);
        lastCameraPos = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX) {
            float offsetPosX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPosX, transform.position.y);
        }
    }
}
