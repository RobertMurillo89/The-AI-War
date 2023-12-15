using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public string PieceType { get; set; }
    public (int, int)[] Direction { get; set; }

    public ChessPiece(string pieceType)
    {
        PieceType = pieceType;
        SetDirection(PieceType);
    }

    public void SetDirection(string pieceType)
    {
        switch (pieceType)
        {
            case "Rook":
                Direction = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) }; // Rook can move horizontally or vertically
                break;
            case "Bishop":
                Direction = new[] { (1, 1), (1, -1), (-1, 1), (-1, -1) }; // Bishop can move diagonally
                break;
            case "Queen":
                Direction = new[] { (0, 1), (0, -1), (1, 0), (-1, 0), (1, 1), (1, -1), (-1, 1), (-1, -1) }; // Queen can move in any direction
                break;
            default:
                throw new ArgumentException("Invalid piece type");
        }
    }
}
