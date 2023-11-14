using UnityEngine;

public class PotatoYoo : CollectableYoo
{
    private const string ITEM_NAME = "감자";
    private const float INIT_REGEN_TIME = 8f;
    private const float INIT_COLLECT_TIME = 3f;

    private void Awake()
    {
        itemName = ITEM_NAME;
        regenTime = INIT_REGEN_TIME;
        collectTime = INIT_COLLECT_TIME;
        state = State.Collectable;
        thisTransform = GetComponent<Transform>();
        collectableScale = transform.localScale;
        collectedScale = Vector3.zero;
    }
}
