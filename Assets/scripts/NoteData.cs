using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time; //ヒット予定時間
    public int type; //ノーツ種類（0＝下段／1＝上段のように）
}

[System.Serializable]
public class NoteDataList
{
    public NoteData[] notes;
}