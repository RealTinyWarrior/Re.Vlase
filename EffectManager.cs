using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject effectObject;

    public void Create(int effect, float duration, Vector2 position, Color color, float size = 3, int layer = 0)
    {
        GameObject createdEffect = Instantiate(effectObject, position, Quaternion.identity);
        SpriteRenderer spriteRenderer = createdEffect.GetComponent<SpriteRenderer>();
        createdEffect.transform.localScale = new Vector3(size, size, 0);

        createdEffect.GetComponent<DestroyObject>().timer = duration;
        spriteRenderer.sortingOrder = layer;
        spriteRenderer.color = color;
    }
}
