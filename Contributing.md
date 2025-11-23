# Padrões de Código

Este documento define o padrão de código usado no projeto. O objetivo é manter o código limpo, consistente e fácil de entender. Ele é baseado principalmente nos padrões de nomeação da Microsoft: https://www.dofactory.com/csharp-coding-standards

---

## 1. Estrutura Geral

- Use nomes em inglês para classes, métodos, variáveis e pastas.
- Sempre use chaves mesmo em blocos de uma linha.
- Mantenha a ordem e a organização visual do código para facilitar a leitura.
- Use substantivos para nomes de classes e variáveis, verbos para nomes de métodos e perguntas para nomes de booleanos.
- Sempre use namespaces e Assembly Definitions.

Exemplo básico:
```csharp
namespace ProjectName.Systems
{
   public class ExampleSystem : MonoBehaviour
   {
       [SerializeField] private Rigidbody2D rigidbody2D;
       [SerializeField] private Animator animator;

       private void Start()
       {
           Initialize();
       }

       private void Initialize()
       {
           // Inicialização do sistema
       }
   }
}
```

---

## 2. Nomeação

| Tipo             | Regra      | Exemplo                           |
|------------------|------------|-----------------------------------|
| Classe/Struct    | PascalCase | PlayerController, EnemyAI         |
| Método           | PascalCase | InitializePlayer(), MoveForward() |
| Variável Privada | camelCase  | playerSpeed, isJumping            |
| Constante        | UPPER_CASE | MAX_HEALTH                        |
| Namespace        | PascalCase | Droppy.Core, Droppy.UI            |

---

## 3. Campos e Atributos

- Prefira SerializeField ao invés de usar GetComponent.
- Variáveis privadas devem ser serializadas quando necessário.
- Propriedades devem ser usadas ao invés de variáveis públicas, e apenas quando fizer sentido para outros scripts.

```csharp
[SerializeField] private Rigidbody2D rigidbody2D;
[SerializeField] private Animator animator;

public Animator Animator => animator;
```

---

## 4. Modificadores de Acesso

A ordem recomendada é:

1. Public static
2. Public
3. Protected static
4. Protected
5. Private static
6. Private

Sempre use private explicitamente.
```csharp
public static event Action OnGameStart = delegate {};

protected bool isAlive;
private float moveSpeed;
```

---

## 5. Propriedades e Métodos

Propriedades públicas devem ter getters e setters claros.
Métodos devem ter nomes descritivos e fazer apenas uma coisa.
Sempre use parênteses e retornos explícitos.
```csharp
public float Health { get; private set; }

public void TakeDamage(float damage)
{
   Health -= damage;

   if (Health <= 0)
   {
       Die();
   }
}

private void Die()
{
   // Código de morte
}
```

---

## 6. Ciclo de Vida (Unity)

Mantenha esta ordem quando houver, e sempre mantenha os eventos da Unity acima de outros métodos da classe para melhor visibilidade:
```csharp
private void Awake() { }
private void OnEnable() { }
private void Start() { }
private void Update() { }
private void OnDisable() { }
private void OnDestroy() { }
```

---

## 7. Eventos e Interações

- Prefira TryGetComponent em vez de CompareTag.
- Use interfaces para interações entre objetos.
- Evite lógica complexa dentro dos eventos do Unity.
```csharp
private void OnTriggerEnter2D(Collider2D collider)
{
   if (collider.TryGetComponent<IInteractable>(out var interactable))
   {
       interactable.OnInteract();
   }
}
```

---

## 8. Constantes e ScriptableObjects

Prefira armazenar valores fixos em ScriptableObjects.
Use constantes apenas para valores realmente imutáveis.
```csharp
[SerializeField] private ScriptableSettings settings;

private const string PLAYER_SAVE_KEY = "Player";
```

---

## 9. Testes Unitários

- Todos os testes devem ser mantidos em pastas separadas do código de produção, em Assembly Definitions editor only.
- Escreva testes unitários em Pascal_Snake_Case
- Adicione Tests no final do nome da classe de testes. Ex.: Player + Tests = PlayerTests
- Use nomes descritivos que expliquem a intenção do teste.
- Cada teste deve validar apenas um comportamento específico.
- Não dependa da ordem de execução entre testes.
- Use Setup e TearDown (ou [SetUp] e [TearDown]) para preparar e limpar o estado global.
- Prefira mocks e stubs em vez de objetos reais sempre que possível.
- Cada método de teste deve seguir o padrão Arrange–Act–Assert, com separação visual entre as etapas.
```csharp
[Test]
public void Add_Item_When_Slot_Is_Empty_Should_Add_Item()
{
    // Arrange (preparar o ambiente e as dependências)
    InventorySystem inventory = new InventorySystem();
    Item item = new Item("Sword");

    // Act (executar o comportamento a ser testado)
    inventory.AddItem(item);

    // Assert (verificar o resultado esperado)
    Assert.IsTrue(inventory.Contains(item));
}
```

---

## 10. Ordem Geral

1. SerializeField
2. Eventos (public - protected - private)
3. Propriedades (public - protected - private)
4. Variáveis (protected - private)
5. Construtores
6. Eventos da Unity
7. Métodos (public - protected - private)

---

## 11. Boas Práticas Gerais

- Evite valores mágicos, use variáveis nomeadas ou constantes.
- Evite singleton sempre que possível.
- Comente apenas o necessário, usando nomes claros no lugar de explicações longas.
- Sempre mantenha o código limpo e legível.
- Só use MonoBehaviour quando necessário, prefira classes puras.
- Use logs e mensagens no console apenas quando necessário, e em inglês.
