using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LSystemGenerator : MonoBehaviour
{
    public Rules[] rules;               //LSystem을 사용할 모든 규칙
    public string rootSentence;         //공리

    [Range(0, 10)]
    public int iterationLimit = 1;

    public bool randomIgnoreRuleModifier = true;
    [Range(0, 1)]
    public float chanceToIgnoreRule = 0.3f;

    private void Start()
    {
        Debug.Log(GenerateSentence());
    }

    public string GenerateSentence(string word = null)
    {
        if(word == null)
        {
            word = rootSentence;
        }

        return GrowRecursive(word);
    }

    private string GrowRecursive(string word, int iterationIndex = 0)
    {
        if(iterationIndex >= iterationLimit)
        {
            return word;
        }
        StringBuilder newWord = new StringBuilder();

        foreach(var c in word)
        {
            newWord.Append(c);
            ProcessRulsRecursivelly(newWord, c, iterationIndex);
        }

        return newWord.ToString();
    }

    private void ProcessRulsRecursivelly(StringBuilder newWord, char c, int iterationIndex)
    {
        foreach(var rule in rules)
        {
            if(rule.letter == c.ToString())
            {
                if(randomIgnoreRuleModifier && iterationIndex > 1)
                {
                    if(UnityEngine.Random.value < chanceToIgnoreRule)
                    {
                        return;
                    }
                }
                newWord.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
            }
        }
    }
}
