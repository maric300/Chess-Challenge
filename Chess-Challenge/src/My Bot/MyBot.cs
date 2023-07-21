using ChessChallenge.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

public class MyBot : IChessBot
{
    // Piece values: null, pawn, knight, bishop, rook, queen, king
    int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };
    int depth = 5;
    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        Move moveToPlay = moves[0];
        int minValue = 100000;


        foreach (Move move in moves)
        {
            board.MakeMove(move);
            int value = Max(board, depth, -100000, 100000);
            if (value < minValue)
            {
                minValue = value;
                moveToPlay = move;
            }
            board.UndoMove(move);
        }
        return moveToPlay;
    }

    private int Max(Board board, int depth, int alpha, int beta)
    {
        Move[] moves = board.GetLegalMoves();
        depth -= 1;
        int maxValue = -100000;

        if (depth < 1)
        {
            return Evaluate(board);
        }

        foreach (Move move in moves)
        {
            board.MakeMove(move);
            int value = Min(board, depth, alpha, beta);

            if (value > maxValue)
            {
                maxValue = value;
            }
            board.UndoMove(move);

            if (maxValue >= beta)
            {
                return maxValue;
            }

            if (maxValue > alpha)
            {
                alpha = maxValue;
            }
        }
        return maxValue;
    }

    private int Evaluate(Board board)
    {

        
        
        int whitePieces = 0;
        whitePieces += board.GetPieceList(PieceType.Pawn, true).Count * 100;
        whitePieces += board.GetPieceList(PieceType.Knight, true).Count * 300;
        whitePieces += board.GetPieceList(PieceType.Bishop, true).Count * 300;
        whitePieces += board.GetPieceList(PieceType.Rook, true).Count * 500;
        whitePieces += board.GetPieceList(PieceType.Queen, true).Count * 900;

        int blackPieces = 0;
        blackPieces += board.GetPieceList(PieceType.Pawn, false).Count * 100;
        blackPieces += board.GetPieceList(PieceType.Knight, false).Count * 300;
        blackPieces += board.GetPieceList(PieceType.Bishop, false).Count * 300;
        blackPieces += board.GetPieceList(PieceType.Rook, false).Count * 500;
        blackPieces += board.GetPieceList(PieceType.Queen, false).Count * 900;



        return whitePieces - blackPieces;
    }

    private int Min(Board board, int depth, int alpha, int beta)
    {
        Move[] moves = board.GetLegalMoves();
        depth -= 1;
        int minValue = 100000;
        if (depth < 1)
        {
            return Evaluate(board);
        }
        foreach (Move move in moves)
        {
            board.MakeMove(move);
            int value = Max(board, depth, alpha, beta);

            if (value < minValue)
            {
                minValue = value;
            }
            board.UndoMove(move);

            if (minValue <= alpha)
            {
                return minValue;
            }

            if (minValue < beta)
            {
                beta = minValue;
            }
        }
        return minValue;
    }
}