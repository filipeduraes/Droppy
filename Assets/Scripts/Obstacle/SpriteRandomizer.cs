using System.Collections.Generic;
using Droppy.Shared;
using UnityEngine;

namespace Droppy.Obstacle
{
    public class SpriteRandomizer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private List<Sprite> sprites;

        private void OnEnable()
        {
            Sprite randomSprite = sprites.GetRandomElement();
            spriteRenderer.sprite = randomSprite;
        }
    }
}