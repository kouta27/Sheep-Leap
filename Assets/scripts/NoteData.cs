using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time; //�q�b�g�\�莞��
    public int type; //�m�[�c��ށi0�����i�^1����i�̂悤�Ɂj
}

[System.Serializable]
public class NoteDataList
{
    public NoteData[] notes;
}