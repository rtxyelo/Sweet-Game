using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGameField : MonoBehaviour
{
    private const int _countOfFruits = 3;

    private const int _fruitRepeatCapacity = 3;

    private int _fruitsCountSum = _countOfFruits * _fruitRepeatCapacity;

    public int FruitRepeatCapacity { get { return _fruitRepeatCapacity; } }

    public List<int> GenerateField(int cellsCount)
    {
        List<int> countOfEachFruits = new(_countOfFruits) { _fruitsCountSum / _fruitRepeatCapacity, 
                                                            _fruitsCountSum / _fruitRepeatCapacity,
                                                            _fruitsCountSum / _fruitRepeatCapacity };

        if (cellsCount > _fruitsCountSum)
        {
            int difference = cellsCount - _fruitsCountSum;
            _fruitsCountSum = cellsCount;

            for (int i = 0; i < difference / _fruitRepeatCapacity; i++)
            {
                int _fruitIndexToAdd = Random.Range(0, _countOfFruits);
                countOfEachFruits[_fruitIndexToAdd] += _fruitRepeatCapacity;
            }
        }

        List<int> _readyGameField = CalculateFruitPositions(_fruitsCountSum, countOfEachFruits);
        //Debug.Log("Game field " + _readyGameField + " " + _readyGameField.Count);

        //for (int i = 0; i < _readyGameField.Count; i++)
        //{
        //    Debug.Log("elem " + i + " = " + _readyGameField[i]);
        //}

        return _readyGameField;
    }

    private List<int> CalculateFruitPositions(int fruitsCountSum, List<int> countOfEachFruits)
    {
        //Debug.Log("COUNT " + fruitsCountSum);

        List<int> _readyGameField = new();
        for (int i = 0; i < fruitsCountSum; )
        {
            int _fruitIndexToAdd = Random.Range(0, _countOfFruits);

            if (countOfEachFruits[_fruitIndexToAdd] > 0)
            {
                _readyGameField.Add(_fruitIndexToAdd);
                countOfEachFruits[_fruitIndexToAdd]--;
                i++;
            }
            else
                continue;
        }
        //Debug.Log("Game field--- " + _readyGameField + " " + _readyGameField.Count);

        return _readyGameField;
    }
}
