using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SequencerButton : MonoBehaviour, IPointerClickHandler {

    public SequencerRow sequencerRow;
    public int sequenceIndex;
    private int noteIndex = 0;

    private Image image;


    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        noteIndex = (noteIndex + 1) % sequencerRow.noteConfigs.Count;

        //

        NoteConfig noteConfig = sequencerRow.noteConfigs[noteIndex];
        image.color = noteConfig.color;

        //

        sequencerRow.Edit(sequenceIndex, noteIndex);
    }
}
