using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;

public class ChangeChara : MonoBehaviour {

    //　現在どのキャラクターを操作しているか
    private int nowChara;
    //　操作可能なゲームキャラクター
    [SerializeField]
    private List<GameObject> charaList;

    void Start () {
        //　最初の操作キャラクターを0番目のキャラクターにする
        charaList[0].GetComponent<ThirdPersonController>().enabled = true;
        charaList[1].GetComponent<CharaMove2D>().enabled = false;
    }

    void Update () {
        //　Qキーが押されたら操作キャラクターを次のキャラクターに変更する
        if(Input.GetKeyDown("q")) {
            ChangeCharacter(nowChara);
        }
    }

    //　操作キャラクター変更メソッド
    void ChangeCharacter(int tempNowChara) {
        //　現在操作しているキャラクターを動かなくする
        var controller = charaList[tempNowChara].GetComponent<ThirdPersonController>();
        if (controller != null) {
            controller.enabled = false;
        }
        var move2D = charaList[tempNowChara].GetComponent<CharaMove2D>();
        if (move2D != null) {
            move2D.enabled = false;
        }
        //　次のキャラクターの番号を設定
        var nextChara = tempNowChara + 1;
        if(nextChara >= charaList.Count) {
            nextChara = 0;
        }
        //　次のキャラクターを動かせるようにする
        controller = charaList[nextChara].GetComponent<ThirdPersonController>();
        if (controller != null) {
            controller.enabled = true;
        }
        move2D = charaList[nextChara].GetComponent<CharaMove2D>();
        if (move2D != null) {
            move2D.enabled = true;
        }
        //　現在のキャラクター番号を保持する
        nowChara = nextChara;
    }
}
