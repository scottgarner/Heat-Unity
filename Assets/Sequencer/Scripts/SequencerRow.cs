using System;
using System.Collections;
using System.Collections.Generic;
using AudioHelm;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class NoteConfig
{
    public int note = -1;
    public Color color = Color.black;
}

public class SequencerRow : MonoBehaviour
{

    public int rowIndex = 0;

    public AudioHelm.HelmSequencer helmSequencer;
    public AudioHelm.SampleSequencer sampleSequencer;

    public List<NoteConfig> noteConfigs;
    private NoteConfig[] sequenceConfigs;

    //

    private void Start()
    {

        sequenceConfigs = new NoteConfig[8];

        if (helmSequencer)
        {
            helmSequencer.beatEvent.AddListener(BeatOn);
        }

        if (sampleSequencer)
        {
            sampleSequencer.beatEvent.AddListener(BeatOn);
        }

    }

    public void Edit(int columnIndex, int noteIndex)
    {

        NoteConfig noteConfig = noteConfigs[noteIndex];

        // Sequencer

        if (helmSequencer)
        {

            List<AudioHelm.Note> onNotes = helmSequencer.GetAllNoteOnsInRange(columnIndex, columnIndex + 1);
            foreach (AudioHelm.Note onNote in onNotes)
            {
                helmSequencer.RemoveNote(onNote);
            }

            //

            if (noteConfig.note != -1)
            {
                helmSequencer.AddNote(noteConfig.note, columnIndex, columnIndex + 1);
            }
        }

        // Sampler

        if (sampleSequencer)
        {

            List<AudioHelm.Note> onNotes = sampleSequencer.GetAllNoteOnsInRange(columnIndex, columnIndex + 1);
            foreach (AudioHelm.Note onNote in onNotes)
            {
                sampleSequencer.RemoveNote(onNote);
            }

            //

            if (noteConfig.note != -1)
            {
                sampleSequencer.AddNote(noteConfig.note, columnIndex, columnIndex + 1);
            }
        }

        //

        sequenceConfigs[columnIndex] = noteConfig;

    }

    public void BeatOn(int beat)
    {

        if (beat >= 0)
        {

            Debug.Log(beat);
            NoteConfig noteConfig = sequenceConfigs[beat]; ;

        }
    }

}
