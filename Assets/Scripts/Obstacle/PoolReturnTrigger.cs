using UnityEngine;
using System.Collections;
using IdeaToGame.ObjectPooling;

namespace Droppy.Obstacle
{
    public class PoolReturnTrigger : MonoBehaviour
    {
        [SerializeField]
        private string animationStateName = "StartAnimation";

        private Animator _animator;
        private Component _prefab;

        [Header("Configuração da Animação")]
        public float animationDuration = 1.0f;

        public void Initialize<T>(T prefab) where T : Component
        {
            _prefab = prefab;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Componente 'Animator' não encontrado em " + gameObject.name + ". O script não funcionará corretamente.");
            }
        }
        public void PlayAnimationAndReturn()
        {
            if (_animator != null)
            {
                _animator.Play(animationStateName, 0, 0f);
            }
            StartCoroutine(WaitAndReturnToPool(animationDuration));
        }

        private IEnumerator WaitAndReturnToPool(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            if (_prefab != null)
            {
                ObjectPool.ReturnToPool(_prefab, this.gameObject);
            }
            else
            {
                Debug.LogWarning("Prefab não inicializado no PoolReturnTrigger. Destruindo o objeto ao invés de retornar ao Pool.");
                Destroy(this.gameObject);
            }
        }
    }
}