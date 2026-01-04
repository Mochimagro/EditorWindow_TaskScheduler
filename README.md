# WorkboardMini

Unity Editor 拡張（UI Toolkit / UIElements）の実装実績を示すために作成した、**軽量タスク管理用のサンプル拡張ウィンドウ**です。

本リポジトリは、転職・業務委託案件・エージェント共有を目的としており、
**「UI Toolkit を使った Editor 拡張を実務レベルで実装できるか」**が一目で分かる構成になっています。

---

## 🎯 目的

* UIElements / UI Toolkit を使った **Unity Editor 拡張の実装能力**を証明する
* UXML / USS / ListView / TwoPaneSplitView など、
  **案件で頻出する要素を一通り網羅**する
* 実務を想定した

  * データ永続化（ScriptableObject）
  * Undo 対応
  * Dirty 管理
  * Inspector 風 UI
    を含める

---

## 🧩 機能概要

* タスクの一覧表示（ListView / 仮想化）
* タスクの追加 / 複製 / 削除
* TwoPaneSplitView による

  * 左：タスク一覧
  * 右：詳細編集 UI
* EnumField を使った

  * ステータス（Todo / Doing / Done）
  * 優先度（Low / Mid / High）
* TextField（複数行）によるメモ編集
* 検索フィルタ（タイトル・メモ部分一致）
* ScriptableObject によるデータ永続化
* Undo / Redo 対応

---

## 🖥 対応 Unity バージョン

* Unity **2022.3 LTS** 以降
* UI Toolkit（Editor 拡張）前提

---

## 🚀 起動方法

1. 本リポジトリを Unity プロジェクトに配置
2. Unity Editor を起動
3. メニューから以下を選択

```
Tools > Workboard (Mini)
```

初回起動時、必要な ScriptableObject データが自動生成されます。

---

## 🏗 使用している主な技術要素

### UI Toolkit / UIElements

* EditorWindow + UI Toolkit
* UXML / USS による View 定義
* TwoPaneSplitView
* ListView（makeItem / bindItem）
* EnumField / TextField / Toolbar / Button

### Editor 拡張実装

* ScriptableObject によるデータ管理
* Undo.RecordObject を用いた Undo 対応
* EditorUtility.SetDirty による Dirty 管理
* SetValueWithoutNotify を使った安全な UI 更新

---

## 📁 フォルダ構成

```
Assets/
  WorkboardMini/
    Editor/
      WorkboardMiniWindow.cs
      Model/
        WorkboardMiniData.cs
        TaskItem.cs
      View/
        WorkboardMiniWindow.uxml
        WorkboardMiniWindow.uss
    README.md
```

* **Model / View / Window（Controller）** を最低限分離
* 実務を意識した構成にしています

---

## 🧠 実装上のポイント（アピール要素）

* ListView の `itemsSource` / `makeItem` / `bindItem` を正しく分離
* EnumField の ChangeEvent を使った enum バインド
* ListView 選択切り替え時に

  * `SetValueWithoutNotify` を使用
  * 無限ループ・誤発火を防止
* データ → UI / UI → データ の責務分離

---

## 🧪 想定ユースケース

* Unity Editor 拡張のサンプル実装
* UI Toolkit 学習用
* 転職・業務委託案件でのポートフォリオ提示

---

## 📌 補足

本プロジェクトは **短時間（約4時間）での実装**を前提に設計されています。
そのため、設計・実装ともに

* 「過度に作り込みすぎない」
* 「だが実務では十分に評価される」

というバランスを意識しています。

---

## 👤 Author

* Unity Engineer / Editor 拡張実装
* UI Toolkit / UIElements 実務経験あり

---

## 📄 License

This project is released under the MIT License.
