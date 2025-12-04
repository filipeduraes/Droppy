# Gota

Protótipo de jogo educativo sobre uso consciente da água para um mundo em emergência climática. A experiência combina minijogos 2D que acompanham o ciclo da “gotinha”, reforçando redução de consumo, combate ao desperdício e reaproveitamento.

## Fases jogáveis
- **A Grande Chuva**: vertical scroller inicial; desvie de poluição e atravesse filtros para manter a pureza da água.
- **Corrida Contra o Desperdício**: feche torneiras abertas e reaproveite água em baldes antes que o nível acabe.
- **Estação de Tratamento**: gire/organize canos para conduzir o fluxo sem vazamentos; conecte entradas e saídas para ganhar mais estrelas.
- **Rumo às Nuvens**: seção de evaporação; suba evitando poluentes e coletando energia para concluir o ciclo.

## Controles (padrão)
- Movimento: `WASD`, setas, direcional ou analógico direito.
- Pulo: `Espaço`/`W`/seta para cima ou clique esquerdo (nos trechos com pulo).
- Interação: `E` ou botão norte do gamepad (fechar torneiras, interagir com peças).
- Mouse: mira/clique para UI e puzzles que usam ponteiro.

## Como executar
1. Instale o Unity **2022.3.62f3** (LTS). Projeto usa o Input System novo e URP.
2. Abra o diretório do repositório no Unity Hub e carregue a pasta raiz (`Droppy`).
3. Abra a cena `Assets/Scenes/MainMenu.unity` (fluxo completo) ou `Assets/Scenes/Gameplay.unity` (sequência de fases) e pressione Play.
4. Para build, use `File > Build Settings`, adicione as cenas acima e gere para a plataforma desejada.

## Estrutura relevante
- Cenas: `Assets/Scenes/` (`MainMenu.unity`, `Gameplay.unity`).
- Scripts principais: `Assets/Scripts/`
  - `Level/`: orquestra a sequência de fases e telas de introdução.
  - `VerticalScrollerMinigame/`: controladores do scroller da chuva/evaporação.
  - `FaucetsMinigame/`: lógica das torneiras e medidor de nível de água.
  - `PieceMinigame/`: puzzle de encanamento (fluxo, peças, grade).
  - `Player/`, `Input/`, `Obstacle/`, `SpawnSystem/`, `StatSystem/`, `UI/`: controle do personagem, entrada, obstáculos, spawns, stats e telas de fim/introdução.
- Dados configuráveis: `Assets/Data/`
  - `LevelIntroductions/` e `LevelQuotes/`: textos de narrativa, dicas e falas de vitória/derrota por fase.
  - `Stats/`: `Pureza`, `Nível de água` e `Timer` usados nos cálculos de estrelas.
  - `VerticalMinigame/` e `PipesMinigame/`: ScriptableObjects para spawn, peças e fluxo.

## Métricas e feedback
- Sistema de estrelas por fase via `EndScreenViewModel` (1–3 estrelas, com mensagens temáticas).
- `StatSystem` persiste e limita valores (pureza, nível de água, tempo). Modificações vêm de obstáculos, vazamentos, coletas e timers.

## Adaptações rápidas
- Textos/ilustrações de introdução: edite os assets em `Assets/Data/LevelIntroductions/`.
- Frases de resultado: ajuste `Assets/Data/LevelQuotes/`.
- Dificuldade/spawn: altere `SpawnerData` em `Assets/Data/VerticalMinigame/` ou parâmetros de torneiras/canos nos prefabs correspondentes.
