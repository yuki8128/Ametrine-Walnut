> このファイルは、同じ信頼境界を持つリポジトリ群（グループ F）で共通の脅威モデルです。Devin のセキュリティスキャンが所見を評価する際の前提になります。デプロイ形態や想定する利用者が変わったら更新してください。

# 脅威モデル F: オフラインゲーム・ライブラリ・Mod

対象: Ametrine-Walnut / UPM-test（Unity ライブラリ）, Hauynite-FREED / Sphalerite-Asahi / Sphalerite-Slackware（Unity ゲーム）, ReAcademyCraft（Minecraft NeoForge 1.21.4 Mod）, beatsaber-nps-sort（Beat Saber Mod + C/C++ ネイティブコア）, reat-stream（Godot Web/ネイティブ音ゲー、利用者の曲・スキンパック読み込み）, town-crew（Godot 街づくり）。
## 信頼境界・攻撃者
サーバーなし（ReAcademyCraft は Minecraft マルチプレイサーバー上でクライアント→サーバーパケットを受ける）。脅威は悪意あるセーブ/曲/スキン/マスターデータ、マルチプレイの悪意あるプレイヤー、ビルドスクリプト。
## 資産
プレイヤー端末のファイル、Minecraft サーバーの整合性、リポジトリ内の秘密情報（ユーザーが最重要と回答）。
## 対象外・受容リスク
（ユーザー回答なし。全攻撃者を想定）
## コンポーネント
- `Unity Libraries`: Ametrine-Walnut, UPM-test
- `Unity Games`: Hauynite-FREED, Sphalerite-Asahi, Sphalerite-Slackware
- `ReAcademyCraft Mod`: `ReAcademyCraft/src`
- `NPS Sort Mod`: `beatsaber-nps-sort/`
- `reat-stream`: `reat-stream/`
- `town-crew`: `town-crew/`

## ユーザーが示した優先度（全グループ共通）
- 最も守りたい資産（ユーザー回答）: **課金・ウォレット（不正付与・二重付与）** と **サーバー/VPS の認証情報・API キー**。
- タグ: `Component`（下記コンポーネント名）, `Group`（A〜F）。

## インフラ
独立したインフラリポジトリはなし。各リポジトリ内の Dockerfile / docker-compose / fly.toml を参照。
