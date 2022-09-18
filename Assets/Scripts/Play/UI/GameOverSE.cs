using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームオーバーになった際に音を鳴らすComponent。
// Colliderと共に設定し、タグを専用のものにする。
[RequireComponent(typeof(Collider2D))]
public class GameOverSE : SoundBeep
{
}
