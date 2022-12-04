using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script.GameSystems
{
    public class DialogueManager : MonoBehaviour
    {
        public Text NameText;
        public Text dialogueText;
        
        private Queue<string> sentences;

        private void Start()
        {
            sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            Debug.Log("Starting conversation with " + dialogue.name);

            NameText.text = dialogue.name;
            
            sentences.Clear();

            foreach (var sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDualogue();
                return;
            }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            dialogueText.text = sentence;
        }

        IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            foreach (var letter in sentence)
            {
                dialogueText.text += letter;
                yield return null;
            }
        }

        void EndDualogue()
        {
            Debug.Log("End of conversation.");
        }
    }
}