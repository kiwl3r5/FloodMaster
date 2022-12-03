using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.GameSystems
{
    public class DialogueManager : MonoBehaviour
    {
        private Queue<string> sentences;

        private void Start()
        {
            sentences = new Queue<string>();
        }
    }
}