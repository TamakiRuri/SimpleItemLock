# Simple Item Lock

![Sample](./Sample.png)

Simple Item Lock はVRChatワールドで、アイテムを特定の人にしか触れない、または見えないようにするギミックです。

オブジェクト本体、またはコライダーに動作するため、ボタンやテレポーターなどにも動作します。

Simple Item Lock is a simple way to make your item being used or seen by using a white list.

It works with game objects and colliders, so items like buttons and teleporters will also work.

### 特徴 / Features

コライダーモード、無効化モード選択可能 / Collider mode and disable mode available.

選べるインスタンスオーナー許可 / Allow instance owner option available.

2つ以上共存可能 / Support for multiple locks to be used at same time.

Join時に実行 / Run at join.

パフォーマンス影響小 / Low performance cost.

Wall Mode: 指定した人だけがぬける壁（コライダー）などを作れます。

Use wall mode to make whitelisted players to go through certain walls etc.

### 注意事項 / Limitations

**Advanced Prefab はUnpackしてからご利用ください。自動導入はPrefabに動作しません。**

これは、Prefabのユーザー変更されていないフィールドにスクリプトでデータを入力しても保存されないためです。

ジョイン時に実行されるため、ターゲットオブジェクトをスイッチでオンにするロックが解除されます。

そのため、ワールドでユーザーを追加したり、削除したりすることができません。

ただし、グローバルボタンをロックして、許可されたユーザーが適切な場合で利用することで、自由にオンオフできます。

この場合では、そのスイッチオフにすればオブジェクトがまたロックされます。

同じオブジェクトに複数のItem Lockに登録した場合はサポート対象外です。

Beta-b4 から、Stack Overflowの影響で、導入ツールを削除しました。

RC-b7 から、変数名の変更がありますので、更新する時に必ずバックアップを取ってからImportしてください。

**Please unpack the advanced prefab before using it. Auto generate won't work in prefabs.**

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

右下の+マークを押しユーザー名を入力します。User Nameにあるすべてのユーザーがこのオブジェクトを操作できます。

Using the + mark at bottom right corner and input usernames for whitelisted users.

アイテムリストを作ります。Target Object は対象アイテムです。Action Mode 0では許可されないユーザーがオブジェクトを見えなくなり（無効化モード）、1では動かせないようになります。Allow Instance Ownerを有効にすれば、インスタンスを作った人が許可されます。WallModeでは、指定した人だけがぬける壁（コライダー）などが作れます。

Create the item list. At action mode 0 only whitelisted users can see the object, and at 1 only they can move the object. Use wall mode to make whitelisted players to go through certain walls etc.

**Advanced PrefabではGenerate Dataを押す必要があります。この作業は毎回編集する時に必要になります。**

**Click the Generate Data Button for Advanced Prefab. This should be done for every time the list is edited.**

Advancedバージョンでは、ユーザー名コピー機能があります。他のPrefab（非Advanced含む）またはItemLockBasicがついているオブジェクトを下のTarget LockにいれてCopy Usernamesを押すとユーザー名が入れたオブジェクトにコピーされます。

In the Advanced prefab, you can put any other objects with ItemLockBasic or Item Lock Prefabs (Including non-advanced ones) and click Copy Usernames to copy the stored usernames to the target object.

#### 一つオブジェクトのみ利用する (Basic) / Use single object script (Basic)

対象オブジェクトにItemLockBasicというスクリプトをD&Dします。

Drag and drop ItemLockBasic script to target object.

右下の+マークを押してユーザー名を入力してください。User Nameにあるすべてのユーザーがこのオブジェクトを操作できます。

Using the + mark at bottom right corner and input usernames for whitelisted users.

Action Mode 0では許可されないユーザーがオブジェクトを見えなくなり（無効化モード）、1では動かせないようになります。Allow Instance Ownerを有効にすれば、インスタンスを作った人が許可されます。Wall Modeでは、特定の人しか通れない壁が作れます。

At action mode 0 only whitelisted users can see the object, and at 1 only they can move the object. Use wall mode to make whitelisted players to go through certain walls or use teleporters etc.

<!-- #### (非推奨 / Not Recommended)導入ツールを利用する / Use the import tool

**UIToolkitのバグより、2つ以上のアイテム追加するとUnityがフリーズ、または落ちる可能性があります。必ず前のアイテムに何かを入力してから次のアイテムを追加してください。**

**Because of a bug in UIToolkit, adding more than 1 item at once may cause Unity to freeze or crash. FILL THE LAST THING YOU ADDED BEFORE ADDING ANOTHER ITEM**

ツールバーのToolsタブで Studio Saphir/Item Lock Settings を開きます。

Open Tools/Studio Saphir/Item Lock Settings at tool bar.

ユーザー名を入力します。入力できない場合では、右下の+マークを押してください。User Nameにあるすべてのユーザーがこのオブジェクトを操作できます。

Using the + mark at bottom right corner and input usernames for whitelisted users.

アイテムリストを作ります。Target Object は対象アイテムです。Action Mode 0では許可されないユーザーがオブジェクトを見えない（無効モード）、1では動かせないです。Allow Instance Ownerを有効にすれば、インスタンスを作った人が許可されます。WallModeでは、指定した人だけがぬける壁（コライダー）、使えるテレポーターなどを作れます。

Create the item list. At action mode 0 only whitelisted users can see the object, and at 1 only they can move the object. Use wall mode to make whitelisted players to go through certain walls or use teleporters etc.

**一番上のGenerate Dataを押す。この作業は毎回編集する時に必要になります。**

**Click the Generate Data Button at top. This should be done for every time the list is edited.**

ItemLockCenter(Managed)がシーンの中で配置されます。（動かさないようにしてください）Advanced Prefabと同じ方法で編集できます。

ItemLockCenter(Managed) will be placed at the scene. (Do not move this object) You can edit this using the same ways as advanced prefabs. -->