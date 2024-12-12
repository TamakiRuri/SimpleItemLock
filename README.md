# Simple Item Lock

![Sample](./Sample.png)

Simple Item Lock はVRChatワールドで、アイテムを特定の人にしか触れない、または見えないようにするギミックです。

オブジェクト本体、またはコライダーに動作するため、ボタンやテレポーターなどにも動作します。

Simple Item Lock is a simple way to prevent your item being used or seen by using a white list.

It works with game objects and colliders, so items like buttons and teleporters will also work.

### 特徴 / Features

2つ以上共存可能 / Support for multiple locks to be used at same time.

Join時に実行 / Run at join.

パフォーマンス影響小 / Low performance cost.

### 説明 / Information

#### Mode 0 - 無効化モード/Disable Mode

許可されていないプレイヤーにはオブジェクトが無効化され、存在しないようになります。Object Syncの場合ではTransformが同期されますが、許可されていないプレイヤーからはアイテムが見えません。ただし、スクリプトも同期されなくなり当たり判定も消えます。

範囲: オブジェクト及び子オブジェクト（子オブジェクト自体は無効化されませんが、親オブジェクトが無効化された影響で無効になります。）

使用例: スタッフ限定で、一般プレイヤーに表示する必要のないアイテムをロックする。たとえば、スタッフ専用エリア行きのテレポーター。

Only for whitelisted players the object is enabled. For un-whitelisted players, the object can be seen as non-existent until it's unlocked.

Target: target object and child objects. (Child objects won't be directed disabled. But it will appear to be disabled because the parent object is disabled.)

#### Mode 1 - コライダーモード/Collider Mode

許可されていないプレイヤーにはオブジェクトのコライダーが無効化され、インタラクトできなくなります。それ以外のスクリプトは正常に同期されます。ただし、アイテムの当たり判定もなくなりますので、ドアなどに利用する場合では追加のコライダーが必要です。

範囲: オブジェクト自身（子オブジェクトを含まず）（オブジェクトにコライダーがある必要があります。子オブジェクトにある場合では、子オブジェクトを入れてください。）

使用例: スタッフ用のアイテムで、同期するパラメータがある場合や、アイテム自身を隠す必要がない場合でアイテムをロックする。たとえば、プレイヤーを掴めるためのギミックをロックする。

Only whitelisted users will be able to interact or grab the item. The Collision will also disappear. So if you're going to place this in a door, please add another collider to make sure other players can't go through it directly.

Target: target object itself. Child objects won't be affected. The target object should have collider directly attached.

#### インスタンスオーナー許可モード / Allow instance owner

インスタンスを立てたプレイヤーを許可します。

使用例: 公開ワールドでの利用や、ユーザー名追加し忘れがある場合での救済措置など。

Allow the player who create this instance to use the item.

#### 壁モード / Wall Mode

動作が逆になります。（Mode 0 の場合: 許可されていないプレイヤーにオブジェクトが表示されるが、許可されたプレイヤーに表示されない）

許可されたプレイヤーだけがぬける壁（コライダー）などを作れます。

Use wall mode to make whitelisted players to go through certain walls etc.


### 注意事項 / Limitations

いたずら防止のためのギミックです。すべての状況に対応するものではありません。

**Advanced Prefab はUnpackしてからご利用ください。Prefabのままでは正常に動作しません。**

これは、Prefabにあるユーザーに変更されていないフィールドに、スクリプトでデータを入力しても保存されないためです。

ジョイン時に実行されるため、ターゲットオブジェクトをスイッチでオンにするロックが解除されます。

そのため、ワールドでユーザーを追加したり、削除したりすることができません。

ただし、グローバルボタンをロックして、許可されたユーザーが適切な場合で利用することで、自由にオンオフできます。

この場合では、そのスイッチオフにすればオブジェクトがまたロックされます。

同じオブジェクトを複数のItem Lockに登録した場合はサポート対象外です。

Beta-b4 から、Stack Overflowの影響で、導入ツールを削除しました。

RC-b7 から、変数名の変更がありますので、更新する時に必ずバックアップを取ってからImportしてください。

This is designed to prevent unwanted pranks. This may not work for all types of attacks.

**Please unpack the advanced prefab before using it. Generate Data function won't work if it's not unpacked.**

This is because, in Unity, adding something to a unchanged prefab with a script won't be saved.

This script will be run at join, thus enabling the targets object with switches will unlock the object.

Therefore, adding or deleting whitelisted users in VRChat is not supported.

However, by locking a global switch and letting whitelisted users using the switch when appropriate, you can enable or disable objects according to your needs.

In this situation, you can lock your items agin by using the switch again.

Using multiple Item Locks in the same object isn't something we tested, nor what we plan to support.

From Beta-b4, due to the Stack Overflow issue, the import tool is removed.

From RC-b7, some variable names are changed. So in case of an update, please make sure you have a backup.

### 導入

2種類の導入方法があります。 / There are 2 ways to import this gimmick.

準備 / Preparation

1. Release でunitypackageをダウンロードします。 / Download at Release page.

2. Unityの**ワールド**プロジェクトに導入します。 / Import to a **World** project.

#### Prefabを利用する / Use the prefab

2種類のPrefabがあります。 Advancedでは、アイテム一つ一つでインスタンスオーナーの許可を編集したり、モードを選択したりすることができますが、毎回Generate Dataを押す必要があります。Advanced でないItem Lockでは、すべてのオブジェクトが同じ設定になります。設定の手順はほぼ同じですが、Advanced PrefabはUnpackする必要があります。

There are two types of prefabs. Advanced prefab allow editing modes and the options for allowing instance owner for each object, with a drawback of requiring clicking the Generate Data button every time the object is edited. The prefab without Advanced will let all target objects have the same settings. However, advanced prefab should be unpacked if you want to use it.

右下の+マークを押しユーザー名を入力します。Usernamesにあるすべてのユーザーがオブジェクトを操作できます。

Using the + mark at bottom right corner and input usernames for whitelisted users.

アイテムリストを作ります。Target Object は対象アイテムです。Modeなどの説明は上にある説明にあります。

Create the item list. Information about modes and other settings are above.

**Advanced PrefabではGenerate Dataを押す必要があります。この作業は毎回編集する時に必要になります。**

**Click the Generate Data Button for Advanced Prefab. This should be done for every time the list is edited.**

Advancedバージョンでは、ユーザー名コピー機能があります。他のPrefab（非Advanced含む）またはItemLockBasicがついているオブジェクトを下のTarget LockにいれてCopy Usernamesを押すとユーザー名が入れたオブジェクトにコピーされます。

In the Advanced prefab, you can put any other objects with ItemLockBasic or Item Lock Prefabs (Including non-advanced ones) and click Copy Usernames to copy the stored usernames to the target object.

#### 一つオブジェクトのみ利用する (Basic) / Use single object script (Basic)

対象オブジェクトにItemLockBasicというスクリプトをD&Dします。

Drag and drop ItemLockBasic script to target object.

右下の+マークを押してユーザー名を入力してください。Usernamesにあるすべてのユーザーがこのオブジェクトを操作できます。

Using the + mark at bottom right corner and input usernames for whitelisted users.

アイテムリストを作ります。Target Object は対象アイテムです。Modeなどの説明は上にある説明にあります。

Create the item list. Information about modes and other settings are above.