using System.Collections.Generic;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    public List<DialogueLine> LoadDialogueFromFile(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        if (jsonFile == null)
        {
            Debug.LogError($"Archivo JSON no encontrado: {fileName}");
            return new List<DialogueLine>();
        }

        DialogueLineList wrapper = JsonUtility.FromJson<DialogueLineList>(jsonFile.text);
        return new List<DialogueLine>(wrapper.lines);
    }
}
