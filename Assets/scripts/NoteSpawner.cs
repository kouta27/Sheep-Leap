using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{

    public AudioSource music; //シーンのAudioSource
    public Transform spawnPoint; //ノーツ出現位置
    public Transform hitPoint;
    public GameObject notePrefab;
    public float travelTime = 2.0f; //出現からヒット位置到達まで時間
    public TextAsset noteJson;

    public List<NoteData> notes = new List<NoteData>();

    int nextIndex = 0;

    void Start()
    {
        // JSONをInspectorで指定していなければResources/Notes/MainSceneを探す
        //if (noteJson == null)
            //noteJson = Resources.Load<TextAsset>("Notes/MainScene");

        if (noteJson != null)
        {
            NoteDataList wrapper = JsonUtility.FromJson<NoteDataList>(noteJson.text);
            if (wrapper != null && wrapper.notes != null)
                notes = new List<NoteData>(wrapper.notes);
        }

        //時間でソートしておく
        notes.Sort((a, b) => a.time.CompareTo(b.time));
        nextIndex = 0;

        //音楽を再生
        if (music != null)
            music.Play();
    }

    void Update()
    {
        if (music == null || notes == null || nextIndex >= notes.Count) return;

        float audioTime = music.time; //現在の再生時間
        //次のノーツのspawnTime = note.time - travelTime
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
        // レーンによるY座標の切り替え
        float laneY = spawnPoint.position.y;
        if (data.type == 1) laneY += 1.6f;

        Vector3 startPos = new Vector3(spawnPoint.position.x, laneY, spawnPoint.position.z);
        GameObject go = Instantiate(notePrefab, startPos, Quaternion.identity);

        //targetはhitPointはノーツの高さに合わせる
        Vector3 target = new Vector3(hitPoint.position.x, laneY, hitPoint.position.z);
        float distance = Mathf.Abs(startPos.x - target.x);
        float speed = 0.0f;
        if (travelTime > 0.0001f) speed = distance / travelTime;

        Note note = go.GetComponent<Note>();
        if (note != null) note.Init(target, speed, data);
    }
}
