Hello Toio
===

## Prepare
- Install ML-Agents Release 9 in following URL and put `ml-agents-release_9` folder in Unity project.
https://github.com/Unity-Technologies/ml-agents/releases/tag/release_9

- In Anaconda Prompt:
```
conda create -n mlagents9 python==3.7
pip install torch -f https://download.pytorch.org/whl/torch_stable.html
cd ml-agents-release_9
pip install -e ./ml-agents-envs
pip install -e ./ml-agents
```

## Usage
1. Clone this repository.
1. In Unity Editor, Select `Window`> `Package Manager` > `+` > `Add package from git URL...` and add `https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask`.
1. In Unity Editor, Select `Window`> `Package Manager` > `+` > `Add package from disk...` and add `package.json` in ml-agents-release_9/com.unity.ml-agents folder.
1. Change directory to ml-agents-release_9 and Run:
```
mlagents-learn config/sample/HelloToio.yaml --run-id=HelloToio-ppo-1 --time-scale=1
```