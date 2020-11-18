Hello Toio
===

## Usage
1. Clone this repository.
1. In Unity Editor, Select `Window`> `Package Manager` > `+` > `Add package from git URL...` and add `https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask`.
1. In Unity Editor, Select `Window`> `Package Manager` > `+` > `Add package from disk...` and add `package.json` in ml-agents-release_9/com.unity.ml-agents folder.
1. Change directory to ml-agents-release_9 and Run:
```
mlagents-learn config/sample/HelloToio.yaml --run-id=HelloToio-ppo-1 --time-scale=1
```