using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            int[,] map = FillMap(grid);
            List<Vector2Int> moves = new List<Vector2Int>();
            if(FillMovesMap(ref map, from, to, unit, grid))
                moves = FillMoveList(map, from, to, unit, grid);

            return moves;
        }

        private int[,] FillMap(ChessGrid grid)
        {
            int[,] map = new int[grid.Size.x, grid.Size.y];
            int rows = map.GetUpperBound(0) + 1;
            int columns = map.Length / rows;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (grid.Get(i, j) != null)
                        map[i, j] = -1;
                    else
                        map[i, j] = 0;
                }
            }

            return map;
        }
        
        private bool FillMovesMap(ref int[,] map, Vector2Int start, Vector2Int end, ChessUnitType unit, ChessGrid grid)
        {
            if (map[end.y, end.x] != 0) return false;
            bool idleIteration;
            int mark = 0;
        
            do
            {
                idleIteration = true;
                for(int i = 0; i <= map.GetUpperBound(0); i++)
                {
                    for(int j = 0; j <= map.GetUpperBound(0); j++)
                    {
                        if (mark != 0 && map[i, j] != mark) continue;
                        int x;
                        int y;
                        if (mark == 0)
                        {
                            x = start.y;
                            y = start.x;
                        }
                        else
                        {
                            x = i;
                            y = j;
                        }

                        switch (unit)
                        {
                            case ChessUnitType.Pon:
                                if (!PonFill(ref map, x, y, mark, grid.Get(start).PieceModel.Color))
                                    idleIteration = false;
                                break;
                            case ChessUnitType.King:
                                if (!KingFill(ref map, x, y, mark))
                                    idleIteration = false;
                                break;
                            case ChessUnitType.Queen:
                                if (!RookFill(ref map, x, y, mark))
                                    idleIteration = false;
                                if (!BishopFill(ref map, x, y, mark))
                                    idleIteration = false;
                                break;
                            case ChessUnitType.Rook:
                                if (!RookFill(ref map, x, y, mark))
                                    idleIteration = false;
                                break;
                            case ChessUnitType.Knight:
                                if (!KnightFill(ref map, x, y, mark))
                                    idleIteration = false;
                                break;
                            case ChessUnitType.Bishop:
                                if (!BishopFill(ref map, x, y, mark))
                                    idleIteration = false;
                                break;
                        }
                    }
                }
            
                mark++;
            }
            while((!idleIteration && map[end.y, end.x] == 0));
            if (map[end.y, end.x] != 0) return true;
            return false;
        }

        private static bool KingFill(ref int[,] map, int x, int y, int mark)
        {
            bool idleIteration = true;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    if (x + i >= 0 && x + i <= map.GetUpperBound(0)
                                   && y + j >= 0 && y + j <= map.GetUpperBound(0))
                    {
                        if (map[x + i, y + j] == 0)
                        {
                            map[x + i, y + j] = mark + 1;
                            idleIteration = false;
                        }
                    }
                }
            }

            return idleIteration;
        }

        private static bool RookFill(ref int[,] map, int x, int y, int mark)
        {
            bool idleIteration = true;
            int i = 1;
            while (x + i <= map.GetUpperBound(0) && map[x + i, y] >= 0)
            {
                if (map[x + i, y] == 0)
                {
                    map[x + i, y] = mark + 1;
                    idleIteration = false;
                }

                i++;
            }

            i = -1;
            while (x + i >= 0 && map[x + i, y] >= 0)
            {
                if (map[x + i, y] == 0)
                {
                    map[x + i, y] = mark + 1;
                    idleIteration = false;
                }

                i--;
            }

            i = 1;
            while (y + i <= map.GetUpperBound(0) && map[x, y + i] >= 0)
            {
                if (map[x, y + i] == 0)
                {
                    map[x, y + i] = mark + 1;
                    idleIteration = false;
                }

                i++;
            }

            i = -1;
            while (y + i >= 0 && map[x, y + i] >= 0)
            {
                if (map[x, y + i] == 0)
                {
                    map[x, y + i] = mark + 1;
                    idleIteration = false;
                }

                i--;
            }

            return idleIteration;
        }

        private static bool BishopFill(ref int[,] map, int x, int y, int mark)
        {
            bool idleIteration = true;
            int i = 1;
            while (x + i <= map.GetUpperBound(0) && y + i <= map.GetUpperBound(0)
                                                 && map[x + i, y + i] >= 0)
            {
                if (map[x + i, y + i] == 0)
                {
                    map[x + i, y + i] = mark + 1;
                    idleIteration = false;
                }

                i++;
            }

            i = 1;
            while (x + i <= map.GetUpperBound(0) && y - i >= 0 && map[x + i, y - i] >= 0)
            {
                if (map[x + i, y - i] == 0)
                {
                    map[x + i, y - i] = mark + 1;
                    idleIteration = false;
                }

                i++;
            }

            i = 1;
            while (x - i >= 0 && y + i <= map.GetUpperBound(0) && map[x - i, y + i] >= 0)
            {
                if (map[x - i, y + i] == 0)
                {
                    map[x - i, y + i] = mark + 1;
                    idleIteration = false;
                }

                i++;
            }

            i = 1;
            while (x - i >= 0 && y - i >= 0 && map[x - i, y - i] >= 0)
            {
                if (map[x - i, y - i] == 0)
                {
                    map[x - i, y - i] = mark + 1;
                    idleIteration = false;
                }

                i++;
            }

            return idleIteration;
        }

        private static bool KnightFill(ref int[,] map, int x, int y, int mark)
        {
            bool idleIteration = true;
            if (x + 1 <= map.GetUpperBound(0) && y + 2 <= map.GetUpperBound(0)
                                              && map[x + 1, y + 2] == 0)
            {
                map[x + 1, y + 2] = mark + 1;
                idleIteration = false;
            }

            if (x + 1 <= map.GetUpperBound(0) && y - 2 >= 0 && map[x + 1, y - 2] == 0)
            {
                map[x + 1, y - 2] = mark + 1;
                idleIteration = false;
            }

            if (x - 1 >= 0 && y + 2 <= map.GetUpperBound(0) && map[x - 1, y + 2] == 0)
            {
                map[x - 1, y + 2] = mark + 1;
                idleIteration = false;
            }

            if (x - 1 >= 0 && y - 2 >= 0 && map[x - 1, y - 2] == 0)
            {
                map[x - 1, y - 2] = mark + 1;
                idleIteration = false;
            }

            if (x + 2 <= map.GetUpperBound(0) && y + 1 <= map.GetUpperBound(0)
                                              && map[x + 2, y + 1] == 0)
            {
                map[x + 2, y + 1] = mark + 1;
                idleIteration = false;
            }

            if (x + 2 <= map.GetUpperBound(0) && y - 1 >= 0 && map[x + 2, y - 1] == 0)
            {
                map[x + 2, y - 1] = mark + 1;
                idleIteration = false;
            }

            if (x - 2 >= 0 && y + 1 <= map.GetUpperBound(0) && map[x - 2, y + 1] == 0)
            {
                map[x - 2, y + 1] = mark + 1;
                idleIteration = false;
            }

            if (x - 2 >= 0 && y - 1 >= 0 && map[x - 2, y - 1] == 0)
            {
                map[x - 2, y - 1] = mark + 1;
                idleIteration = false;
            }

            return idleIteration;
        }

        private static bool PonFill(ref int[,] map, int x, int y, int mark, ChessUnitColor color)
        {
            bool idleIteration = true;
            if (color == ChessUnitColor.Black && x - 1 >= 0 && map[x - 1, y] == 0)
            {
                map[x - 1, y] = mark + 1;
                idleIteration = false;
            }

            if (color == ChessUnitColor.White && x + 1 <= map.GetUpperBound(0)
                                              && map[x + 1, y] == 0)
            {
                map[x + 1, y] = mark + 1;
                idleIteration = false;
            }

            return idleIteration;
        }

        private static List<Vector2Int> FillMoveList(int[,] map, Vector2Int start, Vector2Int end, ChessUnitType unit, ChessGrid grid)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            Vector2Int lastPoint = new Vector2Int(end.x, end.y);
            moves.Add(lastPoint);

            for (int mark = map[end.y, end.x] - 1; mark > 0; mark--)
            {
                int x = lastPoint.y;
                int y = lastPoint.x;
                
                switch (unit)
                {
                    case ChessUnitType.Pon:
                        lastPoint = PonMove(map, x, y, mark, grid.Get(start).PieceModel.Color);
                        break;
                    case ChessUnitType.King:
                        lastPoint = KingMove(map, x, y, mark);
                        break;
                    case ChessUnitType.Queen:
                        lastPoint = RookMove(map, x, y, mark);
                        if(lastPoint.x < 0)
                            lastPoint = BishopMove(map, x, y, mark);
                        break;
                    case ChessUnitType.Rook:
                        lastPoint = RookMove(map, x, y, mark);
                        break;
                    case ChessUnitType.Knight:
                        lastPoint = KnightMove(map, x, y, mark);
                        break;
                    case ChessUnitType.Bishop:
                        lastPoint = BishopMove(map, x, y, mark);
                        break;
                }
                moves.Add(lastPoint);
            }

            moves.Reverse();
            return moves;
        }

        private static Vector2Int KingMove(int[,] map, int x, int y, int mark)
        {
            Vector2Int lastPoint = new Vector2Int();
            bool nextIteration = false;
            for (int i = -1; i <= 1; i++)
            {
                if (nextIteration) break;
                for (int j = -1; j <= 1; j++)
                {
                    if (nextIteration) break;
                    if (x + i >= 0 && x + i <= map.GetUpperBound(0) && y + j >= 0 && y + j <= map.GetUpperBound(0))
                    {
                        if (map[x + i, y + j] == mark)
                        {
                            lastPoint = new Vector2Int(y + j, x + i);
                            nextIteration = true;
                        }
                    }
                }
            }

            return lastPoint;
        }

        private static Vector2Int RookMove(int[,] map, int x, int y, int mark)
        {
            Vector2Int lastPoint = new Vector2Int(-1, -1);
            bool nextIteration = false;
            int i = 1;
            while (x + i <= map.GetUpperBound(0) && map[x + i, y] >= 0)
            {
                if (nextIteration) break;
                if (map[x + i, y] == mark)
                {
                    lastPoint = new Vector2Int(y, x + i);
                    nextIteration = true;
                }

                i++;
            }

            i = -1;
            while (x + i >= 0 && map[x + i, y] >= 0)
            {
                if (nextIteration) break;
                if (map[x + i, y] == mark)
                {
                    lastPoint = new Vector2Int(y, x + i);
                    nextIteration = true;
                }

                i--;
            }

            i = 1;
            while (y + i <= map.GetUpperBound(0) && map[x, y + i] >= 0)
            {
                if (nextIteration) break;
                if (map[x, y + i] == mark)
                {
                    lastPoint = new Vector2Int(y + i, x);
                    nextIteration = true;
                }

                i++;
            }

            i = -1;
            while (y + i >= 0 && map[x, y + i] >= 0)
            {
                if (nextIteration) break;
                if (map[x, y + i] == mark)
                {
                    lastPoint = new Vector2Int(y + i, x);
                    nextIteration = true;
                }

                i--;
            }

            return lastPoint;
        }

        private static Vector2Int BishopMove(int[,] map, int x, int y, int mark)
        {
            Vector2Int lastPoint = new Vector2Int(-1, -1);
            bool nextIteration = false;
            int i = 1;
            while (x + i <= map.GetUpperBound(0) && y + i <= map.GetUpperBound(0)
                                                 && map[x + i, y + i] >= 0)
            {
                if (nextIteration) break;
                if (map[x + i, y + i] == mark)
                {
                    lastPoint = new Vector2Int(y + i, x + i);
                    nextIteration = true;
                }

                i++;
            }

            i = 1;
            while (x + i <= map.GetUpperBound(0) && y - i >= 0 && map[x + i, y - i] >= 0)
            {
                if (nextIteration) break;
                if (map[x + i, y - i] == mark)
                {
                    lastPoint = new Vector2Int(y - i, x + i);
                    nextIteration = true;
                }

                i++;
            }

            i = 1;
            while (x - i >= 0 && y + i <= map.GetUpperBound(0) && map[x - i, y + i] >= 0)
            {
                if (nextIteration) break;
                if (map[x - i, y + i] == mark)
                {
                    lastPoint = new Vector2Int(y + i, x - i);
                    nextIteration = true;
                }

                i++;
            }

            i = 1;
            while (x - i >= 0 && y - i >= 0 && map[x - i, y - i] >= 0)
            {
                if (nextIteration) break;
                if (map[x - i, y - i] == mark)
                {
                    lastPoint = new Vector2Int(y - i, x - i);
                    nextIteration = true;
                }

                i++;
            }

            return lastPoint;
        }

        private static Vector2Int KnightMove(int[,] map, int x, int y, int mark)
        {
            Vector2Int lastPoint = new Vector2Int();
            if (x + 1 <= map.GetUpperBound(0) && y + 2 <= map.GetUpperBound(0)
                                              && map[x + 1, y + 2] == mark)
                lastPoint = new Vector2Int(y + 2, x + 1);
            else if (x + 1 <= map.GetUpperBound(0) && y - 2 >= 0 && map[x + 1, y - 2] == mark)
                lastPoint = new Vector2Int(y - 2, x + 1);
            else if (x - 1 >= 0 && y + 2 <= map.GetUpperBound(0) && map[x - 1, y + 2] == mark)
                lastPoint = new Vector2Int(y + 2, x - 1);
            else if (x - 1 >= 0 && y - 2 >= 0 && map[x - 1, y - 2] == mark)
                lastPoint = new Vector2Int(y - 2, x - 1);
            else if (x + 2 <= map.GetUpperBound(0) && y + 1 <= map.GetUpperBound(0)
                                                   && map[x + 2, y + 1] == mark)
                lastPoint = new Vector2Int(y + 1, x + 2);
            else if (x + 2 <= map.GetUpperBound(0) && y - 1 >= 0 && map[x + 2, y - 1] == mark)
                lastPoint = new Vector2Int(y - 1, x + 2);
            else if (x - 2 >= 0 && y + 1 <= map.GetUpperBound(0) && map[x - 2, y + 1] == mark)
                lastPoint = new Vector2Int(y + 1, x - 2);
            else if (x - 2 >= 0 && y - 1 >= 0 && map[x - 2, y - 1] == mark)
                lastPoint = new Vector2Int(y - 1, x - 2);
            return lastPoint;
        }

        private static Vector2Int PonMove(int[,] map, int x, int y, int mark, ChessUnitColor color)
        {
            Vector2Int lastPoint = new Vector2Int();
            if (color == ChessUnitColor.White && x - 1 >= 0 && map[x - 1, y] == mark)
                lastPoint = new Vector2Int(y, x - 1);
            else if (color == ChessUnitColor.Black && x + 1 <= map.GetUpperBound(0)
                                                   && map[x + 1, y] == mark)
                lastPoint = new Vector2Int(y, x + 1);
            return lastPoint;
        }
    }
}