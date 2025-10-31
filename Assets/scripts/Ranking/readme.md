# ランキングシステム使い方

## セットアップ方法

1. シーン上に空オブジェクトを作成し、Rankingコンポーネントをアタッチする。
2. Rankingコンポーネント内の暗号化キーとランキングURLを設定する  
(Discordのmune1012にDMしてチーム名(半角英小文字)とランキングデータ名(半角英小文字)を送ってもらえれば提供します。難易度別など複数のランキングが必要な場合はその数だけランキングデータ名が必要; 例 race_joycon, craft_easy)
3. 新しくC# Scriptを作成する。
4. 作成したスクリプトの先頭に以下のコードを書く。
```csharp
using RankingSystem
```
5. 作成したスクリプトにIRankingReceiverインターフェースを継承させる。

例
```csharp
public class RankingManager : MonoBehaviour, IRankingReceiver
```
6. スクリプトに以下の関数を定義する。これらの関数はイベント関数となっており、ランキングの取得や投稿が完了した際に呼び出される。

- ランキングの取得が成功したときに呼ばれるイベント関数
```csharp
public void OnRankingLoadSuccess(RankingData[] datas)
```
- ランキングの取得に失敗したときに呼ばれるイベント関数
```csharp
public void OnRankingLoadError()
```

- ランキングの投稿に成功したときに呼ばれるイベント関数
```csharp
public void OnRankingPostSuccess()
```

- ランキングの投稿に失敗したときに呼ばれるイベント関数
```csharp
public void OnRankingPostError()
```

## RankingData構造体について
| メンバ名 | 説明 | 型 |
| ---- | ---- | ---- |
| name | 記録の樹立者名 | string |
| score | スコア(代わりに時間ミリ秒にすることも可) | int |

## Rankingクラスの関数
- ランキングを取得する関数
```csharp
ranking.GetRanking(IRankingReceiver receiver);
```
| 引数 | 説明 | 型 |
| ---- | ---- | ---- |
| receiver | イベント関数の呼び出し先のIRankingReceiverを継承したクラス。基本的にthisを指定すればよい。 | IRankingReceiver |

取得に成功または失敗すると上述のイベント関数が実行される。成功した場合は引数に結果が格納される。

- ランキングを投稿する関数
```csharp
ranking.PostRanking(string name, int score, IRankingReceiver receiver);
```
| 引数 | 説明 | 型 |
| ---- | ---- | ---- |
| name | 記録の樹立者名 | string |
| score | スコア(または時間ミリ秒) | int |
| receiver | イベント関数の呼び出し先のIRankingReceiverを継承したクラス。基本的にthisを指定すればよい。 | IRankingReceiver |

## Tips
- 複数のランキングを用意したい場合はRankingコンポーネントをアタッチしたオブジェクトを複数用意し、endpointを別々で設定することで対応できる。

## サンプルコード
```csharp
using RankingSystem;
using UnityEngine;

public class RankingManager : MonoBehaviour, IRankingReceiver
{
    [SerializeField] Ranking ranking;
    void Start()
    {
        // ランキングを取得する
        ranking.GetRanking(this);
    }

    public void TestRank()
    {
        // ランキングを投稿する
        ranking.PostRanking("test", 97, this);
    }

    // ランキングの読み込みに成功したときに呼び出される
    public void OnRankingLoadSuccess(RankingData[] datas)
    {
        // ランキング情報をログに表示する
        for (int i = 0; i < datas.Length; i++)
        {
            Debug.Log($"{datas[i].name}, {datas[i].score}");
        }
    }

    // ランキングの取得に失敗したときに呼び出される
    public void OnRankingLoadError()
    {
        Debug.Log("load failed!");
    }

    // ランキングの投稿に成功したときに呼び出される
    public void OnRankingPostSuccess()
    {
        Debug.Log("post succeeded!");
    }

    // ランキングの投稿に失敗したときに呼び出される
    public void OnRankingPostError()
    {
        Debug.Log("post failed!");
    }
}

```