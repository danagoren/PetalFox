using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Sprite sprite;
        public float scrollSpeed = 1f;
        public int sortingOrder;
        public float verticalOffset;
    }

    [SerializeField] private ParallaxLayer[] layers;

    private Camera cam;
    private List<LayerState> layerStates;

    private class LayerState
    {
        public ParallaxLayer config;
        public List<GameObject> segments = new List<GameObject>();
        public float spriteWidth;
        public float halfSpriteWidth;
    }

    void Start()
    {
        cam = Camera.main;
        layerStates = new List<LayerState>();

        foreach (var config in layers)
        {
            if (config.sprite == null) continue;

            var state = new LayerState
            {
                config = config,
                spriteWidth = config.sprite.rect.width / config.sprite.pixelsPerUnit,
            };
            state.halfSpriteWidth = state.spriteWidth / 2f;

            float viewWidth = 2f * cam.orthographicSize * cam.aspect;
            int count = Mathf.CeilToInt(viewWidth / state.spriteWidth) + 1;
            float startX = cam.transform.position.x - viewWidth / 2f;

            for (int i = 0; i < count; i++)
            {
                float centerX = startX + i * state.spriteWidth + state.halfSpriteWidth;
                state.segments.Add(CreateSegment(config, centerX));
            }

            layerStates.Add(state);
        }
    }

    void Update()
    {
        float viewLeft = cam.transform.position.x - cam.orthographicSize * cam.aspect;
        float viewRight = cam.transform.position.x + cam.orthographicSize * cam.aspect;

        foreach (var state in layerStates)
        {
            float speed = state.config.scrollSpeed * Time.deltaTime;
            float halfW = state.halfSpriteWidth;

            for (int i = state.segments.Count - 1; i >= 0; i--)
            {
                var t = state.segments[i].transform;
                t.position += Vector3.left * speed;
            }

            for (int i = state.segments.Count - 1; i >= 0; i--)
            {
                float rightEdge = state.segments[i].transform.position.x + halfW;
                if (rightEdge < viewLeft)
                {
                    Destroy(state.segments[i]);
                    state.segments.RemoveAt(i);
                }
            }

            if (state.segments.Count > 0)
            {
                var last = state.segments[^1];
                float rightEdge = last.transform.position.x + halfW;

                while (rightEdge < viewRight + state.spriteWidth)
                {
                    float newX = rightEdge + state.halfSpriteWidth;
                    state.segments.Add(CreateSegment(state.config, newX));
                    rightEdge += state.spriteWidth;
                }
            }
        }
    }

    private GameObject CreateSegment(ParallaxLayer config, float x)
    {
        var go = new GameObject(string.Format("{0}_Segment", config.sprite.name));
        go.transform.position = new Vector3(x, config.verticalOffset, 0f);
        go.transform.SetParent(transform);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = config.sprite;
        sr.sortingOrder = config.sortingOrder;

        return go;
    }
}
