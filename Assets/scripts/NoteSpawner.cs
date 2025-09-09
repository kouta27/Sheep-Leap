using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{

    public AudioSource music; //�V�[����AudioSource
    public Transform spawnPoint; //�m�[�c�o���ʒu
    public Transform hitPoint;
    public GameObject notePrefab;
    public float travelTime = 2.0f; //�o������q�b�g�ʒu���B�܂Ŏ���
    public TextAsset noteJson;

    public List<NoteData> notes = new List<NoteData>();

    int nextIndex = 0;

    void Start()
    {
        // JSON��Inspector�Ŏw�肵�Ă��Ȃ����Resources/Notes/MainScene��T��
        //if (noteJson == null)
            //noteJson = Resources.Load<TextAsset>("Notes/MainScene");

        if (noteJson != null)
        {
            NoteDataList wrapper = JsonUtility.FromJson<NoteDataList>(noteJson.text);
            if (wrapper != null && wrapper.notes != null)
                notes = new List<NoteData>(wrapper.notes);
        }

        //���ԂŃ\�[�g���Ă���
        notes.Sort((a, b) => a.time.CompareTo(b.time));
        nextIndex = 0;

        //���y���Đ�
        if (music != null)
            music.Play();
    }

    void Update()
    {
        if (music == null || notes == null || nextIndex >= notes.Count) return;

        float audioTime = music.time; //���݂̍Đ�����
        //���̃m�[�c��spawnTime = note.time - travelTime
        while (nextIndex < notes.Count)
        {
            float spawnTime = notes[nextIndex].time - travelTime;
            if (audioTime >= spawnTime)
            {
                Spawn(notes[nextIndex]);
                nextIndex++;
            }
            else break;
        }
    }

    void Spawn(NoteData data)
    {
        // ���[���ɂ��Y���W�̐؂�ւ�
        float laneY = spawnPoint.position.y;
        if (data.type == 1) laneY += 1.6f;

        Vector3 startPos = new Vector3(spawnPoint.position.x, laneY, spawnPoint.position.z);
        GameObject go = Instantiate(notePrefab, startPos, Quaternion.identity);

        //target��hitPoint�̓m�[�c�̍����ɍ��킹��
        Vector3 target = new Vector3(hitPoint.position.x, laneY, hitPoint.position.z);
        float distance = Mathf.Abs(startPos.x - target.x);
        float speed = 0.0f;
        if (travelTime > 0.0001f) speed = distance / travelTime;

        Note note = go.GetComponent<Note>();
        if (note != null) note.Init(target, speed, data);
    }
}
