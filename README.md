# Native Game State Machine

Unity向けのシンプルで軽量なステートマシンライブラリです。外部依存なしでゲームの進行管理を簡単に実装できます。

## 特徴

- 🚀 **軽量**: 外部ライブラリを一切使用しない純粋なC#実装
- 🔄 **シンプル**: 最小限のAPIで直感的に使える
- 💾 **効率的**: ステートをキャッシュし、無駄なインスタンス生成を防ぐ
- 🎮 **Unity最適化**: Unity 6000.0以降に対応
- 🔌 **即使用可能**: 自動初期化により設定不要で使い始められる

## インストール

### Unity Package Manager経由

1. Package Managerを開く（Window > Package Manager）
2. 「+」ボタンをクリック
3. 「Add package from git URL...」を選択
4. 以下のURLを入力:
```
https://github.com/Yuffter/UnityGameStateMachine.git?path=/Assets/GameStateMachine
```

### 手動インストール

1. このリポジトリをクローンまたはダウンロード
2. `Assets/GameStateMachine`フォルダをプロジェクトにコピー

## 使い方

### 基本的な使い方

#### 1. ステートクラスを作成

`IState`インターフェースを実装するか、`StateBase`クラスを継承してステートを作成します。

```csharp
using Yuffter.GameStateMachine;

public class TitleState : StateBase
{
    public override void Enter()
    {
        Debug.Log("タイトル画面に入りました");
        // タイトル画面のUIを表示する処理など
    }

    public override void Update()
    {
        // タイトル画面での毎フレームの処理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameStateMachine.Instance.ChangeState<GamePlayState>();
        }
    }

    public override void Exit()
    {
        Debug.Log("タイトル画面を離れました");
        // タイトル画面のUIを非表示にする処理など
    }
}

public class GamePlayState : StateBase
{
    public override void Enter()
    {
        Debug.Log("ゲームプレイ開始");
        // ゲーム開始時の処理
    }

    public override void Update()
    {
        // ゲームプレイ中の処理
    }

    public override void Exit()
    {
        Debug.Log("ゲームプレイ終了");
        // ゲーム終了時の処理
    }
}
```

#### 2. 初期ステートを設定

任意のスクリプトから初期ステートを設定します。

```csharp
using UnityEngine;
using Yuffter.GameStateMachine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // タイトル画面から開始
        GameStateMachine.Instance.SetInitialState<TitleState>();
    }
}
```

#### 3. ステートを遷移

`ChangeState<T>()`メソッドでステートを切り替えます。

```csharp
// ゲームプレイステートに遷移
GameStateMachine.Instance.ChangeState<GamePlayState>();

// タイトルステートに戻る
GameStateMachine.Instance.ChangeState<TitleState>();
```

### 現在のステートを取得

```csharp
IState currentState = GameStateMachine.Instance.CurrentState;
Debug.Log($"現在のステート: {currentState.GetType().Name}");
```

## API リファレンス

### GameStateMachine

シングルトンパターンで実装されたステートマシン本体。

#### プロパティ

- `static GameStateMachine Instance` - シングルトンインスタンス
- `IState CurrentState` - 現在のステート（読み取り専用）

#### メソッド

- `void SetInitialState<T>() where T : IState`
  - 初期ステートを設定します
  - 通常はゲーム開始時に一度だけ呼び出します

- `void ChangeState<T>() where T : IState`
  - 指定したステートに遷移します
  - 現在のステートと同じステートを指定した場合は何もしません
  - 前のステートの`Exit()`を呼び、新しいステートの`Enter()`を呼びます

### IState

ステートが実装すべきインターフェース。

#### メソッド

- `void Enter()` - このステートに入ったときに呼ばれます
- `void Update()` - このステートの間、毎フレーム呼ばれます
- `void Exit()` - このステートから出るときに呼ばれます

### StateBase

`IState`インターフェースの基本実装を提供する抽象クラス。すべてのメソッドは`virtual`なので、必要なものだけオーバーライドできます。

## 実装例

### ゲームフロー管理

```csharp
public class TitleState : StateBase
{
    public override void Enter()
    {
        // タイトルUIを表示
        UIManager.Instance.ShowTitle();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameStateMachine.Instance.ChangeState<GamePlayState>();
        }
    }

    public override void Exit()
    {
        // タイトルUIを非表示
        UIManager.Instance.HideTitle();
    }
}

public class GamePlayState : StateBase
{
    public override void Enter()
    {
        // ゲームを初期化
        GameController.Instance.Initialize();
    }

    public override void Update()
    {
        // ゲームロジック実行
        if (GameController.Instance.IsGameOver())
        {
            GameStateMachine.Instance.ChangeState<GameOverState>();
        }
    }

    public override void Exit()
    {
        // ゲームをクリーンアップ
        GameController.Instance.Cleanup();
    }
}

public class GameOverState : StateBase
{
    public override void Enter()
    {
        // ゲームオーバーUIを表示
        UIManager.Instance.ShowGameOver();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameStateMachine.Instance.ChangeState<GamePlayState>();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateMachine.Instance.ChangeState<TitleState>();
        }
    }
}
```

## 技術的な詳細

### 自動初期化

`RuntimeInitializeOnLoadMethod`属性を使用して、シーン読み込み前に自動的にGameStateMachineを初期化します。手動でGameObjectに追加する必要はありません。

### ステートキャッシュ

一度作成されたステートはキャッシュされ、再利用されます。これにより、頻繁なステート遷移でもパフォーマンスが維持されます。

### シングルトン

GameStateMachineはシングルトンパターンで実装されており、どこからでも`GameStateMachine.Instance`でアクセスできます。

## 必要環境

- Unity 6000.0 以降
- .NET Standard 2.1 以降

## ライセンス

MITライセンス - 詳細は[LICENSE](LICENSE)ファイルを参照してください。

## 貢献

プルリクエストや Issue の報告を歓迎します！

## 作者

Yuffter

## リンク

- [GitHub Repository](https://github.com/Yuffter/UnityGameStateMachine)
