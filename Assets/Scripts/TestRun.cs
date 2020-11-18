using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using toio;
using toio.Simulator;

public class TestRun : Agent
{
    CubeManager cubeManager; // キューブマネージャ
    CubeHandle playerHandle; // プレイヤーハンドル
    CubeHandle targetHandle; // ターゲットハンドル
    GameObject playerCube; // プレイヤーキューブ
    GameObject targetCube; // ターゲットキューブ

    bool initFlag = false; // 初期化フラグ

    // 初期化時に呼ばれる
    public override void Initialize()
    {
        cubeManager = new CubeManager();
    }

    public async void Connect()
    {
        // キューブの複数台接続
        await cubeManager.SingleConnect();

        if (cubeManager.handles.Count == 2) {
            // 参照の取得
            this.playerHandle = cubeManager.handles[0];
            this.targetHandle = cubeManager.handles[1];
            this.playerCube = GameObject.FindGameObjectWithTag("PlayerCube");
            this.targetCube = GameObject.FindGameObjectWithTag("TargetCube");

            // LED色の指定
            this.playerHandle.cube.TurnLedOn(255, 0, 0, 0);
            this.targetHandle.cube.TurnLedOn(0, 0, 255, 0);

            // 初期化フラグ
            this.initFlag = true;
        }
    }
 
    // フレーム毎に呼ばれる
    public void FixedUpdate()
    {
        // 初期化前
        if (!this.initFlag) return;

        // プレイヤーキューブの同期
        this.playerCube.transform.localPosition = new Vector3(
            (float)(this.playerHandle.cube.x-250f)/(191f/0.26f), 0f,
            -(float)(this.playerHandle.cube.y-250f)/(191f/0.26f));
        this.playerCube.transform.localRotation = Quaternion.Euler(
            0f, (float)this.playerHandle.deg+90f, 0f);

        // ターゲットキューブの同期
        this.targetCube.transform.localPosition = new Vector3(
            (float)(this.targetHandle.cube.x-250f)/(191f/0.26f), 0f,
            -(float)(this.targetHandle.cube.y-250f)/(191f/0.26f));
        this.targetCube.transform.localRotation = Quaternion.Euler(
            0f, (float)this.targetHandle.deg+90f, 0f);
    }

    // エピソード開始時に呼ばれる
    public override void OnEpisodeBegin()
    {
        // 初期化前
        if (!this.initFlag) return;
    }

    // 行動実行時に呼ばれる
    public override void OnActionReceived(float[] vectorAction)
    {
        // 初期化前 or 操作不可
        if (!this.initFlag ||
            !this.cubeManager.IsControllable(this.playerHandle)) return;

        // プレイヤーの移動
        int action = (int)vectorAction[0];
        this.playerHandle.Update();
        if (action == 0)
        {
            this.playerHandle.Move(100, 0, 100, border:false);
        }
        else if (action == 1)
        {
            this.playerHandle.Move(0, -50, 100, border:false);
        }
        else if (action == 2)
        {
            this.playerHandle.Move(0, 50, 100, border:false);
        }
        else
        {
            this.playerHandle.Move(0, 0, 100, border:false);
        }

        // プレイヤーがターゲットの位置に到着した時
        float distanceToTarget = Vector2.Distance(playerHandle.cube.pos, targetHandle.cube.pos);
        if (distanceToTarget < 30f)
        {
            AddReward(1.0f);
            EndEpisode();
        }

        // プレイヤーがプレイマットから落下した時
        if (this.playerHandle.cube.x <= 50 || 450 < this.playerHandle.cube.x ||
            this.playerHandle.cube.y <= 50 || 450 < this.playerHandle.cube.y)
        {
            EndEpisode();
        }
    }
}