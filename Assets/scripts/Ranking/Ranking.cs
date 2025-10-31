using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace RankingSystem
{
    public class Ranking : MonoBehaviour
    {
        // 初期化ベクトル"<半角16文字（1byte=8bit, 8bit*16=128bit>"
        private const string AES_IV_256 = @"gUC@g1XV&yoYW@0L";
        // 暗号化鍵<半角32文字（8bit*32文字=256bit）>
        [Header("暗号化キー(32文字)")]
        [SerializeField] string AES_Key_256 = string.Empty;
        [Header("ランキングURL")]
        [SerializeField] string endpoint = string.Empty;

        /// <summary>
        /// ランキング情報を取得する
        /// </summary>
        /// <param name="receiver">ランキング取得イベントを受信するIRankingReceiverなクラス</param>
        public void GetRanking(IRankingReceiver receiver)
        {
            StartCoroutine(_GetRanking(receiver));
        }

        /// <summary>
        /// スコアを送信する
        /// </summary>
        /// <param name="name">プレイヤー名</param>
        /// <param name="score">スコア</param>
        /// <param name="receiver">ランキング送信イベントを受信するIRankingReceiverなクラス</param>
        public void PostRanking(string name, int score, IRankingReceiver receiver)
        {
            StartCoroutine(_PostRanking(name, score, receiver));
        }


        private IEnumerator _GetRanking(IRankingReceiver receiver)
        {
            UnityWebRequest req = UnityWebRequest.Get(endpoint);
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(req.error);
                receiver.OnRankingLoadError();
            }
            else if (req.responseCode == 200)
            {
                RankingDataStruct rankdatastruct = JsonUtility.FromJson<RankingDataStruct>(req.downloadHandler.text);
                receiver.OnRankingLoadSuccess(rankdatastruct.data);
            }
        }

        private IEnumerator _PostRanking(string name, int score, IRankingReceiver receiver)
        {
            RankingData rankingData = new RankingData();
            rankingData.name = name;
            rankingData.score = score;
            string reqJson = JsonUtility.ToJson(rankingData);
            string data_enc = Encrypt(reqJson);
            byte[] postData = System.Text.Encoding.UTF8.GetBytes($"{{\"data\": \"{data_enc}\"}}");
            var req = new UnityWebRequest(endpoint, "POST");
            req.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
            req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(req.error);
                receiver.OnRankingPostError();
            }
            else if (req.responseCode == 200)
            {
                receiver.OnRankingPostSuccess();
            }
        }

        /// <summary>
        /// 対称鍵暗号を使って文字列を暗号化する
        /// </summary>
        /// <param name="text">暗号化する文字列</param>
        /// <param name="iv">対称アルゴリズムの初期ベクター</param>
        /// <param name="key">対称アルゴリズムの共有鍵</param>
        /// <returns>暗号化された文字列</returns>
        private string Encrypt(string text)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            // ブロックサイズ（何文字単位で処理するか）
            myRijndael.BlockSize = 128;
            // 暗号化方式はAES-256を採用
            myRijndael.KeySize = 256;
            // 暗号利用モード
            myRijndael.Mode = CipherMode.CBC;
            // パディング
            myRijndael.Padding = PaddingMode.PKCS7;

            myRijndael.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            myRijndael.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            // 暗号化
            ICryptoTransform encryptor = myRijndael.CreateEncryptor(myRijndael.Key, myRijndael.IV);

            byte[] encrypted;
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream ctStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(ctStream))
                    {
                        sw.Write(text);
                    }//using
                    encrypted = mStream.ToArray();
                }//using
            }//using
             // Base64形式（64種類の英数字で表現）で返す
            return (System.Convert.ToBase64String(encrypted));
        }//Encrypt
    }

    [Serializable]
    public struct RankingDataStruct
    {
        public RankingData[] data;
    }

    [Serializable]
    public struct RankingData
    {
        public string name;
        public int score;
    }
}
